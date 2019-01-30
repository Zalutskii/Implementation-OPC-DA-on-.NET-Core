using System.Runtime.InteropServices;

namespace OPCDataAccessLibraries.BaseEntity.Structures
{
	/// <summary>
	/// OPC DA group item properties.
	/// </summary>
	public struct Item
	{
		/// <summary>
		/// Item name.
		/// </summary>
		public string ItemId { get; }

		/// <summary>
		/// Item client ID.
		/// </summary>
		public int ClientId { get; }

		/// <summary>
		/// Requested value data type.
		/// </summary>
		public VarEnum RequestedDataType { get; }

		/// <summary>
		/// Requested value data sub type (if type is array).
		/// </summary>
		public VarEnum RequestedDataSubType { get; }

		/// <summary>
		/// Item read mode.
		/// </summary>
		public bool Active { get; }

		/// <summary>
		/// Item access path.
		/// </summary>
		public string AccessPath { get; }
	}
}
