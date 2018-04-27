using System;
using Trinity.VM;

namespace XCoin.SmartContract
{
    internal abstract class Iterator : IDisposable, IInteropInterface
    {
        public abstract void Dispose();
        public abstract StackItem Key();
        public abstract bool Next();
        public abstract StackItem Value();
    }
}
