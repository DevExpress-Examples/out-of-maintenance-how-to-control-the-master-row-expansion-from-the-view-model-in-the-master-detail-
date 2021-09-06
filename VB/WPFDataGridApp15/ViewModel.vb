Imports System
Imports System.Collections.ObjectModel
Imports DevExpress.Xpf.Core.Commands

Namespace WPFDataGridApp15
	Public Class ViewModel
		Private privateData As ObservableCollection(Of DataClass)
		Public Property Data() As ObservableCollection(Of DataClass)
			Get
				Return privateData
			End Get
			Private Set(ByVal value As ObservableCollection(Of DataClass))
				privateData = value
			End Set
		End Property
		Private privateExpandedItems As ObservableCollection(Of DataClass)
		Public Property ExpandedItems() As ObservableCollection(Of DataClass)
			Get
				Return privateExpandedItems
			End Get
			Private Set(ByVal value As ObservableCollection(Of DataClass))
				privateExpandedItems = value
			End Set
		End Property

		Private privateCollapseAllCommand As DelegateCommand(Of Object)
		Public Property CollapseAllCommand() As DelegateCommand(Of Object)
			Get
				Return privateCollapseAllCommand
			End Get
			Private Set(ByVal value As DelegateCommand(Of Object))
				privateCollapseAllCommand = value
			End Set
		End Property

		Public Sub New()
			Data = New ObservableCollection(Of DataClass)()
			ExpandedItems = New ObservableCollection(Of DataClass)()
			CollapseAllCommand = New DelegateCommand(Of Object)(New Action(Of Object)(AddressOf CollapseAllExecute))
			GenerateData(20)

			ExpandedItems.Add(Data(1))
			ExpandedItems.Add(Data(10))
		End Sub

		Private Sub CollapseAllExecute(ByVal arg As Object)
			ExpandedItems.Clear()
		End Sub

		Private Sub GenerateData(ByVal count As Integer)
			For i As Integer = 0 To count - 1
				Data.Add(New DataClass(i))
			Next i
		End Sub
	End Class
End Namespace

