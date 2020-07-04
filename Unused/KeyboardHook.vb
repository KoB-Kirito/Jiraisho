'Dropped this approach, because some security software marks the app as spyware
'as SetWindowsHookEx is often used in keyloggers

'Imports System.Runtime.InteropServices
'Imports System.Windows.Forms

'Module Hotkeys
'    Private modKey = Keys.LMenu
'    Private modKeyPressed As Boolean
'    Private saveKey = Keys.S
'    Private saveKeyPressed As Boolean
'    Private saveTriggered As Boolean

'    AddHandler() KeyboardHook.OnKeyDown, AddressOf KeyDown
'    AddHandler() KeyboardHook.OnKeyUp, AddressOf KeyUp

'    Public Sub KeyDown(Key As Keys)
'        Select Case Key
'            Case modKey
'                modKeyPressed = True
'            Case saveKey
'                saveKeyPressed = True
'        End Select

'        If Not saveTriggered AndAlso modKeyPressed AndAlso saveKeyPressed Then
'            saveTriggered = True
'            Log(LogLvl.Debug, "Alt + S pressed")
'        End If
'    End Sub

'    Public Sub KeyUp(Key As Keys)
'        Select Case Key
'            Case modKey
'                modKeyPressed = False
'            Case saveKey
'                saveKeyPressed = False
'        End Select

'        If Not modKeyPressed OrElse Not saveKeyPressed Then
'            saveTriggered = False
'        End If
'    End Sub
'End Module

'Class KeyboardHook
'    Private Const WH_KEYBOARD_LL As Integer = 13
'    Private Const WM_KEYDOWN As Integer = &H100
'    Private Const WM_KEYUP As Integer = &H101
'    Private Const WM_SYSKEYDOWN As Integer = &H104
'    Private Const WM_SYSKEYUP As Integer = &H105
'    Private Shared ReadOnly _proc As LowLevelKeyboardProc = AddressOf HookCallback
'    Private Shared _hookID As IntPtr = IntPtr.Zero

'    Public Shared Event OnKeyDown(Key As Windows.Forms.Keys)
'    Public Shared Event OnKeyUp(Key As Windows.Forms.Keys)

'    Public Shared Sub Hook()
'        Using curProcess As Process = Process.GetCurrentProcess()
'            Using curModule As ProcessModule = curProcess.MainModule
'                _hookID = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, GetModuleHandle(curModule.ModuleName), 0)
'            End Using
'        End Using
'    End Sub

'    Public Shared Sub Unhook()
'        UnhookWindowsHookEx(_hookID)
'    End Sub

'    Private Delegate Function LowLevelKeyboardProc(ByVal nCode As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr

'    Private Shared Function HookCallback(ByVal nCode As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
'        If nCode >= 0 Then
'            Select Case CType(wParam, Integer)
'                Case WM_KEYDOWN, WM_SYSKEYDOWN
'                    Dim vkCode As Integer = Marshal.ReadInt32(lParam)
'                    RaiseEvent OnKeyDown(vkCode)

'                Case WM_KEYUP, WM_SYSKEYUP
'                    Dim vkCode As Integer = Marshal.ReadInt32(lParam)
'                    RaiseEvent OnKeyUp(vkCode)

'            End Select
'        End If

'        Return CallNextHookEx(_hookID, nCode, wParam, lParam)
'    End Function

'#Region "PInvoke"
'    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
'    Private Shared Function SetWindowsHookEx(ByVal idHook As Integer, ByVal lpfn As LowLevelKeyboardProc, ByVal hMod As IntPtr, ByVal dwThreadId As UInteger) As IntPtr
'    End Function

'    '<MarshalAs(UnmanagedType.Bool)> vb doesn't support unmanaged code, works anyway~
'    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
'    Private Shared Function UnhookWindowsHookEx(ByVal hhk As IntPtr) As Boolean
'    End Function

'    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
'    Private Shared Function CallNextHookEx(ByVal hhk As IntPtr, ByVal nCode As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
'    End Function

'    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
'    Private Shared Function GetModuleHandle(ByVal lpModuleName As String) As IntPtr
'    End Function
'#End Region
'End Class