using System.Runtime.InteropServices;

namespace OPCDataAccessLibraries.BaseEntity.Structures
{
	/// <summary>
	/// OPC DA group item properties.
	/// </summary>
	public struct Item
	{
		/// <summary>
		/// Item tag.
		/// </summary>
		public string Tag { get; set; }

		/// <summary>
		/// Item client ID.
		/// </summary>
		public int ClientId { get; set; }

		/// <summary>
		/// Requested value data type.
		/// </summary>
		public VarEnum RequestedDataType { get; set; }

		/// <summary>
		/// Requested value data sub type (if type is array).
		/// </summary>
		public VarEnum RequestedDataSubType { get; set; }

		/// <summary>
		/// Item read mode.
		/// </summary>
		public bool Active { get; set; }

		/// <summary>
		/// Item access path.
		/// </summary>
		public string AccessPath { get; set; }
	}
}
