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
		}

        void OnTargetControlChanged() {

        }

		private static MasterDetailMVVMHelper CreateInstance()
		{
			return new MasterDetailMVVMHelper();
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
            currentInstance.SetTargetControl(targetGridControl);
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

                    if(TargetControl.IsMasterRowExpanded(rowHandle))
                        TargetControl.CollapseMasterRow(rowHandle);
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
                    TargetControl.SetMasterRowExpanded(rowHandle, expand);
				}
			}

			expandedRowsUpdate = false;
		}

        void SetTargetControl(GridControl grid) {
            if(TargetControl != null) {
                TargetControl.MasterRowCollapsed -= TargetControl_MasterRowCollapsed;
                TargetControl.MasterRowExpanded -= TargetControl_MasterRowExpanded;
            }
            TargetControl = grid;
            if(TargetControl != null) {
                TargetControl.MasterRowCollapsed += TargetControl_MasterRowCollapsed;
                TargetControl.MasterRowExpanded += TargetControl_MasterRowExpanded;
            }
        }

        void TargetControl_MasterRowExpanded(object sender, RowEventArgs e) {
            if(!InternalExpandedRowsUpdate)
                ExpandedItemsCollection.Add(e.Row);
        }

        void TargetControl_MasterRowCollapsed(object sender, RowEventArgs e) {
            if(!InternalExpandedRowsUpdate)
                ExpandedItemsCollection.Remove(e.Row);
        }

		protected internal bool InternalExpandedRowsUpdate {
			get { return expandedRowsUpdate; }
		}
	}
}

