Imports Microsoft.VisualBasic
Imports System.Windows

Namespace WPFDataGridApp15
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			InitializeComponent()
			Me.DataContext = New ViewModel()
		End Sub
	End Class
End Namespace
