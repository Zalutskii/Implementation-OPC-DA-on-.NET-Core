using System;
using System.ComponentModel;
using OPCDataAccessLibraries.BaseEntity.Enums;

namespace OPCDataAccessLibraries.BaseEntity
{
	/// <summary>
	/// OPC DA Server properties.
	/// </summary>
	public class ServerProperties : INotifyPropertyChanged
	{
		[Category("Basic")]
		public Guid Id { get; set; }

		[Category("Basic")]
		public string ProgramId { get; set; }

		[Category("Basic")]
		public string ServerName { get; set; }

		/// <summary>
		/// Time of start.
		/// </summary>
		[Category("Operative")]
		//[DisplayName("")]
		//[Description("This property uses a TextBox as the default editor.")]
		public DateTime? StartTime { get; set; }

		/// <summary>
		/// Current time in OPC DA Server.
		/// </summary>
		[Category("Operative")]
		public DateTime? CurrentTime { get; set; }

		/// <summary>
		/// Last time of update item values.
		/// </summary>
		[Category("Operative")]
		public DateTime? LastUpdateTime { get; set; }

		/// <summary>
		/// Current OPC DA Server state.
		/// </summary>
		[Category("Operative")]
		public ServerState? ServerState { get; set; }

		/// <summary>
		/// Number of OPC DA groups registered on server.
		/// </summary>
		[Category("Operative")]
		public int? GroupCount { get; set; }

		/// <summary>
		/// Current bandwidth.
		/// </summary>
		[Category("Operative")]
		public int? Bandwidth { get; set; }

		/// <summary>
		/// OPC DA Server major version number.
		/// </summary>

		[Category("Operative")]
		public int MajorVersion { get; set; }

		/// <summary>
		/// OPC DA Server minor version number.
		/// </summary>
		public int MinorVersion { get; set; }

		/// <summary>
		/// OPC DA Server build version number.
		/// </summary>
		public int BuildNumber { get; set; }


		/// <summary>
		/// Description of OPC DA Server.
		/// </summary>
		[Category("Operative")]
		public string VendorInfo { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public void Refreshed()
		{
			var handler = PropertyChanged;
			handler?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
		}

	}
}
