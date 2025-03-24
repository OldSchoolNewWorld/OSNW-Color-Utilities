﻿Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System.Windows
Imports System.Windows.Input

Partial Friend Class ColorDlgWindow

#Region "Event Utilities"

    Private Sub InitializeTabVisibility()
        With Me
            .ConvertTabItem.Visibility =
                If(.ShowConvertTab, Visibility.Visible, Visibility.Collapsed)
            .DefinedTabItem.Visibility =
                If(.ShowDefinedTab, Visibility.Visible, Visibility.Collapsed)
            .RgbTabItem.Visibility =
                If(.ShowRgbTab, Visibility.Visible, Visibility.Collapsed)
            .HslTabItem.Visibility =
                If(.ShowHslTab, Visibility.Visible, Visibility.Collapsed)
            .HsvTabItem.Visibility =
                If(.ShowHsvTab, Visibility.Visible, Visibility.Collapsed)
            .ShadeTabItem.Visibility =
                If(.ShowShadeTab, Visibility.Visible, Visibility.Collapsed)
            .TintTabItem.Visibility =
                If(.ShowTintTab, Visibility.Visible, Visibility.Collapsed)
            .ToneTabItem.Visibility =
                If(.ShowToneTab, Visibility.Visible, Visibility.Collapsed)
            .BlendTabItem.Visibility =
                If(.ShowBlendTab, Visibility.Visible, Visibility.Collapsed)
        End With
    End Sub ' InitializeTabVisibility   

    Private Sub LoadToolTips()
        With Me

            ' Constants to ensure consistent appearance.
            Const LARGEDECREASE As System.String = "Click for a large decrease in "
            Const SMALLDECREASE As System.String = "Click for a small decrease in "
            Const SMALLINCREASE As System.String = "Click for a small increase in "
            Const LARGEINCREASE As System.String = "Click for a large increase in "
            Const ENTER255 As System.String = "Enter the component value (0-255)"
            Const ENTER9999 As System.String = "Enter the component value (0.0-.9999)"
            Const ENTER10 As System.String = "Enter the component value (0.0-1.0)"
            Const THISCOMPONENT As System.String = "this component"
            Const SELECTINDICATED As System.String = "Click to select the indicated "

            .CancelButton.ToolTip = "Cancel changes and close the dialog"
            .OkButton.ToolTip = "Accept status and close the dialog"
            .RememberButton.ToolTip = "Save the current color for recall"
            .RestoreButton.ToolTip = "Restore the saved color"

            .ConvertTabTextBlock.ToolTip = "Perform conversions using entered component values"
            .ConvertRgbRedTextBox.ToolTip = ENTER255
            .ConvertRgbGreenTextBox.ToolTip = ENTER255
            .ConvertRgbBlueTextBox.ToolTip = ENTER255
            .ConvertHslHueTextBox.ToolTip = ENTER9999
            .ConvertHslSaturationTextBox.ToolTip = ENTER10
            .ConvertHslLuminanceTextBox.ToolTip = ENTER10
            .ConvertHsvHueTextBox.ToolTip = ENTER9999
            .ConvertHsvSaturationTextBox.ToolTip = ENTER10
            .ConvertHsvValueTextBox.ToolTip = ENTER10

            .DefinedTabTextBlock.ToolTip = "Select a predefined color"
            .DefinedComboBox.ToolTip = "Click to select from a list"

            .RgbTabTextBlock.ToolTip = "Visually select a color from red, green, and blue components"
            .RgbImage.ToolTip = $"{SELECTINDICATED}color"
            .RedMM.ToolTip = $"{LARGEDECREASE} {THISCOMPONENT}"
            .RedM.ToolTip = $"{SMALLDECREASE} {THISCOMPONENT}"
            .RedP.ToolTip = $"{SMALLINCREASE} {THISCOMPONENT}"
            .RedPP.ToolTip = $"{LARGEINCREASE} {THISCOMPONENT}"
            .GreenMM.ToolTip = $"{LARGEDECREASE} {THISCOMPONENT}"
            .GreenM.ToolTip = $"{SMALLDECREASE} {THISCOMPONENT}"
            .GreenP.ToolTip = $"{SMALLINCREASE} {THISCOMPONENT}"
            .GreenPP.ToolTip = $"{LARGEINCREASE} {THISCOMPONENT}"
            .BlueMM.ToolTip = $"{LARGEDECREASE} {THISCOMPONENT}"
            .BlueM.ToolTip = $"{SMALLDECREASE} {THISCOMPONENT}"
            .BlueP.ToolTip = $"{SMALLINCREASE} {THISCOMPONENT}"
            .BluePP.ToolTip = $"{LARGEINCREASE} {THISCOMPONENT}"

            .HslTabTextBlock.ToolTip = "Visually select a color from hue, saturation, and luminance components"
            .HslSelectHueImage.ToolTip = $"{SELECTINDICATED}hue"
            .HslSelectSatLumImage.ToolTip = $"{SELECTINDICATED}saturation/luminance combination"

            .HsvTabTextBlock.ToolTip = "Visually select a color from hue, saturation, and value components"
            .HsvSelectHueImage.ToolTip = $"{SELECTINDICATED}hue"
            .HsvSelectSatValImage.ToolTip = $"{SELECTINDICATED}saturation/value combination"

            .ShadeTabTextBlock.ToolTip = "Visually select a shade of a base color"
            .ShadeImage.ToolTip = $"{SELECTINDICATED}shade"
            .ShadeMM.ToolTip = $"{LARGEDECREASE}shading"
            .ShadeM.ToolTip = $"{SMALLDECREASE}shading"
            .ShadeP.ToolTip = $"{SMALLINCREASE}shading"
            .ShadePP.ToolTip = $"{LARGEINCREASE}shading"

            .TintTabTextBlock.ToolTip = "Visually select a tint of a base color"
            .TintImage.ToolTip = $"{SELECTINDICATED}tint"
            .TintMM.ToolTip = $"{LARGEDECREASE}tinting"
            .TintM.ToolTip = $"{SMALLDECREASE}tinting"
            .TintP.ToolTip = $"{SMALLINCREASE}tinting"
            .TintPP.ToolTip = $"{LARGEINCREASE}tinting"

            .ToneTabTextBlock.ToolTip = "Visually select a tone of a base color"
            .ToneImage.ToolTip = $"{SELECTINDICATED}gray and toning values"
            .ToneGrayMM.ToolTip = $"{LARGEDECREASE}gray value"
            .ToneGrayM.ToolTip = $"{SMALLDECREASE}gray value"
            .ToneGrayP.ToolTip = $"{SMALLINCREASE}gray value"
            .ToneGrayPP.ToolTip = $"{LARGEINCREASE}gray value"
            .ToneFactorMM.ToolTip = $"{LARGEDECREASE}toning"
            .ToneFactorM.ToolTip = $"{SMALLDECREASE}toning"
            .ToneFactorP.ToolTip = $"{SMALLINCREASE}toning"
            .ToneFactorPP.ToolTip = $"{LARGEINCREASE}toning"

            .BlendTabTextBlock.ToolTip = "Create a blend of two colors"
            .BlendRgb1RedTextBox.ToolTip = ENTER255
            .BlendRgb1GreenTextBox.ToolTip = ENTER255
            .BlendRgb1BlueTextBox.ToolTip = ENTER255
            .BlendRgb1RatioTextBox.ToolTip = "Enter the blend ratio of this color"
            .BlendRgb2RedTextBox.ToolTip = ENTER255
            .BlendRgb2GreenTextBox.ToolTip = ENTER255
            .BlendRgb2BlueTextBox.ToolTip = ENTER255
            .BlendRgb2RatioTextBox.ToolTip = "Enter the blend ratio of this color"

        End With
    End Sub ' LoadToolTips

