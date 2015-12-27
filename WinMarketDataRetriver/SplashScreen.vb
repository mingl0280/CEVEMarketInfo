Imports System.Threading
Imports System.Drawing

Public NotInheritable Class SplashScreen

#Region "Public Delegations and Vars"
    Public Delegate Sub OneIntParamDeleg(ByRef i As Integer)
    Public Delegate Sub NonParamDeleg()
#End Region

#Region "UI Operations"

#Region "Progress Bar Operations"
    Public Sub SetPBMax(ByRef i As Integer)
        ProgressBar1.Maximum = i
    End Sub

    Public Sub SetPBValue(ByRef i As Integer)
        ProgressBar1.Value = i
    End Sub

    Public Sub AddOneToProgressBar()
        ProgressBar1.Value = ProgressBar1.Value + 1
    End Sub
#End Region

#Region "Other UI Operations and returns"
    Public Shared Function isEnabled() As Boolean
        Return SplashScreen.IsHandleCreated
    End Function

    Private Sub HideWindow()
        Me.Hide()
    End Sub
#End Region

#Region "UI Loading Operations"
    Private Sub SplashScreen_Load(sender As Object, e As EventArgs) Handles Me.Load
        Randomize()
        Me.TopMost = False
        Me.ShowInTaskbar = True
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
        Timer1.Start()
        Timer2.Start()
        paintTextOverlay()
    End Sub

    Private Function paintTextOverlay()
        Dim x As SolidBrush = Brushes.White
        Dim ffont As Font = New Font("Arial", 48)
        Dim g As Graphics = Graphics.FromImage(PictureBox1.Image)
        g.DrawString("Initializing...", ffont, x, 128, 0)
        'g.Flush()
    End Function

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Me.UpdateZOrder()
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Me.ProgressBar1.Value = CurrentValue
        Timer2.Start()
    End Sub
#End Region
#End Region


End Class
