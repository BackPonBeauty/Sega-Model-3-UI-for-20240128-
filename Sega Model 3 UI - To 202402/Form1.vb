Imports System.Net
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Xml

Public Class Form1

    <DllImport("KERNEL32.DLL", CharSet:=CharSet.Auto)>
    Public Shared Function GetPrivateProfileString(
                              lpAppName As String,
                              lpKeyName As String,
                              lpDefault As String,
                              lpReturnedString As StringBuilder,
                              nSize As Integer,
                              lpFileName As String) As Integer
    End Function

    <DllImport("KERNEL32.DLL", CharSet:=CharSet.Auto)>
    Public Shared Function WritePrivateProfileString(
                              ByVal lpApplicationName As String,
                              ByVal lpKeyName As String,
                              ByVal lpString As String,
                              ByVal lpFileName As StringBuilder) As Integer
    End Function

    Dim GameData As New DataTable
    Public Roms As String
    Public ScreenN(3) As String
    Public Bx(3) As Integer
    Public By(3) As Integer
    Public Bw(3) As Integer
    Public Bh(3) As Integer

    Dim Columns0Width As StringBuilder = New StringBuilder(300)
    Dim Columns1Width As StringBuilder = New StringBuilder(300)
    Dim Columns2Width As StringBuilder = New StringBuilder(300)
    Dim Columns3Width As StringBuilder = New StringBuilder(300)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Initialize DataTable
        GameData.Columns.Add("Games", GetType(String))
        GameData.Columns.Add("Version", GetType(String))
        GameData.Columns.Add("Roms", GetType(String))
        GameData.Columns.Add("Step", GetType(String))

        Load_gamexml()
        Load_initialfile()
        DataGridView_Setting()

    End Sub

    Private Sub Load_gamexml()
        'XmlReader
        Dim xmlDoc As New XmlDocument()
        Dim xroot As XmlNode
        Dim xfolder As XmlNodeList
        Dim xnode As XmlNode
        'StringBuilder
        Dim xname(100) As String
        Dim xVersion(100) As String
        Dim xRoms(100) As String
        Dim xStep(100) As String

        Dim appPath As String = System.Windows.Forms.Application.StartupPath
        xmlDoc.Load(appPath & "\Config\Games.xml")
        xroot = xmlDoc.DocumentElement
        xfolder = xroot.SelectNodes("//game")

        Dim i As Integer = 1
        For Each xnode In xfolder
            xname(i) = xnode.SelectSingleNode("//game[" & i & "]/identity/title").InnerText
            xVersion(i) = xnode.SelectSingleNode("//game[" & i & "]/identity/version").InnerText
            xRoms(i) = xnode.SelectSingleNode("//game[" & i & "]/@name").Value
            xStep(i) = xnode.SelectSingleNode("//game[" & i & "]/hardware/stepping").InnerText
            GameData.Rows.Add(xname(i), xVersion(i), xRoms(i), xStep(i))
            i += 1
        Next

    End Sub

    Private Sub DataGridView_Setting()

        DataGridView1.DataSource = GameData
        DataGridView1.RowHeadersVisible = False
        'DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.AllowUserToResizeColumns = True
        DataGridView1.AllowUserToResizeRows = False
        DataGridView1.MultiSelect = False
        DataGridView1.ReadOnly = True
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(54, 57, 63)
        DataGridView1.DefaultCellStyle.ForeColor = Color.White
        DataGridView1.Columns(0).Width = Integer.Parse(Columns0Width.ToString)
        DataGridView1.Columns(1).Width = Integer.Parse(Columns1Width.ToString)
        DataGridView1.Columns(2).Width = Integer.Parse(Columns2Width.ToString)
        DataGridView1.Columns(3).Width = Integer.Parse(Columns3Width.ToString)
        Try
            DataGridView1.CurrentCell = DataGridView1.Rows(0).Cells(0)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Load_initialfile()

        Dim iniFileName = "Config\Supermodel.ini"
        Dim RefreshRate As StringBuilder = New StringBuilder(300)
        Dim XResolution As StringBuilder = New StringBuilder(300)
        Dim YResolution As StringBuilder = New StringBuilder(300)
        Dim WindowXPosition As StringBuilder = New StringBuilder(300)
        Dim WindowYPosition As StringBuilder = New StringBuilder(300)
        Dim BorderlessWindow As StringBuilder = New StringBuilder(300)

        Dim New3DEngine As StringBuilder = New StringBuilder(300)
        Dim QuadRendering As StringBuilder = New StringBuilder(300)
        Dim WideScreen As StringBuilder = New StringBuilder(300)
        Dim Stretch As StringBuilder = New StringBuilder(300)
        Dim WideBackground As StringBuilder = New StringBuilder(300)
        Dim Crosshairs As StringBuilder = New StringBuilder(300)
        Dim GPUMultiThreaded As StringBuilder = New StringBuilder(300)
        Dim MultiThreaded As StringBuilder = New StringBuilder(300)
        Dim MultiTexture As StringBuilder = New StringBuilder(300)
        Dim VSync As StringBuilder = New StringBuilder(300)
        Dim FullScreen As StringBuilder = New StringBuilder(300)
        Dim Throttle As StringBuilder = New StringBuilder(300)
        Dim ShowFrameRate As StringBuilder = New StringBuilder(300)

        Dim PowerPCFrequency As StringBuilder = New StringBuilder(300)
        Dim Supersampling As StringBuilder = New StringBuilder(300)

        Dim EmulateSound As StringBuilder = New StringBuilder(300)
        Dim EmulateDSB As StringBuilder = New StringBuilder(300)
        Dim FlipStereo As StringBuilder = New StringBuilder(300)
        Dim LegacySoundDSP As StringBuilder = New StringBuilder(300)
        Dim MusicVolume As StringBuilder = New StringBuilder(300)
        Dim SoundVolume As StringBuilder = New StringBuilder(300)
        Dim Balance As StringBuilder = New StringBuilder(300)

        Dim ForceFeedback As StringBuilder = New StringBuilder(300)

        Dim Network As StringBuilder = New StringBuilder(300)
        Dim SimulateNet As StringBuilder = New StringBuilder(300)
        Dim PortIn As StringBuilder = New StringBuilder(300)
        Dim PortOut As StringBuilder = New StringBuilder(300)
        Dim AddressOut As StringBuilder = New StringBuilder(300)

        Dim DirectInputConstForceLeftMax As StringBuilder = New StringBuilder(300)
        Dim DirectInputConstForceRightMax As StringBuilder = New StringBuilder(300)
        Dim DirectInputSelfCenterMax As StringBuilder = New StringBuilder(300)
        Dim DirectInputFrictionMax As StringBuilder = New StringBuilder(300)
        Dim DirectInputVibrateMax As StringBuilder = New StringBuilder(300)

        Dim XInputConstForceThreshold As StringBuilder = New StringBuilder(300)
        Dim XInputConstForceMax As StringBuilder = New StringBuilder(300)
        Dim XInputVibrateMax As StringBuilder = New StringBuilder(300)

        Dim InputSystem As StringBuilder = New StringBuilder(300)

        Dim InputAutoTrigger As StringBuilder = New StringBuilder(300)
        Dim InputAutoTrigger2 As StringBuilder = New StringBuilder(300)
        Dim HideCMD As StringBuilder = New StringBuilder(300)
        Dim Dir As StringBuilder = New StringBuilder(300)
        Dim CrosshairStyle As StringBuilder = New StringBuilder(300)


        GetPrivateProfileString(" Global ", "RefreshRate", "57.524160", RefreshRate, 15, iniFileName)
        GetPrivateProfileString(" Global ", "Supersampling", "1", Supersampling, 15, iniFileName)
        GetPrivateProfileString(" Global ", "XResolution", "496", XResolution, 15, iniFileName)
        GetPrivateProfileString(" Global ", "YResolution", "384", YResolution, 15, iniFileName)
        GetPrivateProfileString(" Global ", "WindowXPosition", "50", WindowXPosition, 15, iniFileName)
        GetPrivateProfileString(" Global ", "WindowYPosition", "50", WindowYPosition, 15, iniFileName)
        GetPrivateProfileString(" Global ", "BorderlessWindow", "False", BorderlessWindow, 15, iniFileName)
        GetPrivateProfileString(" Global ", "New3DEngine", "True", New3DEngine, 15, iniFileName)
        GetPrivateProfileString(" Global ", "QuadRendering", "True", QuadRendering, 15, iniFileName)
        GetPrivateProfileString(" Global ", "WideScreen", "True", WideScreen, 15, iniFileName)
        GetPrivateProfileString(" Global ", "Stretch", "False", Stretch, 15, iniFileName)
        GetPrivateProfileString(" Global ", "WideBackground", "False", WideBackground, 15, iniFileName)

        GetPrivateProfileString(" Global ", "Crosshairs", "3", Crosshairs, 15, iniFileName)

        GetPrivateProfileString(" Global ", "GPUMultiThreaded", "True", GPUMultiThreaded, 15, iniFileName)
        GetPrivateProfileString(" Global ", "MultiThreaded", "True", MultiThreaded, 15, iniFileName)
        GetPrivateProfileString(" Global ", "MultiTexture", "True", MultiTexture, 15, iniFileName)
        GetPrivateProfileString(" Global ", "VSync", "False", VSync, 15, iniFileName)

        GetPrivateProfileString(" Global ", "FullScreen", "False", FullScreen, 15, iniFileName)
        GetPrivateProfileString(" Global ", "Throttle", "True", Throttle, 15, iniFileName)
        GetPrivateProfileString(" Global ", "ShowFrameRate", "False", ShowFrameRate, 15, iniFileName)
        GetPrivateProfileString(" Global ", "PowerPCFrequency", "58", PowerPCFrequency, 15, iniFileName)

        GetPrivateProfileString(" Global ", "EmulateSound", "True", EmulateSound, 15, iniFileName)
        GetPrivateProfileString(" Global ", "EmulateDSB", "True", EmulateDSB, 15, iniFileName)
        GetPrivateProfileString(" Global ", "FlipStereo", "False", FlipStereo, 15, iniFileName)
        GetPrivateProfileString(" Global ", "LegacySoundDSP", "False", LegacySoundDSP, 15, iniFileName)
        GetPrivateProfileString(" Global ", "MusicVolume", "100", MusicVolume, 15, iniFileName)
        GetPrivateProfileString(" Global ", "SoundVolume", "100", SoundVolume, 15, iniFileName)
        GetPrivateProfileString(" Global ", "Balance", "0", Balance, 15, iniFileName)

        GetPrivateProfileString(" Global ", "ForceFeedback", "True", ForceFeedback, 15, iniFileName)

        GetPrivateProfileString(" Global ", "Network", "True", Network, 15, iniFileName)
        GetPrivateProfileString(" Global ", "SimulateNet", "True", SimulateNet, 15, iniFileName)
        GetPrivateProfileString(" Global ", "PortIn", "1971", PortIn, 15, iniFileName)
        GetPrivateProfileString(" Global ", "PortOut", "1972", PortOut, 15, iniFileName)
        GetPrivateProfileString(" Global ", "AddressOut", "127.0.0.1", AddressOut, 15, iniFileName)

        GetPrivateProfileString(" Global ", "DirectInputConstForceLeftMax", "100", DirectInputConstForceLeftMax, 15, iniFileName)
        GetPrivateProfileString(" Global ", "DirectInputConstForceRightMax", "100", DirectInputConstForceRightMax, 15, iniFileName)
        GetPrivateProfileString(" Global ", "DirectInputSelfCenterMax", "100", DirectInputSelfCenterMax, 15, iniFileName)
        GetPrivateProfileString(" Global ", "DirectInputFrictionMax", "100", DirectInputFrictionMax, 15, iniFileName)
        GetPrivateProfileString(" Global ", "DirectInputVibrateMax", "100", DirectInputVibrateMax, 15, iniFileName)

        GetPrivateProfileString(" Global ", "XInputConstForceThreshold", "100", XInputConstForceThreshold, 15, iniFileName)
        GetPrivateProfileString(" Global ", "XInputConstForceMax", "100", XInputConstForceMax, 15, iniFileName)
        GetPrivateProfileString(" Global ", "XInputVibrateMax", "100", XInputVibrateMax, 15, iniFileName)

        GetPrivateProfileString(" Global ", "InputSystem", "xinput", InputSystem, 15, iniFileName)

        'GetPrivateProfileString(" Global ", "InputAutoTrigger", "Error", InputAutoTrigger, 15, iniFileName)
        'GetPrivateProfileString(" Global ", "InputAutoTrigger2", "Error", InputAutoTrigger2, 15, iniFileName)

        GetPrivateProfileString(" Supermodel3 UI ", "HideCMD", "False", HideCMD, 15, iniFileName)

        GetPrivateProfileString(" Supermodel3 UI ", "Dir", "C:\supermodel\Roms", Dir, 150, iniFileName)

        GetPrivateProfileString(" Global ", "CrosshairStyle", "vector", CrosshairStyle, 15, iniFileName)

        GetPrivateProfileString(" Supermodel3 UI ", "Columns0Width", 250, Columns0Width, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "Columns1Width", 150, Columns1Width, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "Columns2Width", 120, Columns2Width, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "Columns3Width", 50, Columns3Width, 15, iniFileName)

        'New3DEngine
        If New3DEngine.ToString() = "True" Or New3DEngine.ToString() = "1" Then
            RadioButton_new3d.Checked = True
            RadioButton_legacy.Checked = False
        Else
            RadioButton_new3d.Checked = False
            RadioButton_legacy.Checked = True
        End If

        'VSync
        If VSync.ToString() = "True" Or VSync.ToString() = "1" Then
            CheckBox_vsync.Checked = True
        Else
            CheckBox_vsync.Checked = False
        End If

        'QuadRendering
        If QuadRendering.ToString() = "True" Or QuadRendering.ToString() = "1" Then
            CheckBox_quadrender.Checked = True
        Else
            CheckBox_quadrender.Checked = False
        End If

        'GPUMultiThreaded
        If GPUMultiThreaded.ToString() = "True" Or GPUMultiThreaded.ToString() = "1" Then
            CheckBox_gpumulti.Checked = True
        Else
            CheckBox_gpumulti.Checked = False
        End If

        'MultiThreaded
        If MultiThreaded.ToString() = "True" Or MultiThreaded.ToString() = "1" Then
            CheckBox_multishread.Checked = True
        Else
            CheckBox_multishread.Checked = False
        End If

        'MultiTexture
        If MultiTexture.ToString() = "True" Or MultiTexture.ToString() = "1" Then
            CheckBox_multitexture.Checked = True
        Else
            CheckBox_multitexture.Checked = False
        End If

        'Borderless
        If BorderlessWindow.ToString() = "True" Or BorderlessWindow.ToString() = "1" Then
            CheckBox_borderless.Checked = True
        Else
            CheckBox_borderless.Checked = False
        End If

        'Borderless
        If BorderlessWindow.ToString() = "True" Or BorderlessWindow.ToString() = "1" Then
            CheckBox_borderless.Checked = True
        Else
            CheckBox_borderless.Checked = False
        End If

        'FullScreen
        If FullScreen.ToString() = "True" Or FullScreen.ToString() = "1" Then
            CheckBox_fullscreen.Checked = True
        Else
            CheckBox_fullscreen.Checked = False
        End If

        'WideScreen
        If WideScreen.ToString() = "True" Or WideScreen.ToString() = "1" Then
            CheckBox_widescreen.Checked = True
        Else
            CheckBox_widescreen.Checked = False
        End If

        'WideBackground
        If WideBackground.ToString() = "True" Or WideBackground.ToString() = "1" Then
            CheckBox_widebg.Checked = True
        Else
            CheckBox_widebg.Checked = False
        End If

        'Stretch
        If Stretch.ToString() = "True" Or Stretch.ToString() = "1" Then
            CheckBox_stretch.Checked = True
        Else
            CheckBox_stretch.Checked = False
        End If

        'ShowFrameRate
        If ShowFrameRate.ToString() = "True" Or ShowFrameRate.ToString() = "1" Then
            CheckBox_showfrmerate.Checked = True
        Else
            CheckBox_showfrmerate.Checked = False
        End If

        'Throttle
        If Throttle.ToString() = "True" Or Throttle.ToString() = "1" Then
            CheckBox_throttle.Checked = True
        Else
            CheckBox_throttle.Checked = False
        End If

        'EmulateSound
        If EmulateSound.ToString() = "True" Or EmulateSound.ToString() = "1" Then
            CheckBox_emulatesound.Checked = True
        Else
            CheckBox_emulatesound.Checked = False
        End If

        'FlipStereo
        If FlipStereo.ToString() = "True" Or FlipStereo.ToString() = "1" Then
            CheckBox_flipstereo.Checked = True
        Else
            CheckBox_flipstereo.Checked = False
        End If

        'EmulateDSB
        If EmulateDSB.ToString() = "True" Or EmulateDSB.ToString() = "1" Then
            CheckBox_emuDSB.Checked = True
        Else
            CheckBox_emuDSB.Checked = False
        End If

        'LegacySoundDSP
        If LegacySoundDSP.ToString() = "True" Or LegacySoundDSP.ToString() = "1" Then
            CheckBox_legacyDSP.Checked = True
        Else
            CheckBox_legacyDSP.Checked = False
        End If

        'MusicVolume
        Label_Music.Text = MusicVolume.ToString()
        MusicBar.Value = MusicVolume.ToString()

        'MusicVolume
        Label_Sound.Text = SoundVolume.ToString()
        SoundBar.Value = SoundVolume.ToString()

        'Balance
        Label_Balance.Text = Balance.ToString()
        BalanceBar.Value = Balance.ToString()

        'RefreshRate
        Dim RR As String = RefreshRate.ToString
        If RR = "57.524160" Then
            CheckBox_TrueHz.Checked = True
        Else
            CheckBox_TrueHz.Checked = False
        End If
        Label_refreshrate.Text = RefreshRate.ToString()

        'PowerPCFrequency
        Dim PPC As String = PowerPCFrequency.ToString()
        If PPC = "0" Then
            PPC = "Auto"
        End If
        Label_PPC.Text = PPC
        PPC_Bar.Value = PowerPCFrequency.ToString()

        'Supersampling
        Label_SS.Text = Supersampling.ToString()
        SS_Bar.Value = Supersampling.ToString()

        'WindowsPosition
        Label_xPos.Text = WindowXPosition.ToString()
        Label_yPos.Text = WindowYPosition.ToString()

        'Resolution
        Label_xRes.Text = XResolution.ToString
        Label_yRes.Text = YResolution.ToString

        'HideCMD
        If HideCMD.ToString() = "True" Or HideCMD.ToString() = "1" Then
            CheckBox_hidecmd.Checked = True
        Else
            CheckBox_hidecmd.Checked = False
        End If
        Debug("HideCMD" & HideCMD.ToString)

        'Dir
        Label_path.Text = Dir.ToString
        Debug("Dir" & Dir.ToString)

        'InputSystem
        ComboBox_input.Text = InputSystem.ToString
        'Crosshairs
        Select Case Crosshairs.ToString
            Case "0"
                ComboBox_crosshair.Text = "Disable"
            Case "1"
                ComboBox_crosshair.Text = "Player1"
            Case "2"
                ComboBox_crosshair.Text = "Player2"
            Case "3"
                ComboBox_crosshair.Text = "2Players"
        End Select

        'CrosshairStyle
        ComboBox_style.Text = CrosshairStyle.ToString

        'ForceFeedback
        If ForceFeedback.ToString() = "True" Or ForceFeedback.ToString() = "1" Then
            CheckBox18.Checked = True
        Else
            CheckBox18.Checked = False
        End If

        'Network
        If Network.ToString() = "True" Or Network.ToString() = "1" Then
            CheckBox_network.Checked = True
        Else
            CheckBox_network.Checked = False
        End If

        'SimulateNet
        If SimulateNet.ToString() = "True" Or SimulateNet.ToString() = "1" Then
            CheckBox_simnetwork.Checked = True
        Else
            CheckBox_simnetwork.Checked = False
        End If

        'PortIn
        TextBox_Portin.Text = PortIn.ToString

        'PortOut
        TextBox_Portout.Text = PortOut.ToString

        'AddressOut
        TextBox_Addressout.Text = AddressOut.ToString

        'DirectInputConstForceLeftMax
        DConstLeft.Text = DirectInputConstForceLeftMax.ToString

        'DirectInputConstForceRightMax
        DConstRight.Text = DirectInputConstForceRightMax.ToString

        'DirectInputSelfCenterMax
        DCenter.Text = DirectInputSelfCenterMax.ToString

        'DirectInputFrictionMax
        DFriction.Text = DirectInputFrictionMax.ToString

        'DirectInputVibrateMax
        DViblate.Text = DirectInputVibrateMax.ToString

        'XInputConstForceThreshold
        XThreshold.Text = XInputConstForceThreshold.ToString

        'XInputConstForceMax
        XConst.Text = XInputConstForceMax.ToString

        'XInputVibrateMax
        XViblate.Text = XInputVibrateMax.ToString

        'Resolution
        ComboBox_resolution.Text = XResolution.ToString & "x" & YResolution.ToString

        'Get Display Size 
        Dim i As Integer = 0

        For Each s In System.Windows.Forms.Screen.AllScreens
            'display device name
            ScreenN(i) = s.DeviceName
            'top left of display
            Bx(i) = s.Bounds.X
            By(i) = s.Bounds.Y
            'Display size
            Bw(i) = s.Bounds.Width
            Label_wScreenRes.Text = s.Bounds.Width
            Bh(i) = s.Bounds.Height
            Label_hScreenRes.Text = s.Bounds.Height
            Debug(i & "::" & ScreenN(i))
            i += 1
        Next

        Label_wScreenRes.Text = System.Windows.Forms.Screen.GetBounds(Me).Width
        Label_hScreenRes.Text = System.Windows.Forms.Screen.GetBounds(Me).Height

    End Sub

    'Where is the debug window? i hate it
    Public Sub Debug(S As String)
        Debugtext.AppendText(S & vbCrLf)
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button_screenpos.Click
        If My.Application.OpenForms("PosResWindow") IsNot Nothing Then

        Else
            If Integer.Parse(Label_wScreenRes.Text) < Integer.Parse(Label_xRes.Text) Or Integer.Parse(Label_hScreenRes.Text) < Integer.Parse(Label_yRes.Text) Then
                Dim result As DialogResult = MessageBox.Show("It's bigger than the screen size.",
                                             "confirmation",
                                             MessageBoxButtons.OKCancel,
                                             MessageBoxIcon.Exclamation,
                                            MessageBoxDefaultButton.Button2)
                If result = DialogResult.OK Then
                    Dim f As PosResWindow = New PosResWindow()
                    f.StartPosition = FormStartPosition.CenterScreen
                    If (f.ShowDialog(Me) = DialogResult.OK) Then
                    End If
                    f.Dispose()
                ElseIf result = DialogResult.Cancel Then
                End If
            Else
                Dim f As PosResWindow = New PosResWindow()
                f.StartPosition = FormStartPosition.CenterScreen
                If (f.ShowDialog(Me) = DialogResult.OK) Then
                End If
                f.Dispose()
            End If
        End If
    End Sub

    Private Sub DataGridView1_SelectCellChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellEnter
        Roms = DataGridView1.CurrentRow.Cells(2).Value
        PictureBox1.ImageLocation = "Snaps\" & Roms & ".jpg"
    End Sub

    Private Sub PPC_Bar_Scroll(sender As Object, e As EventArgs) Handles PPC_Bar.Scroll
        If PPC_Bar.Value = 0 Then
            Label_PPC.Text = "Auto"
        Else
            Label_PPC.Text = PPC_Bar.Value
        End If
    End Sub

    Private Sub SS_Bar_Scroll(sender As Object, e As EventArgs) Handles SS_Bar.Scroll
        Label_SS.Text = SS_Bar.Value
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox_resolution.SelectedIndexChanged
        Dim S_Select As String = ComboBox_resolution.SelectedItem
        Debug(S_Select)
        Dim S_Split() As String = Split(S_Select, "x")
        Label_xRes.Text = S_Split(0)
        Label_yRes.Text = S_Split(1)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button_loadrom.Click
        Load_Roms()
    End Sub
    Private Sub Load_Roms()
        WriteIni()
        Dim appPath As String = System.Windows.Forms.Application.StartupPath
        Dim startInfo As New ProcessStartInfo(appPath & "\Supermodel.exe ", " """ & Label_path.Text & "\" & Roms & ".zip""")
        startInfo.CreateNoWindow = CheckBox_hidecmd.Checked
        startInfo.UseShellExecute = False
        Process.Start(startInfo)
        loading.Show()
    End Sub


    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        If My.Application.OpenForms("About") IsNot Nothing Then
        Else
            Dim f As About = New About()
            If (f.ShowDialog(Me) = DialogResult.OK) Then
            End If
            f.Dispose()
        End If
    End Sub

    Private Sub MusicBar_Scroll(sender As Object, e As EventArgs) Handles MusicBar.Scroll
        Label_Music.Text = MusicBar.Value
    End Sub

    Private Sub BalanceBar_Scroll(sender As Object, e As EventArgs) Handles BalanceBar.Scroll
        Label_Balance.Text = BalanceBar.Value
    End Sub

    Private Sub SoundBar_Scroll(sender As Object, e As EventArgs) Handles SoundBar.Scroll
        Label_Sound.Text = SoundBar.Value
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Dim appPath As String = System.Windows.Forms.Application.StartupPath
        Process.Start(appPath & "\Supermodel.exe ", " -config-inputs")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button_folder.Click
        Dim fbd As New FolderBrowserDialog
        fbd.Description = "Select Roms folder."
        fbd.RootFolder = Environment.SpecialFolder.Desktop
        fbd.SelectedPath = "C:\Windows"
        fbd.ShowNewFolderButton = True
        If fbd.ShowDialog(Me) = DialogResult.OK Then
            Label_path.Text = fbd.SelectedPath
        End If
    End Sub

    Private Sub TextBox1and2_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox_Portin.KeyPress, TextBox_Portout.KeyPress
        If (e.KeyChar < "0"c OrElse "9"c < e.KeyChar) AndAlso e.KeyChar <> ControlChars.Back Then
            e.Handled = True
        End If
    End Sub

    Private Sub TextBox3_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox_Addressout.KeyPress
        If (e.KeyChar < "0"c OrElse "9"c < e.KeyChar) AndAlso e.KeyChar <> ControlChars.Back AndAlso e.KeyChar <> "."c Then
            e.Handled = True
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Panel_Video.Left = 612
        Panel_Video.Top = 336
        Panel_Sound.Left = 2000
        Panel_Sound.Top = 336
        Panel_Input.Left = 2000
        Panel_Input.Top = 336
        Panel_Network.Left = 2000
        Panel_Network.Top = 336
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Panel_Video.Left = 2000
        Panel_Video.Top = 336
        Panel_Sound.Left = 612
        Panel_Sound.Top = 336
        Panel_Input.Left = 2000
        Panel_Input.Top = 336
        Panel_Network.Left = 2000
        Panel_Network.Top = 336
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Panel_Video.Left = 2000
        Panel_Video.Top = 336
        Panel_Sound.Left = 2000
        Panel_Sound.Top = 336
        Panel_Input.Left = 612
        Panel_Input.Top = 336
        Panel_Network.Left = 2000
        Panel_Network.Top = 336
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Panel_Video.Left = 2000
        Panel_Video.Top = 336
        Panel_Sound.Left = 2000
        Panel_Sound.Top = 336
        Panel_Input.Left = 2000
        Panel_Input.Top = 336
        Panel_Network.Left = 612
        Panel_Network.Top = 336
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button_writeini.Click
        Dim result As DialogResult = MessageBox.Show("Do you want to overwrite the ini file?",
                                             "confirmation",
                                             MessageBoxButtons.OKCancel,
                                             MessageBoxIcon.Exclamation,
                                            MessageBoxDefaultButton.Button2)
        If result = DialogResult.OK Then
            WriteIni()
        End If
    End Sub

    Private Sub WriteIni()
        Dim iniFileName As New StringBuilder(300)
        iniFileName.Append("Config\Supermodel.ini")
        Dim Section As String = " Global "
        WritePrivateProfileString(Section, "RefreshRate", Label_refreshrate.Text, iniFileName)
        WritePrivateProfileString(Section, "XResolution", Label_xRes.Text, iniFileName)
        WritePrivateProfileString(Section, "YResolution", Label_yRes.Text, iniFileName)
        WritePrivateProfileString(Section, "WindowXPosition", Label_xPos.Text, iniFileName)
        WritePrivateProfileString(Section, "WindowYPosition", Label_yPos.Text, iniFileName)
        WritePrivateProfileString(Section, "BorderlessWindow", CheckBox_borderless.Checked.ToString, iniFileName)
        WritePrivateProfileString(Section, "New3DEngine", RadioButton_new3d.Checked.ToString, iniFileName)
        WritePrivateProfileString(Section, "QuadRendering", CheckBox_quadrender.Checked.ToString, iniFileName)
        WritePrivateProfileString(Section, "WideScreen", CheckBox_widescreen.Checked.ToString, iniFileName)
        WritePrivateProfileString(Section, "Stretch", CheckBox_stretch.Checked.ToString, iniFileName)
        WritePrivateProfileString(Section, "WideBackground", CheckBox_widebg.Checked.ToString, iniFileName)

        WritePrivateProfileString(Section, "Crosshairs", ComboBox_crosshair.SelectedIndex, iniFileName)

        WritePrivateProfileString(Section, "GPUMultiThreaded", CheckBox_gpumulti.Checked.ToString, iniFileName)
        WritePrivateProfileString(Section, "MultiThreaded", CheckBox_multishread.Checked.ToString, iniFileName)
        WritePrivateProfileString(Section, "MultiTexture", CheckBox_multitexture.Checked.ToString, iniFileName)
        WritePrivateProfileString(Section, "VSync", CheckBox_vsync.Checked.ToString, iniFileName)
        WritePrivateProfileString(Section, "FullScreen", CheckBox_fullscreen.Checked.ToString, iniFileName)
        WritePrivateProfileString(Section, "Throttle", CheckBox_throttle.Checked.ToString, iniFileName)
        WritePrivateProfileString(Section, "ShowFrameRate", CheckBox_showfrmerate.Checked.ToString, iniFileName)
        If Label_PPC.Text = "Auto" Then
            WritePrivateProfileString(Section, "PowerPCFrequency", "0", iniFileName)
        Else
            WritePrivateProfileString(Section, "PowerPCFrequency", Label_PPC.Text, iniFileName)
        End If

        WritePrivateProfileString(Section, "Supersampling", Label_SS.Text, iniFileName)
        WritePrivateProfileString(Section, "EmulateSound", CheckBox_emulatesound.Checked.ToString, iniFileName)
        WritePrivateProfileString(Section, "EmulateDSB", CheckBox_emuDSB.Checked.ToString, iniFileName)
        WritePrivateProfileString(Section, "FlipStereo", CheckBox_flipstereo.Checked.ToString, iniFileName)
        WritePrivateProfileString(Section, "LegacySoundDSP", CheckBox_legacyDSP.Checked.ToString, iniFileName)
        WritePrivateProfileString(Section, "MusicVolume", Label_Music.Text, iniFileName)
        WritePrivateProfileString(Section, "SoundVolume", Label_Sound.Text, iniFileName)
        WritePrivateProfileString(Section, "Balance", Label_Balance.Text, iniFileName)
        WritePrivateProfileString(Section, "ForceFeedback", CheckBox18.Checked.ToString, iniFileName)
        WritePrivateProfileString(Section, "Network", CheckBox_network.Checked.ToString, iniFileName)
        WritePrivateProfileString(Section, "SimulateNet", CheckBox_simnetwork.Checked.ToString, iniFileName)
        WritePrivateProfileString(Section, "PortIn", TextBox_Portin.Text, iniFileName)
        WritePrivateProfileString(Section, "PortOut", TextBox_Portout.Text, iniFileName)
        WritePrivateProfileString(Section, "AddressOut", TextBox_Addressout.Text, iniFileName)

        WritePrivateProfileString(Section, "InputSystem", ComboBox_input.SelectedItem, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "HideCMD", CheckBox_hidecmd.Checked.ToString, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "Dir", Label_path.Text, iniFileName)

        WritePrivateProfileString(" Supermodel3 UI ", "Columns0Width", DataGridView1.Columns(0).Width, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "Columns1Width", DataGridView1.Columns(1).Width, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "Columns2Width", DataGridView1.Columns(2).Width, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "Columns3Width", DataGridView1.Columns(3).Width, iniFileName)

        WritePrivateProfileString(Section, "DirectInputConstForceLeftMax", DConstLeft.Text, iniFileName)
        WritePrivateProfileString(Section, "DirectInputConstForceRightMax", DConstRight.Text, iniFileName)
        WritePrivateProfileString(Section, "DirectInputSelfCenterMax", DCenter.Text, iniFileName)
        WritePrivateProfileString(Section, "DirectInputFrictionMax", DFriction.Text, iniFileName)
        WritePrivateProfileString(Section, "DirectInputVibrateMax", DViblate.Text, iniFileName)

        WritePrivateProfileString(Section, "XInputConstForceThreshold", XThreshold.Text, iniFileName)
        WritePrivateProfileString(Section, "XInputConstForceMax", XConst.Text, iniFileName)
        WritePrivateProfileString(Section, "XInputVibrateMax", XViblate.Text, iniFileName)

        WritePrivateProfileString(Section, "CrosshairStyle", ComboBox_style.SelectedItem, iniFileName)
    End Sub

    Private Sub Me_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Dim iniFileName As New StringBuilder(300)
        iniFileName.Append("Config\Supermodel.ini")
        WritePrivateProfileString(" Supermodel3 UI ", "Columns0Width", DataGridView1.Columns(0).Width, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "Columns1Width", DataGridView1.Columns(1).Width, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "Columns2Width", DataGridView1.Columns(2).Width, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "Columns3Width", DataGridView1.Columns(3).Width, iniFileName)
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If My.Application.OpenForms("DinputForce") IsNot Nothing Then

        Else
            Dim f As DinputForce = New DinputForce()
            f.Label_ConstLeft.Text = DConstLeft.Text
            f.ConstLeftBar.Value = DConstLeft.Text
            f.Label_ConstRight.Text = DConstRight.Text
            f.ConsRightBar.Value = DConstRight.Text
            f.Label_Center.Text = DCenter.Text
            f.SelfBar.Value = DCenter.Text
            f.Label_Friction.Text = DFriction.Text
            f.FrictionBar.Value = DFriction.Text
            f.Label_Viblate.Text = DViblate.Text
            f.ViblateBar.Value = DViblate.Text
            If (f.ShowDialog(Me) = DialogResult.OK) Then
            End If
            f.Dispose()
        End If
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If My.Application.OpenForms("XinputForce") IsNot Nothing Then

        Else
            Dim f As XinputForce = New XinputForce()
            f.Label_XThreshold.Text = XThreshold.Text
            f.ThresholdBar.Value = XThreshold.Text
            f.Label_XConstMax.Text = XConst.Text
            f.ConstMaxBar.Value = XConst.Text
            f.Label_XViblate.Text = XViblate.Text
            f.ViblateBar.Value = XViblate.Text

            If (f.ShowDialog(Me) = DialogResult.OK) Then
            End If
            f.Dispose()
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentDoubleClick
        Roms = DataGridView1.CurrentRow.Cells(2).Value
        PictureBox1.ImageLocation = "Snaps\" & Roms & ".jpg"
        Load_Roms()
    End Sub

    Private Sub CheckBox_TrueHz_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_TrueHz.CheckedChanged
        If CheckBox_TrueHz.Checked = False Then
            Label_refreshrate.Text = "60"
        Else
            Label_refreshrate.Text = "57.524160"
        End If
    End Sub

    Private Sub Get_IP_Address(sender As Object, e As EventArgs) Handles Button_GetIPAddress.Click
        Dim hostname As String = Dns.GetHostName()
        Dim adrList As IPAddress() = Dns.GetHostAddresses(hostname)
        'For Each address As IPAddress In adrList
        '    Debug(address.ToString())
        'Next
        Dim adrLength = adrList.Count - 1
        Label_myaddress.Text = adrList(adrLength).ToString()
    End Sub
End Class
