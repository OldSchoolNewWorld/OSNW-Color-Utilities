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
Friend Class ColorDlgWindow


    ' These links are from looking into being able to have the dialog window not
    ' be accessible outside of the DLL.

    ' REF: How do I mark a control as 'Private' in WPF?
    ' https://stackoverflow.com/questions/29525968/how-do-i-mark-a-control-as-private-in-wpf

    ' REF: In WPF, how do I make my controls inside a usercontrol private?
    ' https://www.ansaurus.com/question/300255-in-wpf-how-do-i-make-my-controls-inside-a-usercontrol-private

    ' REF: x:FieldModifier Directive
    ' https://learn.microsoft.com/en-us/dotnet/desktop/xaml-services/xfieldmodifier-directive

    ' REF: x:ClassModifier Directive
    ' https://learn.microsoft.com/en-us/dotnet/desktop/xaml-services/xclassmodifier-directive
    ' For Microsoft Visual Basic .NET, the string to pass to designate TypeAttributes.NotPublic is Friend.
    ' That is done in HostedDialogWindow.xaml.

    ' A signal to distinguish between aborts and acceptance, at closure.
    Private ClosingViaOk As System.Boolean

    ' A signal to prevent recursive responses.
    Private SettingSliders As System.Boolean

#Region "Properties"

    ' In general, properties like these should not need examination by the
    ' setter; that should normally be handled in the associated
    ' <see cref="ColorDialog"/>.

    Public Property Red As System.Byte
    Public Property Green As System.Byte
    Public Property Blue As System.Byte
    Public Property ShowConvertTab As System.Boolean
    Public Property ShowDefinedTab As System.Boolean
    Public Property ShowRgbTab As System.Boolean
    Public Property ShowHslTab As System.Boolean
    Public Property ShowHsvTab As System.Boolean
    Public Property ShowShadeTab As System.Boolean
    Public Property ShowTintTab As System.Boolean
    Public Property ShowToneTab As System.Boolean
    Public Property ShowBlendTab As System.Boolean

#End Region ' "Properties"

#Region "Internal utilities"
    ' DEV: These utilities are not intended as part of the model. Any dialog may
    ' need to perform operations unique to itself.

    ''' <summary>
    ''' Selects either black or white for maximum contrast to, for example, a
    ''' background color.
    ''' </summary>
    ''' <param name="r">Specifies the red component of the reference
    ''' color.</param>
    ''' <param name="g">Specifies the green component of the reference
    ''' color.</param>
    ''' <param name="b">Specifies the blue component of the reference
    ''' color.</param>
    ''' <returns>
    ''' Either black or white as a <c>System.Windows.Media.Color</c>.
    ''' </returns>
    Private Shared Function ContrastingBW(ByVal r As System.Byte,
        ByVal g As System.Byte, ByVal b As System.Byte) _
        As System.Windows.Media.Color

        ' Ref: 3D Distance Formula
        ' https://www.cuemath.com/3d-distance-formula/

        ' No argument checking. Accept any valid System.Byte values.

        ' Hypotenuse3 is reproduced here to avoid excess subroutine calls when
        ' this method is called from a loop.
        Dim DistFromBlack As System.Double =
            System.Math.Sqrt(r ^ 2 + g ^ 2 + b ^ 2)
        Dim DistFromWhite As System.Double =
            System.Math.Sqrt((255 - r) ^ 2 + (255 - g) ^ 2 + (255 - b) ^ 2)

        Return If(DistFromWhite > DistFromBlack,
            System.Windows.Media.Colors.White,
            System.Windows.Media.Colors.Black)

    End Function ' ContrastingBW

#End Region ' "Internal utilities"

#Region "Model utilities"
    ' These utilities are intended as part of the model, but the implementation
    ' may vary or they may be omitted in individual cases.

    ''' <summary>
    ''' Update visual items that reflect the impact of state changes.
    ''' </summary>
    Private Sub UpdateVisuals()
        ' DEV: The entries below are speficic the the sample dialog window.
        'Dim BackgroundColor As System.Windows.Media.Color =
        '    System.Windows.Media.Color.FromRgb(Me.Red, Me.Green, Me.Blue)
        'Me.ColorTextBox.Background =
        '    New System.Windows.Media.SolidColorBrush(BackgroundColor)
        'Dim ForegroundColor As System.Windows.Media.Color =
        '    ContrastingBW(Me.Red, Me.Green, Me.Blue)
        'Me.ColorTextBox.Foreground =
        '    New System.Windows.Media.SolidColorBrush(ForegroundColor)
        'Me.ColorTextBox.Text = $"R:{Me.Red} G:{Me.Green} B:{Me.Blue}"
        Throw New System.NotImplementedException(
            $"Thrown by {System.Reflection.MethodBase.GetCurrentMethod}")
    End Sub ' UpdateVisuals

#End Region ' "Model utilities"

#Region "Event Implementations"
    ' These routines contain detailed implementations of Event handlers.

    Private Sub DoWindow_Loaded(sender As Object, e As RoutedEventArgs)

        ' Initialize the dialog's state.
        With Me
            .ShowConvertTab = True
            .ShowDefinedTab = True
            .ShowRgbTab = True
            .ShowHslTab = True
            .ShowHsvTab = True
            .ShowShadeTab = True
            .ShowTintTab = True
            .ShowToneTab = True
            .ShowBlendTab = True
        End With



        '' Update visual items based on the incoming state.
        'With Me

        '    ' DEV: The specific code here is unique to the sample dialog. The
        '    ' underlying reason for the Sub may be of use in certain cases.

        '    ' Suppress having Red changed when SliderR moves to match Red.
        '    .SettingSliders = True
        '    Try
        '        .SliderR.Value = .Red
        '        .SliderG.Value = .Green
        '        .SliderB.Value = .Blue
        '    Finally
        '        ' Restore normal slider response.
        '        .SettingSliders = False
        '    End Try

        '    .UpdateVisuals()
        '    .StringTextBox.Text = .TheString
        '    .IntegerTextBox.Text = .TheInteger.ToString

        'End With



    End Sub ' DoWindow_Loaded

#End Region ' "Event Implementations"

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
