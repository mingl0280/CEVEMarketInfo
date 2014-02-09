Imports System.Diagnostics
Imports System.Diagnostics.Process
Imports System.Text
Imports System.Text.Encoding
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Threading

Public Class Form1
    Private Structure itemData
        Dim description As String
        Dim itemName As String
    End Structure

    Dim chkA, chkB As String
    Dim st, allowsearch, lengthI As Integer
    Dim isk As String
    Private itm As String
    Private selectdata As DataSaved()
    Public Delegate Sub OneStrParamDeleg(ByRef x As String)
    Public Delegate Sub TwoStrParamDeleg(ByRef StrA As String, ByRef StrB As String)
    Private itemDataArray() As itemData

    Private HeadContent As String = My.Resources.HeadContent

    Public Sub InitData()
        Me.Invoke(New OneStrParamDeleg(AddressOf WndE), "hide")
        Dim conn As OleDbConnection = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=eve_db.mdb")
        Dim GetItemList As OleDbCommand = New OleDbCommand("select 物品名称 from 物品列表 order by 物品名称", conn)
        Dim GetItemDescription As OleDbCommand = New OleDbCommand("select 描述 from 物品列表 order by 物品名称", conn)
        Dim GetItemListCount As OleDbCommand = New OleDbCommand("select count(*) from 物品列表", conn)
        Dim szList(), DescList() As String
        Dim listtable As DataTable = New DataTable()
        Dim desctable As DataTable = New DataTable
        Dim rder As OleDbDataAdapter = New OleDbDataAdapter(GetItemList)
        Dim desc As OleDbDataAdapter = New OleDbDataAdapter(GetItemDescription)
        Dim lines As Integer
        Dim xi As Integer = 0
        conn.Open()
        lines = GetItemListCount.ExecuteScalar()
        rder.Fill(listtable)
        desc.Fill(desctable)
        ReDim szList(lines)
        ReDim DescList(lines)
        ReDim itemDataArray(lines)
        For i As Integer = 0 To lines - 1
            szList(i) = listtable(i)(0).ToString
            DescList(i) = desctable(i)(0).ToString
            DescList(i).Replace(vbCrLf, "<br />")
            DescList(i).Replace(vbCr, "<br />")
            DescList(i).Replace(vbLf, "<br />")
            itemDataArray(i).itemName = szList(i)
            itemDataArray(i).description = DescList(i)

            Me.Invoke(New OneStrParamDeleg(AddressOf AddComboboxItem), szList(i))
        Next
        SplashScreen.Hide()
        Me.Invoke(New OneStrParamDeleg(AddressOf WndE), "show")
    End Sub

    Public Sub AddComboboxItem(ByRef x As String)
        ComboBox1.Items.Add(x)
    End Sub

    Private Sub WndE(ByRef t As String)
        Select Case t
            Case "show"
                Me.Enabled = True
                SplashScreen.Hide()
            Case "hide"
                Me.Enabled = False
                SplashScreen.Show()
        End Select
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.Hide()
        'Button_GoQuery.Enabled = False
        st = 1
        RadioButton_Buy.Checked = True
        RadioButton_Sell.Checked = False
        Label4.Visible = False
        SplashScreen.Show()
        Dim th As Thread = New Thread(AddressOf InitData)
        th.Start()
        'Me.Enabled = False
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        PlaceSelection.Show()
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Start("http://www.ceve-market.org")
    End Sub

    Private Sub Button_GoQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_GoQuery.Click
        If ComboBox1.Text = "" Then Exit Sub
        chkA = checkvalue(ComboBox1.Text, 1)
        If chkA = "-1" Then
            MsgBox("查无此物")
            ComboBox1.Text = ""
            Exit Sub
        Else
            Dim t As Thread = New Thread(AddressOf Query)
            itm = ComboBox1.Text
            t.Start()
            ComboBox1.Enabled = False
            'Button1.Enabled = False
            Button2.Enabled = False
            GroupBox1.Enabled = False
            Label4.Visible = True
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_Buy.CheckedChanged
        st = 1
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_Sell.CheckedChanged
        st = 2
    End Sub

    Private Sub Query()

        isk = GetValue(chkA, st)
        Dim repl, tm As String
        tm = Date.Now.ToLongDateString & Date.Now.ToLongTimeString
        repl = "在 " & tm & " 查询了"
        If szname <> "" Then
            repl = repl & " " & szname & " 星域"
        End If
        If spstname <> "" Then
            repl = repl & " " & spstname & " 星系"
        End If
        repl = repl & " " & itm & "的"
        If RadioButton_Buy.Checked = True Then
            repl = repl & "最低卖出价格（最低可购买价格），为"
        ElseIf RadioButton_Sell.Checked = True Then
            repl = repl & "最高可卖出价格（最高收购价格），为"
        End If
        Dim s As String = Reverse(isk)
        Dim sx As String = ""
        sx = s(0) + s(1) + s(2)
        For i As Integer = 3 To s.Length - 1
            sx += s(i)
            Try
                If (i + 1) Mod 3 = 0 And s(i + 1) <> Nothing Then
                    sx += ","
                End If
            Catch ex As Exception
            End Try
        Next
        isk = Reverse(sx)
        repl = repl & isk & " ISK"
        Dim obj(2) As String
        obj(0) = repl
        obj(1) = isk
        Me.Invoke(New TwoStrParamDeleg(AddressOf updateCheckList), repl, isk)
    End Sub

    Private Sub updateCheckList(ByRef ret As String, ByRef isk As String)
        lengthI = ListBox1.Items.Add(ret) + 1
        ReDim Preserve selectdata(lengthI)
        selectdata(lengthI - 1).itemID = lengthI - 1
        selectdata(lengthI - 1).itemName = ComboBox1.Text
        selectdata(lengthI - 1).itemISK = isk
        ComboBox1.Enabled = True
        'Button1.Enabled = True
        Button2.Enabled = True
        GroupBox1.Enabled = True
        Label4.Visible = False
    End Sub

    Private Function Reverse(ByRef s As String)
        Dim t As String = ""
        For i = s.Length - 1 To 0 Step -1
            t += s(i)
        Next
        Return t
    End Function

    Private Sub Button_ClearLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_ClearLog.Click
        ListBox1.Items.Clear()
    End Sub

    Private Sub 复制物品名称ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 复制物品名称ToolStripMenuItem.Click
        Dim selectedi As Integer = 0
        selectedi = ListBox1.SelectedIndex
        Clipboard.Clear()
        Clipboard.SetData(DataFormats.Text, selectdata(selectedi).itemName)
    End Sub

    Private Sub 复制当前ISKToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 复制当前ISKToolStripMenuItem.Click
        Dim selectedi As Integer = 0
        selectedi = ListBox1.SelectedIndex
        Clipboard.Clear()
        Clipboard.SetData(DataFormats.Text, selectdata(selectedi).itemISK)
    End Sub

    Private Sub 全部复制ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 全部复制ToolStripMenuItem.Click
        Dim selectedi As Integer = 0
        selectedi = ListBox1.SelectedIndex
        Clipboard.Clear()
        Dim p As String = ""
        For i As Integer = 0 To ListBox1.SelectedItems.Count - 1
            p += ListBox1.SelectedItems(i) + vbCrLf
        Next
        Clipboard.SetData(DataFormats.Text, p)
    End Sub

    Private Sub ListBox1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListBox1.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Dim ht As Integer = ListBox1.ItemHeight
            Dim rect As New Rectangle _
                (0, 0, ListBox1.ClientSize.Width, ht)
            For i As Integer = 0 To ListBox1.Items.Count - 1
                If rect.Contains(e.Location) Then
                    ListBox1.SelectedIndex = i + ListBox1.TopIndex
                    Exit For
                Else
                    rect.Y += ht
                End If
            Next
        End If
    End Sub

    Private Sub ComboBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ComboBox1.KeyPress
        If e.KeyChar = Chr(13) Then
            Button_GoQuery.PerformClick()
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        WebBrowser1.DocumentText = HeadContent + itemDataArray(ComboBox1.SelectedIndex).description + "</div></body></html>"
    End Sub

    Private Sub WebBrowser1_Navigating(sender As Object, e As WebBrowserNavigatingEventArgs) Handles WebBrowser1.Navigating
        If Not e.Url.OriginalString = "about:blank" Then
            If Not e.Url.OriginalString.IndexOf("showinfo") >= 0 Then
                System.Diagnostics.Process.Start(e.Url.OriginalString)
                e.Cancel = True
                WebBrowser1.Stop()
            ElseIf e.Url.OriginalString.IndexOf("showinfo:") >= 0 Then
                Dim spt As String() = e.Url.OriginalString.Split(":")
                ComboBox1.SelectedItem = findItemIndex(spt(1))
                'ComboBox1_SelectedIndexChanged(sender, EventArgs.Empty)
                e.Cancel = True
                WebBrowser1.Stop()
            End If
        End If
    End Sub
    Private Function findItemIndex(ByVal s As String)
        Return checkvalue(s, 6)
    End Function
End Class
