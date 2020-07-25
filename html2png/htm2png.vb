Imports System.Threading
Imports System.Drawing
Imports PdfSharp

Namespace AgogeComponents
    Namespace Web

        Public Class Render

            Public Property Url() As String

            Public Property Width() As Integer
            Public Property Height() As Integer
            Public Property ThumbnailImage() As Bitmap

            Public Shared Function ToBitmap(Url As String, Optional Width As Integer = 1024, Optional Height As Integer = 768) As Bitmap
                Dim r As New Render(Url, Width, Height)
                Dim Img As Bitmap = r.GenerateThumbnail
                Return Img
            End Function

            Public Shared Sub ToPDF(Url As String, LocalPDFPath As String)
                ToBitmap(Url).ToPDF(LocalPDFPath)
            End Sub

            Private Sub New(Url As String, Width As Integer, Height As Integer)
                Me.Url = Url
                Me.Width = Width
                Me.Height = Height
            End Sub

            Public Function GenerateThumbnail() As Bitmap
                Dim thread As New Thread(New ThreadStart(AddressOf GenerateThumbnailInteral))
                thread.SetApartmentState(ApartmentState.STA)
                thread.Start()
                thread.Join()
                Return ThumbnailImage
            End Function

            Private Sub GenerateThumbnailInteral()
                Dim webBrowser As New WebBrowser()
                webBrowser.ScrollBarsEnabled = False
                webBrowser.ScriptErrorsSuppressed = True
                webBrowser.Navigate(Me.Url)

                AddHandler webBrowser.DocumentCompleted, AddressOf WebBrowser_DocumentCompleted

                While webBrowser.ReadyState <> WebBrowserReadyState.Complete
                    Application.DoEvents()
                End While

                webBrowser.Dispose()
            End Sub

            Private Sub WebBrowser_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs)
                Dim webBrowser As WebBrowser = DirectCast(sender, WebBrowser)
                webBrowser.ClientSize = New Size(Me.Width, Me.Height)
                webBrowser.ScrollBarsEnabled = False
                Me.ThumbnailImage = New Bitmap(webBrowser.Bounds.Width, webBrowser.Bounds.Height)
                webBrowser.BringToFront()
                webBrowser.DrawToBitmap(ThumbnailImage, webBrowser.Bounds)
            End Sub

        End Class

        Public Module ImageExtensions

            <Runtime.CompilerServices.Extension()> _
            Public Sub ToPDF(Img As Image, LocalPDFPath As String)
                Using PDF As New PdfSharp.Pdf.PdfDocument
                    Dim Page1 As New PdfSharp.Pdf.PdfPage()
                    PDF.Pages.Add(Page1)

                    Dim G As PdfSharp.Drawing.XGraphics = PdfSharp.Drawing.XGraphics.FromPdfPage(Page1)
                    Dim XImg As PdfSharp.Drawing.XImage = PdfSharp.Drawing.XImage.FromGdiPlusImage(Img)

                    Dim PageW As Integer = Page1.Width
                    Dim PageH As Integer = Page1.Height

                    Dim ImageW As Integer = XImg.Width
                    Dim ImageH As Integer = XImg.Height

                    G.DrawImage(XImg, 0, 0)

                    PDF.Save(LocalPDFPath)
                    PDF.Close()
                End Using
            End Sub

        End Module

    End Namespace
End Namespace