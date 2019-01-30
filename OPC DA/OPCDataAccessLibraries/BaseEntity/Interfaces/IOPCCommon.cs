﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OPCDataAccessLibraries.BaseEntity.Interfaces
{
	/// <summary>
	/// OPC Server common interface.
	/// Other OPC Servers such as alarms and events share this interface design. It provides the ability to set and query a LocaleID that would be in effect for the particular client/server session. That is, as with a Group definition, the actions of one client do not affect any other clients.
	/// </summary>
	[Guid("F31DFDE2-07B6-11D2-B2D8-0060083BA1FB"), ComImport, InterfaceType(1)]
	public interface IOPCCommon
	{
		/// <summary>
		/// Sets OPC Server locale ID.
		/// </summary>
		/// <param name="lcid">New locale ID.</param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetLocaleID([In] int lcid);

		/// <summary>
		/// Retrieves OPC Server locale ID.
		/// </summary>
		/// <param name="lcid">Current locale ID.</param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetLocaleID([Out] out int lcid);

		/// <summary>
		/// Retrieves all possible locales of OPC Server.
		/// </summary>
		/// <param name="count">Number of locale ID.</param>
		/// <param name="lcid">Array of locale ID.</param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void QueryAvailableLocaleIDs(
			[Out] out uint count,
			[Out] out IntPtr lcid);

		/// <summary>
		/// Converts error code to localized message based on current OPC Server locale.
		/// </summary>
		/// <param name="error">Error code.</param>
		/// <param name="string">Message for specified error code.</param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetErrorString(
			[In] int error,
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string @string);

		/// <summary>
		/// Sets name of current connection to OPC Server.
		/// </summary>
		/// <param name="name">Name of current connection to OPC Server.</param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetClientName([In] string name);
	}

}