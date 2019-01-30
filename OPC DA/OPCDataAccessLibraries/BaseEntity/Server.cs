using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using OPCDataAccessLibraries.BaseEntity.Interfaces;

namespace OPCDataAccessLibraries.BaseEntity
{
	/// <inheritdoc />
	/// <summary>
	/// OPC Server client. Helps to connect to the OPC server and read information about it.
	/// </summary>
	public class Server : IDisposable
	{
		/// <inheritdoc />
		/// <summary>
		/// Connects to specified OPC Server.
		/// </summary>
		/// <param name="id">UUID of OPC Server.</param>
		public Server(Guid id) :
			this(id, string.Empty)
		{
		}

		/// <inheritdoc />
		/// <summary>
		/// Connects to specified OPC Server.
		/// </summary>
		/// <param name="id"> Program ID of OPC Server.</param>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public Server(string id) :
			this(id, string.Empty)
		{
		}

		/// <summary>
		/// Connects to specified remote OPC Server.
		/// </summary>
		/// <param name="id">UUID of OPC Server.</param>
		/// <param name="host">Host name or IP address of computer with OPC Server.</param>
		public Server(Guid id, string host)
		{
			var type = string.IsNullOrEmpty(host) ?
				Type.GetTypeFromCLSID(id, true) :
				Type.GetTypeFromCLSID(id, host, true);
			Common = (IOPCCommon)Activator.CreateInstance(type);
		}

		/// <summary>
		/// Connects to specified remote OPC Server.
		/// </summary>
		/// <param name="id">Program ID of OPC Server.</param>
		/// <param name="host">Host name or IP address of computer with OPC Server.</param>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public Server(string id, string host)
		{
			if (id == null)
				throw new ArgumentNullException(nameof(id));

			var type = string.IsNullOrEmpty(host) ?
				Type.GetTypeFromProgID(id, true) :
				Type.GetTypeFromProgID(id, host, true);
			Common = (IOPCCommon)Activator.CreateInstance(type);
		}

		[SecurityPermission(SecurityAction.LinkDemand)]
		~Server()
		{
			Dispose(false);
		}

		/// <inheritdoc />
		/// <summary>
		/// Disconnects from OPC Server.
		/// </summary>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Disconnects from OPC Server.
		/// </summary>
		/// <param name="dispose">true - call in Dispose.</param>
		[SecurityPermission(SecurityAction.LinkDemand)]
		protected virtual void Dispose(bool dispose)
		{
			if (Common != null)
				Marshal.ReleaseComObject(Common);
			Common = null;
		}

		/// <summary>
		/// OPC Server IOPCCommon interface.
		/// </summary>
		protected IOPCCommon Common { get; private set; }

		/// <summary>
		/// Sets current UI culture locale.
		/// </summary>
		public void SetLocale()
		{
			Common.SetLocaleID(CultureInfo.CurrentUICulture.LCID);
		}

		/// <summary>
		/// Sets specified culture locale.
		/// </summary>
		/// <param name="locale">Culture info for OPC Server.</param>
		public void SetLocale(CultureInfo locale)
		{
			if (locale == null) throw new ArgumentNullException(nameof(locale));
			Common.SetLocaleID(locale.LCID);
		}

		/// <summary>
		/// Retrieves current locale of OPC Server.
		/// </summary>
		/// <returns>Current locale of OPC Server.</returns>
		public CultureInfo GetLocale()
		{
			Common.GetLocaleID(out var localeId);
			return new CultureInfo(localeId);
		}

		/// <summary>
		/// Retrieves all possible locales of OPC Server.
		/// </summary>
		/// <returns>All possible locales of OPC Server.</returns>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public CultureInfo[] QueryAvailableLocales()
		{
			Common.QueryAvailableLocaleIDs(out var size, out var localesPtr);
			try
			{
				var result = new CultureInfo[size];
				for (var i = 0; i < size; i++)
					result[i] = new CultureInfo(Marshal.ReadInt32(localesPtr, i * sizeof(int)));

				return result;
			}
			finally
			{
				if (localesPtr != IntPtr.Zero)
					Marshal.FreeCoTaskMem(localesPtr);
			}
		}

		/// <summary>
		/// Converts error code to localized message based on current OPC Server locale.
		/// </summary>
		/// <param name="error">Error code.</param>
		/// <returns>Message for specified error code.</returns>
		public virtual string GetErrorString(int error)
		{
			Common.GetErrorString(error, out var description);
			return description;
		}

		/// <summary>
		/// Sets name of current connection to OPC Server.
		/// </summary>
		/// <param name="name">Name of current connection to OPC Server.</param>
		public void SetClientName(string name)
		{
			Common.SetClientName(name);
		}

		/// <summary>
		/// Tests OPC Server connection state.
		/// </summary>
		protected void DisposedCheck()
		{
			if (Common == null)
				throw new ObjectDisposedException("Server");
		}
	}

}