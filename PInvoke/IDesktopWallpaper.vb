Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Text

Namespace NativeMethod

#Region "IDesktopWallpaper"
    <ComImport>
    <Guid("B92B56A9-8B55-4E14-9A89-0199BBB6F93B")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IDesktopWallpaper
        Function SetWallpaper(<MarshalAs(UnmanagedType.LPWStr)> ByVal monitorID As String,
                              <MarshalAs(UnmanagedType.LPWStr)> ByVal wallpaper As String) As HRESULT

        Function GetWallpaper(<MarshalAs(UnmanagedType.LPWStr)> ByVal monitorID As String,
                              <MarshalAs(UnmanagedType.LPWStr)> ByRef wallpaper As String) As HRESULT

        Function GetMonitorDevicePathAt(ByVal monitorIndex As UInteger,
                                        <MarshalAs(UnmanagedType.LPWStr)> ByRef monitorID As String) As HRESULT

        Function GetMonitorDevicePathCount(ByRef count As UInteger) As HRESULT

        Function GetMonitorRECT(<MarshalAs(UnmanagedType.LPWStr)> ByVal monitorID As String,
                                <MarshalAs(UnmanagedType.Struct)> ByRef displayRect As User32.RECT) As HRESULT

        Function SetBackgroundColor(ByVal color As UInteger) As HRESULT

        Function GetBackgroundColor(ByRef color As UInteger) As HRESULT

        Function SetPosition(ByVal position As DESKTOP_WALLPAPER_POSITION) As HRESULT

        Function GetPosition(ByRef position As DESKTOP_WALLPAPER_POSITION) As HRESULT

        Function SetSlideshow(ByVal items As IShellItemArray) As HRESULT

        Function GetSlideshow(ByRef items As IShellItemArray) As HRESULT

        Function SetSlideshowOptions(ByVal options As DESKTOP_SLIDESHOW_OPTIONS, ByVal slideshowTick As UInteger) As HRESULT

        <PreserveSig>
        Function GetSlideshowOptions(<Out> ByRef options As DESKTOP_SLIDESHOW_OPTIONS, <Out> ByRef slideshowTick As UInteger) As HRESULT

        Function AdvanceSlideshow(<MarshalAs(UnmanagedType.LPWStr)> ByVal monitorID As String, ByVal direction As DESKTOP_SLIDESHOW_DIRECTION) As HRESULT

        Function GetStatus(ByRef state As DESKTOP_SLIDESHOW_STATE) As HRESULT

        Function Enable(ByVal benable As Boolean) As HRESULT
    End Interface

    Public Enum DESKTOP_WALLPAPER_POSITION
        DWPOS_CENTER = 0
        DWPOS_TILE = 1
        DWPOS_STRETCH = 2
        DWPOS_FIT = 3
        DWPOS_FILL = 4
        DWPOS_SPAN = 5
    End Enum

    Public Enum DESKTOP_SLIDESHOW_OPTIONS
        DSO_SHUFFLEIMAGES = &H1
    End Enum

    Public Enum DESKTOP_SLIDESHOW_STATE
        DSS_ENABLED = &H1
        DSS_SLIDESHOW = &H2
        DSS_DISABLED_BY_REMOTE_SESSION = &H4
    End Enum

    Public Enum DESKTOP_SLIDESHOW_DIRECTION
        DSD_FORWARD = 0
        DSD_BACKWARD = 1
    End Enum

    <ComImport, Guid("C2CF3110-460E-4fc1-B9D0-8A1C0C9CC4BD")>
    Public Class DesktopWallpaperClass
    End Class

    Public Enum HRESULT As Integer
        S_OK = 0
        S_FALSE = 1
        E_NOINTERFACE = &H80004002
        E_NOTIMPL = &H80004001
        E_FAIL = &H80004005
    End Enum
#End Region


