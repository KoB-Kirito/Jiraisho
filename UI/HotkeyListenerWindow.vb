﻿Imports System.Windows.Forms
Imports Newtonsoft.Json
Imports Jiraisho.NativeMethod

Public Class HotkeyListenerWindow
    Inherits NativeWindow

    Public Event HotkeyPressed(Hotkey As Hotkey)

    Private Const WM_HOTKEY As Integer = &H312
    Private Const WM_DESTROY As Integer = &H2

    Public Sub New()
        Me.CreateHandle(New CreateParams())
        SharedHandle = Me.Handle
        AddHandler Application.ApplicationExit, AddressOf Application_ApplicationExit

        'Init Hotkeys
        UpdateHotkey(Hotkey.SaveCurrentImage, CFG.HK_SaveCurrent.Item1, CFG.HK_SaveCurrent.Item2)
        UpdateHotkey(Hotkey.OpenCurrentImage, CFG.HK_OpenCurrent.Item1, CFG.HK_OpenCurrent.Item2)
        UpdateHotkey(Hotkey.FavCurrentImage, CFG.HK_FavCurrent.Item1, CFG.HK_FavCurrent.Item2)
    End Sub

    Private Sub Application_ApplicationExit(ByVal sender As Object, ByVal e As EventArgs)
        Me.DestroyHandle() ' -> triggers WM_DESTROY
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case WM_HOTKEY
                RaiseEvent HotkeyPressed(m.WParam.ToInt32())
            Case WM_DESTROY
                'Unregister hotkeys in case the handle gets destroyed,
                'because we don't get more messages without a handle anyway
                For Each ID As Integer In RegisteredHotkeys
                    User32.UnregisterHotKey(Me.Handle, ID)
                Next
        End Select

        MyBase.WndProc(m)
    End Sub

#Region "Shared"
    Public Shared SharedHandle As Object
    Private Shared ReadOnly RegisteredHotkeys As List(Of Integer) = New List(Of Integer)

    Public Shared Sub UpdateHotkey(Hotkey As Hotkey, Modifier As HK_Modifier, vlc As Keys)
        Try
            If RegisteredHotkeys.Contains(Hotkey) Then
                If User32.UnregisterHotKey(SharedHandle, Hotkey) Then
                    RegisteredHotkeys.Remove(Hotkey)
                Else
                    Log(LogLvl.Error, "Failed to unregister hotkey")
                    Return
                End If
            End If

            If User32.RegisterHotKey(SharedHandle, Hotkey, Modifier + HK_Modifier.MOD_NOREPEAT, vlc) Then
                RegisteredHotkeys.Add(Hotkey)
                Log(LogLvl.Debug, "Hotkey registered")
            Else
                Log(LogLvl.Error, "Can't register hotkey")
            End If
        Catch ex As Exception
            Log(LogLvl.Error, "Can't register hotkey", ex)
        End Try
    End Sub

    Public Shared Function ModifierToKeys(modifier As HK_Modifier) As Keys
        Select Case modifier
            Case HK_Modifier.MOD_ALT
                Return Keys.Alt

            Case HK_Modifier.MOD_CONTROL
                Return Keys.Control

            Case HK_Modifier.MOD_CONTROL_ALT
                Return Keys.Control Or Keys.Alt

            Case HK_Modifier.MOD_CONTROL_SHIFT
                Return Keys.Control Or Keys.Shift

            Case HK_Modifier.MOD_SHIFT
                Return Keys.Shift

            Case HK_Modifier.MOD_SHIFT_ALT
                Return Keys.Shift Or Keys.Alt

            Case Else
                Return Nothing
        End Select
    End Function

#End Region
End Class

Public Enum Hotkey As Integer
    SaveCurrentImage = 1001
    OpenCurrentImage = 1002
    FavCurrentImage = 1003
End Enum

<Flags>
<JsonConverter(GetType(Converters.StringEnumConverter))>
Public Enum HK_Modifier As UInteger
    MOD_ALT = &H1
    MOD_CONTROL = &H2
    MOD_NOREPEAT = &H4000 'Not supported in Vista
    MOD_SHIFT = &H4
    MOD_WIN = &H8 'Reserved by OS
    MOD_CONTROL_ALT = MOD_CONTROL + MOD_ALT
    MOD_SHIFT_ALT = MOD_SHIFT + MOD_ALT
    MOD_CONTROL_SHIFT = MOD_CONTROL + MOD_SHIFT
End Enum