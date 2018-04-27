﻿using XCoin.IO;
using XCoin.IO.Json;
using System;
using System.IO;
using System.Linq;
using Trinity.VM;

namespace XCoin.Core
{
 
    public class TransactionAttribute : IInteropInterface, ISerializable
    {
        
        public TransactionAttributeUsage Usage;
        
        public byte[] Data;

        public int Size
        {
            get
            {
                if (Usage == TransactionAttributeUsage.ContractHash || Usage == TransactionAttributeUsage.ECDH02 || Usage == TransactionAttributeUsage.ECDH03 || Usage == TransactionAttributeUsage.Vote || (Usage >= TransactionAttributeUsage.Hash1 && Usage <= TransactionAttributeUsage.Hash15))
                    return sizeof(TransactionAttributeUsage) + 32;
                if (Usage == TransactionAttributeUsage.Script)
                    return sizeof(TransactionAttributeUsage) + 20;
                if (Usage == TransactionAttributeUsage.DescriptionUrl)
                    return sizeof(TransactionAttributeUsage) + sizeof(byte) + Data.Length;
                return sizeof(TransactionAttributeUsage) + Data.GetVarSize();
            }
        }

        void ISerializable.Deserialize(BinaryReader reader)
        {
            Usage = (TransactionAttributeUsage)reader.ReadByte();
            if (Usage == TransactionAttributeUsage.ContractHash || Usage == TransactionAttributeUsage.Vote || (Usage >= TransactionAttributeUsage.Hash1 && Usage <= TransactionAttributeUsage.Hash15))
                Data = reader.ReadBytes(32);
            else if (Usage == TransactionAttributeUsage.ECDH02 || Usage == TransactionAttributeUsage.ECDH03)
                Data = new[] { (byte)Usage }.Concat(reader.ReadBytes(32)).ToArray();
            else if (Usage == TransactionAttributeUsage.Script)
                Data = reader.ReadBytes(20);
            else if (Usage == TransactionAttributeUsage.DescriptionUrl)
                Data = reader.ReadBytes(reader.ReadByte());
            else if (Usage == TransactionAttributeUsage.Description || Usage >= TransactionAttributeUsage.Remark)
                Data = reader.ReadVarBytes(ushort.MaxValue);
            else
                throw new FormatException();
        }

        void ISerializable.Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Usage);
            if (Usage == TransactionAttributeUsage.DescriptionUrl)
                writer.Write((byte)Data.Length);
            else if (Usage == TransactionAttributeUsage.Description || Usage >= TransactionAttributeUsage.Remark)
                writer.WriteVarInt(Data.Length);
            if (Usage == TransactionAttributeUsage.ECDH02 || Usage == TransactionAttributeUsage.ECDH03)
                writer.Write(Data, 1, 32);
            else
                writer.Write(Data);
        }

        public JObject ToJson()
        {
            var json = new JObject
            {
                ["usage"] = Usage,
                ["data"] = Data.ToHexString()
            };
            return json;
        }
    }
}
