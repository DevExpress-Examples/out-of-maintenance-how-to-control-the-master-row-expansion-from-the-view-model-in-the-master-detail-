using System;
using System.Collections.ObjectModel;

namespace WPFDataGridApp15
{
	public class DataClass
	{
		private static Random rnd;
		public static Random Rnd
		{
			get
			{
				if ( rnd == null )
					rnd = new Random();

				return rnd;
			}
		}

		public DataClass(int seed)
		{
			IntValue = seed;
			Text = "Text " + seed;
			DateValue = DateTime.Now.AddDays(-seed);

			ChildData = new ObservableCollection<ChildDataClass>();
			for ( int i = 0; i < Rnd.Next(1, 10); i++ )
				ChildData.Add(new ChildDataClass() { ChildTextValue = "Detail value " + i });
		}

		public DataClass()
		{
		}

		public int IntValue { get; set; }
		public string Text { get; set; }
		public DateTime DateValue { get; set; }
		public ObservableCollection<ChildDataClass> ChildData { get; private set; }
	}

	public class ChildDataClass
	{
		public string ChildTextValue { get; set; }
	}
}

