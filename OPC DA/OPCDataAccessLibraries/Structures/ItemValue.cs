using System;

namespace OPCDataAccessLibraries.Structures
{
	/// <summary>
	/// OPC DA group item value.
	/// </summary>
	public struct ItemValue
	{
		/// <summary>
		/// Client ID of item.
		/// </summary>
		public int ClientId { get; set; }

		/// <summary>
		/// Change date of item value.
		/// </summary>
		public DateTime Timestamp { get; set; }

		/// <summary>
		/// Item data quality.
		/// </summary>
		public int Quality { get; set; }

		/// <summary>
		/// Item value.
		/// </summary>
		public object Value { get; set; }

		/// <summary>
		/// Error code of reading item value.
		/// </summary>
		public int Error { get; set; }
	}
}
