Imports System.Runtime.InteropServices
Imports System.Reflection

Public Class Prompt
    Private isVisible As Boolean = False
    Private currentAnimation As Threading.Timer
    Private previousForeground As IntPtr

    <DllImport("user32.dll")>
    Private Shared Function SetForegroundWindow(hWnd As IntPtr) As Integer

    End Function

    <DllImport("user32.dll")>
    Private Shared Function GetForegroundWindow() As IntPtr

    End Function

#Region "Key Hook"
    Private hook As IntPtr
    Private hookDelegate As New KBDLLHookProc(AddressOf KeyHook)

    Private Sub Prompt_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Position the form:
        Dim workingArea As Rectangle = My.Computer.Screen.WorkingArea

        Me.Bounds = New Rectangle(workingArea.X, workingArea.Y, workingArea.Width, 1)
        Me.Opacity = 0

        ' Create the key hook:
        Dim hookAssembly As Assembly

        ' If in debugging mode, the assembly we need is that of the Visual Studio hosting process.
#If DEBUG Then
        hookAssembly = Assembly.GetCallingAssembly()
#Else
        hookAssembly = Assembly.GetExecutingAssembly()
#End If

        hook = KeyboardHook.SetWindowsHookEx(KeyboardHook.WH_KEYBOARD_LL, hookDelegate, Marshal.GetHINSTANCE(hookAssembly.Modules.First()), 0)

        If hook = IntPtr.Zero Then
            Throw New ApplicationException("Failed to set key hook: " & Marshal.GetLastWin32Error())
        End If
    End Sub

    Private Function KeyHook(nCode As Integer, wParam As IntPtr, lParam As IntPtr) As Integer
        If nCode = KeyboardHook.HC_ACTION Then
            Dim p As Integer = wParam.ToInt32()

            If p = KeyboardHook.WM_KEYDOWN OrElse p = KeyboardHook.WM_SYSKEYDOWN Then
                Dim keyCode As Keys = CType(CType(Marshal.PtrToStructure(lParam, GetType(KeyboardHook.KBDLLHOOKSTRUCT)), KeyboardHook.KBDLLHOOKSTRUCT).vkCode, Keys)

                If keyCode = Keys.CapsLock Then
                    ' Toggle the command bar:
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

                        previousForeground = GetForegroundWindow()
                        SetForegroundWindow(Me.Handle)
                        Me.Command.Focus()
                    Else
                        currentAnimation = Me.Animate(Sub(t)
                                                          t = EaseInOut(t)

                                                          Me.Opacity = currentOpacity * (1 - t)
                                                          Me.Height = CInt(currentHeight * (1 - t))
                                                      End Sub, 0.2)

                        SetForegroundWindow(previousForeground)
                    End If

                    ' Prevent Caps Lock from being turned on:
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
#End Region

    Private Sub Command_KeyDown(sender As Object, e As KeyEventArgs) Handles Command.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Me.Command.Text = "quit" Then
                Me.Close()
            End If

            Me.Command.Clear()
            e.SuppressKeyPress = True
        End If
    End Sub
End Class