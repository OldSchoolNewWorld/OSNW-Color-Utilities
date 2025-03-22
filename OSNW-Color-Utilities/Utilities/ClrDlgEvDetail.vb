Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System.Windows.Input

Partial Friend Class ColorDlgWindow

#Region "Event Utilities"

#End Region ' "Event Utilities"

#Region "Event Responses"

#End Region ' "Event Responses"

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

End Class ' ColorDlgWindow
