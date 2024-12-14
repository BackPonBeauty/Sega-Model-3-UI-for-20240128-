<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class loading
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(loading))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(0, 40)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(140, 18)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Loading..."
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.Color.Black
        Me.Button1.Location = New System.Drawing.Point(38, 70)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(64, 24)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        Me.Timer1.Interval = 3000
        '
        'Timer2
        '
        Me.Timer2.Interval = 3000
        '
        'loading
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(147, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(140, 100)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "loading"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Loading..."
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Timer2 As Timer
End Class
