Imports System
Imports System.Collections.ObjectModel

Namespace WPFDataGridApp15
	Public Class DataClass
		Private Shared rnd_Renamed As Random
		Public Shared ReadOnly Property Rnd() As Random
			Get
				If rnd_Renamed Is Nothing Then
					rnd_Renamed = New Random()
				End If

				Return rnd_Renamed
			End Get
		End Property

		Public Sub New(ByVal seed As Integer)
			IntValue = seed
			Text = "Text " & seed
			DateValue = DateTime.Now.AddDays(-seed)

            _childData = New ObservableCollection(Of ChildDataClass)()
			For i As Integer = 0 To Rnd.Next(1, 10) - 1
                _childData.Add(New ChildDataClass() With {.ChildTextValue = "Detail value " & i})
			Next i
		End Sub

		Public Sub New()
		End Sub

        Public Property IntValue() As Integer
        Public Property Text() As String
        Public Property DateValue() As DateTime

        Private ReadOnly _childData As ObservableCollection(Of ChildDataClass)
        Public ReadOnly Property ChildData() As ObservableCollection(Of ChildDataClass)
            Get
                Return _childData
            End Get
        End Property
	End Class

	Public Class ChildDataClass
        Public Property ChildTextValue() As String
	End Class
End Namespace

