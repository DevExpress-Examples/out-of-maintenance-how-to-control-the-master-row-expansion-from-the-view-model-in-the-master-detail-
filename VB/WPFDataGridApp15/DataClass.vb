Imports Microsoft.VisualBasic
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

			ChildData = New ObservableCollection(Of ChildDataClass)()
			For i As Integer = 0 To Rnd.Next(1, 10) - 1
				ChildData.Add(New ChildDataClass() With {.ChildTextValue = "Detail value " & i})
			Next i
		End Sub

		Public Sub New()
		End Sub

		Private privateIntValue As Integer
		Public Property IntValue() As Integer
			Get
				Return privateIntValue
			End Get
			Set(ByVal value As Integer)
				privateIntValue = value
			End Set
		End Property
		Private privateText As String
		Public Property Text() As String
			Get
				Return privateText
			End Get
			Set(ByVal value As String)
				privateText = value
			End Set
		End Property
		Private privateDateValue As DateTime
		Public Property DateValue() As DateTime
			Get
				Return privateDateValue
			End Get
			Set(ByVal value As DateTime)
				privateDateValue = value
			End Set
		End Property
		Private privateChildData As ObservableCollection(Of ChildDataClass)
		Public Property ChildData() As ObservableCollection(Of ChildDataClass)
			Get
				Return privateChildData
			End Get
			Private Set(ByVal value As ObservableCollection(Of ChildDataClass))
				privateChildData = value
			End Set
		End Property
	End Class

	Public Class ChildDataClass
		Private privateChildTextValue As String
		Public Property ChildTextValue() As String
			Get
				Return privateChildTextValue
			End Get
			Set(ByVal value As String)
				privateChildTextValue = value
			End Set
		End Property
	End Class
End Namespace

