using System.Runtime.InteropServices;

namespace OPCDataAccessLibraries.Structures
{
	/// <summary>
	/// OPC DA group item property description.
	/// </summary>
	public struct ItemProperty
	{
		/// <summary>
		/// Property ID.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Property description.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Property data type.
		/// </summary>
		public VarEnum Type { get; set; }

		/// <summary>
		/// Property item data type (if type is array).
		/// </summary>
		public VarEnum SubType { get; set; }
	}
}
