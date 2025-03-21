Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System.Windows
Imports System.Windows.Controls

' NOTE: <UseWPF>true</UseWPF> may need to be added to the dialogs'
' <projectname>.vbproj file.
'   https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props-desktop
'   Maybe just include PresentationFramework.dll? Or System.Windows?

' For Dialog.ico, set "Build Action" to "Resource" and "Copy to Output
' Directory" to "Do not copy".

' Trying to put XML comments here results in "BC42314 XML comment cannot be
'   applied more than once on a partial class. XML comments for this class will
'   be ignored."
'
'''' <summary>
'''' Represents a model for the window displayed by a <see cref="DialogHost"/>.
'''' </summary>
'''' <remarks>
'''' A <see cref="DialogHost"/> creates a layer of abstraction between its
'''' underlying <c>HostedDialogWindow</c> and the consuming assembly.
'''' <c>HostedDialogWindow</c> is designated as <c>Friend</c> and its XAML
'''' contains <c>x:ClassModifier="Friend"</c>; it is only directly available to
'''' the associated <see cref="DialogHost"/>. Public members of
'''' <see cref="System.Windows.Window"/> are not reachable by the consuming
'''' assembly unless exposed by the <see cref="DialogHost"/>.
'''' </remarks>
Partial Friend Class ColorDlgWindow

