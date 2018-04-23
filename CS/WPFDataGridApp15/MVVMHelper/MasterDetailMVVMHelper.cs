using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Threading;
using DevExpress.Xpf.Grid;

namespace WPFDataGridApp15
{
	public class MasterDetailMVVMHelper
	{
		private readonly static Dictionary<GridControl, MasterDetailMVVMHelper> instanceDictionary;

		private bool expandedRowsUpdate;
		public GridControl TargetControl { get; private set; }
		public IList ExpandedItemsCollection { get; private set; }

		static MasterDetailMVVMHelper()
		{
			instanceDictionary = new Dictionary<GridControl, MasterDetailMVVMHelper>();
			DXDetailPresenter.IsDetailVisibleProperty.OverrideMetadata(typeof(DependencyObject), new UIPropertyMetadata(new PropertyChangedCallback(OnIsDetailVisibleChanged)));
		}

		private static void OnIsDetailVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			foreach ( GridControl keyGridControl in instanceDictionary.Keys )
			{
				MasterDetailMVVMHelper instance = instanceDictionary[keyGridControl];
				if ( instance.InternalExpandedRowsUpdate )
					continue;

				for ( int i = 0; i < instance.TargetControl.VisibleRowCount; i++ )
				{
					int rowHandle = instance.TargetControl.GetRowHandleByVisibleIndex(i);
					if ( instance.TargetControl.IsGroupRowHandle(rowHandle) )
						continue;

					DependencyObject rowState = instance.TargetControl.GetRowState(rowHandle);
					if ( rowState != d )
						continue;

					object rowObject = instance.TargetControl.GetRow(rowHandle);
					if ( (bool)e.NewValue )
						instance.ExpandedItemsCollection.Add(rowObject);
					else
						instance.ExpandedItemsCollection.Remove(rowObject);

					break;
				}
			}
		}

		private static MasterDetailMVVMHelper CreateInstance()
		{
			return new MasterDetailMVVMHelper();
		}

		public static MasterDetailMVVMHelper GetInstance(GridControl targetGridControl)
		{
			if ( instanceDictionary.ContainsKey(targetGridControl) )
				return instanceDictionary[targetGridControl];

			return null;
		}


		public static readonly DependencyProperty ExpandedMasterRowsSourceProperty = DependencyProperty.RegisterAttached("ExpandedMasterRowsSource", typeof(IList), typeof(MasterDetailMVVMHelper), new UIPropertyMetadata(null, new PropertyChangedCallback(OnExpandedMasterRowsSourceChanged)));
		public static IList GetExpandedMasterRowsSource(GridControl target)
		{
			return (IList)target.GetValue(ExpandedMasterRowsSourceProperty);
		}

		public static void SetExpandedMasterRowsSource(GridControl target, IList value)
		{
			target.SetValue(ExpandedMasterRowsSourceProperty, value);
		}

		private static void OnExpandedMasterRowsSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			MasterDetailMVVMHelper currentInstance;

			GridControl targetGridControl = (GridControl)o;
			if ( !instanceDictionary.ContainsKey(targetGridControl) )
				instanceDictionary[targetGridControl] = CreateInstance();

			currentInstance = instanceDictionary[targetGridControl];
			currentInstance.TargetControl = targetGridControl;
			currentInstance.Instance_OnExpandedMasterRowsSourceChanged(e.NewValue as IList);
		}

		protected internal void Instance_OnExpandedMasterRowsSourceChanged(IList newValue)
		{
			ExpandedItemsCollection = newValue;
			Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => { ToggleItemRowsExpansion(ExpandedItemsCollection, true);}), DispatcherPriority.Loaded);

			INotifyCollectionChanged collectionWithNotification = newValue as INotifyCollectionChanged;
			if ( collectionWithNotification != null )
				collectionWithNotification.CollectionChanged += new NotifyCollectionChangedEventHandler(OnExpandedItemCollectionChanged);
		}

		private void OnExpandedItemCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if ( e.Action == NotifyCollectionChangedAction.Add )
				ToggleItemRowsExpansion(e.NewItems, true);

			if ( e.Action == NotifyCollectionChangedAction.Remove )
				ToggleItemRowsExpansion(e.OldItems, false);

			if ( e.Action == NotifyCollectionChangedAction.Replace )
			{
				ToggleItemRowsExpansion(e.OldItems, false);
				ToggleItemRowsExpansion(e.NewItems, true);
			}

			if ( e.Action == NotifyCollectionChangedAction.Reset )
			{
				for ( int i = 0; i < TargetControl.VisibleRowCount; i++ )
				{
					int rowHandle = TargetControl.GetRowHandleByVisibleIndex(i);
					if ( TargetControl.IsGroupRowHandle(rowHandle) )
						continue;

					DependencyObject rowState = TargetControl.GetRowState(rowHandle);
					if ( DXDetailPresenter.GetIsDetailVisible(rowState) )
						DXDetailPresenter.SetIsDetailVisible(rowState, false);
				}
			}
		}

		private void ToggleItemRowsExpansion(IList itemList, bool expand)
		{
			if ( itemList.Count == 0 )
				return;

			expandedRowsUpdate = true;

			foreach ( object item in itemList )
			{
				int itemIndex = ((IList)TargetControl.ItemsSource).IndexOf(item);
				if ( itemIndex != -1 )
				{
					int rowHandle = TargetControl.GetRowHandleByListIndex(itemIndex);
					DXDetailPresenter.SetIsDetailVisible(TargetControl.GetRowState(rowHandle), expand);
				}
			}

			expandedRowsUpdate = false;
		}

		protected internal bool InternalExpandedRowsUpdate
		{
			get { return expandedRowsUpdate; }
		}
	}
}

