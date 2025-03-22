Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System.Windows
Imports System.Windows.Input
Imports System.Windows.Controls

'Imports System.Buffers.Text
'Imports System.ComponentModel.DataAnnotations
'Imports System.IO
'Imports System.Net.WebRequestMethods
'Imports System.Reflection.Emit
'Imports System.Threading
'Imports System.Buffers.Text
'Imports System.Configuration
'Imports System.Net.Security
'Imports System.Reflection
'Imports System.Runtime.InteropServices.JavaScript.JSType
'Imports System.Windows.Media.Animation
'Imports System.Buffers.Text
'Imports System.ComponentModel.DataAnnotations
'Imports System.IO
'Imports System.Net.WebRequestMethods
'Imports System.Reflection.Emit
'Imports System.Threading

Partial Friend Class ColorDlgWindow

#Region "Global"

    Private Sub ShowErrMessageBox(ByVal caption As System.String,
        ByVal errStr As System.String, ByVal caughtEx As System.Exception)

        Dim MessageBoxText As System.String = errStr
        Dim Button As System.Windows.MessageBoxButton =
            System.Windows.MessageBoxButton.OK
        Dim Icon As System.Windows.MessageBoxImage =
            System.Windows.MessageBoxImage.Error
        Dim DefaultResult As System.Windows.MessageBoxResult =
            System.Windows.MessageBoxResult.OK
        Dim Result As System.Windows.MessageBoxResult =
            System.Windows.MessageBox.Show(MessageBoxText, caption, Button,
                Icon, DefaultResult)


        ' FROM HERE:
        ' Public Shared Function Show(messageBoxText As String,
        '   caption As String, button As MessageBoxButton,
        '   icon As MessageBoxImage, defaultResult As MessageBoxResult)
        '   As MessageBoxResult
        ' Displays a message box that has a message, title bar caption, button,
        ' and icon; and that accepts a default message box result and returns a
        ' result.

        ' FROM EXCEPTION PROJECT
        ' Public Shared Function Show(messageBoxText As String,
        '   caption As String, button As MessageBoxButton,
        '   icon As MessageBoxImage) As MessageBoxResult
        ' Displays a message box that has a message, title bar caption, button,
        ' and icon; and that returns a result.

        ' ALSO FROM EXCEPTION PROJECT
        ' Public Shared Function Show(owner As Window, messageBoxText As String,
        '   caption As String, button As MessageBoxButton,
        '   icon As MessageBoxImage) As MessageBoxResult
        ' Displays a message box in front of the specified window. The message
        ' box displays a message, title bar caption, button, and icon; and it
        ' also returns a result.


    End Sub ' ShowErrMessageBox

    '''' <summary>
    '''' Creates a string in a standardized form.
    '''' </summary>
    Private Function G4ValueStr(ByVal componentValue As System.Double) _
        As System.String

        Return $"{componentValue:G4}"
    End Function

    '''' <summary>
    '''' Creates a string in a standardized form.
    '''' </summary>
    Private Function DoubleValueStr(ByVal componentName As System.String,
        ByVal componentValue As System.Double) As System.String

        Return $"{componentName}:{Me.G4ValueStr(componentValue)}"
    End Function

    '''' <summary>
    '''' Creates a string in a standardized form.
    '''' </summary>
    Private Function ByteValueStr(ByVal componentName As System.String,
        ByVal componentValue As System.Byte) As System.String

        Return $"{componentName}:{componentValue}"
    End Function

    Private Sub UpdatePreviewLabel()
        ' Update the preview.
        With Me.PreviewLabel
            .Background = Me.RgbWorkSolidBrush
            .Foreground = Me.RgbWorkContrastSolidBrush
            .Content = $"{Me.ByteValueStr(REDWORD, RgbWorkR)}" &
                       $" {Me.ByteValueStr(GREENWORD, RgbWorkG)}" &
                       $" {Me.ByteValueStr(BLUEWORD, RgbWorkB)}"
        End With
    End Sub

    '''' <summary>
    '''' Whole LastRgbChange on the predominant component.
    '''' </summary>
    Private Sub ResetRgbLastChange()
        With Me
            If .UnderlyingB > .UnderlyingR Then
                ' Not red; might be blue.
                .LastRgbChange = If(
                    .UnderlyingB > .UnderlyingG,
                    LastRgbChangeEnum.Blue,
                    LastRgbChangeEnum.Green)
            Else
                ' Not blue; might be red.
                .LastRgbChange = If(
                    .UnderlyingG > .UnderlyingR,
                    LastRgbChangeEnum.Green,
                    LastRgbChangeEnum.Red)
            End If
        End With
    End Sub ' ResetRgbLastChange

    ''' <summary>
    ''' Derives the basic reference values from the specified component values.
    ''' </summary>
    Private Sub DeriveFromRGB(ByVal r As System.Double,
        ByVal g As System.Double, ByVal b As System.Double)

        With Me

            ' Set the base components.
            .UnderlyingR = r
            .UnderlyingG = g
            .UnderlyingB = b

            ' Set the derived values.
            .RgbWorkR = CByte(.UnderlyingR)
            .RgbWorkG = CByte(.UnderlyingG)
            .RgbWorkB = CByte(.UnderlyingB)
            With .RgbWorkColor
                .A = &HFF
                .R = RgbWorkR
                .G = RgbWorkG
                .B = RgbWorkB
            End With
            .RgbWorkContrastColor =
                OSNW.Graphics.ColorUtilities.ContrastingBw(.RgbWorkColor)
            .RgbWorkSolidBrush =
                New System.Windows.Media.SolidColorBrush(.RgbWorkColor)
            .RgbWorkContrastSolidBrush =
                New System.Windows.Media.SolidColorBrush(.RgbWorkContrastColor)

            .UpdatePreviewLabel()

        End With
    End Sub ' DeriveFromRGB

    ''' <summary>
    ''' Distributes the impact of the associated change.
    ''' </summary>
    ''' <param name="baseR">Specifies the red component.</param>
    ''' <param name="baseG">Specifies the green component.</param>
    ''' <param name="baseB">Specifies the blue component.</param>
    ''' <param name="baseLastRgbChange">Specifies which color component was most
    ''' recently set. The value is chosen from <see cref="LastRgbChangeEnum"/>.
    ''' The default value is <c>LastRgbChangeEnum.Auto</c>, which selects the
    ''' most predominant component.</param>
    Private Sub UpdateBaseValuesFromRGB(ByVal baseR As System.Double,
        ByVal baseG As System.Double, ByVal baseB As System.Double,
        Optional ByVal baseLastRgbChange As LastRgbChangeEnum =
            LastRgbChangeEnum.Auto)

        ' Set base color components and derived values.
        With Me

            .DeriveFromRGB(baseR, baseG, baseB)
            OSNW.Graphics.ColorUtilities.RGBtoHSL(.UnderlyingR, .UnderlyingG,
                .UnderlyingB, .HslWorkH, .HslWorkS, .HslWorkL)
            OSNW.Graphics.ColorUtilities.RGBtoHSV(.UnderlyingR, .UnderlyingG,
                .UnderlyingB, .HsvWorkH, .HsvWorkS, .HsvWorkV)
            If baseLastRgbChange.Equals(LastRgbChangeEnum.Auto) Then
                ' Assume the most prominant.
                .ResetRgbLastChange()
            Else
                .LastRgbChange = baseLastRgbChange
            End If

        End With
    End Sub ' UpdateBaseValuesFromRGB

    '''' <summary>
    '''' Distributes the impact of the associated change.
    '''' </summary>
    '''' <param name="baseH">Specifies the hue component.</param>
    '''' <param name="baseS">Specifies the saturation component.</param>
    '''' <param name="baseL">Specifies the value component.</param>
    Private Sub UpdateBaseValuesFromHSL(ByVal baseH As System.Double,
        ByVal baseS As System.Double, ByVal baseL As System.Double)

        ' Set base color components and derived values.
        With Me

            OSNW.Graphics.ColorUtilities.HSLtoRGB(baseH, baseS, baseL,
                .UnderlyingR, .UnderlyingG, .UnderlyingB)

            .DeriveFromRGB(.UnderlyingR, .UnderlyingG, .UnderlyingB)
            .ResetRgbLastChange()
            OSNW.Graphics.ColorUtilities.RGBtoHSV(.UnderlyingR, .UnderlyingG,
                .UnderlyingB, .HsvWorkH, .HsvWorkS, .HsvWorkV)

        End With
    End Sub ' UpdateBaseValuesFromHSL

    ''' <summary>
    ''' Distributes the impact of the associated change.
    ''' </summary>
    ''' <param name="baseH">Specifies the hue component.</param>
    ''' <param name="baseS">Specifies the saturation component.</param>
    ''' <param name="baseV">Specifies the value component.</param>
    Private Sub UpdateBaseValuesFromHSV(ByVal baseH As System.Double,
        ByVal baseS As System.Double, ByVal baseV As System.Double)

        ' Set base color components and derived values.
        With Me

            OSNW.Graphics.ColorUtilities.HSVtoRGB(baseH, baseS, baseV,
                .UnderlyingR, .UnderlyingG, .UnderlyingB)

            .DeriveFromRGB(.UnderlyingR, .UnderlyingG, .UnderlyingB)
            .ResetRgbLastChange()

        End With
    End Sub ' UpdateBaseValuesFromHSV

    '''' <summary>
    '''' Distributes the impact of the associated change.
    '''' </summary>
    '''' <param name="shadeFactor">Specifies the degree of shading to be applied.
    '''' The input range is 0 to 1000, which is scaled later to 0.000 to 1.000.
    '''' Low values favor the base color; high values favor black.</param>
    Private Sub UpdateBaseValuesFromShade(ByVal shadeFactor As System.Int32)
        ' Set base color components and derived values.
        With Me

            .ShadeWorkFactor = shadeFactor
            .ShadeDeriveBase(shadeFactor)

            OSNW.Graphics.ColorUtilities.RGBtoHSL(.UnderlyingR, .UnderlyingG,
                .UnderlyingB, .HslWorkH, .HslWorkS, .HslWorkL)
            OSNW.Graphics.ColorUtilities.RGBtoHSV(.UnderlyingR, .UnderlyingG,
                .UnderlyingB, .HsvWorkH, .HsvWorkS, .HsvWorkV)
            ' Assume the most prominant.
            .ResetRgbLastChange()

            .UpdatePreviewLabel()

        End With
    End Sub ' UpdateBaseValuesFromShade

    '''' <summary>
    '''' Distributes the impact of the associated change.
    '''' </summary>
    '''' <param name="tintFactor">Specifies the degree of tinting to be applied.
    '''' The input range is 0 to 1000, which is scaled later to 0.000 to 1.000.
    '''' Low values favor the base color; high values favor white.</param>
    Private Sub UpdateBaseValuesFromTint(ByVal tintFactor As System.Int32)
        ' Set base color components and derived values.
        With Me

            .TintWorkFactor = tintFactor
            .TintDeriveBase(tintFactor)

            OSNW.Graphics.ColorUtilities.RGBtoHSL(.UnderlyingR, .UnderlyingG,
                .UnderlyingB, .HslWorkH, .HslWorkS, .HslWorkL)
            OSNW.Graphics.ColorUtilities.RGBtoHSV(.UnderlyingR, .UnderlyingG,
                .UnderlyingB, .HsvWorkH, .HsvWorkS, .HsvWorkV)
            ' Assume the most prominant.
            .ResetRgbLastChange()

            .UpdatePreviewLabel()

        End With
    End Sub ' UpdateBaseValuesFromTint

    '''' <summary>
    '''' Distributes the impact of the associated change.
    '''' </summary>
    '''' <param name="grayVal">Specifies the gray (range 0 to 255) color to be
    '''' blended with the base color.</param>
    '''' <param name="toneFactor">Specifies the degree of toning to be applied.
    '''' The input range is 0 to 1000, which is scaled later to 0.000 to 1.000.
    '''' Low values favor the base color; high values favor gray.</param>
    '''' <remarks>
    '''' <paramref name="grayVal"/> is used to create a gray color where
    '''' R=grayVal, G=grayVal, B=grayVal.
    '''' </remarks>
    Private Sub UpdateBaseValuesFromTone(ByVal grayVal As System.Byte,
                                         ByVal toneFactor As System.Int32)
        ' Set base color components and derived values.
        With Me

            .ToneWorkFactor = toneFactor
            .ToneWorkGray = grayVal
            .ToneDeriveBase(grayVal, toneFactor)

            OSNW.Graphics.ColorUtilities.RGBtoHSL(.UnderlyingR, .UnderlyingG,
                .UnderlyingB, .HslWorkH, .HslWorkS, .HslWorkL)
            OSNW.Graphics.ColorUtilities.RGBtoHSV(.UnderlyingR, .UnderlyingG,
                .UnderlyingB, .HsvWorkH, .HsvWorkS, .HsvWorkV)
            ' Assume the most prominant.
            .ResetRgbLastChange()

            .UpdatePreviewLabel()

        End With
    End Sub ' UpdateBaseValuesFromTone

    '''' <summary>
    '''' Go up to the next lower small step.
    '''' </summary>
    '''' <param name="currVal">Specifies the starting value.</param>
    '''' <returns>The next lower step value.</returns>
    Private Function DownPoint001From(currVal As System.Int32) As System.Int32
        If currVal <= 1 Then
            Return 0
        ElseIf currVal >= 1000 Then
            Return 999
        Else
            Return currVal - 1
        End If
    End Function

    '''' <summary>
    '''' Go up to the next higher small step.
    '''' </summary>
    '''' <param name="currVal">Specifies the starting value.</param>
    '''' <returns>The next higher step value.</returns>
    Private Function UpPoint001From(currVal As System.Int32) As System.Int32
        If currVal >= 999 Then
            Return 1000
        ElseIf currVal <= 0 Then
            Return 1
        Else
            Return currVal + 1
        End If
    End Function

    '''' <summary>
    '''' Go down to the next lower big step.
    '''' </summary>
    '''' <param name="currVal">Specifies the starting value.</param>
    '''' <returns>The next lower step value.</returns>
    Private Function DownPoint01From(currVal As System.Int32) As System.Int32
        If currVal <= 10 Then
            Return 0
        ElseIf currVal > 990 Then
            Return 990
        Else
            Dim ScaleTo10 As System.Double = CDbl(currVal) / 10.0
            Dim IntPart As System.Int32 = CInt(System.Math.Floor(ScaleTo10))
            Dim FracPart As System.Double = ScaleTo10 - IntPart
            If FracPart > 0.0 Then
                ' Drop to the step.
                Return IntPart * 10
            Else
                Return (IntPart - 1) * 10
            End If
        End If
    End Function

    '''' <summary>
    '''' Go down to the next higher big step.
    '''' </summary>
    '''' <param name="currVal">Specifies the starting value.</param>
    '''' <returns>The next higher step value.</returns>
    Private Function UpPoint01From(currVal As System.Int32) As System.Int32
        If currVal >= 990 Then
            Return 1000
        ElseIf currVal <= 10 Then
            Return 10
        Else
            Dim ScaleTo10 As System.Double = CDbl(currVal) / 10.0
            Dim IntPart As System.Int32 = CInt(System.Math.Floor(ScaleTo10))
            Return (IntPart + 1) * 10
        End If
    End Function

    '''' <summary>
    '''' Go down to the next lower big step.
    '''' </summary>
    '''' <param name="currVal">Specifies the starting value.</param>
    '''' <returns>The next lower step value.</returns>
    Private Function Down17From(currVal As System.Byte) As System.Byte

        If currVal = &H0 Then
            Return &H0 ' Early exit.
        End If

        'Dim Whole As System.Byte = CByte(currVal \ 17)
        'Dim Remainder As System.Int32 = currVal Mod 17
        'If Remainder.Equals(0) Then
        '    Return CByte((Whole - 1) * 17)
        'Else
        '    Return CByte(Whole * 17)
        'End If

        Dim Whole As System.Byte = CByte(currVal \ 17)
        Dim Remainder As System.Int32 = currVal Mod 17
        Dim Base As System.Int32 =
            If(Remainder.Equals(0), Whole - 1, Whole)
        Return CByte(Base * 17)

    End Function

    '''' <summary>
    '''' Go down to the next higher big step.
    '''' </summary>
    '''' <param name="currVal">Specifies the starting value.</param>
    '''' <returns>The next higher step value.</returns>
    Private Function Up17From(currVal As System.Byte) As System.Byte

        If currVal = &HFF Then
            Return &HFF ' Early exit.
        End If

        Dim Whole As System.Byte = CByte(currVal \ 17)
        Return CByte((Whole + 1) * 17)

    End Function

#End Region ' "Global"

#Region "Convert tab"

    Private Sub DoConvertTextBoxByteTextChanged(
        ByVal SendingTextBox As System.Windows.Controls.TextBox, ByVal ByteVal As System.Byte)
        With Me

            Dim PushOnArrival As System.Boolean = .ConvertTabPushing
            .ConvertTabPushing = True
            Try
                If SendingTextBox.Equals(.ConvertRgbRedTextBox) Then
                    .ConvertSetWorkColorsFromRgb(ByteVal, .RgbWorkG, RgbWorkB)
                    .LastRgbChange = LastRgbChangeEnum.Red
                ElseIf SendingTextBox.Equals(.ConvertRgbGreenTextBox) Then
                    .ConvertSetWorkColorsFromRgb(.RgbWorkR, ByteVal, .RgbWorkB)
                    .LastRgbChange = LastRgbChangeEnum.Green
                ElseIf SendingTextBox.Equals(.ConvertRgbBlueTextBox) Then
                    .ConvertSetWorkColorsFromRgb(.RgbWorkR, .RgbWorkG, ByteVal)
                    .LastRgbChange = LastRgbChangeEnum.Blue
                End If
            Finally
                .ConvertTabPushing = PushOnArrival
            End Try

            .UpdatePreviewLabel()
            .ConvertUpdateVisuals(SendingTextBox)

        End With
    End Sub ' DoConvertTextBoxByteTextChanged

    Private Sub DoConvertTextBoxDoubleTextChanged(
        ByVal sendingTextBox As System.Windows.Controls.TextBox,
        ByVal doubleVal As System.Double)

        With Me

            Dim PushOnArrival As System.Boolean = .ConvertTabPushing
            .ConvertTabPushing = True
            Try
                ' Combine the changed value with two existing values.
                If sendingTextBox.Equals(.ConvertHslHueTextBox) Then
                    OSNW.Graphics.ColorUtilities.HSLtoRGB(doubleVal, .HslWorkS,
                        .HslWorkL, .UnderlyingR, .UnderlyingG, .UnderlyingB)
                ElseIf sendingTextBox.Equals(.ConvertHslSaturationTextBox) Then
                    OSNW.Graphics.ColorUtilities.HSLtoRGB(.HslWorkH, doubleVal,
                        .HslWorkL, .UnderlyingR, .UnderlyingG, .UnderlyingB)
                ElseIf sendingTextBox.Equals(.ConvertHslLuminanceTextBox) Then
                    OSNW.Graphics.ColorUtilities.HSLtoRGB(.HslWorkH, .HslWorkS,
                        doubleVal, .UnderlyingR, .UnderlyingG, .UnderlyingB)
                ElseIf sendingTextBox.Equals(.ConvertHsvHueTextBox) Then
                    OSNW.Graphics.ColorUtilities.HSVtoRGB(doubleVal, .HsvWorkS,
                        .HsvWorkV, .UnderlyingR, .UnderlyingG, .UnderlyingB)
                ElseIf sendingTextBox.Equals(.ConvertHsvSaturationTextBox) Then
                    OSNW.Graphics.ColorUtilities.HSVtoRGB(.HsvWorkH, doubleVal,
                        .HsvWorkV, .UnderlyingR, .UnderlyingG, .UnderlyingB)
                ElseIf sendingTextBox.Equals(.ConvertHsvValueTextBox) Then
                    OSNW.Graphics.ColorUtilities.HSVtoRGB(.HsvWorkH, .HsvWorkS,
                        doubleVal, .UnderlyingR, .UnderlyingG, .UnderlyingB)
                End If
            Finally
                .ConvertTabPushing = PushOnArrival
            End Try

            .ConvertSetWorkColorsFromRgb(.UnderlyingR, .UnderlyingG,
                                         .UnderlyingB)
            .ResetRgbLastChange()
            .UpdatePreviewLabel()
            .ConvertUpdateVisuals(sendingTextBox)

        End With
    End Sub ' DoConvertTextBoxDoubleTextChanged

    '''' <summary>
    '''' Update the visual items unique to this tab; prepare for the next
    '''' adjustment.
    '''' </summary>
    Private Sub ConvertUpdateVisuals(
        Optional ByVal exceptFor As System.Windows.Controls.TextBox =
            Nothing)

        With Me
            Dim PushOnArrival As System.Boolean = .ConvertTabPushing
            .ConvertTabPushing = True
            Try

                If exceptFor IsNot .ConvertRgbRedTextBox Then
                    .ConvertRgbRedTextBox.Text = .RgbWorkR.ToString
                End If
                If exceptFor IsNot .ConvertRgbGreenTextBox Then
                    .ConvertRgbGreenTextBox.Text = .RgbWorkG.ToString
                End If
                If exceptFor IsNot .ConvertRgbBlueTextBox Then
                    .ConvertRgbBlueTextBox.Text = .RgbWorkB.ToString
                End If

                If exceptFor IsNot .ConvertHslHueTextBox Then
                    .ConvertHslHueTextBox.Text = .G4ValueStr(.HslWorkH)
                End If
                If exceptFor IsNot .ConvertHslSaturationTextBox Then
                    .ConvertHslSaturationTextBox.Text = .G4ValueStr(.HslWorkS)
                End If
                If exceptFor IsNot .ConvertHslLuminanceTextBox Then
                    .ConvertHslLuminanceTextBox.Text = .G4ValueStr(.HslWorkL)
                End If

                If exceptFor IsNot .ConvertHsvHueTextBox Then
                    .ConvertHsvHueTextBox.Text = .G4ValueStr(.HsvWorkH)
                End If
                If exceptFor IsNot .ConvertHsvSaturationTextBox Then
                    .ConvertHsvSaturationTextBox.Text = .G4ValueStr(.HsvWorkS)
                End If
                If exceptFor IsNot .ConvertHsvValueTextBox Then
                    .ConvertHsvValueTextBox.Text = .G4ValueStr(.HsvWorkV)
                End If

            Finally
                .ConvertTabPushing = PushOnArrival
            End Try
        End With
    End Sub ' ConvertUpdateVisuals

    Private Sub ConvertSetWorkColorsFromRgb(ByVal r As System.Double,
        ByVal g As System.Double, ByVal b As System.Double)

        With Me
            .DeriveFromRGB(r, g, b)
            OSNW.Graphics.ColorUtilities.RGBtoHSL(.UnderlyingR, .UnderlyingG,
                .UnderlyingB, .HslWorkH, .HslWorkS, .HslWorkL)
            OSNW.Graphics.ColorUtilities.RGBtoHSV(.UnderlyingR, .UnderlyingG,
                .UnderlyingB, .HsvWorkH, .HsvWorkS, .HsvWorkV)
        End With
    End Sub ' ConvertSetRgbWorkColors

#End Region ' "Convert tab"

#Region "Defined tab"

    Public Shared ReadOnly DefinedAllColors As NamedColorPair() = {
        New NamedColorPair("AliceBlue", System.Windows.Media.Colors.AliceBlue),
        New NamedColorPair("AntiqueWhite", System.Windows.Media.Colors.AntiqueWhite),
        New NamedColorPair("Aqua", System.Windows.Media.Colors.Aqua),
        New NamedColorPair("Aquamarine", System.Windows.Media.Colors.Aquamarine),
        New NamedColorPair("Azure", System.Windows.Media.Colors.Azure),
        New NamedColorPair("Beige", System.Windows.Media.Colors.Beige),
        New NamedColorPair("Bisque", System.Windows.Media.Colors.Bisque),
        New NamedColorPair("Black", System.Windows.Media.Colors.Black),
        New NamedColorPair("BlanchedAlmond", System.Windows.Media.Colors.BlanchedAlmond),
        New NamedColorPair("Blue", System.Windows.Media.Colors.Blue),
        New NamedColorPair("BlueViolet", System.Windows.Media.Colors.BlueViolet),
        New NamedColorPair("Brown", System.Windows.Media.Colors.Brown),
        New NamedColorPair("BurlyWood", System.Windows.Media.Colors.BurlyWood),
        New NamedColorPair("CadetBlue", System.Windows.Media.Colors.CadetBlue),
        New NamedColorPair("Chartreuse", System.Windows.Media.Colors.Chartreuse),
        New NamedColorPair("Chocolate", System.Windows.Media.Colors.Chocolate),
        New NamedColorPair("Coral", System.Windows.Media.Colors.Coral),
        New NamedColorPair("CornflowerBlue", System.Windows.Media.Colors.CornflowerBlue),
        New NamedColorPair("Cornsilk", System.Windows.Media.Colors.Cornsilk),
        New NamedColorPair("Crimson", System.Windows.Media.Colors.Crimson),
        New NamedColorPair("Cyan", System.Windows.Media.Colors.Cyan),
        New NamedColorPair("DarkBlue", System.Windows.Media.Colors.DarkBlue),
        New NamedColorPair("DarkCyan", System.Windows.Media.Colors.DarkCyan),
        New NamedColorPair("DarkGoldenrod", System.Windows.Media.Colors.DarkGoldenrod),
        New NamedColorPair("DarkGray", System.Windows.Media.Colors.DarkGray),
        New NamedColorPair("DarkGreen", System.Windows.Media.Colors.DarkGreen),
        New NamedColorPair("DarkKhaki", System.Windows.Media.Colors.DarkKhaki),
        New NamedColorPair("DarkMagenta", System.Windows.Media.Colors.DarkMagenta),
        New NamedColorPair("DarkOliveGreen", System.Windows.Media.Colors.DarkOliveGreen),
        New NamedColorPair("DarkOrange", System.Windows.Media.Colors.DarkOrange),
        New NamedColorPair("DarkOrchid", System.Windows.Media.Colors.DarkOrchid),
        New NamedColorPair("DarkRed", System.Windows.Media.Colors.DarkRed),
        New NamedColorPair("DarkSalmon", System.Windows.Media.Colors.DarkSalmon),
        New NamedColorPair("DarkSeaGreen", System.Windows.Media.Colors.DarkSeaGreen),
        New NamedColorPair("DarkSlateBlue", System.Windows.Media.Colors.DarkSlateBlue),
        New NamedColorPair("DarkSlateGray", System.Windows.Media.Colors.DarkSlateGray),
        New NamedColorPair("DarkTurquoise", System.Windows.Media.Colors.DarkTurquoise),
        New NamedColorPair("DarkViolet", System.Windows.Media.Colors.DarkViolet),
        New NamedColorPair("DeepPink", System.Windows.Media.Colors.DeepPink),
        New NamedColorPair("DeepSkyBlue", System.Windows.Media.Colors.DeepSkyBlue),
        New NamedColorPair("DimGray", System.Windows.Media.Colors.DimGray),
        New NamedColorPair("DodgerBlue", System.Windows.Media.Colors.DodgerBlue),
        New NamedColorPair("Firebrick", System.Windows.Media.Colors.Firebrick),
        New NamedColorPair("FloralWhite", System.Windows.Media.Colors.FloralWhite),
        New NamedColorPair("ForestGreen", System.Windows.Media.Colors.ForestGreen),
        New NamedColorPair("Fuchsia", System.Windows.Media.Colors.Fuchsia),
        New NamedColorPair("Gainsboro", System.Windows.Media.Colors.Gainsboro),
        New NamedColorPair("GhostWhite", System.Windows.Media.Colors.GhostWhite),
        New NamedColorPair("Gold", System.Windows.Media.Colors.Gold),
        New NamedColorPair("Goldenrod", System.Windows.Media.Colors.Goldenrod),
        New NamedColorPair("Gray", System.Windows.Media.Colors.Gray),
        New NamedColorPair("Green", System.Windows.Media.Colors.Green),
        New NamedColorPair("GreenYellow", System.Windows.Media.Colors.GreenYellow),
        New NamedColorPair("Honeydew", System.Windows.Media.Colors.Honeydew),
        New NamedColorPair("HotPink", System.Windows.Media.Colors.HotPink),
        New NamedColorPair("IndianRed", System.Windows.Media.Colors.IndianRed),
        New NamedColorPair("Indigo", System.Windows.Media.Colors.Indigo),
        New NamedColorPair("Ivory", System.Windows.Media.Colors.Ivory),
        New NamedColorPair("Khaki", System.Windows.Media.Colors.Khaki),
        New NamedColorPair("Lavender", System.Windows.Media.Colors.Lavender),
        New NamedColorPair("LavenderBlush", System.Windows.Media.Colors.LavenderBlush),
        New NamedColorPair("LawnGreen", System.Windows.Media.Colors.LawnGreen),
        New NamedColorPair("LemonChiffon", System.Windows.Media.Colors.LemonChiffon),
        New NamedColorPair("LightBlue", System.Windows.Media.Colors.LightBlue),
        New NamedColorPair("LightCoral", System.Windows.Media.Colors.LightCoral),
        New NamedColorPair("LightCyan", System.Windows.Media.Colors.LightCyan),
        New NamedColorPair("LightGoldenrodYellow", System.Windows.Media.Colors.LightGoldenrodYellow),
        New NamedColorPair("LightGray", System.Windows.Media.Colors.LightGray),
        New NamedColorPair("LightGreen", System.Windows.Media.Colors.LightGreen),
        New NamedColorPair("LightPink", System.Windows.Media.Colors.LightPink),
        New NamedColorPair("LightSalmon", System.Windows.Media.Colors.LightSalmon),
        New NamedColorPair("LightSeaGreen", System.Windows.Media.Colors.LightSeaGreen),
        New NamedColorPair("LightSkyBlue", System.Windows.Media.Colors.LightSkyBlue),
        New NamedColorPair("LightSlateGray", System.Windows.Media.Colors.LightSlateGray),
        New NamedColorPair("LightSteelBlue", System.Windows.Media.Colors.LightSteelBlue),
        New NamedColorPair("LightYellow", System.Windows.Media.Colors.LightYellow),
        New NamedColorPair("Lime", System.Windows.Media.Colors.Lime),
        New NamedColorPair("LimeGreen", System.Windows.Media.Colors.LimeGreen),
        New NamedColorPair("Linen", System.Windows.Media.Colors.Linen),
        New NamedColorPair("Magenta", System.Windows.Media.Colors.Magenta),
        New NamedColorPair("Maroon", System.Windows.Media.Colors.Maroon),
        New NamedColorPair("MediumAquamarine", System.Windows.Media.Colors.MediumAquamarine),
        New NamedColorPair("MediumBlue", System.Windows.Media.Colors.MediumBlue),
        New NamedColorPair("MediumOrchid", System.Windows.Media.Colors.MediumOrchid),
        New NamedColorPair("MediumPurple", System.Windows.Media.Colors.MediumPurple),
        New NamedColorPair("MediumSeaGreen", System.Windows.Media.Colors.MediumSeaGreen),
        New NamedColorPair("MediumSlateBlue", System.Windows.Media.Colors.MediumSlateBlue),
        New NamedColorPair("MediumSpringGreen", System.Windows.Media.Colors.MediumSpringGreen),
        New NamedColorPair("MediumTurquoise", System.Windows.Media.Colors.MediumTurquoise),
        New NamedColorPair("MediumVioletRed", System.Windows.Media.Colors.MediumVioletRed),
        New NamedColorPair("MidnightBlue", System.Windows.Media.Colors.MidnightBlue),
        New NamedColorPair("MintCream", System.Windows.Media.Colors.MintCream),
        New NamedColorPair("MistyRose", System.Windows.Media.Colors.MistyRose),
        New NamedColorPair("Moccasin", System.Windows.Media.Colors.Moccasin),
        New NamedColorPair("NavajoWhite", System.Windows.Media.Colors.NavajoWhite),
        New NamedColorPair("Navy", System.Windows.Media.Colors.Navy),
        New NamedColorPair("OldLace", System.Windows.Media.Colors.OldLace),
        New NamedColorPair("Olive", System.Windows.Media.Colors.Olive),
        New NamedColorPair("OliveDrab", System.Windows.Media.Colors.OliveDrab),
        New NamedColorPair("Orange", System.Windows.Media.Colors.Orange),
        New NamedColorPair("OrangeRed", System.Windows.Media.Colors.OrangeRed),
        New NamedColorPair("Orchid", System.Windows.Media.Colors.Orchid),
        New NamedColorPair("PaleGoldenrod", System.Windows.Media.Colors.PaleGoldenrod),
        New NamedColorPair("PaleGreen", System.Windows.Media.Colors.PaleGreen),
        New NamedColorPair("PaleTurquoise", System.Windows.Media.Colors.PaleTurquoise),
        New NamedColorPair("PaleVioletRed", System.Windows.Media.Colors.PaleVioletRed),
        New NamedColorPair("PapayaWhip", System.Windows.Media.Colors.PapayaWhip),
        New NamedColorPair("PeachPuff", System.Windows.Media.Colors.PeachPuff),
        New NamedColorPair("Peru", System.Windows.Media.Colors.Peru),
        New NamedColorPair("Pink", System.Windows.Media.Colors.Pink),
        New NamedColorPair("Plum", System.Windows.Media.Colors.Plum),
        New NamedColorPair("PowderBlue", System.Windows.Media.Colors.PowderBlue),
        New NamedColorPair("Purple", System.Windows.Media.Colors.Purple),
        New NamedColorPair("Red", System.Windows.Media.Colors.Red),
        New NamedColorPair("RosyBrown", System.Windows.Media.Colors.RosyBrown),
        New NamedColorPair("RoyalBlue", System.Windows.Media.Colors.RoyalBlue),
        New NamedColorPair("SaddleBrown", System.Windows.Media.Colors.SaddleBrown),
        New NamedColorPair("Salmon", System.Windows.Media.Colors.Salmon),
        New NamedColorPair("SandyBrown", System.Windows.Media.Colors.SandyBrown),
        New NamedColorPair("SeaGreen", System.Windows.Media.Colors.SeaGreen),
        New NamedColorPair("SeaShell", System.Windows.Media.Colors.SeaShell),
        New NamedColorPair("Sienna", System.Windows.Media.Colors.Sienna),
        New NamedColorPair("Silver", System.Windows.Media.Colors.Silver),
        New NamedColorPair("SkyBlue", System.Windows.Media.Colors.SkyBlue),
        New NamedColorPair("SlateBlue", System.Windows.Media.Colors.SlateBlue),
        New NamedColorPair("SlateGray", System.Windows.Media.Colors.SlateGray),
        New NamedColorPair("Snow", System.Windows.Media.Colors.Snow),
        New NamedColorPair("SpringGreen", System.Windows.Media.Colors.SpringGreen),
        New NamedColorPair("SteelBlue", System.Windows.Media.Colors.SteelBlue),
        New NamedColorPair("Tan", System.Windows.Media.Colors.Tan),
        New NamedColorPair("Teal", System.Windows.Media.Colors.Teal),
        New NamedColorPair("Thistle", System.Windows.Media.Colors.Thistle),
        New NamedColorPair("Tomato", System.Windows.Media.Colors.Tomato),
        New NamedColorPair("Transparent", System.Windows.Media.Colors.Transparent),
        New NamedColorPair("Turquoise", System.Windows.Media.Colors.Turquoise),
        New NamedColorPair("Violet", System.Windows.Media.Colors.Violet),
        New NamedColorPair("Wheat", System.Windows.Media.Colors.Wheat),
        New NamedColorPair("White", System.Windows.Media.Colors.White),
        New NamedColorPair("WhiteSmoke", System.Windows.Media.Colors.WhiteSmoke),
        New NamedColorPair("Yellow", System.Windows.Media.Colors.Yellow),
        New NamedColorPair("YellowGreen", System.Windows.Media.Colors.YellowGreen)
    }

    Private Sub DefinedInitComboBox()

        ' Remove the XAML items that were only for layout design.
        Me.DefinedComboBox.Items.Clear()

        Dim OptionPreviewColor As System.Windows.Media.SolidColorBrush
        Dim OptionTextColor As System.Windows.Media.SolidColorBrush

        For Each OneDefinedColor As NamedColorPair In DefinedAllColors

            ' Set up coloring for one option.
            OptionPreviewColor =
                New System.Windows.Media.SolidColorBrush(OneDefinedColor.Color)
            OptionTextColor = New System.Windows.Media.SolidColorBrush(
                OSNW.Graphics.ColorUtilities.ContrastingBw(OneDefinedColor.Color))

            ' Set up the label shown for this option.
            Dim NewLabel As New System.Windows.Controls.Label
            With NewLabel
                .Width = DEFINEDCOMBOBOXLABELWIDTH
                .Background = OptionPreviewColor
                .Foreground = OptionTextColor
                .Content = OneDefinedColor.Name
            End With

            ' Store the new option.
            Me.DefinedComboBox.Items.Add(NewLabel)

        Next

    End Sub ' DefinedInitComboBox

#End Region ' "Defined tab"

#Region "RGB tab"

    '''' <summary>
    '''' Update the visual items unique to this tab; prepare for the next
    '''' adjustment.
    '''' </summary>
    Private Sub RgbUpdateVisuals()

        With Me

            ' Update the labels around the image.
            Select Case Me.LastRgbChange
                Case LastRgbChangeEnum.Red
                    .RgbBaseLabel.Content =
                        $"{ByteValueStr(REDWORD, .RgbWorkR)} Whole"
                    .RgbXLabel.Content = Green
                    .RgbYLabel.Content = Blue
                Case LastRgbChangeEnum.Green
                    .RgbBaseLabel.Content =
                        $"{ByteValueStr(GREENWORD, .RgbWorkG)} Whole"
                    .RgbXLabel.Content = Red
                    .RgbYLabel.Content = Blue
                Case Else
                    .RgbBaseLabel.Content =
                        $"{ByteValueStr(BLUEWORD, .RgbWorkB)} Whole"
                    .RgbXLabel.Content = Red
                    .RgbYLabel.Content = Green
            End Select

            ' Update the displayed per-component values.
            .TweakRedLabel.Content = .ByteValueStr(REDWORD, .RgbWorkR)
            .TweakGreenLabel.Content = .ByteValueStr(GREENWORD, .RgbWorkG)
            .TweakBlueLabel.Content = .ByteValueStr(BLUEWORD, .RgbWorkB)

            ' Update the square.
            .RgbFillImage()

        End With

    End Sub ' RgbUpdateVisuals

    '''' <summary>
    '''' Respond to  click in the image.
    '''' </summary>
    Private Sub RgbProcessMouseClick(
        sender As Object, e As MouseButtonEventArgs)

        ' Ref: Type Conversion Functions (Visual Basic)
        ' https://learn.microsoft.com/en-us/dotnet/visual-basic/language-reference/functions/type-conversion-functions?f1url=%3FappId%3DDev17IDEF1%26l%3DEN-US%26k%3Dk(vb.CInt)%3Bk(DevLang-VB)%26rd%3Dtrue#remarks
        ' 
        ' Fractional Parts. When you convert a nonintegral value to an integral
        ' type, the integer conversion functions (CByte, CInt, CLng, CSByte,
        ' CShort, CUInt, CULng, and CUShort) remove the fractional part and
        ' round the value to the closest integer.
        ' If the fractional part is exactly 0.5, the integer conversion
        ' functions round it to the nearest even integer. For example, 0.5
        ' rounds to 0, and 1.5 and 2.5 both round to 2. This is sometimes called
        ' banker's rounding, and its purpose is to compensate for a bias
        ' that could accumulate when adding many such numbers together.
        ' CInt and CLng differ from the Int and Fix functions, which truncate,
        ' rather than round, the fractional part of a number. Also, Fix and Int
        ' always return a value of the same data type as you pass in.

        ' Determine the mouse position within the image.
        ' Floor may actually be more precise than rounding. Nothing has been
        ' found yet to get the proper understanding of the Doubles in Point. See
        ' above regarding rounding.
        Dim PixelPoint As System.Windows.Point =
            e.GetPosition(CType(sender, System.Windows.IInputElement))
        Dim PixelX As System.Int32 = CInt(System.Math.Floor(PixelPoint.X))
        Dim PixelY As System.Int32 = CInt(System.Math.Floor(PixelPoint.Y))

        With Me

            ' Determine which color that represents. Whole the image update on the
            ' predominant color that was clicked.
            Select Case .LastRgbChange
                Case LastRgbChangeEnum.Red
                    .UnderlyingG = CByte(PixelX)
                    .UnderlyingB = CByte(255 - PixelY)
                    .LastRgbChange = If(
                        .UnderlyingG > .UnderlyingB,
                        LastRgbChangeEnum.Green,
                        LastRgbChangeEnum.Blue)
                    Me.ResetRgbLastChange()
                Case LastRgbChangeEnum.Green
                    .UnderlyingR = CByte(PixelX)
                    .UnderlyingB = CByte(255 - PixelY)
                    .LastRgbChange = If(
                        .UnderlyingR > .UnderlyingB,
                        LastRgbChangeEnum.Red,
                        LastRgbChangeEnum.Blue)
                Case Else
                    .UnderlyingR = CByte(PixelX)
                    .UnderlyingG = CByte(255 - PixelY)
                    .LastRgbChange = If(
                        .UnderlyingR > .UnderlyingG,
                        LastRgbChangeEnum.Red,
                        LastRgbChangeEnum.Green)
            End Select

            .UpdateBaseValuesFromRGB(.UnderlyingR, .UnderlyingG, .UnderlyingB,
                                     .LastRgbChange)

            ' Update the displays.
            .RgbUpdateVisuals()

        End With

    End Sub ' RgbProcessMouseClick

    Private Sub RgbFillImage()

        ' These are Int32 to allow for left shift without exceptions.
        Dim RVal, GVal, BVal As System.Int32

        ' The constructed color for one pixel.
        Dim ConstructedColor As System.Int32

        ' Construct an array with colors for the individual pixels. Column is
        ' first here because of the way that the array offsets are processed in
        ' PixelsToImage.
        Dim PixelArray(CInt(Me.RgbImage.Width) - 1,
                       CInt(Me.RgbImage.Height) - 1) As System.Int32
        For Col As Int32 = 0 To PixelArray.GetUpperBound(0)
            For Row As Int32 = 0 To PixelArray.GetUpperBound(1)

                ' Update the working component values.
                Select Case Me.LastRgbChange
                    Case LastRgbChangeEnum.Red
                        RVal = Me.RgbWorkR
                        GVal = Col
                        BVal = 255 - Row
                    Case LastRgbChangeEnum.Green
                        RVal = Col
                        GVal = Me.RgbWorkG
                        BVal = 255 - Row
                    Case Else
                        RVal = Col
                        GVal = 255 - Row
                        BVal = Me.RgbWorkB
                End Select

                ConstructedColor = HFF + (RVal << 16) + (GVal << 8) + BVal
                PixelArray(Col, Row) = ConstructedColor

            Next
        Next

        Me.RgbImage.Source =
            OSNW.Graphics.ColorUtilities.PixelsToImageSource(PixelArray)

    End Sub ' RgbFillImage