#Region "Model Event Utilities"

    ''' <summary>
    ''' Evaluate whether there is any reason to consider aborting closure via
    ''' <c>CancelButton</c>, etc.
    ''' </summary>
    ''' <returns><c>True</c> if closure via <c>CancelButton</c>, etc. should be
    ''' reconsidered; otherwise, <c>False</c>.</returns>
    Private Function WarnClose() As System.Boolean
        ' DEV: Add code here to determine if some risky condition exists when
        ' faced with a closure. If so, display a message to decide how to
        ' proceed. This can be left as is and returning False. It can also be
        ' deleted, or commented out, to avoid the useless call.
        Return False
    End Function

    ''' <summary>
    ''' Evaluate whether there is any reason to prevent closure.
    ''' </summary>
    ''' <returns><c>True</c> if closure via <c>CancelButton</c>, etc. should be
    ''' prevented; otherwise, <c>False</c>.</returns>
    Private Function BlockClose() As System.Boolean
        ' DEV: Add code here to determine if closure should be prevented. If so,
        ' display a message or other visual indication to explain the problem.
        ' This can be left as is and returning False. It can also be deleted, or
        ' commented out, to avoid the useless call.
        Return False
    End Function ' BlockClose

    ''' <summary>
    ''' Evaluate whether everything is ready to allow closure via
    ''' <c>OkButton</c>.
    ''' </summary>
    ''' <returns><c>True</c> if everything is ready to allow closure via
    ''' OkButton; otherwise, <c>False</c>.</returns>
    Private Function OkToOk() As System.Boolean

        ' DEV: The specific code here is unique to the sample dialog. The
        ' underlying reason for the function may be of use in certain cases.
        ' Add code here to determine if closure is ok. If not, display a message
        ' or other visual indication to explain the problem. This can be similar
        ' to below. It can also be deleted, or commented out, to avoid a useless
        ' call.

        '' Does IntegerTextBox contain a valid integer string?
        'Dim TestDestination As System.Int32
        'If System.Int32.TryParse(Me.IntegerTextBox.Text,
        '                         TestDestination) Then
        '    Return True
        'Else
        '    ' Display a message?
        '    Return False
        'End If



        Return True


    End Function ' OkToOk

#End Region ' "Model Event Utilities"

#Region "Model Events"

    ''' <summary>
    ''' Occurs when this <c>Window</c> is initialized. Backing fields and local
    ''' variables can usually be set after arriving here. See
    ''' <see cref="System.Windows.FrameworkElement.Initialized"/>.
    ''' </summary>
    Private Sub Window_Initialized(sender As Object, e As EventArgs) _
        Handles Me.Initialized

        Me.ClosingViaOk = False
    End Sub ' Window_Initialized

    ''' <summary>
    ''' Occurs when the <c>Window</c> is laid out, rendered, and ready for
    ''' interaction. Sometimes updates have to wait until arriving here. See
    ''' <see cref="System.Windows.FrameworkElement.Loaded"/>.
    ''' </summary>
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs) _
        Handles Me.Loaded

        Me.DoWindow_Loaded(sender, e)
    End Sub ' Window_Loaded

    ''' <summary>
    ''' Occurs directly after <see cref="System.Windows.Window.Close"/> is
    ''' called, and can be handled to cancel window closure. See
    ''' <see cref="System.Windows.Window.Closing"/>.
    ''' </summary>
    ''' <remarks>
    ''' This gets hit for <c>CancelButton</c>, Escape, ALT+F4,
    ''' System menu | Close, and the window's red X. It also gets hit whenever
    ''' <c>DialogResult</c> is set. It also gets hit for <c>OkButton</c>, if
    ''' only because it sets <c>DialogResult</c>.
    ''' </remarks>
    Private Sub Window_Closing(sender As Object,
        e As ComponentModel.CancelEventArgs) _
        Handles Me.Closing

        ' In general, do not interfere when OkButton was used.
        If Me.ClosingViaOk Then
            Exit Sub ' Early exit.
        End If

        '' This is an option for an absolute rejection.
        '' Do a local evaluation, or implement and call BlockClose(),
        '' to determine if the closure should be ignored for some reason.
        'If BlockClose() Then
        '    e.Cancel = True
        '    Exit Sub ' Early exit.
        'End If

        '' This is an option to make a choice.
        '' Do a local evaluation, or implement and call WarnClose(), to
        '' determine if the closure should be reconsidered for some reason.
        '' REF: https://learn.microsoft.com/en-us/dotnet/api/system.windows.window.closing?view=windowsdesktop-9.0#system-windows-window-closing
        'If Me.WarnClose() Then
        '    Dim Msg As System.String = "Allow close?"
        '    Dim MsgResult As System.Windows.MessageBoxResult =
        '        System.Windows.MessageBox.Show(Msg, "Approve closure",
        '            System.Windows.MessageBoxButton.YesNo,
        '            System.Windows.MessageBoxImage.Warning)
        '    If MsgResult = MessageBoxResult.No Then
        '        ' If user doesn't want to close, cancel closure.
        '        e.Cancel = True
        '        Exit Sub ' Early exit.
        '    End If
        'End If

        ' Falling through to here allows the closure to continue.

    End Sub ' Window_Closing

    '''' <summary>
    '''' Occurs when the window is about to close. See
    '''' <see cref="System.Windows.Window.Closed"/>.
    '''' </summary>
    '''' <remarks>Once this event is raised, a window cannot be prevented from
    '''' closing.</remarks>
    'Private Sub Window_Closed(sender As Object, e As EventArgs) _
    '    Handles Me.Closed

    '    Throw New System.NotImplementedException(
    '        $"Thrown by {System.Reflection.MethodBase.GetCurrentMethod}")
    'End Sub ' Window_Closed

    '''' <summary>
    '''' Abandon the current dialog session.
    '''' </summary>
    '''' <remarks>
    '''' This only responds to <c>CancelButton</c> or Escape; it does not
    '''' respond to ALT+F4, System menu | Close, or the window's red X. See
    '''' <see cref="Window_Closing"/>.
    '''' </remarks>
    'Private Sub CancelButton_Click(sender As Object, e As RoutedEventArgs) _
    '    Handles CancelButton.Click

    '    Throw New System.NotImplementedException(
    '        $"Thrown by {System.Reflection.MethodBase.GetCurrentMethod}")
    'End Sub ' CancelButton_Click

    ''' <summary>
    ''' Fill in any updates to the passed data then close the window.
    ''' </summary>
    Private Sub OkButton_Click(sender As Object, e As Windows.RoutedEventArgs) _
        Handles OkButton.Click

        ' Do a local evaluation, or implement and call OkToOk(), to determine
        ' if the current status is suitable for closure.
        If Me.OkToOk() Then

            ' Set any return values.

            ' Get ready to shut down.
            Me.ClosingViaOk = True
            Me.DialogResult = True

            'Else
            ' Display a message?
            ' Ignore the click and wait for Cancel or correction.
        End If
    End Sub ' OkButton_Click

#End Region ' "Model Events"

#Region "Example Events"
    ' DEV: These events are not intended as part of the model. They are included
    ' to support operation of the example.

    'Private Sub SliderR_ValueChanged(sender As Object,
    '    e As RoutedPropertyChangedEventArgs(Of System.Double)) _
    '    Handles SliderR.ValueChanged

    '    If Not Me.SettingSliders Then
    '        Me.Red = CType(SliderR.Value, System.Byte)
    '        Me.UpdateVisuals()
    '    End If
    'End Sub ' SliderR_ValueChanged

    'Private Sub SliderG_ValueChanged(sender As Object,
    '    e As RoutedPropertyChangedEventArgs(Of System.Double)) _
    '    Handles SliderG.ValueChanged
    '    If Not Me.SettingSliders Then
    '        Me.Green = CType(SliderG.Value, System.Byte)
    '        Me.UpdateVisuals()
    '    End If
    'End Sub ' SliderG_ValueChanged

    'Private Sub SliderB_ValueChanged(sender As Object,
    '    e As RoutedPropertyChangedEventArgs(Of System.Double)) _
    '    Handles SliderB.ValueChanged
    '    If Not Me.SettingSliders Then
    '        Me.Blue = CType(SliderB.Value, System.Byte)
    '        Me.UpdateVisuals()
    '    End If
    'End Sub ' SliderB_ValueChanged

#End Region ' "Example Events"

End Class ' ColorDlgWindow
