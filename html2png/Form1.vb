Imports html2png.AgogeComponents.Web
Imports BoletoNet

Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    Sub Abc()

        Me.PictureBox1.Image = AgogeComponents.Web.Render.ToBitmap(Me.TextBox1.Text, 794, 1050)
        'Me.PictureBox1.Image.Save("C:\Boleto.png")
        Me.PictureBox1.Image.ToPDF("C:\Boleto.pdf")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles OK.Click
        Abc()
    End Sub


End Class
