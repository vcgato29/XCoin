﻿using XCoin.IO;
using XCoin.IO.Json;
using System;
using System.IO;
using Trinity.VM;

namespace XCoin.Core
{
    
    public class CoinReference : IEquatable<CoinReference>, IInteropInterface, ISerializable
    {
        
        public UInt256 PrevHash;
       
        public ushort PrevIndex;

        public int Size => PrevHash.Size + sizeof(ushort);

        void ISerializable.Deserialize(BinaryReader reader)
        {
            PrevHash = reader.ReadSerializable<UInt256>();
            PrevIndex = reader.ReadUInt16();
        }

        
        public bool Equals(CoinReference other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;
            return PrevHash.Equals(other.PrevHash) && PrevIndex.Equals(other.PrevIndex);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(null, obj)) return false;
            if (!(obj is CoinReference)) return false;
            return Equals((CoinReference)obj);
        }

        
        public override int GetHashCode()
        {
            return PrevHash.GetHashCode() + PrevIndex.GetHashCode();
        }

        void ISerializable.Serialize(BinaryWriter writer)
        {
            writer.Write(PrevHash);
            writer.Write(PrevIndex);
        }

        
        public JObject ToJson()
        {
            var json = new JObject
            {
                ["txid"] = PrevHash.ToString(),
                ["vout"] = PrevIndex
            };
            return json;
        }
    }
}
