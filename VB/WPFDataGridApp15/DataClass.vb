Imports System
Imports System.Collections.ObjectModel

Namespace WPFDataGridApp15
	Public Class DataClass
'INSTANT VB NOTE: The field rnd was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private Shared rnd_Conflict As Random
		Public Shared ReadOnly Property Rnd() As Random
			Get
				If rnd_Conflict Is Nothing Then
					rnd_Conflict = New Random()
				End If

				Return rnd_Conflict
			End Get
		End Property

		Public Sub New(ByVal seed As Integer)
			IntValue = seed
			Text = "Text " & seed
			DateValue = DateTime.Now.AddDays(-seed)

			ChildData = New ObservableCollection(Of ChildDataClass)()
			Dim i As Integer = 0
			Do While i < Rnd.Next(1, 10)
				ChildData.Add(New ChildDataClass() With {.ChildTextValue = "Detail value " & i})
				i += 1
			Loop
		End Sub

		Public Sub New()
		End Sub

		Public Property IntValue() As Integer
		Public Property Text() As String
		Public Property DateValue() As DateTime
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
		Public Property ChildTextValue() As String
	End Class
End Namespace

