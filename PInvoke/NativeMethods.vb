Imports System.Drawing
Imports System.Runtime.InteropServices

Namespace NativeMethod

    Public Class Kernel32
        Public Shared Lock As New Object

        'Friend Declare Auto Function GetLastError Lib "kernel32.dll" () As UInteger
        'Use Marshal.GetLastWin32Error instead
        Friend Shared Function GetLastError() As Integer
            Return Marshal.GetLastWin32Error()
        End Function

    End Class
    Public Class User32
        Public Shared Lock As New Object

#Region "Desktop"

        ''' <summary>
        ''' https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-enumdesktopwindows
        ''' </summary>
        <DllImport("user32.dll", EntryPoint:="EnumDesktopWindows", ExactSpelling:=False, CharSet:=CharSet.Auto, SetLastError:=True)>
        Public Shared Function EnumDesktopWindows(ByVal hDesktop As IntPtr, ByVal lpEnumCallbackFunction As EnumDelegate, ByVal lParam As IntPtr) As Boolean
        End Function
        Public Delegate Function EnumDelegate(ByVal hWnd As IntPtr, ByVal lParam As Integer) As Boolean


        '<DllImport("user32.dll", EntryPoint:="OpenDesktopW", CharSet:=CharSet.Unicode)>
        'Public Shared Function OpenDesktop(ByVal lpszDesktop As String, ByVal dwFlags As Integer, ByVal fInderit As Boolean, ByVal dwDesiredAccess As AccessRight) As IntPtr
        'End Function
        Friend Declare Auto Function OpenDesktop Lib "User32" (<MarshalAs(UnmanagedType.LPTStr)> ByVal DesktopName As String, ByVal dwFlags As Integer, <MarshalAs(UnmanagedType.Bool)> ByVal fInherit As Boolean, ByVal dwDesiredAccess As AccessRight) As IntPtr


        '<DllImport("user32.dll", EntryPoint:="OpenDesktopW", CharSet:=CharSet.Unicode)>
        'Public Shared Function EnumDesktops(ByVal hwinsta As IntPtr, ByVal lpEnumFunc As EnumDesktopsDelegate, ByVal lParam As IntPtr) As Boolean
        'End Function
        Friend Declare Auto Function EnumDesktops Lib "user32.dll" (ByVal hwinsta As IntPtr, ByVal lpEnumFunc As EnumDesktopsDelegate, ByVal lParam As IntPtr) As Boolean
        Public Delegate Function EnumDesktopsDelegate(ByVal desktop As String, ByVal lParam As IntPtr) As Boolean

        <DllImport("user32.dll", EntryPoint:="EnumDisplayDevicesW", CharSet:=CharSet.Unicode, CallingConvention:=CallingConvention.StdCall)>
        Public Shared Function EnumDisplayDevices(ByVal lpDevice As String, ByVal iDevNum As UInteger, ByRef lpDisplayDevice As DISPLAY_DEVICE, ByVal dwFlags As UInteger) As Integer
        End Function

        Friend Declare Auto Function GetProcessWindowStation Lib "user32.dll" () As IntPtr

        Friend Declare Auto Function GetDesktopWindow Lib "user32.dll" () As IntPtr

        Friend Declare Auto Function GetShellWindow Lib "user32.dll" () As IntPtr
#End Region

#Region "Windows"

        Friend Declare Auto Function IsWindowVisible Lib "user32.dll" (ByVal hWnd As IntPtr) As Boolean

        Friend Declare Auto Function GetForegroundWindow Lib "user32.dll" () As IntPtr

        Private Declare Auto Function GetWindowRectInt Lib "user32.dll" Alias "GetWindowRect" (ByVal hWnd As HandleRef, <[In], Out> ByRef rect As RECT) As Boolean
        'Wrap to accept direct handle and return Rectangle
        Friend Shared Function GetWindowRect(ByVal hWnd As IntPtr, ByRef rectangle As Rectangle) As Boolean
            Dim rect = New RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom)
            If GetWindowRectInt(New HandleRef(Nothing, hWnd), rect) Then
                rectangle = ToRectangle(rect)
                Return True
            Else
                Return False
            End If
        End Function

        Friend Declare Auto Function EnumWindows Lib "user32.dll" (enumProc As EnumWindowsDelegate, lParam As IntPtr) As Boolean
        Friend Delegate Function EnumWindowsDelegate(hWnd As IntPtr, ByRef lParam As Integer) As Boolean

        Friend Declare Auto Function GetClassName Lib "user32.dll" (hWnd As IntPtr, lpString As System.Text.StringBuilder, nMaxCount As Integer) As Boolean

        Friend Declare Auto Function GetWindowText Lib "user32.dll" (hWnd As IntPtr, lpString As System.Text.StringBuilder, nMaxCount As Integer) As Boolean

        Friend Declare Auto Function GetWindowLong Lib "user32.dll" (hWnd As IntPtr, Optional nIndex As Integer = GWL_STYLE) As UInteger
        Friend Const GWL_STYLE As Integer = -16
#End Region

#Region "Hotkeys"
        <DllImport("user32.dll")>
        Public Shared Function RegisterHotKey(ByVal hWnd As IntPtr, ByVal id As Integer, ByVal fsModifiers As UInteger, ByVal vlc As UInteger) As Boolean
        End Function

        <DllImport("user32.dll")>
        Public Shared Function UnregisterHotKey(ByVal hWnd As IntPtr, ByVal id As Integer) As Boolean
        End Function
#End Region


        <StructLayout(LayoutKind.Sequential)>
        Private Structure RECT
            Public left As Integer
            Public top As Integer
            Public right As Integer
            Public bottom As Integer

            Public Sub New(ByVal left As Integer, ByVal top As Integer, ByVal right As Integer, ByVal bottom As Integer)
                Me.left = left
                Me.top = top
                Me.right = right
                Me.bottom = bottom
            End Sub
        End Structure
        Private Shared Function ToRectangle(rect As RECT) As Rectangle
            Return New Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top)
        End Function


    End Class

    'https://docs.microsoft.com/en-us/windows/win32/winstation/desktop-security-and-access-rights
    <Flags>
    Public Enum AccessRight As Long
        DESKTOP_READOBJECTS = &H1L
        DESKTOP_CREATEWINDOW = &H2L
        DESKTOP_CREATEMENU = &H4L
        DESKTOP_HOOKCONTROL = &H8L
        DESKTOP_JOURNALRECORD = &H10L
        DESKTOP_JOURNALPLAYBACK = &H20L
        DESKTOP_ENUMERATE = &H40L
        DESKTOP_WRITEOBJECTS = &H80L
    End Enum

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
    Public Structure DISPLAY_DEVICE
        <MarshalAs(UnmanagedType.U4)>
        Public cb As Integer
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=32)>
        Public DeviceName As String
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=128)>
        Public DeviceString As String
        <MarshalAs(UnmanagedType.U4)>
        Public StateFlags As DisplayDeviceStateFlags
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=128)>
        Public DeviceID As String
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=128)>
        Public DeviceKey As String

        Public Overrides Function ToString() As String
            Return $"cb={{{Me.cb}}} DeviceName={{{If(Me.DeviceName, "Nothing")}}} DeviceString={{{If(Me.DeviceString, "Nothing")}}} StateFlags={{{Me.StateFlags.ToString()}}} DeviceID={{{If(Me.DeviceID, "Nothing")}}} DeviceKey={{{If(Me.DeviceKey, "Nothing")}}}"
        End Function
    End Structure

    <Flags()>
    Public Enum DisplayDeviceStateFlags As Integer
        ''' <summary>The device is part of the desktop.</summary>
        AttachedToDesktop = &H1
        MultiDriver = &H2
        ''' <summary>The device is part of the desktop.</summary>
        PrimaryDevice = &H4
        ''' <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
        MirroringDriver = &H8
        ''' <summary>The device is VGA compatible.</summary>
        VGACompatible = &H10
        ''' <summary>The device is removable; it cannot be the primary display.</summary>
        Removable = &H20
        ''' <summary>The device has more display modes than its output devices support.</summary>
        ModesPruned = &H8000000
        Remote = &H4000000
        Disconnect = &H2000000
    End Enum

    ''' <summary>
    ''' Window Styles.
    ''' The following styles can be specified wherever a window style is required. After the control has been created, these styles cannot be modified, except as noted.
    ''' </summary>
    <Flags()>
    Public Enum WindowStyles As UInteger
        ''' <summary>The window has a thin-line border.</summary>
        WS_BORDER = &H800000

        ''' <summary>The window has a title bar (includes the WS_BORDER style).</summary>
        WS_CAPTION = &HC00000

        ''' <summary>The window is a child window. A window with this style cannot have a menu bar. This style cannot be used with the WS_POPUP style.</summary>
        WS_CHILD = &H40000000

        ''' <summary>Excludes the area occupied by child windows when drawing occurs within the parent window. This style is used when creating the parent window.</summary>
        WS_CLIPCHILDREN = &H2000000

        ''' <summary>
        ''' Clips child windows relative to each other; that is, when a particular child window receives a WM_PAINT message, the WS_CLIPSIBLINGS style clips all other overlapping child windows out of the region of the child window to be updated.
        ''' If WS_CLIPSIBLINGS is not specified and child windows overlap, it is possible, when drawing within the client area of a child window, to draw within the client area of a neighboring child window.
        ''' </summary>
        WS_CLIPSIBLINGS = &H4000000

        ''' <summary>The window is initially disabled. A disabled window cannot receive input from the user. To change this after a window has been created, use the EnableWindow function.</summary>
        WS_DISABLED = &H8000000

        ''' <summary>The window has a border of a style typically used with dialog boxes. A window with this style cannot have a title bar.</summary>
        WS_DLGFRAME = &H400000

        ''' <summary>
        ''' The window is the first control of a group of controls. The group consists of this first control and all controls defined after it, up to the next control with the WS_GROUP style.
        ''' The first control in each group usually has the WS_TABSTOP style so that the user can move from group to group. The user can subsequently change the keyboard focus from one control in the group to the next control in the group by using the direction keys.
        ''' You can turn this style on and off to change dialog box navigation. To change this style after a window has been created, use the SetWindowLong function.
        ''' </summary>
        WS_GROUP = &H20000

        ''' <summary>The window has a horizontal scroll bar.</summary>
        WS_HSCROLL = &H100000

        ''' <summary>The window is initially maximized.</summary>
        WS_MAXIMIZE = &H1000000

        ''' <summary>The window has a maximize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.</summary>
        WS_MAXIMIZEBOX = &H10000

        ''' <summary>The window is initially minimized.</summary>
        WS_MINIMIZE = &H20000000

        ''' <summary>The window has a minimize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.</summary>
        WS_MINIMIZEBOX = &H20000

        ''' <summary>The window is an overlapped window. An overlapped window has a title bar and a border.</summary>
        WS_OVERLAPPED = &H0

        ''' <summary>The window is an overlapped window.</summary>
        WS_OVERLAPPEDWINDOW = WS_OVERLAPPED Or WS_CAPTION Or WS_SYSMENU Or WS_SIZEFRAME Or WS_MINIMIZEBOX Or WS_MAXIMIZEBOX

        ''' <summary>The window is a pop-up window. This style cannot be used with the WS_CHILD style.</summary>
        WS_POPUP = &H80000000UI

        ''' <summary>The window is a pop-up window. The WS_CAPTION and WS_POPUPWINDOW styles must be combined to make the window menu visible.</summary>
        WS_POPUPWINDOW = WS_POPUP Or WS_BORDER Or WS_SYSMENU

        ''' <summary>The window has a sizing border.</summary>
        WS_SIZEFRAME = &H40000

        ''' <summary>The window has a window menu on its title bar. The WS_CAPTION style must also be specified.</summary>
        WS_SYSMENU = &H80000

        ''' <summary>
        ''' The window is a control that can receive the keyboard focus when the user presses the TAB key.
        ''' Pressing the TAB key changes the keyboard focus to the next control with the WS_TABSTOP style.  
        ''' You can turn this style on and off to change dialog box navigation. To change this style after a window has been created, use the SetWindowLong function.
        ''' For user-created windows and modeless dialogs to work with tab stops, alter the message loop to call the IsDialogMessage function.
        ''' </summary>
        WS_TABSTOP = &H10000

        ''' <summary>The window is initially visible. This style can be turned on and off by using the ShowWindow or SetWindowPos function.</summary>
        WS_VISIBLE = &H10000000

        ''' <summary>The window has a vertical scroll bar.</summary>
        WS_VSCROLL = &H200000
    End Enum
End Namespace
