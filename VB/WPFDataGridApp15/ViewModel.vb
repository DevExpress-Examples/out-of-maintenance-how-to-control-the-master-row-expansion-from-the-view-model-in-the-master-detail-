Imports System
Imports System.Collections.ObjectModel
Imports DevExpress.Xpf.Core.Commands

Namespace WPFDataGridApp15

    Public Class ViewModel

        Private _Data As ObservableCollection(Of WPFDataGridApp15.DataClass), _ExpandedItems As ObservableCollection(Of WPFDataGridApp15.DataClass), _CollapseAllCommand As DelegateCommand(Of Object)

        Public Property Data As ObservableCollection(Of DataClass)
            Get
                Return _Data
            End Get

            Private Set(ByVal value As ObservableCollection(Of DataClass))
                _Data = value
            End Set
        End Property

        Public Property ExpandedItems As ObservableCollection(Of DataClass)
            Get
                Return _ExpandedItems
            End Get

            Private Set(ByVal value As ObservableCollection(Of DataClass))
                _ExpandedItems = value
            End Set
        End Property

        Public Property CollapseAllCommand As DelegateCommand(Of Object)
            Get
                Return _CollapseAllCommand
            End Get

            Private Set(ByVal value As DelegateCommand(Of Object))
                _CollapseAllCommand = value
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
            Next
        End Sub
    End Class
End Namespace
