Imports System.Threading
Imports System.Drawing

Public NotInheritable Class SplashScreen
    Public Delegate Sub OneIntParamDeleg(ByRef i As Integer)
    Private Sub SplashScreen_Load(sender As Object, e As EventArgs) Handles Me.Load
        Randomize()
        Me.TopMost = True
        Dim rnd As Random = New Random
        Dim x As Integer = rnd.Next(1, 4)
        Select Case x
            Case 1
                PictureBox1.Image = My.Resources._1
            Case 2
                PictureBox1.Image = My.Resources._2
            Case 3
                PictureBox1.Image = My.Resources._3
        End Select
        Form1.Show()
        paintTextOverlay()
    End Sub
    Private Function paintTextOverlay()
        Dim x As SolidBrush = Brushes.White
        Dim ffont As Font = New Font("Arial", 48)
        Dim g As Graphics = Graphics.FromImage(PictureBox1.Image)
        g.DrawString("Initializing...", ffont, x, 128, 0)
        'g.Flush()

    End Function
End Class
