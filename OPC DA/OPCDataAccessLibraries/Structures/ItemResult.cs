using System.Runtime.InteropServices;

namespace OPCDataAccessLibraries.Structures
{
	/// <summary>
	/// Result of add/validate OPC DA group item.
	/// </summary>
	public struct ItemResult
	{
		/// <summary>
		/// Server ID of item.
		/// </summary>
		public int ServerId { get; set; }

		/// <summary>
		/// Item value data type.
		/// </summary>
		public VarEnum CanonicalDataType { get; set; }

		/// <summary>
		/// Item value data sub type (if type is array).
		/// </summary>
		public VarEnum CanonicalDataSubType { get; set; }

		/// <summary>
		/// Item security.
		/// </summary>
		public int AccessRights { get; set; }

		/// <summary>
		/// Error code of adding/validating OPC DA group item.
		/// </summary>
		public int Error { get; set; }

		/// <summary>
		/// Item BLOB.
		/// </summary>
		public byte[] Blob { get; set; }
	}
}
