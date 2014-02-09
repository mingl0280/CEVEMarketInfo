Imports System.Data.OleDb
Imports System.Data.Sql
Imports System.Data.SqlClient
Public Class PlaceSelection
    Dim err As Integer = 0
    Private Sub Fillchart()
        Dim conn As OleDbConnection = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=eve_db.mdb")
        Dim cmdsz As OleDbCommand = New OleDbCommand("select 星域名称 from 星域列表 order by 星域名称", conn)
        Dim cmdspst As OleDbCommand = New OleDbCommand("select 星系名称 from 星系列表 order by 星系名称", conn)
        Dim cmdGetLines As OleDbCommand = New OleDbCommand("select count(*) from 星域列表", conn)
        Dim cmdGetLines2 As OleDbCommand = New OleDbCommand("select count(*) from 星系列表", conn)
        Dim szList(), SpstList() As String
        Dim listtable As DataTable = New DataTable()
        Dim rder As OleDbDataAdapter = New OleDbDataAdapter(cmdsz)
        Dim lines As Integer
        Dim xi As Integer = 0
        conn.Open()
        lines = cmdGetLines.ExecuteScalar
        rder.Fill(listtable)
        ReDim szList(lines)
        For i As Integer = 0 To lines - 1
            szList(i) = listtable(i)(0)
            ComboBox1.Items.Add(szList(i))
        Next

        lines = cmdGetLines2.ExecuteScalar
        listtable = New DataTable()
        rder = New OleDbDataAdapter(cmdspst)
        rder.Fill(listtable)
        ReDim SpstList(lines)
        For i As Integer = 0 To lines - 1
            SpstList(i) = listtable(i)(0)
            ComboBox2.Items.Add(SpstList(i))
        Next
        Label3.Visible = False
        Label4.Visible = False
    End Sub


    Private Sub PlaceSelection_Close(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.FormClosing

    End Sub
    Private Sub PlaceSelection_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Button3.Enabled = False
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        Fillchart()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ComboBox1.Text = ""
        ComboBox2.Text = ""
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim chk As String = checkvalue(ComboBox1.Text, 2)
        err = 0
        If ComboBox1.Text <> "" Then
            If chk = "-1" Then
                MsgBox("错误的星域!")
                ComboBox1.Text = ""
                err = 1
            End If
            sz = chk
        Else
            sz = ""
        End If
        If ComboBox2.Text <> "" Then
            chk = checkvalue(ComboBox2.Text, 5, ComboBox1.Text)
            If chk = "-1" Then
                MsgBox("错误的星系！")
                ComboBox2.Text = ""
                err = 2
            End If
            syst = chk
        Else
            spst = ""
        End If
        If (err <> 0) Then
            Exit Sub
        End If
        Button3.Enabled = True
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        szname = ""
        spstname = ""
        Button2_Click(sender, e)
        If err <> 0 Then
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
        End If
        Me.Close()
        Me.Dispose()
    End Sub


    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        Dim ss As String = checkvalue(ComboBox2.Text, 3)
        If ss = "-1" Then
            MsgBox("错误的星系")
        Else
            ComboBox1.Text = ss
        End If
    End Sub


End Class