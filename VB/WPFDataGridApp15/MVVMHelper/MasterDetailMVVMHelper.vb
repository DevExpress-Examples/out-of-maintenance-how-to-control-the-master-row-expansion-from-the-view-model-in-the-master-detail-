Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Windows
Imports System.Windows.Threading
Imports DevExpress.Xpf.Grid

Namespace WPFDataGridApp15
	Public Class MasterDetailMVVMHelper
		Private ReadOnly Shared instanceDictionary As Dictionary(Of GridControl, MasterDetailMVVMHelper)

		Private expandedRowsUpdate As Boolean
		Private privateTargetControl As GridControl
		Public Property TargetControl() As GridControl
			Get
				Return privateTargetControl
			End Get
			Private Set(ByVal value As GridControl)
				privateTargetControl = value
			End Set
		End Property
		Private privateExpandedItemsCollection As IList
		Public Property ExpandedItemsCollection() As IList
			Get
				Return privateExpandedItemsCollection
			End Get
			Private Set(ByVal value As IList)
				privateExpandedItemsCollection = value
			End Set
		End Property

		Shared Sub New()
			instanceDictionary = New Dictionary(Of GridControl, MasterDetailMVVMHelper)()
			DXDetailPresenter.IsDetailVisibleProperty.OverrideMetadata(GetType(DependencyObject), New UIPropertyMetadata(New PropertyChangedCallback(AddressOf OnIsDetailVisibleChanged)))
		End Sub

		Private Shared Sub OnIsDetailVisibleChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
			For Each keyGridControl As GridControl In instanceDictionary.Keys
				Dim instance As MasterDetailMVVMHelper = instanceDictionary(keyGridControl)
				If instance.InternalExpandedRowsUpdate Then
					Continue For
				End If

				For i As Integer = 0 To instance.TargetControl.VisibleRowCount - 1
					Dim rowHandle As Integer = instance.TargetControl.GetRowHandleByVisibleIndex(i)
					If instance.TargetControl.IsGroupRowHandle(rowHandle) Then
						Continue For
					End If

					Dim rowState As DependencyObject = instance.TargetControl.GetRowState(rowHandle)
					If rowState IsNot d Then
						Continue For
					End If

					Dim rowObject As Object = instance.TargetControl.GetRow(rowHandle)
					If CBool(e.NewValue) Then
						instance.ExpandedItemsCollection.Add(rowObject)
					Else
						instance.ExpandedItemsCollection.Remove(rowObject)
					End If

					Exit For
				Next i
			Next keyGridControl
		End Sub

		Private Shared Function CreateInstance() As MasterDetailMVVMHelper
			Return New MasterDetailMVVMHelper()
		End Function

		Public Shared Function GetInstance(ByVal targetGridControl As GridControl) As MasterDetailMVVMHelper
			If instanceDictionary.ContainsKey(targetGridControl) Then
				Return instanceDictionary(targetGridControl)
			End If

			Return Nothing
		End Function


		Public Shared ReadOnly ExpandedMasterRowsSourceProperty As DependencyProperty = DependencyProperty.RegisterAttached("ExpandedMasterRowsSource", GetType(IList), GetType(MasterDetailMVVMHelper), New UIPropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf OnExpandedMasterRowsSourceChanged)))
		Public Shared Function GetExpandedMasterRowsSource(ByVal target As GridControl) As IList
			Return CType(target.GetValue(ExpandedMasterRowsSourceProperty), IList)
		End Function

		Public Shared Sub SetExpandedMasterRowsSource(ByVal target As GridControl, ByVal value As IList)
			target.SetValue(ExpandedMasterRowsSourceProperty, value)
		End Sub

		Private Shared Sub OnExpandedMasterRowsSourceChanged(ByVal o As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
			Dim currentInstance As MasterDetailMVVMHelper

			Dim targetGridControl As GridControl = CType(o, GridControl)
			If (Not instanceDictionary.ContainsKey(targetGridControl)) Then
				instanceDictionary(targetGridControl) = CreateInstance()
			End If

			currentInstance = instanceDictionary(targetGridControl)
			currentInstance.TargetControl = targetGridControl
			currentInstance.Instance_OnExpandedMasterRowsSourceChanged(TryCast(e.NewValue, IList))
		End Sub

		Protected Friend Sub Instance_OnExpandedMasterRowsSourceChanged(ByVal newValue As IList)
			ExpandedItemsCollection = newValue
			Dispatcher.CurrentDispatcher.BeginInvoke(New Action(Function() AnonymousMethod1()), DispatcherPriority.Loaded)

			Dim collectionWithNotification As INotifyCollectionChanged = TryCast(newValue, INotifyCollectionChanged)
			If collectionWithNotification IsNot Nothing Then
				AddHandler collectionWithNotification.CollectionChanged, AddressOf OnExpandedItemCollectionChanged
			End If
		End Sub
		
		Private Function AnonymousMethod1() As Boolean
			ToggleItemRowsExpansion(ExpandedItemsCollection, True)
			Return True
		End Function

		Private Sub OnExpandedItemCollectionChanged(ByVal sender As Object, ByVal e As NotifyCollectionChangedEventArgs)
			If e.Action = NotifyCollectionChangedAction.Add Then
				ToggleItemRowsExpansion(e.NewItems, True)
			End If

			If e.Action = NotifyCollectionChangedAction.Remove Then
				ToggleItemRowsExpansion(e.OldItems, False)
			End If

			If e.Action = NotifyCollectionChangedAction.Replace Then
				ToggleItemRowsExpansion(e.OldItems, False)
				ToggleItemRowsExpansion(e.NewItems, True)
			End If

			If e.Action = NotifyCollectionChangedAction.Reset Then
				For i As Integer = 0 To TargetControl.VisibleRowCount - 1
					Dim rowHandle As Integer = TargetControl.GetRowHandleByVisibleIndex(i)
					If TargetControl.IsGroupRowHandle(rowHandle) Then
						Continue For
					End If

					Dim rowState As DependencyObject = TargetControl.GetRowState(rowHandle)
					If DXDetailPresenter.GetIsDetailVisible(rowState) Then
						DXDetailPresenter.SetIsDetailVisible(rowState, False)
					End If
				Next i
			End If
		End Sub

		Private Sub ToggleItemRowsExpansion(ByVal itemList As IList, ByVal expand As Boolean)
			If itemList.Count = 0 Then
				Return
			End If

			expandedRowsUpdate = True

			For Each item As Object In itemList
				Dim itemIndex As Integer = (CType(TargetControl.ItemsSource, IList)).IndexOf(item)
				If itemIndex <> -1 Then
					Dim rowHandle As Integer = TargetControl.GetRowHandleByListIndex(itemIndex)
					DXDetailPresenter.SetIsDetailVisible(TargetControl.GetRowState(rowHandle), expand)
				End If
			Next item

			expandedRowsUpdate = False
		End Sub

		Protected Friend ReadOnly Property InternalExpandedRowsUpdate() As Boolean
			Get
				Return expandedRowsUpdate
			End Get
		End Property
	End Class
End Namespace