#End Region ' "Event Utilities"

#Region "Model Event Responses"

    Private Sub Do_Window_Initialized(sender As Object, e As EventArgs)
        With Me

            ' Initialize the control data.
            .ConvertTabPushing = False
            .ShadeFactorClicked = False
            .TintFactorClicked = False
            .ToneValuesClicked = False

            ' Not being used at this time.
            '            ' Signal that Window_Initialized already happened.
            '            .WindowInitialized = True

            ' Initialize the color data.

            .UnderlyingR = INITIALR
            .UnderlyingG = INITIALG
            .UnderlyingB = INITIALB
            .UpdateBaseValuesFromRGB(.UnderlyingR, .UnderlyingG, .UnderlyingB)

            .RgbWorkR = INITIALR ' DONE ABOVE ???????????????????????
            .RgbWorkG = INITIALG ' DONE ABOVE ???????????????????????
            .RgbWorkB = INITIALB ' DONE ABOVE ???????????????????????

        End With
    End Sub ' Do_Window_Initialized

    Private Sub Do_Window_Loaded(sender As Object, e As RoutedEventArgs)

        '''''''''''Try

        With Me

            ' Apply the incoming color component set.
            Me.UnderlyingR = Red
            Me.UnderlyingG = Green
            Me.UnderlyingB = Blue
            Me.RgbWorkR = Red
            Me.RgbWorkG = Green
            Me.RgbWorkB = Blue

            .InitializeTabVisibility()

            ' Replaced by reference to Me.IsLoaded.
            '            ' Signal that Window_Loaded already happened.
            '            .WindowLoaded = True

            .LoadToolTips()

            ' Initialize the control data.
            .RestoreButton.IsEnabled = False

            ' Create an original background color. Establish the bad text color.
            .GoodBackgroundBrush = .ConvertRgbRedTextBox.Background
            Dim BadBackgroundColor As New System.Windows.Media.Color With {
                .A = &HFF, .R = BADTEXTR, .G = BADTEXTG, .B = BADTEXTB}
            .BadBackgroundBrush =
                New System.Windows.Media.SolidColorBrush(BadBackgroundColor)

            ' Record the original cursors that will change when needed.
            .OriginalHslHueCursor = .HslSelectHueImage.Cursor
            .OriginalHsvHueCursor = .HsvSelectHueImage.Cursor

            .DefinedInitComboBox()





            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            ' STEP THROUGH AND SELECT THE FIRST ONE THAT IS VISIBLE.

            ' Control which tab shows at startup.
            .ConvertTabItem.Focus()
            '.DefinedTabItem.Focus()
            '.RgbTabItem.Focus()
            '.HslTabItem.Focus()
            '.HsvTabItem.Focus()
            '.ShadeTabItem.Focus()
            '.TintTabItem.Focus()
            '.ToneTabItem.Focus()
            '.BlendTabItem.Focus()

        End With

        ' Update visual items based on the incoming state.
        With Me

            ' DEV: The specific code here is unique to the sample dialog. The
            ' underlying reason for the Sub may be of use in certain cases.

            '' Suppress having Red changed when SliderR moves to match Red.
            '.SettingSliders = True
            'Try
            '    .SliderR.Value = .Red
            '    .SliderG.Value = .Green
            '    .SliderB.Value = .Blue
            'Finally
            '    ' Restore normal slider response.
            '    .SettingSliders = False
            'End Try

            .UpdateVisuals()

        End With




        '''''''''''Catch CaughtEx As System.Exception
        '''''''''''    ' Report the unexpected exception.
        '''''''''''    Dim CaughtBy As System.Reflection.MethodBase =
        '''''''''''        System.Reflection.MethodBase.GetCurrentMethod()
        '''''''''''    Me.ShowExceptionMessageBox(CaughtBy, CaughtEx, sender, e)
        '''''''''''End Try









    End Sub ' Do_Window_Loaded

#End Region ' "Model Event Responses"

#Region "Localized Event Responses"

    '''' <summary>
    '''' Restores the color that was saved when RememberButton was clicked.
    '''' </summary>
    Private Sub DoRestoreButtonClick()
        With Me

            ' Reload from the saved component values.
            .UnderlyingR = .RememberR
            .UnderlyingG = .RememberG
            .UnderlyingB = .RememberB
            .UpdateBaseValuesFromRGB(.RememberR, .RememberG, .RememberB)

            ' Identify and redisplay the current tab.
            Dim CurrentTabItem As System.Windows.Controls.TabItem =
                CType(ViewsTabControl.SelectedItem,
                      System.Windows.Controls.TabItem)
            If .ViewsTabControl.SelectedItem.Equals(.ConvertTabItem) Then
                .ConvertUpdateVisuals()
            ElseIf .ViewsTabControl.SelectedItem.Equals(.RgbTabItem) Then
                .RgbUpdateVisuals()
            ElseIf .ViewsTabControl.SelectedItem.Equals(.HslTabItem) Then
                .HslUpdateVisuals()
            ElseIf .ViewsTabControl.SelectedItem.Equals(.HsvTabItem) Then
                .HsvUpdateVisuals()
            ElseIf .ViewsTabControl.SelectedItem.Equals(.ShadeTabItem) Then
                .ShadeStartR = .RememberR
                .ShadeStartG = .RememberG
                .ShadeStartB = .RememberB
                .ShadeStartH = 0.0
                .ShadeUpdateVisuals()
            ElseIf .ViewsTabControl.SelectedItem.Equals(.TintTabItem) Then
                .TintStartR = .RememberR
                .TintStartG = .RememberG
                .TintStartB = .RememberB
                .TintStartH = 0.0
                .TintUpdateVisuals()
            ElseIf .ViewsTabControl.SelectedItem.Equals(.ToneTabItem) Then
                .ToneStartR = .RememberR
                .ToneStartG = .RememberG
                .ToneStartB = .RememberB
                .ToneStartH = 0.0
                .ToneUpdateVisuals()
                'ElseIf .ViewsTabControl.SelectedItem.Equals(.BlendTabItem) Then
                ' Do nothing.
                '                .BlendUpdateVisuals()
            End If

        End With
    End Sub ' DoRestoreButtonClick

#End Region ' "Localized Event Responses"

End Class ' ColorDlgWindow
