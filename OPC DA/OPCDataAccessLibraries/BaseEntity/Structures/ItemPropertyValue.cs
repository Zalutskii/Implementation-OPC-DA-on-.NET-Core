namespace OPCDataAccessLibraries.BaseEntity.Structures
{
	/// <summary>
	/// OPC DA group item property value.
	/// </summary>
	public struct ItemPropertyValue
	{
		/// <summary>
		/// Value of property.
		/// </summary>
		public object Value { get; set; }

		/// <summary>
		/// Error code of  name retrieve.
		/// </summary>
		public int Error { get; set; }
	}
}
