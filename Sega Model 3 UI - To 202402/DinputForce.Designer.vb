<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DinputForce
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DinputForce))
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ConstLeftBar = New System.Windows.Forms.TrackBar()
        Me.Label_ConstLeft = New System.Windows.Forms.Label()
        Me.Label_ConstRight = New System.Windows.Forms.Label()
        Me.ConsRightBar = New System.Windows.Forms.TrackBar()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label_Center = New System.Windows.Forms.Label()
        Me.SelfBar = New System.Windows.Forms.TrackBar()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label_Friction = New System.Windows.Forms.Label()
        Me.FrictionBar = New System.Windows.Forms.TrackBar()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label_Viblate = New System.Windows.Forms.Label()
        Me.ViblateBar = New System.Windows.Forms.TrackBar()
        Me.Label10 = New System.Windows.Forms.Label()
        CType(Me.ConstLeftBar, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ConsRightBar, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SelfBar, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FrictionBar, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ViblateBar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(12, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(141, 25)
        Me.Label3.TabIndex = 52
        Me.Label3.Text = "ConstForceLeftMax"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ConstLeftBar
        '
        Me.ConstLeftBar.AutoSize = False
        Me.ConstLeftBar.Location = New System.Drawing.Point(159, 11)
        Me.ConstLeftBar.Maximum = 100
        Me.ConstLeftBar.Name = "ConstLeftBar"
        Me.ConstLeftBar.Size = New System.Drawing.Size(196, 36)
        Me.ConstLeftBar.TabIndex = 67
        Me.ConstLeftBar.TickFrequency = 20
        Me.ConstLeftBar.Value = 100
        '
        'Label_ConstLeft
        '
        Me.Label_ConstLeft.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_ConstLeft.ForeColor = System.Drawing.Color.White
        Me.Label_ConstLeft.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label_ConstLeft.Location = New System.Drawing.Point(361, 13)
        Me.Label_ConstLeft.Name = "Label_ConstLeft"
        Me.Label_ConstLeft.Size = New System.Drawing.Size(40, 20)
        Me.Label_ConstLeft.TabIndex = 68
        Me.Label_ConstLeft.Text = "888"
        Me.Label_ConstLeft.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label_ConstRight
        '
        Me.Label_ConstRight.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_ConstRight.ForeColor = System.Drawing.Color.White
        Me.Label_ConstRight.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label_ConstRight.Location = New System.Drawing.Point(361, 49)
        Me.Label_ConstRight.Name = "Label_ConstRight"
        Me.Label_ConstRight.Size = New System.Drawing.Size(40, 20)
        Me.Label_ConstRight.TabIndex = 71
        Me.Label_ConstRight.Text = "888"
        Me.Label_ConstRight.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ConsRightBar
        '
        Me.ConsRightBar.AutoSize = False
        Me.ConsRightBar.Location = New System.Drawing.Point(159, 47)
        Me.ConsRightBar.Maximum = 100
        Me.ConsRightBar.Name = "ConsRightBar"
        Me.ConsRightBar.Size = New System.Drawing.Size(196, 36)
        Me.ConsRightBar.TabIndex = 70
        Me.ConsRightBar.TickFrequency = 20
        Me.ConsRightBar.Value = 100
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(12, 45)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(141, 25)
        Me.Label4.TabIndex = 69
        Me.Label4.Text = "ConstForceRightMax"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label_Center
        '
        Me.Label_Center.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_Center.ForeColor = System.Drawing.Color.White
        Me.Label_Center.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label_Center.Location = New System.Drawing.Point(361, 90)
        Me.Label_Center.Name = "Label_Center"
        Me.Label_Center.Size = New System.Drawing.Size(40, 20)
        Me.Label_Center.TabIndex = 74
        Me.Label_Center.Text = "888"
        Me.Label_Center.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'SelfBar
        '
        Me.SelfBar.AutoSize = False
        Me.SelfBar.Location = New System.Drawing.Point(159, 88)
        Me.SelfBar.Maximum = 100
        Me.SelfBar.Name = "SelfBar"
        Me.SelfBar.Size = New System.Drawing.Size(196, 36)
        Me.SelfBar.TabIndex = 73
        Me.SelfBar.TickFrequency = 20
        Me.SelfBar.Value = 100
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(12, 86)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(141, 25)
        Me.Label6.TabIndex = 72
        Me.Label6.Text = "SelfCenterMax"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label_Friction
        '
        Me.Label_Friction.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_Friction.ForeColor = System.Drawing.Color.White
        Me.Label_Friction.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label_Friction.Location = New System.Drawing.Point(361, 131)
        Me.Label_Friction.Name = "Label_Friction"
        Me.Label_Friction.Size = New System.Drawing.Size(40, 20)
        Me.Label_Friction.TabIndex = 77
        Me.Label_Friction.Text = "888"
        Me.Label_Friction.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'FrictionBar
        '
        Me.FrictionBar.AutoSize = False
        Me.FrictionBar.Location = New System.Drawing.Point(159, 129)
        Me.FrictionBar.Maximum = 100
        Me.FrictionBar.Name = "FrictionBar"
        Me.FrictionBar.Size = New System.Drawing.Size(196, 36)
        Me.FrictionBar.TabIndex = 76
        Me.FrictionBar.TickFrequency = 20
        Me.FrictionBar.Value = 100
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(12, 127)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(141, 25)
        Me.Label8.TabIndex = 75
        Me.Label8.Text = "FrictionMax"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label_Viblate
        '
        Me.Label_Viblate.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_Viblate.ForeColor = System.Drawing.Color.White
        Me.Label_Viblate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label_Viblate.Location = New System.Drawing.Point(361, 172)
        Me.Label_Viblate.Name = "Label_Viblate"
        Me.Label_Viblate.Size = New System.Drawing.Size(40, 20)
        Me.Label_Viblate.TabIndex = 80
        Me.Label_Viblate.Text = "888"
        Me.Label_Viblate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ViblateBar
        '
        Me.ViblateBar.AutoSize = False
        Me.ViblateBar.Location = New System.Drawing.Point(159, 170)
        Me.ViblateBar.Maximum = 100
        Me.ViblateBar.Name = "ViblateBar"
        Me.ViblateBar.Size = New System.Drawing.Size(196, 36)
        Me.ViblateBar.TabIndex = 79
        Me.ViblateBar.TickFrequency = 20
        Me.ViblateBar.Value = 100
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.White
        Me.Label10.Location = New System.Drawing.Point(12, 168)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(141, 25)
        Me.Label10.TabIndex = 78
        Me.Label10.Text = "ViblateMax"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Form3
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(147, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(439, 214)
        Me.Controls.Add(Me.Label_Viblate)
        Me.Controls.Add(Me.ViblateBar)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label_Friction)
        Me.Controls.Add(Me.FrictionBar)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label_Center)
        Me.Controls.Add(Me.SelfBar)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label_ConstRight)
        Me.Controls.Add(Me.ConsRightBar)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label_ConstLeft)
        Me.Controls.Add(Me.ConstLeftBar)
        Me.Controls.Add(Me.Label3)
        Me.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form3"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Dinput"
        CType(Me.ConstLeftBar, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ConsRightBar, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SelfBar, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FrictionBar, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ViblateBar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label3 As Label
    Friend WithEvents ConstLeftBar As TrackBar
    Friend WithEvents Label_ConstLeft As Label
    Friend WithEvents Label_ConstRight As Label
    Friend WithEvents ConsRightBar As TrackBar
    Friend WithEvents Label4 As Label
    Friend WithEvents Label_Center As Label
    Friend WithEvents SelfBar As TrackBar
    Friend WithEvents Label6 As Label
    Friend WithEvents Label_Friction As Label
    Friend WithEvents FrictionBar As TrackBar
    Friend WithEvents Label8 As Label
    Friend WithEvents Label_Viblate As Label
    Friend WithEvents ViblateBar As TrackBar
    Friend WithEvents Label10 As Label
End Class
