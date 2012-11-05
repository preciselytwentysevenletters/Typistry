Imports System.Runtime.InteropServices
Imports System.Reflection

Public Class Prompt
    Private hook As IntPtr
    Private hookDelegate As New KBDLLHookProc(AddressOf KeyHook)
    Private isVisible As Boolean = False
    Private currentAnimation As Threading.Timer

    <DllImport("user32.dll")>
    Private Shared Function SetForegroundWindow(hWnd As IntPtr) As Integer

    End Function

    Private Sub Prompt_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim workingArea As Rectangle = My.Computer.Screen.WorkingArea

        Me.Bounds = New Rectangle(workingArea.X, workingArea.Y, workingArea.Width, 1)
        Me.Opacity = 0

        Dim hookAssembly As Assembly

        ' The Visual Studio hosting process interferes with this =(
#If DEBUG Then
        hookAssembly = Assembly.GetCallingAssembly()
#Else
        hookAssembly = Assembly.GetExecutingAssembly()
#End If

        hook = KeyboardHook.SetWindowsHookEx(KeyboardHook.WH_KEYBOARD_LL, hookDelegate, Marshal.GetHINSTANCE(hookAssembly.Modules.First()), 0)

        If hook = IntPtr.Zero Then
            Throw New ApplicationException("Failed to set key hook.")
        End If
    End Sub

    Private Function KeyHook(nCode As Integer, wParam As IntPtr, lParam As IntPtr) As Integer
        If nCode = KeyboardHook.HC_ACTION Then
            Dim p As Integer = wParam.ToInt32()

            If p = KeyboardHook.WM_KEYDOWN OrElse p = KeyboardHook.WM_SYSKEYDOWN Then
                Dim keyCode As Keys = CType(CType(Marshal.PtrToStructure(lParam, GetType(KeyboardHook.KBDLLHOOKSTRUCT)), KeyboardHook.KBDLLHOOKSTRUCT).vkCode, Keys)

                If keyCode = Keys.CapsLock Then
                    isVisible = Not isVisible

                    If currentAnimation IsNot Nothing Then currentAnimation.Dispose()

                    Dim currentHeight As Integer = Me.Height
                    Dim currentOpacity As Double = Me.Opacity

                    If isVisible Then
                        currentAnimation = Me.Animate(Sub(t)
                                                          t = EaseInOut(t)

                                                          Me.Opacity = currentOpacity + (1 - currentOpacity) * t
                                                          Me.Height = CInt(currentHeight + (40 - currentHeight) * t)
                                                      End Sub, 0.2)

                        SetForegroundWindow(Me.Handle)
                    Else
                        currentAnimation = Me.Animate(Sub(t)
                                                          t = EaseInOut(t)

                                                          Me.Opacity = currentOpacity * (1 - t)
                                                          Me.Height = CInt(currentHeight * (1 - t))
                                                      End Sub, 0.2)
                    End If

                    ' Finally, prevent Caps Lock from being turned on:
                    Return 1
                End If
            End If
        End If

        Return KeyboardHook.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam)
    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If

            If hook <> IntPtr.Zero Then
                Dim success As Boolean = KeyboardHook.UnhookWindowsHookEx(hook)

                Debug.Assert(success)
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub
End Class