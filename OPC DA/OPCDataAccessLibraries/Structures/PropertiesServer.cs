using System.Linq;

namespace OPCDataAccessLibraries.Structures
{
	/// <summary>
	/// Все свойства сигналов ОРС сервера
	/// </summary>
	public struct PropertiesServer
	{
		public static readonly ItemProp Cdt = new ItemProp { Namber = 1, Name = "CDT", Discription = "Канонический тип данных" };
		public static readonly ItemProp Value = new ItemProp { Namber = 2, Name = "Value", Discription = "Инженерное значение параметра" };
		public static readonly ItemProp Quality = new ItemProp { Namber = 3, Name = "Quality", Discription = "Качество параметра" };
		public static readonly ItemProp TimeStamp = new ItemProp { Namber = 4, Name = "TimeStamp", Discription = "Метка времени параметра" };
		public static readonly ItemProp AccRight = new ItemProp { Namber = 5, Name = "AccRight", Discription = "Права доступа к параметру" };
		public static readonly ItemProp ScanRate = new ItemProp { Namber = 6, Name = "ScanRate", Discription = "Интервал обновления данных с источника" };
		public static readonly ItemProp EUnit = new ItemProp { Namber = 100, Name = "EUnit", Discription = "Единици измерения" };
		public static readonly ItemProp Description = new ItemProp { Namber = 101, Name = "Description", Discription = "Описание узла" };
		public static readonly ItemProp Address = new ItemProp { Namber = 5000, Name = "Address", Discription = "Адрес параметра" };
		public static readonly ItemProp Active = new ItemProp { Namber = 5001, Name = "Active", Discription = "HZ" };
		public static readonly ItemProp RawValue = new ItemProp { Namber = 5002, Name = "RawValue", Discription = "HZ" };
		public static readonly ItemProp RecalcRawLow = new ItemProp { Namber = 5100, Name = "RecalcRawLow", Discription = "Нижняя граница физического значения" };
		public static readonly ItemProp RecalcRawMiddle = new ItemProp { Namber = 5101, Name = "RecalcRawMiddle", Discription = "HZ" };
		public static readonly ItemProp RecalcRawHigh = new ItemProp { Namber = 5102, Name = "RecalcRawHigh", Discription = "Верхняя граница физического значения" };
		public static readonly ItemProp RecalcValLow = new ItemProp { Namber = 5103, Name = "RecalcValLow", Discription = "Нижняя граница инженерного значения" };
		public static readonly ItemProp RecalcValMiddle = new ItemProp { Namber = 5104, Name = "RecalcValMiddle", Discription = "HZ" };
		public static readonly ItemProp RecalcValHigh = new ItemProp { Namber = 5105, Name = "RecalcValHigh", Discription = "Верхняя граница инженерного значения" };
		public static readonly ItemProp RecalcTruncate = new ItemProp { Namber = 5106, Name = "RecalcTruncate", Discription = "Усекать значение по границе пересчета и добавлять в качество флаги усечения" };
		public static readonly ItemProp RecalcSetFailureQuality = new ItemProp { Namber = 5107, Name = "RecalcSetFailureQuality", Discription = "При усечении по границе пересчета выставлять SENSOR_FAILURE" };
		public static readonly ItemProp RecalcInvert = new ItemProp { Namber = 5108, Name = "RecalcInvert", Discription = "HZ" };
		public static readonly ItemProp NotAckEventCount = new ItemProp { Namber = 6000, Name = "NotAckEventCount", Discription = "Количество неквитированных событий" };
		public static readonly ItemProp AddressVqt = new ItemProp { Namber = 6500, Name = "AddressVqt", Discription = "HZ" };
		public static readonly ItemProp IsNeededHistory = new ItemProp { Namber = 9000, Name = "IsNeededHistory", Discription = "Признак необходимости ведения модулем истории изменения значения данного сигнала" };
		public static readonly ItemProp HistiryParametrs = new ItemProp { Namber = 9001, Name = "HistiryParametrs", Discription = "Параметры ведения истории изменения значений данного сигнала" };
		public static readonly ItemProp Param9002 = new ItemProp { Namber = 9002, Name = "HZ", Discription = "HZ" };
		public static readonly ItemProp Param10000 = new ItemProp { Namber = 10000, Name = "HZ", Discription = "HZ" };
		public static readonly ItemProp ObjectType = new ItemProp { Namber = 999000, Name = "ObjectType", Discription = "Тип объекта" };
		public static readonly ItemProp ObjectCode = new ItemProp { Namber = 999001, Name = "ObjectCode", Discription = "HZ" };
		public static readonly ItemProp ObjectSound = new ItemProp { Namber = 999002, Name = "ObjectSound", Discription = "Звук объекта" };
		public static readonly ItemProp EventsEnabled = new ItemProp { Namber = 999003, Name = "EventsEnabled", Discription = "HZ" };
		public static readonly ItemProp Conditions = new ItemProp { Namber = 999004, Name = "Conditions", Discription = "Условия генерации событий" };
		public static readonly ItemProp IsAbstract = new ItemProp { Namber = 999004, Name = "IsAbstract", Discription = "HZ" };

		/// <summary>
		////Все свойства
		/// </summary>
		public static ItemProp[] PropertyIdsAll = new ItemProp[]
		{
			Cdt,
			Value,
			Quality,
			TimeStamp,
			AccRight,
			ScanRate,
			EUnit,
			Description,
			Address,
			Active,
			RawValue,
			RecalcRawLow,
			RecalcRawMiddle,
			RecalcRawHigh,
			RecalcValLow,
			RecalcValMiddle,
			RecalcValHigh,
			RecalcTruncate,
			RecalcSetFailureQuality,
			RecalcInvert,
			NotAckEventCount,
			AddressVqt,
			IsNeededHistory,
			HistiryParametrs,
			Param9002,
			Param10000,
			ObjectType,
			ObjectCode,
			ObjectSound,
			EventsEnabled,
			Conditions,
			IsAbstract,
		};

		public static int[] PropertyIdsAllNamber = PropertyIdsAll.Select(s => s.Namber).ToArray();

		/// <summary>
		/// Свойство Description
		/// </summary>
		public static int[] PropertyIdsDescription = new int[]
		{
			Description.Namber,
		};
	}
}
