<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class captureForm
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
        Me.PictureBox = New System.Windows.Forms.PictureBox()
        Me.FPS_Timer = New System.Windows.Forms.Timer(Me.components)
        Me.FPS_Label = New System.Windows.Forms.Label()
        CType(Me.PictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox
        '
        Me.PictureBox.BackColor = System.Drawing.Color.Black
        Me.PictureBox.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox.Name = "PictureBox"
        Me.PictureBox.Size = New System.Drawing.Size(960, 540)
        Me.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox.TabIndex = 0
        Me.PictureBox.TabStop = False
        '
        'FPS_Timer
        '
        Me.FPS_Timer.Enabled = True
        Me.FPS_Timer.Interval = 1000
        '
        'FPS_Label
        '
        Me.FPS_Label.BackColor = System.Drawing.Color.Black
        Me.FPS_Label.Font = New System.Drawing.Font("hooge 05_57", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FPS_Label.ForeColor = System.Drawing.Color.Green
        Me.FPS_Label.Location = New System.Drawing.Point(-3, 0)
        Me.FPS_Label.Name = "FPS_Label"
        Me.FPS_Label.Size = New System.Drawing.Size(29, 18)
        Me.FPS_Label.TabIndex = 50
        Me.FPS_Label.Text = "888"
        Me.FPS_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'captureForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 22.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Gray
        Me.ClientSize = New System.Drawing.Size(960, 540)
        Me.Controls.Add(Me.FPS_Label)
        Me.Controls.Add(Me.PictureBox)
        Me.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
        Me.Name = "captureForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "capture"
        CType(Me.PictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PictureBox As PictureBox
    Friend WithEvents FPS_Timer As Timer
    Friend WithEvents FPS_Label As Label
End Class
