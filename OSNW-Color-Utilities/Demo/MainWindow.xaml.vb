Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports OSNW.Graphics

Class MainWindow

    Const DEFAULTRED As System.String = "64"
    Const DEFAULTGREEN As System.String = "128"
    Const DEFAULTBLUE As System.String = "192"

    Dim Red As System.Int32
    Dim Green As System.Int32
    Dim Blue As System.Int32

#Region "Model Events"

    '''' <summary>
    '''' Occurs when this <c>Window</c> is initialized. Backing fields and local
    '''' variables can usually be set after arriving here. See
    '''' <see cref="System.Windows.FrameworkElement.Initialized"/>.
    '''' </summary>
    'Private Sub Window_Initialized(sender As Object, e As EventArgs) _
    '    Handles Me.Initialized

    '    Me.ClosingViaOk = False
    'End Sub ' Window_Initialized

    ''' <summary>
    ''' Occurs when the <c>Window</c> is laid out, rendered, and ready for
    ''' interaction. Sometimes updates have to wait until arriving here. See
    ''' <see cref="System.Windows.FrameworkElement.Loaded"/>.
    ''' </summary>
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs) _
        Handles Me.Loaded

        '        Me.DoWindow_Loaded(sender, e)

        Me.ConvertRgbRedTextBox.Text = DEFAULTRED
        Me.ConvertRgbGreenTextBox.Text = DEFAULTGREEN
        Me.ConvertRgbBlueTextBox.Text = DEFAULTBLUE

    End Sub ' Window_Loaded

    Private Sub ExitButton_Click(sender As Object, e As RoutedEventArgs) _
        Handles ExitButton.Click

        Me.Close()
    End Sub ' ExitButton_Click

#End Region ' "Model Events"

#Region "Localized Events"

    Private Sub SelectButton_Click(sender As Object, e As RoutedEventArgs) _
        Handles SelectButton.Click

        Try
            '            Me.Do_Window_Initialized(sender, e)

            ' Extract the values entered.
            Dim Red As System.Byte =
                System.Byte.Parse(Me.ConvertRgbRedTextBox.Text)
            Dim Green As System.Byte =
                System.Byte.Parse(Me.ConvertRgbGreenTextBox.Text)
            Dim Blue As System.Byte =
                System.Byte.Parse(Me.ConvertRgbBlueTextBox.Text)

            ' Set up and show dialog.
            Dim Dlg As New OSNW.Graphics.ColorDialog With {
                .Owner = Me, .Red = Red, .Green = Green, .Blue = Blue}
            Dlg.ShowDialog()

            If Dlg.DialogResult Then
                ' Update text boxes.
                Me.ConvertRgbRedTextBox.Text = Dlg.Red.ToString()
                Me.ConvertRgbGreenTextBox.Text = Dlg.Green.ToString()
                Me.ConvertRgbBlueTextBox.Text = Dlg.Blue.ToString()
            Else
                ' ????????
            End If

        Catch CaughtEx As System.Exception
            ' Report the unexpected exception.
            Dim CaughtBy As System.Reflection.MethodBase =
                System.Reflection.MethodBase.GetCurrentMethod()
            Me.ShowExceptionMessageBox(CaughtBy, CaughtEx, sender, e)
        End Try

    End Sub ' SelectButton_Click

#End Region ' "Localized Events"

End Class ' MainWindow
