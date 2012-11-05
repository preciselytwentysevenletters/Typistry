Imports System.Runtime.InteropServices

Public Module KeyboardHook
    <DllImport("user32.dll", SetLastError:=True)>
    Public Function SetWindowsHookEx(idHook As Integer, HookProc As KBDLLHookProc, hInstance As IntPtr, dwThreadId As UShort) As IntPtr

    End Function

    <DllImport("user32.dll")>
    Public Function CallNextHookEx(idHook As IntPtr, nCode As Integer, wParam As IntPtr, lParam As IntPtr) As Integer

    End Function

    <DllImport("user32.dll")>
    Public Function UnhookWindowsHookEx(idHook As IntPtr) As Boolean

    End Function

    <StructLayout(LayoutKind.Sequential)>
    Public Structure KBDLLHOOKSTRUCT
        Public vkCode As UInteger
        Public scanCode As UInteger
        Public flags As KBDLLHOOKSTRUCTFlags
        Public time As UInteger
        Public dwExtraInfo As UIntPtr
    End Structure

    <Flags()>
    Public Enum KBDLLHOOKSTRUCTFlags As UInteger
        LLKHF_EXTENDED = &H1
        LLKHF_INJECTED = &H10
        LLKHF_ALTDOWN = &H20
        LLKHF_UP = &H80
    End Enum

    Public Const WH_KEYBOARD As Integer = 2
    Public Const WH_KEYBOARD_LL As Integer = 13
    Public Const HC_ACTION As Integer = 0
    Public Const WM_KEYDOWN As Integer = &H100
    Public Const WM_KEYUP As Integer = &H101
    Public Const WM_SYSKEYDOWN As Integer = &H104
    Public Const WM_SYSKEYUP As Integer = &H105

    Public Delegate Function KBDLLHookProc(nCode As Integer, wParam As IntPtr, lParam As IntPtr) As Integer
End Module