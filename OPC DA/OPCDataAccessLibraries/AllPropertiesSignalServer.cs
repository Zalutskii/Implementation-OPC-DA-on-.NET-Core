using System;
using System.ComponentModel;

namespace OPCDataAccessLibraries
{
	[Serializable]
	public class AllPropertiesSignalServer
	{
		public AllPropertiesSignalServer() { }
		[Description("Канонический тип данных, свойство №1")]
		public string Cdt { get; set; }

		[Description("Инженерное значение параметра, свойство №2")]
		public string Value { get; set; }
		[Description("Качество параметра")]
		public string Quality { get; set; }

		[Description("Метка времени параметра, свойство №4")]
		public string TimeStamp { get; set; }

		[Description("Права доступа к параметру, свойство №5")]
		public string AccRight { get; set; }

		[Description("Интервал обновления данных с источника, свойство №6")]
		public string ScanRate { get; set; }

		[Description("Единици измерения, свойство №100")]
		public string EUnit { get; set; }

		[Description("Описание узла, свойство №101")]
		public string Description { get; set; }

		[Description("Адрес параметра, свойство №5000")]
		public string Address { get; set; }

		[Description("Active, свойство №5001")]
		public string Active { get; set; }

		[Description("RawValue, свойство №5002")]
		public string RawValue { get; set; }

		[Description("Нижняя граница физического значения, свойство №5100")]
		public string RecalcRawLow { get; set; }

		[Description("RecalcRawMiddle, свойство №5101")]
		public string RecalcRawMiddle { get; set; }

		[Description("Верхняя граница физического значения, свойство №5102")]
		public string RecalcRawHigh { get; set; }


		[Description("Нижняя граница инженерного значения, свойство №5103")]
		public string RecalcValLow { get; set; }

		[Description("RecalcValMiddle, свойство №5104")]
		public string RecalcValMiddle { get; set; }

		[Description("Верхняя граница инженерного значения, свойство №5105")]
		public string RecalcValHigh { get; set; }

		[Description("Усекать значение по границе пересчета и добавлять в качество флаги усечения, свойство №5106")]
		public string RecalcTruncate { get; set; }

		[Description("При усечении по границе пересчета выставлять SENSOR_FAILURE, свойство №5107")]
		public string RecalcSetFailureQuality { get; set; }

		[Description("RecalcInvert, свойство №5108")]
		public string RecalcInvert { get; set; }

		[Description("Количество неквитированных событий, свойство №6000")]
		public string NotAckEventCount { get; set; }

		[Description("AddressVqt, свойство №6500")]
		public string AddressVqt { get; set; }

		[Description("Признак необходимости ведения модулем истории изменения значения данного сигнала, свойство №9000")]
		public string IsNeededHistory { get; set; }

		[Description("Параметры ведения истории изменения значений данного сигнала, свойство №9001")]
		public string HistiryParametrs { get; set; }

		[Description("9002")]
		public string Param9002 { get; set; }

		[Description("10000")]
		public string Param10000 { get; set; }

		[Description("Тип объекта, свойство №999000")]
		public string ObjectType { get; set; }

		[Description("ObjectCode, свойство №999001")]
		public string ObjectCode { get; set; }

		[Description("Звук объекта, свойство №999002")]
		public string ObjectSound { get; set; }

		[Description("EventsEnabled, свойство №999003")]
		public string EventsEnabled { get; set; }

		[Description("Условия генерации событий, свойство №999004")]
		public string Conditions { get; set; }

		[Description("IsAbstract, свойство №999004")]
		public string IsAbstract { get; set; }

		public string Tag { get; set; }

		public string Kilometr { get; set; }

		public string Servise { get; set; }



		public AllPropertiesSignalServer(string[] str)
		{

			Cdt = str[0];
			Value = str[1];
			Quality = str[2];
			TimeStamp = str[3];
			AccRight = str[4];
			ScanRate = str[5];
			EUnit = str[6];
			Description = str[7];
			Address = str[8];
			Active = str[9];
			RawValue = str[10];
			RecalcRawLow = str[11];
			RecalcRawMiddle = str[12];
			RecalcRawHigh = str[13];
			RecalcValLow = str[14];
			RecalcValMiddle = str[15];
			RecalcValHigh = str[16];
			RecalcTruncate = str[17];
			RecalcSetFailureQuality = str[18];
			RecalcInvert = str[19];
			NotAckEventCount = str[20];
			AddressVqt = str[21];
			IsNeededHistory = str[22];
			HistiryParametrs = str[23];
			Param9002 = str[24];
			Param10000 = str[25];
			ObjectType = str[26];
			ObjectCode = str[27];
			ObjectSound = str[28];
			EventsEnabled = str[29];
			Conditions = str[30];
			IsAbstract = str[31];
		}
	}
}
