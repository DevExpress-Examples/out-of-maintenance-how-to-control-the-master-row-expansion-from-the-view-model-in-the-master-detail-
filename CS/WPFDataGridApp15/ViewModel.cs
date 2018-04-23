using System;
using System.Collections.ObjectModel;
using DevExpress.Xpf.Core.Commands;

namespace WPFDataGridApp15
{
	public class ViewModel
	{
		public ObservableCollection<DataClass> Data { get; private set; }
		public ObservableCollection<DataClass> ExpandedItems { get; private set; }

		public DelegateCommand<object> CollapseAllCommand { get; private set; }

		public ViewModel()
		{
			Data = new ObservableCollection<DataClass>();
			ExpandedItems = new ObservableCollection<DataClass>();
			CollapseAllCommand = new DelegateCommand<object>(new Action<object>(CollapseAllExecute));
			GenerateData(20);

			ExpandedItems.Add(Data[1]);
			ExpandedItems.Add(Data[10]);
		}

		private void CollapseAllExecute(object arg)
		{
			ExpandedItems.Clear();
		}

		private void GenerateData(int count)
		{
			for ( int i = 0; i < count; i++ )
				Data.Add(new DataClass(i));
		}
	}
}

