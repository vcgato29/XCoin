using XCoin.IO;
using Trinity.VM;
using System.IO;


namespace XCoin.Core
{
    /// <summary>
    /// Provides an interface for data that needs to be signed
    /// </summary>
    public interface IVerifiable : ISerializable, IScriptContainer
    {
        /// <summary>
        /// List of scripts used to validate this object
        /// </summary>
        Witness[] Scripts { get; set; }

        /// <summary>
        /// Deserialize unsigned data
        /// </summary>
        /// <param name="reader">reader</param>
        void DeserializeUnsigned(BinaryReader reader);

        /// <summary>
        /// Get the script hash value that needs verification
        /// </summary>
        /// <returns>hash</returns>
        UInt160[] GetScriptHashesForVerifying();
        
   
        void SerializeUnsigned(BinaryWriter writer);
    }
}