#Region "IShellItemArray"
    Public Enum SIATTRIBFLAGS
        SIATTRIBFLAGS_AND = &H1
        SIATTRIBFLAGS_OR = &H2
        SIATTRIBFLAGS_APPCOMPAT = &H3
        SIATTRIBFLAGS_MASK = &H3
        SIATTRIBFLAGS_ALLITEMS = &H4000
    End Enum

    <ComImport()>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid("b63ea76d-1f85-456f-a19c-48159efa858b")>
    Public Interface IShellItemArray
        Function BindToHandler(ByVal pbc As IntPtr, ByRef bhid As Guid, ByRef riid As Guid, ByRef ppvOut As IntPtr) As HRESULT
        Function GetPropertyStore(ByVal flags As GETPROPERTYSTOREFLAGS, ByRef riid As Guid, ByRef ppv As IntPtr) As HRESULT
        Function GetPropertyDescriptionList(ByVal keyType As REFPROPERTYKEY, ByRef riid As Guid, ByRef ppv As IntPtr) As HRESULT
        Function GetAttributes(ByVal AttribFlags As SIATTRIBFLAGS, ByVal sfgaoMask As Integer, ByRef psfgaoAttribs As Integer) As HRESULT
        Function GetCount(ByRef pdwNumItems As Integer) As HRESULT
        Function GetItemAt(ByVal dwIndex As Integer, ByRef ppsi As IShellItem) As HRESULT
        Function EnumItems(ByRef ppenumShellItems As IntPtr) As HRESULT
    End Interface

    <ComImport()>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")>
    Public Interface IShellItem
        <PreserveSig()>
        Function BindToHandler(ByVal pbc As IntPtr, ByRef bhid As Guid, ByRef riid As Guid, ByRef ppv As IntPtr) As HRESULT
        Function GetParent(ByRef ppsi As IShellItem) As HRESULT
        Function GetDisplayName(ByVal sigdnName As SIGDN, ByRef ppszName As StringBuilder) As HRESULT
        Function GetAttributes(ByVal sfgaoMask As UInteger, ByRef psfgaoAttribs As UInteger) As HRESULT
        Function Compare(ByVal psi As IShellItem, ByVal hint As UInteger, ByRef piOrder As Integer) As HRESULT
    End Interface

    Public Enum SIGDN As Integer
        SIGDN_NORMALDISPLAY = &H0
        SIGDN_PARENTRELATIVEPARSING = &H80018001
        SIGDN_DESKTOPABSOLUTEPARSING = &H80028000
        SIGDN_PARENTRELATIVEEDITING = &H80031001
        SIGDN_DESKTOPABSOLUTEEDITING = &H8004C000
        SIGDN_FILESYSPATH = &H80058000
        SIGDN_URL = &H80068000
        SIGDN_PARENTRELATIVEFORADDRESSBAR = &H8007C001
        SIGDN_PARENTRELATIVE = &H80080001
    End Enum

    Public Enum GETPROPERTYSTOREFLAGS
        GPS_DEFAULT = 0
        GPS_HANDLERPROPERTIESONLY = &H1
        GPS_READWRITE = &H2
        GPS_TEMPORARY = &H4
        GPS_FASTPROPERTIESONLY = &H8
        GPS_OPENSLOWITEM = &H10
        GPS_DELAYCREATION = &H20
        GPS_BESTEFFORT = &H40
        GPS_NO_OPLOCK = &H80
        GPS_PREFERQUERYPROPERTIES = &H100
        GPS_EXTRINSICPROPERTIES = &H200
        GPS_EXTRINSICPROPERTIESONLY = &H400
        GPS_MASK_VALID = &H7FF
    End Enum

    <StructLayout(LayoutKind.Sequential, Pack:=4)>
    Public Structure REFPROPERTYKEY
        Private fmtid As Guid
        Private pid As Integer

        Public ReadOnly Property FormatId As Guid
            Get
                Return fmtid
            End Get
        End Property

        Public ReadOnly Property PropertyId As Integer
            Get
                Return pid
            End Get
        End Property

        Public Sub New(ByVal formatId As Guid, ByVal propertyId As Integer)
            fmtid = formatId
            pid = propertyId
        End Sub

        Public Shared ReadOnly PKEY_DateCreated As REFPROPERTYKEY = New REFPROPERTYKEY(New Guid("B725F130-47EF-101A-A5F1-02608C9EEBAC"), 15)
    End Structure
#End Region

End Namespace


