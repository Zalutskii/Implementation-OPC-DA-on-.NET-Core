using OPCDataAccessLibraries.BaseEntity.Structures;

namespace OPCDataAccessLibraries.BaseEntity
{
	/// <inheritdoc />
	/// <summary>
	/// Arguments for data change events.
	/// </summary>
	public class DataChangeEventArgs : CompleteEventArgs
	{
		/// <inheritdoc />
		/// <summary>
		/// Constructor. 
		/// </summary>
		/// <param name="groupId">ID of OPC DA group.</param>
		/// <param name="transactionId">ID of completed transaction.</param>
		/// <param name="quality">Master quality.</param>
		/// <param name="error">Master error code.</param>
		/// <param name="values">OPC DA item values.</param>
		public DataChangeEventArgs(int groupId, int transactionId, int quality, int error, ItemValue[] values) :
			base(groupId, transactionId)
		{
			Quality = quality;
			Error = error;
			Values = values;
		}

		/// <summary>
		/// Master quality.
		/// </summary>
		public int Quality { get; }

		/// <summary>
		/// Master error code.
		/// </summary>
		public int Error { get; }

		/// <summary>
		/// OPC DA item values.
		/// </summary>
		public ItemValue[] Values { get; }
	}
}
