Imports System.Windows.Forms
Imports System.Drawing
Imports System.Threading
Public Class Form1
    Private cts As CancellationTokenSource
    ''' <summary>
    ''' Move o cursor de um ponto A(X,Y) para um ponto B(X,Y).
    ''' </summary>
    ''' <param name="pointA">Posição A.</param>
    ''' <param name="pointB">Posição B.</param>
    ''' <param name="token">Token para cancelar.</param>
    Private Async Function SmoothCursorMoveAsync(pointA As Point, pointB As Point, token As CancellationToken) As Task
        Dim steps As Integer = 75    ' (opção) quanto maior, mais suave
        Dim delay As Integer = 5     ' (opção) milissegundos entre passos
        Dim dx As Double = (pointB.X - pointA.X) / steps
        Dim dy As Double = (pointB.Y - pointA.Y) / steps
        Dim currentX As Double = pointA.X
        Dim currentY As Double = pointA.Y
        For i As Integer = 1 To steps
            token.ThrowIfCancellationRequested()
            currentX += dx
            currentY += dy
            Cursor.Position = New Point(CInt(currentX), CInt(currentY))
            Await Task.Delay(delay, token)
        Next
    End Function
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = Keys.Escape Then
            If cts IsNot Nothing Then
                cts.Cancel()
            End If
            Return True
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function
    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        cts = New CancellationTokenSource()
        Try
            Await SmoothCursorMoveAsync(New Point(230, 240), New Point(800, 600), cts.Token)
        Catch ex As OperationCanceledException
            MessageBox.Show("A tecla <Esc> foi pressionada: o movimento do cursor foi cancelado!", "Automatic Click Bot Tool", MessageBoxButtons.OK)
        End Try
    End Sub
End Class