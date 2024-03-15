<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class XinputForce
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(XinputForce))
        Me.Label_XViblate = New System.Windows.Forms.Label()
        Me.ViblateBar = New System.Windows.Forms.TrackBar()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label_XConstMax = New System.Windows.Forms.Label()
        Me.ConstMaxBar = New System.Windows.Forms.TrackBar()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label_XThreshold = New System.Windows.Forms.Label()
        Me.ThresholdBar = New System.Windows.Forms.TrackBar()
        Me.Label3 = New System.Windows.Forms.Label()
        CType(Me.ViblateBar, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ConstMaxBar, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ThresholdBar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label_XViblate
        '
        Me.Label_XViblate.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_XViblate.ForeColor = System.Drawing.Color.White
        Me.Label_XViblate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label_XViblate.Location = New System.Drawing.Point(364, 90)
        Me.Label_XViblate.Name = "Label_XViblate"
        Me.Label_XViblate.Size = New System.Drawing.Size(40, 20)
        Me.Label_XViblate.TabIndex = 83
        Me.Label_XViblate.Text = "888"
        Me.Label_XViblate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ViblateBar
        '
        Me.ViblateBar.AutoSize = False
        Me.ViblateBar.Location = New System.Drawing.Point(162, 88)
        Me.ViblateBar.Maximum = 100
        Me.ViblateBar.Name = "ViblateBar"
        Me.ViblateBar.Size = New System.Drawing.Size(196, 36)
        Me.ViblateBar.TabIndex = 82
        Me.ViblateBar.TickFrequency = 20
        Me.ViblateBar.Value = 100
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(12, 86)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(144, 25)
        Me.Label6.TabIndex = 81
        Me.Label6.Text = "ViblateMax"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label_XConstMax
        '
        Me.Label_XConstMax.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_XConstMax.ForeColor = System.Drawing.Color.White
        Me.Label_XConstMax.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label_XConstMax.Location = New System.Drawing.Point(364, 49)
        Me.Label_XConstMax.Name = "Label_XConstMax"
        Me.Label_XConstMax.Size = New System.Drawing.Size(40, 20)
        Me.Label_XConstMax.TabIndex = 80
        Me.Label_XConstMax.Text = "888"
        Me.Label_XConstMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ConstMaxBar
        '
        Me.ConstMaxBar.AutoSize = False
        Me.ConstMaxBar.Location = New System.Drawing.Point(162, 47)
        Me.ConstMaxBar.Maximum = 100
        Me.ConstMaxBar.Name = "ConstMaxBar"
        Me.ConstMaxBar.Size = New System.Drawing.Size(196, 36)
        Me.ConstMaxBar.TabIndex = 79
        Me.ConstMaxBar.TickFrequency = 20
        Me.ConstMaxBar.Value = 100
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(12, 45)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(144, 25)
        Me.Label4.TabIndex = 78
        Me.Label4.Text = "ConstForceMax"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label_XThreshold
        '
        Me.Label_XThreshold.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_XThreshold.ForeColor = System.Drawing.Color.White
        Me.Label_XThreshold.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label_XThreshold.Location = New System.Drawing.Point(364, 13)
        Me.Label_XThreshold.Name = "Label_XThreshold"
        Me.Label_XThreshold.Size = New System.Drawing.Size(40, 20)
        Me.Label_XThreshold.TabIndex = 77
        Me.Label_XThreshold.Text = "888"
        Me.Label_XThreshold.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ThresholdBar
        '
        Me.ThresholdBar.AutoSize = False
        Me.ThresholdBar.Location = New System.Drawing.Point(162, 11)
        Me.ThresholdBar.Maximum = 100
        Me.ThresholdBar.Name = "ThresholdBar"
        Me.ThresholdBar.Size = New System.Drawing.Size(196, 36)
        Me.ThresholdBar.TabIndex = 76
        Me.ThresholdBar.TickFrequency = 20
        Me.ThresholdBar.Value = 100
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(12, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(144, 25)
        Me.Label3.TabIndex = 75
        Me.Label3.Text = "ConstForceThreshold"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'XinputForce
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(147, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(427, 128)
        Me.Controls.Add(Me.Label_XViblate)
        Me.Controls.Add(Me.ViblateBar)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label_XConstMax)
        Me.Controls.Add(Me.ConstMaxBar)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label_XThreshold)
        Me.Controls.Add(Me.ThresholdBar)
        Me.Controls.Add(Me.Label3)
        Me.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "XinputForce"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Xinput"
        CType(Me.ViblateBar, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ConstMaxBar, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ThresholdBar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Label_XViblate As Label
    Friend WithEvents ViblateBar As TrackBar
    Friend WithEvents Label6 As Label
    Friend WithEvents Label_XConstMax As Label
    Friend WithEvents ConstMaxBar As TrackBar
    Friend WithEvents Label4 As Label
    Friend WithEvents Label_XThreshold As Label
    Friend WithEvents ThresholdBar As TrackBar
    Friend WithEvents Label3 As Label
End Class
