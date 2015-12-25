Imports System.Windows.Forms
Imports System.Data.OleDb

Public Class FuzzyQuery

    Private DataTempSave() As DataSaved

    Private Sub Initial()
        Dim fuzzyqstr As String = Form1.ComboBox1.Text
        Dim FuzzyQuery As OleDbCommand = New OleDbCommand("select itemName,typeID from invItems where itemName like '%" + fuzzyqstr + "%'", conn)
        Dim FuzzyQueryCount As OleDbCommand = New OleDbCommand("select count(*) from invItems where itemName like '%" + fuzzyqstr + "%'", conn)
        Dim dtable As DataTable = New DataTable
        Dim oddAdapter As OleDbDataAdapter = New OleDbDataAdapter(FuzzyQuery)
        Dim count As Integer = 0
        conn.Open()
        count = FuzzyQueryCount.ExecuteScalar()
        oddAdapter.Fill(dtable)
        ReDim DataTempSave(count)
        For i As Integer = 0 To count - 1
            DataTempSave(i).itemName = dtable(i)(0)
            DataTempSave(i).itemID = dtable(i)(1)
            ComboBox1.Items.Add(DataTempSave(i).itemName)
        Next
        conn.Close()
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            FZRetstr = DataTempSave(ComboBox1.SelectedIndex).itemID.ToString + "|" + DataTempSave(ComboBox1.SelectedIndex).itemName
            Me.DialogResult = Windows.Forms.DialogResult.OK
        Catch ex As Exception
            Me.DialogResult = Windows.Forms.DialogResult.None
        End Try
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ComboBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles ComboBox1.KeyDown
        e.SuppressKeyPress = False
    End Sub

    Private Sub ComboBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ComboBox1.KeyPress
        e.KeyChar = Nothing
    End Sub

    Private Sub FuzzyQuery_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Clear()
        ComboBox1.Text = ""
        Initial()
    End Sub
End Class