#End Region ' "RGB tab"

#Region "HSx tabs"
    ' These are routines shared by HSL and HSV.

    ''' <summary>
    ''' Identifies the mouse location within an Image and changes the cursor if
    ''' the mouse in in the desired circle.
    ''' </summary>
    Private Sub HsxSelectHueImageProcessMouseMove(
        sendingImage As System.Windows.Controls.Image, e As MouseEventArgs)

        ' FrameworkElement.Width/Height. The width/height of the element, in
        ' device-independent units (1/96th inch per unit). The default value is
        ' NaN. This value must be equal to or greater than 0.0.
        Dim ImageWidth As System.Double = sendingImage.Width
        Dim ImageHeight As System.Double = sendingImage.Height

        ' Calculate the center.
        Dim CenterW As System.Double = ImageWidth / 2.0
        Dim CenterH As System.Double = ImageHeight / 2.0

        ' Locate the mouse.
        Dim MousePoint As System.Windows.Point = e.GetPosition(sendingImage)
        Dim MouseX As System.Double = MousePoint.X
        Dim MouseY As System.Double = MousePoint.Y

        ' Calculate the mouse distance from the center point.
        ' Hypotenuse3 is reproduced here to avoid excess subroutine calls when
        ' this method is called from a loop.
        Dim MouseDeltaX As System.Double = MouseX - CenterW
        Dim MouseDeltaY As System.Double = MouseY - CenterH
        Dim MouseRadius As System.Double =
            System.Math.Sqrt(MouseDeltaX ^ 2 + MouseDeltaY ^ 2)

        ' Whole the radius on the smaller dimension of the rectangle.
        Dim ImageDiameter As System.Double =
            System.Math.Min(ImageWidth, ImageHeight)
        Dim ImageRadius As System.Double = ImageDiameter / 2.0

        ' Only react if the mouse was inside the circular area.
        If MouseRadius <= ImageRadius Then
            ' Set the cursor.
            sendingImage.Cursor = System.Windows.Input.Cursors.Cross
        Else
            ' Restore the cursor.
            If sendingImage.Equals(Me.HslSelectHueImage) Then
                Me.HslSelectHueImage.Cursor = Me.OriginalHslHueCursor
            Else
                Me.HsvSelectHueImage.Cursor = Me.OriginalHsvHueCursor
            End If
        End If

    End Sub ' HsxSelectHueImageProcessMouseMove

    '''' <summary>
    '''' Respond to a click in the specified (sendingImage) Hue image.
    '''' </summary>
    Private Sub HsxHueProcessMouseClick(
        sendingImage As System.Windows.Controls.Image,
        e As MouseButtonEventArgs)

        ' FrameworkElement.Width/Height. The width/height of the element, in
        ' device-independent units (1/96th inch per unit). The default value is
        ' NaN. This value must be equal to or greater than 0.0.
        Dim ImageWidth As System.Double = sendingImage.Width
        Dim ImageHeight As System.Double = sendingImage.Height

        '' FrameworkElement.ActualWidth/ActualHeight. Gets the rendered
        '' width/height of this element.
        '' The element's width/height, as a value in device-independent
        '' units (1/96th inch per unit).
        'Dim ImageActualWidth As System.Double = sendingHueImage.ActualWidth
        'Dim ImageActualHeight As System.Double = sendingHueImage.ActualHeight

        ' Calculate the center.
        Dim CenterW As System.Double = ImageWidth / 2.0
        Dim CenterH As System.Double = ImageHeight / 2.0

        ' Locate the mouse.
        Dim MousePoint As System.Windows.Point = e.GetPosition(sendingImage)
        Dim MouseX As System.Double = MousePoint.X
        Dim MouseY As System.Double = MousePoint.Y

        ' Calculate the mouse distance from the center point.
        ' Hypotenuse3 is reproduced here to avoid excess subroutine calls when
        ' this method is called from a loop.
        Dim MouseDeltaX As System.Double = MouseX - CenterW
        Dim MouseDeltaY As System.Double = MouseY - CenterH
        Dim MouseRadius As System.Double =
            System.Math.Sqrt(MouseDeltaX ^ 2 + MouseDeltaY ^ 2)

        ' Whole the radius on the smaller dimension of the rectangle.
        Dim ImageDiameter As System.Double =
            System.Math.Min(ImageWidth, ImageHeight)
        Dim ImageRadius As System.Double = ImageDiameter / 2.0

        ' Only react if the mouse was inside the circular area.
        If MouseRadius <= ImageRadius Then

            ' Determine the mouse pixel-position within the image.
            Dim MouseCol As System.Byte = CByte(System.Math.Floor(MouseX))
            Dim MouseRow As System.Byte = CByte(System.Math.Floor(MouseY))

            ' Determine which hue that pixel location represents. Calculate the
            ' hue as a fraction of a circle rotation.
            Dim SameWorkH As System.Double =
                OSNW.Graphics.ColorUtilities.GetHueFromPixel(MouseCol, MouseRow,
                    CInt(sendingImage.Width), CInt(sendingImage.Height),
                    OSNW.Graphics.ColorUtilities.HueScaleEnum.Fraction)
            If sendingImage.Equals(Me.HslSelectHueImage) Then
                HslWorkH = SameWorkH
            Else
                HsvWorkH = SameWorkH
            End If

        End If

        ' Create a color reflecting the selected component values.
        With Me
            If sendingImage.Equals(Me.HslSelectHueImage) Then
                .UpdateBaseValuesFromHSL(.HslWorkH, .HslWorkS, .HslWorkL)
            Else
                .UpdateBaseValuesFromHSV(.HsvWorkH, .HsvWorkS, .HsvWorkV)
            End If
        End With

        If sendingImage.Equals(Me.HslSelectHueImage) Then
            Me.HslUpdateVisuals()
        Else
            Me.HsvUpdateVisuals()
            'Else
            '
            '
            ' Do nothing?
            '
            '
        End If

    End Sub ' HsxHueProcessMouseClick

    '''' <summary>
    '''' Respond to a click in the specified (sendingImage) saturation vs.
    '''' luminance/value image.
    '''' </summary>
    '''' <param name="sendingImage">xxxxxxxxxxxxxxxxxxxxxxx</param>
    '''' <param name="e">xxxxxxxxxxxxxxxxxxxxxxx</param>
    Private Sub HsxSatProcessMouseClick(
        sendingImage As System.Windows.Controls.Image,
        e As MouseButtonEventArgs)

        ' Ref: Type Conversion Functions (Visual Basic)
        ' https://learn.microsoft.com/en-us/dotnet/visual-basic/language-reference/functions/type-conversion-functions?f1url=%3FappId%3DDev17IDEF1%26l%3DEN-US%26k%3Dk(vb.CInt)%3Bk(DevLang-VB)%26rd%3Dtrue#remarks
        ' 
        ' Fractional Parts. When you convert a nonintegral value to an integral
        ' type, the integer conversion functions (CByte, CInt, CLng, CSByte,
        ' CShort, CUInt, CULng, and CUShort) remove the fractional part and
        ' round the value to the closest integer.
        ' If the fractional part is exactly 0.5, the integer conversion
        ' functions round it to the nearest even integer. For example, 0.5
        ' rounds to 0, and 1.5 and 2.5 both round to 2. This is sometimes called
        ' banker's rounding, and its purpose is to compensate for a bias
        ' that could accumulate when adding many such numbers together.
        ' CInt and CLng differ from the Int and Fix functions, which truncate,
        ' rather than round, the fractional part of a number. Also, Fix and Int
        ' always return a value of the same data type as you pass in.

        ' FrameworkElement.Width/Height. The width/height of the element, in
        ' device-independent units (1/96th inch per unit). The default value is
        ' NaN. This value must be equal to or greater than 0.0.
        Dim ImageWidth As System.Double = sendingImage.Width
        Dim ImageHeight As System.Double = sendingImage.Height

        ' Determine the mouse position within the image.
        Dim MousePoint As System.Windows.Point = e.GetPosition(sendingImage)
        Dim MouseX As System.Double = MousePoint.X
        Dim MouseY As System.Double = MousePoint.Y

        ' Identify that as one pixel in an array of pixels.
        Dim PixelX As System.Byte = CByte(System.Math.Floor(MouseX))
        Dim PixelY As System.Byte = CByte(System.Math.Floor(MouseY))

        With Me

            ' Determine which components that represents.
            If sendingImage.Equals(Me.HslSelectSatLumImage) Then
                .HslWorkS = PixelX / ImageWidth
                .HslWorkL = (255.0 - PixelY) / ImageWidth
            Else
                .HsvWorkS = PixelX / ImageHeight
                .HsvWorkV = (255.0 - PixelY) / ImageHeight
            End If

            ' Create a color reflecting the selected component values.
            If sendingImage.Equals(Me.HslSelectSatLumImage) Then
                .UpdateBaseValuesFromHSL(.HslWorkH, .HslWorkS, .HslWorkL)
            Else
                .UpdateBaseValuesFromHSV(.HsvWorkH, .HsvWorkS, .HsvWorkV)
            End If

            ' Update the visual items unique to the associated tab; prepare for
            ' the next adjustment.
            If sendingImage.Equals(Me.HslSelectSatLumImage) Then
                .HslUpdateVisuals()
            Else
                .HsvUpdateVisuals()
            End If

        End With

    End Sub ' HsxSatProcessMouseClick

    Private Sub HsxFillImages(
        ByRef SelectHueImage As System.Windows.Controls.Image,
        ByRef SelectSatImage As System.Windows.Controls.Image)

        ' FrameworkElement.Width/Height. The width/height of the element, in
        ' device-independent units (1/96th inch per unit). The default value is
        ' NaN. This value must be equal to or greater than 0.0.
        Dim ImageWidth As System.Double = SelectHueImage.Width
        Dim ImageHeight As System.Double = SelectHueImage.Height

        ' Find the center.
        Dim CenterW As System.Double = ImageWidth / 2.0
        Dim CenterH As System.Double = ImageHeight / 2.0

        ' Whole the radius on the smaller dimension of the rectangle.
        ' Should this be based on .RenderSize vs .Width/.Height ????????
        Dim ImageDiameter As System.Double =
            System.Math.Min(ImageWidth, ImageHeight)
        Dim ImageRadius As System.Double = ImageDiameter / 2.0

        ' The constructed color for one pixel.
        Dim PixelHue As System.Double
        Dim PixelR, PixelG, PixelB As System.Double
        Dim PixelColor As System.Int32

        ' Update the hue selection image.
        ' Construct an array with colors for the individual pixels. Column is
        ' first here because of the way that the array offsets are processed in
        ' PixelsToImage.
        ' Should this be based on .RenderSize vs .Width/.Height ??????????
        Dim PixelArray(CInt(SelectHueImage.Width) - 1,
                       CInt(SelectHueImage.Height) - 1) As System.Int32
        For Col As System.Int32 = 0 To PixelArray.GetUpperBound(0)
            For Row As System.Int32 = 0 To PixelArray.GetUpperBound(1)

                ' Calculate the distance from the center point.
                ' Hypotenuse3 is reproduced here to avoid excess subroutine
                ' calls when this method is called from a loop.
                Dim PixelOffsetX As System.Double = Col - CenterW
                Dim PixelOffsetY As System.Double = Row - CenterH
                Dim PixelOffset As System.Double =
                    System.Math.Sqrt(PixelOffsetX ^ 2 + PixelOffsetY ^ 2)

                If PixelOffset <= ImageRadius Then
                    ' Construct and apply a color for one pixel.
                    PixelHue = OSNW.Graphics.ColorUtilities.GetHueFromPixel(Col,
                        Row, CInt(ImageWidth), CInt(ImageHeight),
                        OSNW.Graphics.ColorUtilities.HueScaleEnum.Fraction)
                    If SelectHueImage.Equals(Me.HslSelectHueImage) Then
                        OSNW.Graphics.ColorUtilities.HSLtoRGB(PixelHue,
                            Me.HslWorkS, Me.HslWorkL, PixelR, PixelG, PixelB)
                    Else
                        OSNW.Graphics.ColorUtilities.HSVtoRGB(PixelHue,
                            Me.HsvWorkS, Me.HsvWorkV, PixelR, PixelG, PixelB)
                    End If
                    PixelColor = HFF + (CInt(PixelR) << 16) +
                        (CInt(PixelG) << 8) + CInt(PixelB)
                    PixelArray(Col, Row) = PixelColor
                Else
                    ' Make the pixel transparent.
                    PixelColor = &H0
                    PixelArray(Col, Row) = PixelColor
                End If

            Next
        Next
        SelectHueImage.Source =
            OSNW.Graphics.ColorUtilities.PixelsToImageSource(PixelArray)

        ' Update the sat/lum or sat/val selection image.
        ' Construct an array with colors for the individual pixels. Column is
        ' first here because of the way that the array offsets are processed in
        ' PixelsToImage.
        ReDim PixelArray(CInt(SelectSatImage.Width) - 1,
                         CInt(SelectSatImage.Height) - 1)
        For Col As System.Int32 = 0 To PixelArray.GetUpperBound(0)
            For Row As System.Int32 = 0 To PixelArray.GetUpperBound(1)
                ' Construct and apply a color for one pixel.
                If SelectSatImage.Equals(Me.HslSelectSatLumImage) Then
                    OSNW.Graphics.ColorUtilities.HSLtoRGB(HslWorkH, Col / 255.0,
                        (255.0 - Row) / 255.0, PixelR, PixelG, PixelB)
                Else
                    OSNW.Graphics.ColorUtilities.HSVtoRGB(HsvWorkH, Col / 255.0,
                        (255.0 - Row) / 255.0, PixelR, PixelG, PixelB)
                End If
                PixelColor = HFF + (CInt(PixelR) << 16) +
                    (CInt(PixelG) << 8) + CInt(PixelB)
                PixelArray(Col, Row) = PixelColor
            Next
        Next

        If SelectSatImage.Equals(Me.HslSelectSatLumImage) Then
            Me.HslSelectSatLumImage.Source =
                OSNW.Graphics.ColorUtilities.PixelsToImageSource(PixelArray)
        Else
            Me.HsvSelectSatValImage.Source =
                OSNW.Graphics.ColorUtilities.PixelsToImageSource(PixelArray)
        End If

    End Sub ' HsxFillImages

#End Region ' "HSx tabs"

#Region "HSL tab"

    '''' <summary>
    '''' Update the visual items unique to this tab; prepare for the next
    '''' adjustment.
    '''' </summary>
    Private Sub HslUpdateVisuals()

        ' Update the displayed per-component values.
        With Me
            .HslTweakHueLabel.Content = .DoubleValueStr("Hue", .HslWorkH)
            .HslTweakSaturationLabel.Content =
                .DoubleValueStr("Saturation", .HslWorkS)
            .HslTweakLuminanceLabel.Content =
                .DoubleValueStr("Luminance", .HslWorkL)
        End With

        Me.HsxFillImages(HslSelectHueImage, HslSelectSatLumImage)

    End Sub ' HslUpdateVisuals

#End Region ' "HSL tab"

#Region "HSV tab"

    '''' <summary>
    '''' Update the visual items unique to this tab; prepare for the next
    '''' adjustment.
    '''' </summary>
    Private Sub HsvUpdateVisuals()

        ' Update the displayed per-component values.
        With Me
            .HsvTweakHueLabel.Content = .DoubleValueStr("Hue", HsvWorkH)
            .HsvTweakSaturationLabel.Content = .DoubleValueStr("Saturation",
                                                               HsvWorkS)
            .HsvTweakValueLabel.Content = .DoubleValueStr("Value", HsvWorkV)
        End With

        ' Update the square.
        Me.HsxFillImages(HsvSelectHueImage, HsvSelectSatValImage)

    End Sub ' HsvUpdateVisuals

#End Region ' "HSV tab"

#Region "Shade tab"

    Private Sub ShadeFillImage()

        ' Determine the pure hue to be used.
        Dim PureHue, DummyS, DummyV As System.Double
        OSNW.Graphics.ColorUtilities.RGBtoHSV(Me.ShadeStartR, Me.ShadeStartG,
            Me.ShadeStartB, PureHue, DummyS, DummyV)

        ' Determine the pure color to be used.
        Dim PureR, PureG, PureB As System.Double
        OSNW.Graphics.ColorUtilities.HSVtoRGB(
            PureHue, 1.0, 1.0, PureR, PureG, PureB)

        ' The shade factor for one column.
        Dim ColShadeFactor As System.Double

        ' Doubles for GetShade results.
        Dim RValD, GValD, BValD As System.Double

        ' These are Int32 to allow for left shift without exceptions.
        Dim RVal, GVal, BVal As System.Int32

        ' The constructed color for one column.
        Dim ConstructedColor As System.Int32

        ' Construct an array with colors for the individual pixels. Column is
        ' first here because of the way that the array offsets are processed in
        ' PixelsToImage.
        Dim PixelArray(CInt(Me.ShadeImage.Width - 1),
                       CInt(Me.ShadeImage.Height - 1)) As System.Int32
        Dim MaxCol As System.Int32 = PixelArray.GetUpperBound(0)
        Dim MaxRow As System.Int32 = PixelArray.GetUpperBound(1)

        ' Populate the array.
        For Col As Int32 = 0 To MaxCol

            ' Get the shade for this column.
            ColShadeFactor = Col / CDbl(MaxCol)
            OSNW.Graphics.ColorUtilities.GetShade(
                PureR, PureG, PureB, ColShadeFactor, RValD, GValD, BValD)

            RVal = CInt(RValD)
            GVal = CInt(GValD)
            BVal = CInt(BValD)
            ConstructedColor = HFF + (RVal << 16) + (GVal << 8) + BVal

            For Row As Int32 = 0 To MaxRow
                PixelArray(Col, Row) = ConstructedColor
            Next

        Next

        Me.ShadeImage.Source =
            OSNW.Graphics.ColorUtilities.PixelsToImageSource(PixelArray)

    End Sub ' ShadeFillImage

    '''' <summary>
    '''' Update the visual items unique to this tab; prepare for the next
    '''' adjustment.
    '''' </summary>
    Private Sub ShadeUpdateVisuals()
        With Me

            ' Update the displayed per-component values.
            .ShadeTweakRedLabel.Content = .DoubleValueStr(REDWORD, .ShadeStartR)
            .ShadeTweakGreenLabel.Content = .DoubleValueStr(GREENWORD, .ShadeStartG)
            .ShadeTweakBlueLabel.Content = .DoubleValueStr(BLUEWORD, .ShadeStartB)
            .ShadeTweakHueLabel.Content = .DoubleValueStr(HUEWORD, .ShadeStartH)
            .ShadeTweakFactorLabel.Content = .DoubleValueStr("Factor",
                                                             .ShadeWorkFactor)

            ' Limit visibility until a selection has been made.
            If .ShadeFactorClicked Then
                .ShadeTweakFactorLabel.Visibility =
                    System.Windows.Visibility.Visible
                .ShadeMM.IsEnabled = True
                .ShadeM.IsEnabled = True
                .ShadeP.IsEnabled = True
                .ShadePP.IsEnabled = True
            Else
                .ShadeTweakFactorLabel.Visibility =
                    System.Windows.Visibility.Collapsed
                .ShadeMM.IsEnabled = False
                .ShadeM.IsEnabled = False
                .ShadeP.IsEnabled = False
                .ShadePP.IsEnabled = False
            End If

            ' Update the image.
            Me.ShadeFillImage()

        End With
    End Sub ' ShadeUpdateVisuals

    '''' <summary>
    '''' Respond to  click in the image.
    '''' </summary>
    Private Sub ShadeProcessMouseClick(
        sender As Object, e As MouseButtonEventArgs)

        ' Ref: Type Conversion Functions (Visual Basic)
        ' https://learn.microsoft.com/en-us/dotnet/visual-basic/language-reference/functions/type-conversion-functions?f1url=%3FappId%3DDev17IDEF1%26l%3DEN-US%26k%3Dk(vb.CInt)%3Bk(DevLang-VB)%26rd%3Dtrue#remarks
        ' 
        ' Fractional Parts. When you convert a nonintegral value to an integral
        ' type, the integer conversion functions (CByte, CInt, CLng, CSByte,
        ' CShort, CUInt, CULng, and CUShort) remove the fractional part and
        ' round the value to the closest integer.
        ' If the fractional part is exactly 0.5, the integer conversion
        ' functions round it to the nearest even integer. For example, 0.5
        ' rounds to 0, and 1.5 and 2.5 both round to 2. This is sometimes called
        ' banker's rounding, and its purpose is to compensate for a bias
        ' that could accumulate when adding many such numbers together.
        ' CInt and CLng differ from the Int and Fix functions, which truncate,
        ' rather than round, the fractional part of a number. Also, Fix and Int
        ' always return a value of the same data type as you pass in.

        ' Determine the mouse position within the image.
        ' Floor may actually be more precise than rounding. Nothing has been
        ' found yet to get the proper understanding of the Doubles in Point. See
        ' above regarding rounding.
        Dim PixelPoint As System.Windows.Point =
            e.GetPosition(CType(sender, System.Windows.IInputElement))
        Dim PixelX As System.Int32 = CInt(System.Math.Floor(PixelPoint.X))

        With Me

            Dim ClickedFactor As System.Double = PixelX / .ShadeImage.Width
            .UpdateBaseValuesFromShade(CInt(ClickedFactor * 1000.0))

            ' Update the displays.
            .ShadeFactorClicked = True
            .ShadeUpdateVisuals()

        End With

    End Sub ' ShadeProcessMouseClick

    Private Sub ShadeDeriveBase(ByVal shadeFactor As System.Int32)
        With Me

            ' Calculate the pure color components for the hue.
            Dim PureR, PureG, PureB As System.Double
            OSNW.Graphics.ColorUtilities.HSVtoRGB(
                .ShadeStartH, 1.0, 1.0, PureR, PureG, PureB)

            ' Calculate the shaded components.
            Dim ShadedR, ShadedG, ShadedB As System.Double
            OSNW.Graphics.ColorUtilities.GetShade(PureR, PureG, PureB,
                CDbl(shadeFactor) / 1000.0, ShadedR, ShadedG, ShadedB)

            ' Push updates.
            .UpdateBaseValuesFromRGB(ShadedR, ShadedG, ShadedB)

        End With
    End Sub ' ShadeDeriveBase

