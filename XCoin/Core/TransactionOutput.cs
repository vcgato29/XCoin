using XCoin.IO;
using XCoin.IO.Json;
using XCoin.Wallets;
using System;
using System.IO;
using Trinity.VM;

namespace XCoin.Core
{
    
    public class TransactionOutput : IInteropInterface, ISerializable
    {
        
        public UInt256 AssetId;
        
        public Fixed8 Value;
        
        public UInt160 ScriptHash;

        public int Size => AssetId.Size + Value.Size + ScriptHash.Size;

        void ISerializable.Deserialize(BinaryReader reader)
        {
            this.AssetId = reader.ReadSerializable<UInt256>();
            this.Value = reader.ReadSerializable<Fixed8>();
            if (Value <= Fixed8.Zero) throw new FormatException();
            this.ScriptHash = reader.ReadSerializable<UInt160>();
        }

        void ISerializable.Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.Write(Value);
            writer.Write(ScriptHash);
        }

        
        public JObject ToJson(ushort index)
        {
            var json = new JObject
            {
                ["n"] = index,
                ["asset"] = AssetId.ToString(),
                ["value"] = Value.ToString(),
                ["address"] = Wallet.ToAddress(ScriptHash)
            };
            return json;
        }
    }
}
