namespace OPCDataAccessLibraries.BaseEntity.Structures
{
	/// <summary>
	/// OPC DA group item property name.
	/// </summary>
	public struct ItemPropertyId
	{
		/// <summary>
		/// Name of property.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Error code of name retrieve.
		/// </summary>
		public int Error { get; set; }
	}
}