#End Region ' "Shade tab"

#Region "Tint tab"

    Private Sub TintFillImage()

        ' Determine the pure hue to be used.
        Dim PureHue, DummyS, DummyV As System.Double
        OSNW.Graphics.ColorUtilities.RGBtoHSV(Me.TintStartR, Me.TintStartG,
            Me.TintStartB, PureHue, DummyS, DummyV)

        ' Determine the pure color to be used.
        Dim PureR, PureG, PureB As System.Double
        OSNW.Graphics.ColorUtilities.HSVtoRGB(
            PureHue, 1.0, 1.0, PureR, PureG, PureB)

        ' The tint factor for one column.
        Dim ColTintFactor As System.Double

        ' Doubles for GetTint results.
        Dim RValD, GValD, BValD As System.Double

        ' These are Int32 to allow for left shift without exceptions.
        Dim RVal, GVal, BVal As System.Int32

        ' The constructed color for one column.
        Dim ConstructedColor As System.Int32

        ' Construct an array with colors for the individual pixels. Column is
        ' first here because of the way that the array offsets are processed in
        ' PixelsToImage.
        Dim PixelArray(CInt(Me.TintImage.Width) - 1,
                       CInt(Me.TintImage.Height) - 1) As System.Int32
        Dim MaxCol As System.Int32 = PixelArray.GetUpperBound(0)
        Dim MaxRow As System.Int32 = PixelArray.GetUpperBound(1)

        ' Populate the array.
        For Col As Int32 = 0 To MaxCol

            ' Get the tint for this column.
            ColTintFactor = Col / CDbl(MaxCol)
            OSNW.Graphics.ColorUtilities.GetTint(PureR, PureG, PureB, ColTintFactor,
                                             RValD, GValD, BValD)

            RVal = CInt(RValD)
            GVal = CInt(GValD)
            BVal = CInt(BValD)
            ConstructedColor = HFF + (RVal << 16) + (GVal << 8) + BVal

            For Row As Int32 = 0 To MaxRow
                PixelArray(Col, Row) = ConstructedColor
            Next

        Next

        Me.TintImage.Source =
            OSNW.Graphics.ColorUtilities.PixelsToImageSource(PixelArray)

    End Sub ' TintFillImage

    '''' <summary>
    '''' Update the visual items unique to this tab; prepare for the next
    '''' adjustment.
    '''' </summary>
    Private Sub TintUpdateVisuals()
        With Me

            ' Update the displayed per-component values.
            .TintTweakRedLabel.Content = .DoubleValueStr(REDWORD, .TintStartR)
            .TintTweakGreenLabel.Content = .DoubleValueStr(GREENWORD, .TintStartG)
            .TintTweakBlueLabel.Content = .DoubleValueStr(BLUEWORD, .TintStartB)
            .TintTweakHueLabel.Content = .DoubleValueStr(HUEWORD, .TintStartH)
            .TintTweakFactorLabel.Content = .DoubleValueStr(
                "Factor", .TintWorkFactor)

            ' Limit visibility until a selection has been made.
            If .TintFactorClicked Then
                .TintTweakFactorLabel.Visibility =
                    System.Windows.Visibility.Visible
                .TintMM.IsEnabled = True
                .TintM.IsEnabled = True
                .TintP.IsEnabled = True
                .TintPP.IsEnabled = True
            Else
                .TintTweakFactorLabel.Visibility =
                    System.Windows.Visibility.Collapsed
                .TintMM.IsEnabled = False
                .TintM.IsEnabled = False
                .TintP.IsEnabled = False
                .TintPP.IsEnabled = False
            End If

            ' Update the image.
            Me.TintFillImage()

        End With
    End Sub ' TintUpdateVisuals

    '''' <summary>
    '''' Respond to  click in the image.
    '''' </summary>
    Private Sub TintProcessMouseClick(
        sender As Object, e As MouseButtonEventArgs)

        ' Ref: Type Conversion Functions (Visual Basic)
        ' https://learn.microsoft.com/en-us/dotnet/visual-basic/language-reference/functions/type-conversion-functions?f1url=%3FappId%3DDev17IDEF1%26l%3DEN-US%26k%3Dk(vb.CInt)%3Bk(DevLang-VB)%26rd%3Dtrue#remarks
        ' 
        ' Fractional Parts. When you convert a nonintegral value to an integral
        ' type, the integer conversion functions (CByte, CInt, CLng, CSByte,
        ' CShort, CUInt, CULng, and CUShort) remove the fractional part and
        ' round the value to the closest integer.
        ' If the fractional part is exactly 0.5, the integer conversion
        ' functions round it to the nearest even integer. For example, 0.5
        ' rounds to 0, and 1.5 and 2.5 both round to 2. This is sometimes called
        ' banker's rounding, and its purpose is to compensate for a bias
        ' that could accumulate when adding many such numbers together.
        ' CInt and CLng differ from the Int and Fix functions, which truncate,
        ' rather than round, the fractional part of a number. Also, Fix and Int
        ' always return a value of the same data type as you pass in.

        ' Determine the mouse position within the image.
        ' Floor may actually be more precise than rounding. Nothing has been
        ' found yet to get the proper understanding of the Doubles in Point. See
        ' above regarding rounding.
        Dim PixelPoint As System.Windows.Point =
            e.GetPosition(CType(sender, System.Windows.IInputElement))
        Dim PixelX As System.Int32 = CInt(System.Math.Floor(PixelPoint.X))

        With Me

            Dim ClickedFactor As System.Double = PixelX / .TintImage.Width
            .UpdateBaseValuesFromTint(CInt(ClickedFactor * 1000.0))

            ' Update the displays.
            .TintFactorClicked = True
            .TintUpdateVisuals()

        End With

    End Sub ' TintProcessMouseClick

    Private Sub TintDeriveBase(ByVal tintFactor As System.Int32)
        With Me

            ' Calculate the pure color components for the hue.
            Dim PureR, PureG, PureB As System.Double
            OSNW.Graphics.ColorUtilities.HSVtoRGB(
                .TintStartH, 1.0, 1.0, PureR, PureG, PureB)

            ' Calculate the tinted components.
            Dim TintedR, TintedG, TintedB As System.Double
            OSNW.Graphics.ColorUtilities.GetTint(PureR, PureG, PureB,
                CDbl(tintFactor) / 1000.0, TintedR, TintedG, TintedB)

            ' Push updates.
            .UpdateBaseValuesFromRGB(TintedR, TintedG, TintedB)

        End With
    End Sub ' TintDeriveBase

#End Region ' "Tint tab"

#Region "Tone tab"

    Private Sub ToneFillImage()

        ' Determine the pure hue to be used.
        Dim PureHue, DummyS, DummyV As System.Double
        OSNW.Graphics.ColorUtilities.RGBtoHSV(Me.ToneStartR, Me.ToneStartG,
            Me.ToneStartB, PureHue, DummyS, DummyV)

        ' Determine the pure color to be used.
        Dim PureR, PureG, PureB As System.Double
        OSNW.Graphics.ColorUtilities.HSVtoRGB(
            PureHue, 1.0, 1.0, PureR, PureG, PureB)

        ' The tone factor for a row.
        Dim ToneFactor As System.Double

        ' Doubles for GetTone results.
        Dim RValD, GValD, BValD As System.Double

        ' These are Int32 to allow for left shift without exceptions.
        Dim RVal, GVal, BVal As System.Int32

        ' The constructed color for one pixel.
        Dim ConstructedColor As System.Int32

        ' Construct an array for colors of the individual pixels.
        Dim PixelArray(CInt(Me.ToneImage.Width) - 1,
                           CInt(Me.ToneImage.Height) - 1) As System.Int32
        Dim MaxCol As System.Int32 = PixelArray.GetUpperBound(0)
        Dim MaxRow As System.Int32 = PixelArray.GetUpperBound(1)

        ' Populate the array.
        For Row As Int32 = 0 To MaxRow
            ToneFactor = 1.0 - (Row / MaxRow)
            For Col As Int32 = 0 To MaxCol

                OSNW.Graphics.ColorUtilities.GetTone(PureR, PureG, PureB, Col,
                    ToneFactor, RValD, GValD, BValD)

                RVal = CInt(RValD)
                GVal = CInt(GValD)
                BVal = CInt(BValD)
                ConstructedColor = HFF + (RVal << 16) + (GVal << 8) + BVal

                PixelArray(Col, Row) = ConstructedColor

            Next
        Next

        Me.ToneImage.Source =
            OSNW.Graphics.ColorUtilities.PixelsToImageSource(PixelArray)

    End Sub ' ToneFillImage

    '''' <summary>
    '''' Update the visual items unique to this tab; prepare for the next
    '''' adjustment.
    '''' </summary>
    Private Sub ToneUpdateVisuals()
        With Me

            ' Update the displayed per-component values for the base reference.
            .ToneBaseRedLabel.Content = .ByteValueStr(REDWORD, .ToneStartR)
            .ToneBaseGreenLabel.Content = .ByteValueStr(GREENWORD, .ToneStartG)
            .ToneBaseBlueLabel.Content = .ByteValueStr(BLUEWORD, .ToneStartB)
            .ToneTweakHueLabel.Content = .DoubleValueStr(HUEWORD, .ToneStartH)

            ' Limit visibility until a selection has been made.
            If .ToneValuesClicked Then
                .ToneTweakFactorLabel.Content =
                    .DoubleValueStr("Tone Factor", .ToneWorkFactor / 1000.0)
                .ToneTweakGrayLabel.Content = .ByteValueStr("Gray Level",
                                                            .ToneWorkGray)
                .ToneGrayMM.IsEnabled = True
                .ToneGrayM.IsEnabled = True
                .ToneGrayP.IsEnabled = True
                .ToneGrayPP.IsEnabled = True
                .ToneFactorMM.IsEnabled = True
                .ToneFactorM.IsEnabled = True
                .ToneFactorP.IsEnabled = True
                .ToneFactorPP.IsEnabled = True
            Else
                .ToneTweakFactorLabel.Content = "Tone Factor not set"
                .ToneTweakGrayLabel.Content = "Gray Level not set"
                .ToneGrayMM.IsEnabled = False
                .ToneGrayM.IsEnabled = False
                .ToneGrayP.IsEnabled = False
                .ToneGrayPP.IsEnabled = False
                .ToneFactorMM.IsEnabled = False
                .ToneFactorM.IsEnabled = False
                .ToneFactorP.IsEnabled = False
                .ToneFactorPP.IsEnabled = False
            End If

            ' Update the image.
            .ToneFillImage()

        End With
    End Sub ' ToneUpdateVisuals

    '''' <summary>
    '''' Respond to  click in the image.
    '''' </summary>
    Private Sub ToneProcessMouseClick(
        sender As Object, e As MouseButtonEventArgs)

        ' Ref: Type Conversion Functions (Visual Basic)
        ' https://learn.microsoft.com/en-us/dotnet/visual-basic/language-reference/functions/type-conversion-functions?f1url=%3FappId%3DDev17IDEF1%26l%3DEN-US%26k%3Dk(vb.CInt)%3Bk(DevLang-VB)%26rd%3Dtrue#remarks
        ' 
        ' Fractional Parts. When you convert a nonintegral value to an integral
        ' type, the integer conversion functions (CByte, CInt, CLng, CSByte,
        ' CShort, CUInt, CULng, and CUShort) remove the fractional part and
        ' round the value to the closest integer.
        ' If the fractional part is exactly 0.5, the integer conversion
        ' functions round it to the nearest even integer. For example, 0.5
        ' rounds to 0, and 1.5 and 2.5 both round to 2. This is sometimes called
        ' banker's rounding, and its purpose is to compensate for a bias
        ' that could accumulate when adding many such numbers together.
        ' CInt and CLng differ from the Int and Fix functions, which truncate,
        ' rather than round, the fractional part of a number. Also, Fix and Int
        ' always return a value of the same data type as you pass in.

        ' Determine the mouse position within the image.
        ' Floor may actually be more precise than rounding. Nothing has been
        ' found yet to get the proper understanding of the Doubles in Point. See
        ' above regarding rounding.
        Dim PixelPoint As System.Windows.Point =
            e.GetPosition(CType(sender, System.Windows.IInputElement))
        Dim PixelX As System.Int32 = CInt(System.Math.Floor(PixelPoint.X))
        Dim PixelY As System.Int32 = CInt(System.Math.Floor(PixelPoint.Y))

        With Me

            Dim ClickedFactor As System.Double = 1.0 - (PixelY / (.ToneImage.Height - 1.0))
            .UpdateBaseValuesFromTone(CByte(PixelX), CInt(ClickedFactor * 1000.0))

            ' Update the displays.
            .ToneValuesClicked = True
            .ToneUpdateVisuals()

        End With

    End Sub ' ToneProcessMouseClick

    Private Sub ToneDeriveBase(ByVal grayVal As System.Byte,
                               ByVal toneFactor As System.Int32)
        With Me

            ' Calculate the pure color components for the hue.
            Dim PureR, PureG, PureB As System.Double
            OSNW.Graphics.ColorUtilities.HSVtoRGB(
                .ToneStartH, 1.0, 1.0, PureR, PureG, PureB)

            ' Calculate the toned components.
            Dim TonedR, TonedG, TonedB As System.Double
            OSNW.Graphics.ColorUtilities.GetTone(PureR, PureG, PureB,
                grayVal, CDbl(toneFactor) / 1000.0, TonedR, TonedG, TonedB)

            ' Push updates.
            .UpdateBaseValuesFromRGB(TonedR, TonedG, TonedB)

        End With
    End Sub ' ToneDeriveBase

