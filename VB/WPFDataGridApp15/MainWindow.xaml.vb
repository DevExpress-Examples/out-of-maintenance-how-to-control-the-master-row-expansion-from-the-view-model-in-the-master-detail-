Imports System.Windows

Namespace WPFDataGridApp15

    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            Me.InitializeComponent()
            DataContext = New ViewModel()
        End Sub
    End Class
End Namespace
