using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OPCDataAccessLibraries.BaseEntity
{
	public class DaGroupItemNotify : DaGroupItemBase, INotifyPropertyChanged
	{
		private DateTime _timestamp;
		private object _value;
		private int _quality;


		public override DateTime Timestamp
		{
			get => _timestamp; set
			{
				if (_timestamp == value) return;
				_timestamp = value;
				NotifyPropertyChanged();
			}
		}

		public override object Value
		{
			get => _value; set
			{
				if (_value == value) return;
				_value = value;
				NotifyPropertyChanged();
			}
		}
		public override int Quality
		{
			get => _quality; set
			{
				if (_quality == value) return;
				_quality = value;
				NotifyPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}