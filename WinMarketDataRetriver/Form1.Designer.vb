<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.复制物品名称ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.复制当前ISKToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.全部复制ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button_GoQuery = New System.Windows.Forms.Button()
        Me.Button_ClearLog = New System.Windows.Forms.Button()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.RadioButton_Sell = New System.Windows.Forms.RadioButton()
        Me.RadioButton_Buy = New System.Windows.Forms.RadioButton()
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
        Me.LineShape1 = New Microsoft.VisualBasic.PowerPacks.LineShape()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 12)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "物品："
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(11, 150)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(65, 12)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "查询记录："
        '
        'ListBox1
        '
        Me.ListBox1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.HorizontalScrollbar = True
        Me.ListBox1.ItemHeight = 12
        Me.ListBox1.Location = New System.Drawing.Point(13, 165)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.ListBox1.Size = New System.Drawing.Size(757, 172)
        Me.ListBox1.TabIndex = 8
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.复制物品名称ToolStripMenuItem, Me.复制当前ISKToolStripMenuItem, Me.全部复制ToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(147, 70)
        '
        '复制物品名称ToolStripMenuItem
        '
        Me.复制物品名称ToolStripMenuItem.Name = "复制物品名称ToolStripMenuItem"
        Me.复制物品名称ToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
        Me.复制物品名称ToolStripMenuItem.Text = "复制物品名称"
        '
        '复制当前ISKToolStripMenuItem
        '
        Me.复制当前ISKToolStripMenuItem.Name = "复制当前ISKToolStripMenuItem"
        Me.复制当前ISKToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
        Me.复制当前ISKToolStripMenuItem.Text = "复制当前ISK"
        '
        '全部复制ToolStripMenuItem
        '
        Me.全部复制ToolStripMenuItem.Name = "全部复制ToolStripMenuItem"
        Me.全部复制ToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
        Me.全部复制ToolStripMenuItem.Text = "复制选中项"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(143, 55)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 6
        Me.Button2.Text = "选择地点"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button_GoQuery
        '
        Me.Button_GoQuery.Location = New System.Drawing.Point(333, 11)
        Me.Button_GoQuery.Name = "Button_GoQuery"
        Me.Button_GoQuery.Size = New System.Drawing.Size(75, 23)
        Me.Button_GoQuery.TabIndex = 2
        Me.Button_GoQuery.Text = "查询"
        Me.Button_GoQuery.UseVisualStyleBackColor = True
        '
        'Button_ClearLog
        '
        Me.Button_ClearLog.Location = New System.Drawing.Point(143, 99)
        Me.Button_ClearLog.Name = "Button_ClearLog"
        Me.Button_ClearLog.Size = New System.Drawing.Size(75, 23)
        Me.Button_ClearLog.TabIndex = 7
        Me.Button_ClearLog.Text = "清除记录"
        Me.Button_ClearLog.UseVisualStyleBackColor = True
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Location = New System.Drawing.Point(224, 104)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(185, 12)
        Me.LinkLabel1.TabIndex = 8
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "感谢EVE国服市场中心提供API支持"
        Me.LinkLabel1.VisitedLinkColor = System.Drawing.Color.Blue
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RadioButton_Sell)
        Me.GroupBox1.Controls.Add(Me.RadioButton_Buy)
        Me.GroupBox1.Location = New System.Drawing.Point(15, 50)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(122, 78)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "买入/卖出选择"
        '
        'RadioButton_Sell
        '
        Me.RadioButton_Sell.AutoSize = True
        Me.RadioButton_Sell.Location = New System.Drawing.Point(19, 44)
        Me.RadioButton_Sell.Name = "RadioButton_Sell"
        Me.RadioButton_Sell.Size = New System.Drawing.Size(47, 16)
        Me.RadioButton_Sell.TabIndex = 5
        Me.RadioButton_Sell.TabStop = True
        Me.RadioButton_Sell.Text = "卖出"
        Me.RadioButton_Sell.UseVisualStyleBackColor = True
        '
        'RadioButton_Buy
        '
        Me.RadioButton_Buy.AutoSize = True
        Me.RadioButton_Buy.Location = New System.Drawing.Point(19, 22)
        Me.RadioButton_Buy.Name = "RadioButton_Buy"
        Me.RadioButton_Buy.Size = New System.Drawing.Size(47, 16)
        Me.RadioButton_Buy.TabIndex = 4
        Me.RadioButton_Buy.TabStop = True
        Me.RadioButton_Buy.Text = "买入"
        Me.RadioButton_Buy.UseVisualStyleBackColor = True
        '
        'ShapeContainer1
        '
        Me.ShapeContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer1.Name = "ShapeContainer1"
        Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.LineShape1})
        Me.ShapeContainer1.Size = New System.Drawing.Size(782, 351)
        Me.ShapeContainer1.TabIndex = 13
        Me.ShapeContainer1.TabStop = False
        '
        'LineShape1
        '
        Me.LineShape1.Name = "LineShape1"
        Me.LineShape1.X1 = 11
        Me.LineShape1.X2 = 767
        Me.LineShape1.Y1 = 135
        Me.LineShape1.Y2 = 135
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("宋体", 40.0!)
        Me.Label4.Location = New System.Drawing.Point(233, 230)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(293, 54)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "查询中……"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(143, 84)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(143, 12)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "未选择地点默认全部星域!"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Red
        Me.Label5.Location = New System.Drawing.Point(224, 60)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(173, 12)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "警告：人少的星域可能数据不准"
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(60, 12)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(267, 20)
        Me.ComboBox1.TabIndex = 1
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(414, 17)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(41, 12)
        Me.Label6.TabIndex = 19
        Me.Label6.Text = "描述："
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Location = New System.Drawing.Point(461, 12)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.Size = New System.Drawing.Size(307, 110)
        Me.WebBrowser1.TabIndex = 9
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(782, 351)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Button_GoQuery)
        Me.Controls.Add(Me.Button_ClearLog)
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me.WebBrowser1)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ShapeContainer1)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(798, 390)
        Me.MinimumSize = New System.Drawing.Size(798, 390)
        Me.Name = "Form1"
        Me.Text = "EVE市场查看器"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button_GoQuery As System.Windows.Forms.Button
    Friend WithEvents Button_ClearLog As System.Windows.Forms.Button
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton_Sell As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_Buy As System.Windows.Forms.RadioButton
    Friend WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
    Friend WithEvents LineShape1 As Microsoft.VisualBasic.PowerPacks.LineShape
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents 复制物品名称ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 复制当前ISKToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents 全部复制ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents WebBrowser1 As System.Windows.Forms.WebBrowser

End Class
