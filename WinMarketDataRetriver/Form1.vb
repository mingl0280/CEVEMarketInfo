Imports System.Diagnostics
Imports System.Diagnostics.Process
Imports System.Text
Imports System.Text.Encoding
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Threading
Imports System.Windows.Forms
Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices


Public Class Form1

#Region "Private Data Structure"
    Private Structure itemData
        Dim description As String
        Dim itemName As String

        ''' <summary>
        ''' item data
        ''' </summary>
        ''' <param name="a">description</param>
        ''' <param name="b">item Name</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal a As String, b As String)
            description = b
            itemName = a
        End Sub
    End Structure

#End Region

#Region "Global Vars and Delegations"
    Dim chkA, chkB As String
    Dim st, allowsearch, lengthI As Integer
    Dim isk As String
    Private itm As String
    Private selectdata As New Dictionary(Of Integer, DataSaved)
    Private onLoading As Boolean = True
    Public Delegate Sub OneStrParamDeleg(ByRef x As String)
    Public Delegate Sub TwoStrParamDeleg(ByRef StrA As String, ByRef StrB As String)
    Public Delegate Sub StrStrBolStrRGIParamDeleg(ByRef A As String, ByRef B As String, ByRef C As Boolean, ByRef D As String, ByRef E As RegionInfo)
    Private itemDataArray As New List(Of itemData)
    Private SPS As New SplashScreen
    Private HeadContent As String = My.Resources.HeadContent
#End Region

#Region "Operation Functions and Subs"

#Region "Initial Sequence Opreations"
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Visible = False
        If Not File.Exists(Application.StartupPath + "\eve_db.mdb") Then
            Dim Fst As Stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WinMarketDataRetriver.eve_db.mdb")
            Dim FF(Fst.Length) As Byte
            Fst.Read(FF, 0, Fst.Length)
            Dim ptrMem As IntPtr = Marshal.AllocCoTaskMem(FF.Length)
            Dim fdataByte(FF.Length) As Byte
            fdataByte = FF
            Marshal.Copy(fdataByte, 0, ptrMem, FF.Length)
            Dim DBFile As New FileStream(Application.StartupPath + "\eve_db.mdb", FileMode.Create)
            DBFile.Write(fdataByte, 0, Fst.Length)
            Fst.Close()
            DBFile.Close()
        End If
        st = 1
        RadioButton_Buy.Checked = True
        RadioButton_Sell.Checked = False
        Label4.Visible = False
        SPS.Show()
        Dim th As Thread = New Thread(AddressOf InitData)
        th.Start()
        WebBrowser1.DocumentText = My.Resources.HeadContentMain + _
            "<div style=""align:center;width:160px;margin-left:auto;margin-right:auto;overflow:none;""> " + _
            "<div style=""color:yellow;font-size:14px;"">欢迎使用价格查询工具!" + _
            "</div></div></body></html>"
        Me.Visible = True
        onLoading = False
    End Sub

    Public Sub InitData()
        Me.Invoke(New OneStrParamDeleg(AddressOf WndE), "hide")
        Dim GetItemList As OleDbCommand = New OleDbCommand("select itemName,Description from invItems order by itemName", conn)
        Dim GetItemListCount As OleDbCommand = New OleDbCommand("select count(*) from invItems", conn)
        Dim listtable As DataTable = New DataTable()
        Dim rder As OleDbDataAdapter = New OleDbDataAdapter(GetItemList)
        Dim lines As Integer
        Dim xi As Integer = 0
        Dim isFirstCreate As Boolean = True
        Try
            conn.Close()
        Catch ex As Exception
        End Try
        conn.Open()
        lines = GetItemListCount.ExecuteScalar()
        SPS.Invoke(New SplashScreen.OneIntParamDeleg(AddressOf SPS.SetPBMax), lines)
        rder.Fill(listtable)
        For i As Integer = 0 To lines - 1
            Dim ItemNameStr = listtable(i)("itemName").ToString
            Dim ItemDescriptionStr = listtable(i)("Description").ToString.Replace(vbCrLf, "<br />").Replace(vbCr, "<br />").Replace(vbLf, "<br />")
            itemDataArray.Add(New itemData(ItemNameStr, ItemDescriptionStr))
            Me.Invoke(New OneStrParamDeleg(AddressOf AddComboboxItem), ItemNameStr)
            SPS.Invoke(New SplashScreen.NonParamDeleg(AddressOf SPS.AddOneToProgressBar))
        Next
        SPS.Invoke(New SplashScreen.NonParamDeleg(AddressOf SPS.Hide))
        Me.Invoke(New OneStrParamDeleg(AddressOf WndE), "show")
        conn.Close()
    End Sub

    Private Sub WndE(ByRef t As String)
        Select Case t
            Case "show"
                Me.Show()
                SPS.Hide()
            Case "hide"
                Me.Hide()
                SPS.Show()
        End Select
    End Sub
#End Region

#Region "Query Opeartions"
    Private Sub NormalQuery()
        Dim t As Thread = New Thread(AddressOf Query)
        itm = ComboBox1.Text
        t.Start()
        ComboBox1.Enabled = False
        'Button1.Enabled = False
        Button2.Enabled = False
        GroupBox1.Enabled = False
        Label4.Visible = True
    End Sub

    Public Function TrialQuery() As Boolean
        Dim QueryStr As OleDbCommand = New OleDbCommand("select count(*) from invItems where itemName like '%" + ComboBox1.Text + "%'", conn)
        conn.Open()
        Try
            Dim count As Integer = QueryStr.ExecuteScalar
            If count > 0 Then
                conn.Close()
                Return True
            End If

        Catch ex As Exception
            'MessageBox.Show("SQL查询错误！请重试！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            conn.Close()
        End Try
        conn.Close()
        Return False
    End Function

    Private Sub Query()
        Dim BuyOrSell As Boolean = False
        Try
            isk = GetValue(chkA, st, SelectedRegion.RegionID, SelectedRegion.SystemID)
            Dim repl, tm As String
            tm = Date.Now.ToLongDateString & Date.Now.ToLongTimeString
            repl = "在 " & tm & " 查询了"
            If SelectedRegion.RegionName <> "" Then
                repl = repl & " " & SelectedRegion.RegionName & " 星域"
            End If
            If SelectedRegion.SystemName <> "" Then
                repl = repl & " " & SelectedRegion.SystemName & " 星系"
            End If
            repl = repl & " " & itm & "的"
            If RadioButton_Buy.Checked = True Then
                repl = repl & "最低卖出价格（卖单最低价），为"
                BuyOrSell = False
            ElseIf RadioButton_Sell.Checked = True Then
                repl = repl & "最高可卖出价格（买单最高价），为"
                BuyOrSell = True
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
            Me.Invoke(New StrStrBolStrRGIParamDeleg(AddressOf updateCheckList), repl, isk, BuyOrSell, tm, SelectedRegion)
        Catch ex As Exception
            MessageBox.Show("Some error has occured: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function SplitFuzzyQueryResult(ByVal bstr As String) As String()
        Dim retArray() As String
        retArray = bstr.Split("|")
        Return retArray
    End Function



    Private Function Reverse(ByRef s As String)
        Dim t As String = ""
        For i = s.Length - 1 To 0 Step -1
            t += s(i)
        Next
        Return t
    End Function

    Public Sub AddComboboxItem(ByRef x As String)
        ComboBox1.Items.Add(x)
    End Sub

    Private Sub updateCheckList(ByRef ret As String, ByRef isk As String, ByRef BoS As Boolean, ByRef QT As String, ByRef RG As RegionInfo)
        lengthI = ListBox1.Items.Add(ret)
        selectdata.Add(lengthI, New DataSaved(QT, lengthI, isk, ComboBox1.Text, BoS, RG))
        ComboBox1.Enabled = True
        'Button1.Enabled = True
        Button2.Enabled = True
        GroupBox1.Enabled = True
        Label4.Visible = False
    End Sub
#End Region

    ''' <summary>
    ''' Browser and Selections
    ''' </summary>
    ''' <param name="s">Item Name</param>
    ''' <returns>Item Description as full HTML</returns>
    ''' <remarks></remarks>
    Private Function findItemIndex(ByVal s As String)
        Return checkvalue(s, 6)
    End Function

#End Region

#Region "UI Controls"
#Region "Buttons And Link Lable Operations"

    Private Sub Button_ClearLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_ClearLog.Click
        ListBox1.Items.Clear()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label2.Text = "未选择地点默认全部星域!"
        syst = ""
        sz = ""
        szname = ""
        spstname = ""
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        PlaceSelection.StartPosition = FormStartPosition.CenterParent
        PlaceSelection.Show()
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Start("http://www.ceve-market.org")
    End Sub

    Private Sub Button_GoQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_GoQuery.Click
        If ComboBox1.Text = "" Then Exit Sub
        chkA = checkvalue(ComboBox1.Text, 1)
        If chkA = "-1" Then
            If TrialQuery() Then
                Dim dlgResult As DialogResult = FuzzyQuery.ShowDialog
                If dlgResult = DialogResult.None Or dlgResult = Windows.Forms.DialogResult.Cancel Then
                    Exit Sub
                Else
                    ComboBox1.Text = SplitFuzzyQueryResult(FZRetstr)(1)
                    ComboBox1.SelectedItem = ComboBox1.Text
                    chkA = checkvalue(ComboBox1.Text, 1)
                    NormalQuery()
                End If
            Else
                MsgBox("查无此物")
                ComboBox1.Text = ""
                Exit Sub
            End If
        Else
            NormalQuery()
        End If
    End Sub
#End Region

#Region "Notify Icon Operations"

    Private Sub 退出ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 退出ToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub 显示ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 显示ToolStripMenuItem.Click
        NotifyIcon1_MouseDoubleClick(sender, New MouseEventArgs(Windows.Forms.MouseButtons.Left, 2, 10, 10, 0))
    End Sub

    Protected Overrides Sub OnClosing(e As System.ComponentModel.CancelEventArgs)
        e.Cancel = True
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Form1_MinimumSizeChanged(sender As Object, e As EventArgs) Handles Me.Resize
        If onLoading = False Then
            If Me.WindowState = FormWindowState.Minimized Then
                Me.Hide()
            End If
        End If
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        If Me.WindowState = FormWindowState.Minimized Then
            Me.Show()
            Me.WindowState = FormWindowState.Normal
            Me.BringToFront()
        Else
            Me.WindowState = FormWindowState.Minimized
        End If
    End Sub

#End Region

#Region "Listbox1 Operations"
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

    Private Sub 全部复制ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 全部复制ToolStripMenuItem.Click
        Dim selectedi As Integer = 0
        selectedi = ListBox1.SelectedIndex
        Clipboard.Clear()
        Dim p As String = ""
        For Each i In ListBox1.SelectedIndices
            If CheckBox1.Checked = True Then
                p += selectdata(i).QueryTime + vbTab
            End If
            If CheckBox2.Checked = True Then
                p += selectdata(i).QueryRegion.RegionName + vbTab
            End If
            If CheckBox3.Checked = True Then
                p += selectdata(i).QueryRegion.SystemName + vbTab
            End If
            If CheckBox4.Checked = True Then
                p += selectdata(i).itemName + vbTab
            End If
            If CheckBox5.Checked = True Then
                p += If(selectdata(i).itemPriceType = False, "卖单最低价", "买单最高价") + vbTab
            End If
            If CheckBox6.Checked = True Then
                p += selectdata(i).itemISK
            End If
            p += vbCrLf
        Next
        Clipboard.SetData(DataFormats.Text, p)
    End Sub

    Private Sub 清除ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 清除ToolStripMenuItem.Click
        ListBox1.Items.Clear()
    End Sub
#End Region

#Region "Radio Button Operations"
    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_Buy.CheckedChanged
        If RadioButton_Buy.Checked = True Then st = 1
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_Sell.CheckedChanged
        If RadioButton_Sell.Checked = True Then st = 2
    End Sub
#End Region

#Region "Combobox1 Opeartions"
    Private Sub ComboBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ComboBox1.KeyPress
        If e.KeyChar = Chr(13) Then
            Button_GoQuery.PerformClick()
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim derText = itemDataArray(ComboBox1.SelectedIndex).description.Replace("<font size=""14"">", "")
        derText = derText.Replace("</font>", "")
        derText.Replace("  ", "<br />")
        WebBrowser1.DocumentText = HeadContent + derText + "</div></body></html>"
    End Sub
#End Region

#Region "WebBrowser1 Operations"
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
#End Region

#End Region

End Class