#End Region ' "Tone tab"

#Region "Blend tab"

    Private Sub BlendValidateRgb(
        ByVal oneTextBox As System.Windows.Controls.TextBox,
        ByRef byteVal As System.Byte, ByRef allValid As System.Boolean)

        ' Avoid processing for items not yet created.
        If IsNothing(oneTextBox) Then
            Exit Sub ' Early exit.
        End If

        If System.Byte.TryParse(oneTextBox.Text, byteVal) Then
            ' Any byte is valid.
            oneTextBox.Background = GoodBackgroundBrush
        Else
            oneTextBox.Background = BadBackgroundBrush
            allValid = False
        End If

    End Sub ' BlendValidateRgb

    Private Sub BlendValidateRatio(
        ByVal oneTextBox As System.Windows.Controls.TextBox,
        ByRef doubleVal As System.Double, ByRef allValid As System.Boolean)

        ' Avoid processing for items not yet created.
        If IsNothing(oneTextBox) Then
            Exit Sub ' Early exit.
        End If

        If Not System.Double.TryParse(oneTextBox.Text, doubleVal) Then
            oneTextBox.Background = BadBackgroundBrush
            allValid = False
        ElseIf doubleVal < 0.0 Then
            oneTextBox.Background = BadBackgroundBrush
            allValid = False
        Else
            oneTextBox.Background = GoodBackgroundBrush
        End If

    End Sub ' BlendValidateRatio

    Private Sub BlendSetRgbWorkColors(ByVal r As System.Double,
        ByVal g As System.Double, ByVal b As System.Double)

        With Me
            .UpdateBaseValuesFromRGB(r, g, b)
            OSNW.Graphics.ColorUtilities.RGBtoHSL(.UnderlyingR, .UnderlyingG,
                .UnderlyingB, .HslWorkH, .HslWorkS, .HslWorkL)
            OSNW.Graphics.ColorUtilities.RGBtoHSV(.UnderlyingR, .UnderlyingG,
                .UnderlyingB, .HsvWorkH, .HsvWorkS, .HsvWorkV)
        End With
    End Sub ' BlendSetRgbWorkColors

#End Region ' "Blend tab"




End Class ' ColorDlgWindow
