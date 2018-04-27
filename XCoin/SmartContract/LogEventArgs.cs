﻿using System;
using Trinity.VM;

namespace XCoin.SmartContract
{
    public class LogEventArgs : EventArgs
    {
        public IScriptContainer ScriptContainer { get; }
        public UInt160 ScriptHash { get; }
        public string Message { get; }

        public LogEventArgs(IScriptContainer container, UInt160 script_hash, string message)
        {
            this.ScriptContainer = container;
            this.ScriptHash = script_hash;
            this.Message = message;
        }
    }
}
