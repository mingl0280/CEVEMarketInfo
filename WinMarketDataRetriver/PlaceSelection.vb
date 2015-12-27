Imports System.Data.OleDb
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Threading
Public Class PlaceSelection

#Region "Private Delegations and Declearations"
    Private Delegate Sub Deleg_change(ByRef c As Boolean)
    Private Delegate Sub Deleg_SetCurrent(ByRef id As Integer, ByRef i As String)
    Private Delegate Sub Deleg_SetBox(ByRef BoxID As Integer, ByRef Values As List(Of String))
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
        Dim RGList, SysList As New List(Of String)
        conn.Open()
        listtable = New DataTable()
        rder = New OleDbDataAdapter(cmdspst)
        rder.Fill(listtable)
        SystemList.Clear()
        SystemList = New Dictionary(Of String, RegionInfo)
        For i As Integer = 0 To listtable.Rows.Count - 1
            Dim SysInfoData As New RegionInfo(listtable(i)("SystemID"), listtable(i)("SystemName"), listtable(i)("RegionID"), listtable(i)("RegionName"))
            SystemList.Add(listtable(i)("SystemName"), SysInfoData)
            Try
                RegionList.Add(listtable(i)("RegionName"), SysInfoData)
            Catch ex As Exception
            End Try
        Next
        For Each i In SystemList
            If Not RGList.Contains(i.Value.RegionName) Then
                RGList.Add(i.Value.RegionName)
                'Me.Invoke(New Deleg_add(AddressOf AddItemToComboBox), 1, i.Value.RegionName)
            End If
            'Me.Invoke(New Deleg_add(AddressOf AddItemToComboBox), 2, i.Key)
            SysList.Add(i.Key)
        Next
        Me.Invoke(New Deleg_change(AddressOf ChangeCover), False)
        Me.Invoke(New Deleg_SetBox(AddressOf SetComboBoxItems), 1, RGList)
        Me.Invoke(New Deleg_SetBox(AddressOf SetComboBoxItems), 2, SysList)
        Me.Invoke(New Deleg_SetCurrent(AddressOf SetCurrent), 1, SelectedRegion.RegionName)
        Me.Invoke(New Deleg_SetCurrent(AddressOf SetCurrent), 2, SelectedRegion.SystemName)
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

    Private Sub SetCurrent(ByRef BoxID As Integer, ByRef i As String)
        Select Case BoxID
            Case 1
                ComboBox1.SelectedItem = i
                ComboBox1.Text = i
            Case 2
                ComboBox2.SelectedItem = i
                ComboBox2.Text = i
        End Select
    End Sub

    Private Sub AddItemToComboBox(ByRef BoxID As Integer, ByRef TextValue As String)
        Select Case BoxID
            Case 1
                ComboBox1.Items.Add(TextValue)
            Case 2
                ComboBox2.Items.Add(TextValue)
        End Select
    End Sub

    Private Sub SetComboBoxItems(ByRef ID As Integer, ByRef Values As List(Of String))
        Select Case ID
            Case 1
                ComboBox1.Items.AddRange(Values.ToArray)
            Case 2
                ComboBox2.Items.AddRange(Values.ToArray)
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
            SelectedRegion = New RegionInfo("", "", "", "")
            If ComboBox1.Text <> "" And ComboBox2.Text = "" Then
                Form1.Label2.Text = "已选择：" & ComboBox1.Text & "星域"
                szname = ComboBox1.Text
                SelectedRegion.RegionName = ComboBox1.Text
                SelectedRegion.RegionID = RegionList(ComboBox1.Text).RegionID
            End If
            If ComboBox1.Text = "" And ComboBox2.Text <> "" Then
                Form1.Label2.Text = "已选择：" & ComboBox2.Text & "星系"
                spstname = ComboBox2.Text
                SelectedRegion.SystemID = SystemList(ComboBox2.Text).SystemID
                SelectedRegion.SystemName = ComboBox2.Text
            End If
            If ComboBox1.Text <> "" And ComboBox2.Text <> "" Then
                szname = ComboBox1.Text
                spstname = ComboBox2.Text
                SelectedRegion.RegionName = ComboBox1.Text
                SelectedRegion.RegionID = RegionList(ComboBox1.Text).RegionID
                SelectedRegion.SystemID = SystemList(ComboBox2.Text).SystemID
                SelectedRegion.SystemName = ComboBox2.Text
                Form1.Label2.Text = "已选择：位于 " & ComboBox1.Text & " 星域的 " & ComboBox2.Text & " 星系"
            End If
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
        If ComboBox1.Text = "" And ComboBox2.Text <> "" Then
            If ComboBox2.Items.Contains(ComboBox2.Text) Then Return True Else Return False
        End If
        If ComboBox1.Text <> "" And ComboBox2.Text = "" Then
            If ComboBox1.Items.Contains(ComboBox1.Text) Then Return True Else Return False
        End If
        If ComboBox1.Text <> "" And ComboBox2.Text <> "" Then
            Dim GetReg = SystemList(ComboBox2.Text).RegionName
            If GetReg <> ComboBox1.Text Then
                Return False
            End If
        End If
        Return True
    End Function
#End Region

End Class