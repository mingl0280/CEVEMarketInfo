Imports System.Data.OleDb
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Threading
Public Class PlaceSelection

#Region "Private Delegations and Declearations"
    Private Delegate Sub Deleg_change(ByRef c As Boolean)
    Private Delegate Sub Deleg_add(ByRef BoxID As Integer, ByRef TextValue As String)
    Private sx, sy, w, h, stepmov As Integer
#End Region

#Region "UI Actions"
#Region "Window load functions and subs"
    Private Sub PlaceSelection_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim rnd As New Random()
        If rnd.Next(0, 10) >= 5 Then sx = 1 Else sx = -1
        If rnd.Next(0, 10) >= 5 Then sy = 1 Else sy = -1
        w = Label3.Width
        h = Label3.Height
        stepmov = 5
        Dim th As New Thread(AddressOf Fillchart)
        th.Start()
        Timer1.Start()
    End Sub

    Private Sub Fillchart()
        Dim cmdspst As OleDbCommand = New OleDbCommand("select SystemID,SystemName,RegionID,RegionName from mapSystemID order by systemName", conn)
        Dim listtable As DataTable = New DataTable()
        Dim rder As OleDbDataAdapter = New OleDbDataAdapter()
        Dim RegionList As New ArrayList()
        conn.Open()
        listtable = New DataTable()
        rder = New OleDbDataAdapter(cmdspst)
        rder.Fill(listtable)
        SystemList.Clear()
        SystemList = New Dictionary(Of String, RegionInfo)
        For i As Integer = 0 To listtable.Rows.Count - 1
            Dim SysInfoData As New RegionInfo(listtable(i)("SystemID"), listtable(i)("SystemName"), listtable(i)("RegionID"), listtable(i)("RegionName"))
            SystemList.Add(listtable(i)(1), SysInfoData)
        Next
        For Each i In SystemList
            If Not ComboBox1.Items.Contains(i.Value.RegionName) Then
                Me.Invoke(New Deleg_add(AddressOf AddItemToComboBox), 1, i.Value.RegionName)
            End If
            Me.Invoke(New Deleg_add(AddressOf AddItemToComboBox), 2, i.Key)
        Next
        Me.Invoke(New Deleg_change(AddressOf ChangeCover), False)
        conn.Close()
    End Sub

    Private Sub ChangeCover(ByRef i As Boolean)
        If i = False Then
            Label3.Visible = False
            Label4.Visible = False
            Timer1.Stop()
        Else
            Label3.Visible = True
            Label4.Visible = True
            Timer1.Start()
        End If
    End Sub

    Private Sub AddItemToComboBox(ByRef BoxID As Integer, ByRef TextValue As String)
        Select Case BoxID
            Case 1
                ComboBox1.Items.Add(TextValue)
            Case 2
                ComboBox2.Items.Add(TextValue)
        End Select
    End Sub
#End Region

#Region "Buttons Reactions"
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ComboBox1.Text = ""
        ComboBox2.Text = ""
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        szname = ""
        spstname = ""
        Dim err As Boolean = check()
        If err = False Then
            Exit Sub
        End If

        If ComboBox1.Text <> "" Or ComboBox2.Text <> "" Then
            If ComboBox1.Text <> "" And ComboBox2.Text = "" Then
                Form1.Label2.Text = "已选择：" & ComboBox1.Text & "星域"
                szname = ComboBox1.Text
            End If
            If ComboBox1.Text = "" And ComboBox2.Text <> "" Then
                Form1.Label2.Text = "已选择：" & ComboBox2.Text & "星系"
                spstname = ComboBox2.Text
            End If
            If ComboBox1.Text <> "" And ComboBox2.Text <> "" Then
                szname = ComboBox1.Text
                spstname = ComboBox2.Text
                Form1.Label2.Text = "已选择：位于 " & ComboBox1.Text & " 星域的 " & ComboBox2.Text & " 星系"
            End If
            SelectedRegion = SystemList(ComboBox2.Text)
            Me.DialogResult = Windows.Forms.DialogResult.OK
        ElseIf ComboBox1.Text = "" And ComboBox2.Text = "" Then
            SelectedRegion.RegionID = "-1"
            SelectedRegion.RegionName = ""
            SelectedRegion.SystemID = "-1"
            SelectedRegion.SystemName = ""
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
        End If
        Me.Close()
    End Sub
#End Region

#Region "Animate"
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Start()
        If sx + w > Me.Width Or sx <= 1 Then sx = -1 * sx
        If sy + w > Me.Height Or sy <= 1 Then sy = -1 * sx
        Label3.Left = Label3.Left + sx * stepmov
        Label3.Top = Label3.Top + sy * stepmov
    End Sub
#End Region
#End Region

#Region "Location Validation Operations"
    Private Sub ComboBox2_LostFocus(sender As Object, e As EventArgs) Handles ComboBox2.LostFocus, ComboBox2.TextChanged, ComboBox2.SelectedIndexChanged, ComboBox2.SelectedValueChanged
        On Error Resume Next
        If Not SystemList(ComboBox2.Text).RegionName = ComboBox1.Text Then
            ComboBox1.Text = SystemList(ComboBox2.Text).RegionName
        End If
    End Sub

    Private Function check() As Boolean
        If ComboBox1.Text <> "" And ComboBox2.Text <> "" Then
            Dim GetReg = SystemList(ComboBox2.Text).RegionName
            If GetReg <> ComboBox1.Text Then
                Return False
            End If
        ElseIf ComboBox1.Text = "" And ComboBox2.Text <> "" Then
            Return True
        ElseIf ComboBox1.Text = "" And ComboBox2.Text = "" Then
            Return True
        End If
        Return True
    End Function
#End Region

End Class