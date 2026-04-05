# 🖱️ Automatic Click Bot Tool (VB.NET)

Automação de movimento de cursor suave usando Windows Forms

<p align="center">
  <img src="https://img.shields.io/badge/Structure-.NET%2010.0-blueviolet?style=for-the-badge">
  <img src="https://img.shields.io/badge/Language-CS.NET-512BD4?style=for-the-badge">
  <img src="https://img.shields.io/badge/Platform-Windows-0078D6?style=for-the-badge">
</p>

## 🖥️ Pré-requisitos

- [Visual Studio](https://visualstudio.microsoft.com/) (2017, 2019, 2022 ou superior)
- Windows SDK (.NET Framework 4.8)

## 🌟 Passo a passo

1. Abra o Microsoft Visual Studio e clique em **Criar um projeto**, para criar nosso projeto `Automatic Click Bot Tool` em `VB.NET`.

2. Em Modelos: `Visual Basic` `Windows` `Área de Trabalho`, selecione **Aplicativo do Windows Forms (.NET Framework)**. Um projeto para criar um aplicativo com uma interface de usuário do Windows Forms (WinForms) e configure o projeto com as seguintes informações:

- **Nome do projeto** `automatic-click-bot-tool-vb`
- **Local** `C:\Users\{USERNAME}\source\repos`
- **Nome da solução** `automatic-click-bot-tool-vb`
- **Estrutura** `.NET Framework 4.8`
- Clique em **Criar**.

3. No Gerenciador de Soluções, clique com o botão direito no formulário `Form1.vb` e selecione **Exibir Código** ou pressione a tecla **F7**. O código inicial será:

```vbnet
Public Class Form1

End Class
```

4. Dentro desta classe `Form1`, criaremos um procedimento `Sub` para mover o cursor do ponto `A(X, Y)` para o ponto `B(X, Y)`:

```vbnet
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Threading
Public Class Form1
    Public Sub SmoothCursorMove(pointA As Point, pointB As Point)
        Dim steps As Integer = 75    ' (opção) quanto maior, mais suave
        Dim delay As Integer = 5     ' (opção) milissegundos entre passos
        Dim dx As Double = (pointB.X - pointA.X) / steps
        Dim dy As Double = (pointB.Y - pointA.Y) / steps
        Dim currentX As Double = pointA.X
        Dim currentY As Double = pointA.Y
        For i As Integer = 1 To steps
            currentX += dx
            currentY += dy
            Cursor.Position = New Point(CInt(currentX), CInt(currentY))
            Thread.Sleep(delay)
        Next
    End Sub
End Class
```

5. Para usar este procedimento `Sub`, basta criar os dois pontos `A(230, 240)` e `B(800, 600)` ou inseri-los diretamente:
```vbnet
SmoothCursorMove(New Point(230, 240), New Point(800, 600))
```
6. Exemplo completo executado ao clicar no `Button1`. Para cancelar, pressione a tecla `Esc`.

```vbnet
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Threading
Public Class Form1
    ''' <summary>
    ''' Move o cursor de um ponto A(X,Y) para um ponto B(X,Y).
    ''' </summary>
    ''' <param name="pointA"></param>
    ''' <param name="pointB"></param>
    Public Sub SmoothCursorMove(pointA As Point, pointB As Point)
        Dim steps As Integer = 75    ' (opção) quanto maior, mais suave
        Dim delay As Integer = 5     ' (opção) milissegundos entre passos
        Dim dx As Double = (pointB.X - pointA.X) / steps
        Dim dy As Double = (pointB.Y - pointA.Y) / steps
        Dim currentX As Double = pointA.X
        Dim currentY As Double = pointA.Y
        For i As Integer = 1 To steps
            currentX += dx
            currentY += dy
            Cursor.Position = New Point(CInt(currentX), CInt(currentY))
            Thread.Sleep(delay)
        Next
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SmoothCursorMove(New Point(230, 240), New Point(800, 600))
    End Sub
End Class
```

## 💡 Dicas

- **Steps**: define a suavidade do movimento (valores maiores tornam o movimento mais fluido)
- **Delay**: define a velocidade (valores maiores tornam o movimento mais lento)
  
## ⚠️ Observações

- `Thread.Sleep` bloqueia a thread da interface (UI Thread), podendo causar congelamentos e falta de responsividade
- Prefira `Async/Await` com `Task.Delay` para manter a aplicação fluida

```vbnet
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
```

## 👨‍💻 Autor

**Fábio Santos**, featuring **Eve Reeve**

## 📄 Licença

- Uso livre para fins educacionais.
