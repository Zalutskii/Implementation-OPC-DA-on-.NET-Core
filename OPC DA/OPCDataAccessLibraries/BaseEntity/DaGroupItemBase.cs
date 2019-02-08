using System;
using System.Runtime.InteropServices;
using OPCDataAccessLibraries.BaseEntity.EventArgs;

namespace OPCDataAccessLibraries.BaseEntity
{
	public class DaGroupItemBase
    {
        public virtual DateTime Timestamp { get; set; }
        public virtual string Tag { get; set; }
        public virtual object Value { get; set; }
        public virtual int Quality { get; set; }        
        public virtual string Description { get; set; }

        public int Error { get; set; }
        public int ClientId { get; set; }
        public int ServerId { get; set; }
        public VarEnum CanonicalDataType { get; set; }
        public VarEnum CanonicalDataSubType { get; set; }
        public int AccessRights { get; set; }

		public event EventHandler OnValueChaged;
		public virtual void ValueChaged(DataChangeEventArgs e)
		{
			OnValueChaged?.Invoke(this, e);
		}
	}
}