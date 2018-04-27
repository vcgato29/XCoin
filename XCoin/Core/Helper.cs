﻿using XCoin.Cryptography;
using XCoin.SmartContract;
using XCoin.Wallets;
using System;
using System.IO;
using System.Linq;
using Trinity.VM;

namespace XCoin.Core
{
   
    public static class Helper
    {
        public static byte[] GetHashData(this IVerifiable verifiable)
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                verifiable.SerializeUnsigned(writer);
                writer.Flush();
                return ms.ToArray();
            }
        }

        
        public static byte[] Sign(this IVerifiable verifiable, KeyPair key)
        {
            using (key.Decrypt())
            {
                return Crypto.Default.Sign(verifiable.GetHashData(), key.PrivateKey, key.PublicKey.EncodePoint(false).Skip(1).ToArray());
            }
        }

        public static UInt160 ToScriptHash(this byte[] script)
        {
            return new UInt160(Crypto.Default.Hash160(script));
        }

        internal static bool VerifyScripts(this IVerifiable verifiable)
        {
            UInt160[] hashes;
            try
            {
                hashes = verifiable.GetScriptHashesForVerifying();
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            if (hashes.Length != verifiable.Scripts.Length) return false;
            for (int i = 0; i < hashes.Length; i++)
            {
                byte[] verification = verifiable.Scripts[i].VerificationScript;
                if (verification.Length == 0)
                {
                    using (ScriptBuilder sb = new ScriptBuilder())
                    {
                        sb.EmitAppCall(hashes[i].ToArray());
                        verification = sb.ToArray();
                    }
                }
                else
                {
                    if (hashes[i] != verifiable.Scripts[i].ScriptHash) return false;
                }
                using (StateReader service = new StateReader())
                {
                    ApplicationEngine engine = new ApplicationEngine(TriggerType.Verification, verifiable, Blockchain.Default, service, Fixed8.Zero);
                    engine.LoadScript(verification, false);
                    engine.LoadScript(verifiable.Scripts[i].InvocationScript, true);
                    if (!engine.Execute()) return false;
                    if (engine.EvaluationStack.Count != 1 || !engine.EvaluationStack.Pop().GetBoolean()) return false;
                }
            }
            return true;
        }
    }
}
