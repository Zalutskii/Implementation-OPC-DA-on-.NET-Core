using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace OPCDataAccessLibraries
{
	internal class ItemValueWriter : IDisposable
	{
		[SecurityPermission(SecurityAction.LinkDemand)]
		public ItemValueWriter(ICollection<object> values)
		{
			Values = Marshal.AllocCoTaskMem(NativeMethods.VariantSize * values.Count);
			_variantsToClear = new List<IntPtr>(values.Count);

			var position = 0;
			var valuesPtrAsLong = Values.ToInt64();
			foreach (var value in values)
			{
				var variant = new IntPtr(valuesPtrAsLong + position);
				_variantsToClear.Add(variant);
				Marshal.GetNativeVariantForObject(value, variant);

				position += NativeMethods.VariantSize;
			}
		}

		~ItemValueWriter()
		{
			Dispose(false);
		}

		public IntPtr Values { get; private set; }

		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			if (Values != IntPtr.Zero)
				Marshal.FreeCoTaskMem(Values);
			Values = IntPtr.Zero;

			if (_variantsToClear != null)
				foreach (var variant in _variantsToClear)
					NativeMethods.VariantClear(variant);
			_variantsToClear = null;
		}

		private List<IntPtr> _variantsToClear;
	}
}
