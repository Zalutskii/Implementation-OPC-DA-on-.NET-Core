using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace OPCDataAccessLibraries.BaseEntity
{
	[SuppressUnmanagedCodeSecurity]
	//[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public static class NativeMethods
	{
		public static readonly int VariantSize = IntPtr.Size == 4 ? 0x10 : 0x18;

		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("oleaut32.dll", PreserveSig = false)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static extern void VariantClear(IntPtr variant);
	}

}