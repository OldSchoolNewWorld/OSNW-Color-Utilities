Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Class MainWindow

    '''' <summary>
    '''' Occurs when this <c>Window</c> is initialized. Backing fields and local
    '''' variables can usually be set after arriving here. See
    '''' <see cref="System.Windows.FrameworkElement.Initialized"/>.
    '''' </summary>
    'Private Sub Window_Initialized(sender As Object, e As EventArgs) _
    '    Handles Me.Initialized

    '    Me.ClosingViaOk = False
    'End Sub ' Window_Initialized

    '''' <summary>
    '''' Occurs when the <c>Window</c> is laid out, rendered, and ready for
    '''' interaction. Sometimes updates have to wait until arriving here. See
    '''' <see cref="System.Windows.FrameworkElement.Loaded"/>.
    '''' </summary>
    'Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs) _
    '    Handles Me.Loaded

    '    Me.DoWindow_Loaded(sender, e)
    'End Sub ' Window_Loaded

    Private Sub ExitButton_Click(sender As Object, e As RoutedEventArgs) Handles ExitButton.Click
        Me.Close()
    End Sub ' ExitButton_Click

End Class ' MainWindow
