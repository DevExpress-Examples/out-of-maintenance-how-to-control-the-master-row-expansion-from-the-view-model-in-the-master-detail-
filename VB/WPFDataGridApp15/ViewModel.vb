Imports System
Imports System.Collections.ObjectModel
Imports DevExpress.Xpf.Core.Commands

Namespace WPFDataGridApp15
	Public Class ViewModel

        Private ReadOnly _data As ObservableCollection(Of DataClass)
        Public ReadOnly Property Data() As ObservableCollection(Of DataClass)
            Get
                Return _data
            End Get
        End Property

        Private ReadOnly _expandedItems As ObservableCollection(Of DataClass)
        Public ReadOnly Property ExpandedItems() As ObservableCollection(Of DataClass)
            Get
                Return _expandedItems
            End Get
        End Property

        Private ReadOnly _collapseAllCommand As DelegateCommand(Of Object)
        Public ReadOnly Property CollapseAllCommand() As DelegateCommand(Of Object)
            Get
                Return _collapseAllCommand
            End Get
        End Property

		Public Sub New()
            _data = New ObservableCollection(Of DataClass)()
            _expandedItems = New ObservableCollection(Of DataClass)()
            _collapseAllCommand = New DelegateCommand(Of Object)(New Action(Of Object)(AddressOf CollapseAllExecute))
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

