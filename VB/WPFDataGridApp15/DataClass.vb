Imports System
Imports System.Collections.ObjectModel

Namespace WPFDataGridApp15

    Public Class DataClass

        Private _ChildData As ObservableCollection(Of WPFDataGridApp15.ChildDataClass)

        Private Shared rndField As Random

        Public Shared ReadOnly Property Rnd As Random
            Get
                If rndField Is Nothing Then rndField = New Random()
                Return rndField
            End Get
        End Property

        Public Sub New(ByVal seed As Integer)
            IntValue = seed
            Text = "Text " & seed
            DateValue = Date.Now.AddDays(-seed)
            ChildData = New ObservableCollection(Of ChildDataClass)()
            For i As Integer = 0 To Rnd.Next(1, 10) - 1
                ChildData.Add(New ChildDataClass() With {.ChildTextValue = "Detail value " & i})
            Next
        End Sub

        Public Sub New()
        End Sub

        Public Property IntValue As Integer

        Public Property Text As String

        Public Property DateValue As Date

        Public Property ChildData As ObservableCollection(Of ChildDataClass)
            Get
                Return _ChildData
            End Get

            Private Set(ByVal value As ObservableCollection(Of ChildDataClass))
                _ChildData = value
            End Set
        End Property
    End Class

    Public Class ChildDataClass

        Public Property ChildTextValue As String
    End Class
End Namespace
