Imports System.Runtime.CompilerServices

Module Extensions
    Public Function EaseInOut(value As Double) As Double
        If value < 0 OrElse value > 1 Then Throw New ArgumentException("Value must be between 0 and 1 (inclusive).", "value")

        Return Math.Sin(value * Math.PI / 2)
    End Function

    ''' <summary>
    ''' Performs an animation on a <see cref="Control" />.
    ''' </summary>
    ''' <param name="this">The control performing the animation.</param>
    ''' <param name="callback">The animation callback.</param>
    ''' <param name="duration">The duration of the animation in seconds.</param>
    <Extension()>
    Public Function Animate(this As Control, callback As Action(Of Double), duration As Double) As Threading.Timer
        Dim start As Date = Date.Now

        Animate = New Threading.Timer(Sub()
                                          Dim time As Double = Date.Now.Subtract(start).TotalSeconds / duration

                                          If time > 1 Then
                                              time = 1
                                              Animate.Dispose()
                                          End If

                                          this.Invoke(callback, time)
                                      End Sub, Nothing, 0, 10)
    End Function
End Module