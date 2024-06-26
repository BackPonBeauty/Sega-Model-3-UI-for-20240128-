﻿Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports System.Reflection.Emit
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Xml
Imports SharpDX
Imports SharpDX.XInput
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
    Dim DT_Roms As New DataTable
    Public ScreenN(3) As String
    Public Bx(3) As Integer
    Public By(3) As Integer
    Public Bw(3) As Integer
    Public Bh(3) As Integer

    Dim Columns0Width As StringBuilder = New StringBuilder(300)
    Dim Columns1Width As StringBuilder = New StringBuilder(300)
    Dim Columns2Width As StringBuilder = New StringBuilder(300)
    Dim Columns3Width As StringBuilder = New StringBuilder(300)
    Dim Columns4Width As StringBuilder = New StringBuilder(300)
    Dim C0_Sort_F As Boolean = False
    Dim C1_Sort_F As Boolean = False
    Dim C2_Sort_F As Boolean = False
    Dim C3_Sort_F As Boolean = False
    Dim C4_Sort_F As Boolean = False

    Dim Last_Sort As Integer = 0
    Dim Onece As Boolean = False
    Dim Last_SelectedItem As String = ""
    Dim Last_SelectedRow As Integer = 0
    Dim Last_SelectedRow_bin As Integer = 0
    Public FontSize_bin As Integer = "10"
    Dim Resolution_index_bin As Integer = 0
    Public Bgcolor_R As Integer = 147
    Public Bgcolor_G As Integer = 0
    Public Bgcolor_B As Integer = 80
    Public Pub_Forecolor_s As Color = Color.White
    Public Forecolor_s As String = "White"
    Public Outputs_F As Boolean
    Public ScanLine_F As Boolean = False
    Dim Front_F As Boolean = True
    Dim Scanline_Enabled As Boolean = False
    Public Opacity_D As Double = 0.5

    WithEvents KeyboardHooker1 As New Key

    'DragMove
    Private Sub Form1_ResizeEnd(sender As Object, e As EventArgs) Handles MyBase.ResizeEnd

        'Label1.Text = "( " & Me.Left & " , " & Me.Top & " )"
        Dim s As System.Windows.Forms.Screen = System.Windows.Forms.Screen.FromControl(Me)
        'ディスプレイの高さと幅を取得
        Dim x As Integer = s.Bounds.X
        Dim y As Integer = s.Bounds.Y
        Label_hScreenRes.Text = s.Bounds.Height
        Label_wScreenRes.Text = s.Bounds.Width

        Dim N As Integer
        For i = 0 To ScreenN.Length - 1
            If Me.Top >= By(i) And Me.Top <= (By(i) + Bh(i)) And Me.Left >= Bx(i) And Me.Left <= (Bx(i) + Bw(i)) Then
                Label2.Text = ScreenN(i).ToString
                N = i
                Exit For

            End If
        Next

    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim appPath As String = System.Windows.Forms.Application.StartupPath
        Dim fileName As String = appPath & "\Config"
        If System.IO.Directory.Exists(fileName) Then
            'Initialize DataTable
            GameData.Columns.Add("Games", GetType(String))
            GameData.Columns.Add("Version", GetType(String))
            GameData.Columns.Add("Roms", GetType(String))
            GameData.Columns.Add("Step", GetType(String))
            GameData.Columns.Add("A-E", GetType(String))
            DT_Roms.Columns.Add("name", GetType(String))

            Load_gamexml()
            Load_initialfile()
            If Forecolor_s = "White" Then
                WhiteToolStripMenuItem.PerformClick()
            Else
                BlackToolStripMenuItem.PerformClick()
            End If
            Me.BackColor = Color.FromArgb(255, Bgcolor_R, Bgcolor_G, Bgcolor_B)
            Label_path.BackColor = Color.FromArgb(255, Bgcolor_R, Bgcolor_G, Bgcolor_B)
            DataGridView_Setting()
            Select Case Last_Sort
                Case 0
                    C0_Sort_F = Not (C0_Sort_F)
                    Header0.PerformClick()
                Case 1
                    C1_Sort_F = Not (C1_Sort_F)
                    Header1.PerformClick()
                Case 2
                    C2_Sort_F = Not (C2_Sort_F)
                    Header2.PerformClick()
                Case 3
                    C3_Sort_F = Not (C3_Sort_F)
                    Header3.PerformClick()
                Case 4
                    C4_Sort_F = Not (C4_Sort_F)
                    Header4.PerformClick()
                Case Else
                    'C0_Sort_F = Not (C0_Sort_F)
                    'Header0.PerformClick()
            End Select
            LoadResolution()
            LastSelectRow()
            GetAllControls(Me, FontSize_bin)
        Else
            MessageBox.Show("Config folder not found.")
            Me.Close()
        End If
        'If KeyboardHooker1.MouseHookStart() = True Then
        '    Button_hook.Text = "Enabled"
        'End If
    End Sub

    Private Sub LoadResolution()
        Dim line As String = ""
        Dim al As New ArrayList

        Using sr As StreamReader = New StreamReader(
          "Resolution.txt", Encoding.GetEncoding("UTF-8"))

            line = sr.ReadLine()
            Do Until line Is Nothing
                ComboBox_resolution.Items.Add(line)
                line = sr.ReadLine()
            Loop
            ComboBox_resolution.SelectedIndex = Resolution_index_bin
        End Using
    End Sub

    Private Sub LastSelectRow()
        DataGridView1.CurrentCell = DataGridView1(0, Last_SelectedRow_bin)
        'Debug("LastSelect = " & Last_SelectedRow.ToString)
    End Sub

    Private Sub Load_gamexml()
        'XmlReader
        Dim xmlDoc As New XmlDocument()
        Dim xroot As XmlNode
        Dim xfolder As XmlNodeList
        Dim xnode As XmlNode
        'StringBuilder
        Dim xname(1000) As String
        Dim xVersion(1000) As String
        Dim xRoms(1000) As String
        Dim xStep(1000) As String

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
            GameData.Rows.Add(xname(i), xVersion(i), xRoms(i), xStep(i), " ")
            i += 1
        Next

    End Sub

    Private Sub DataGridView_Setting()

        DataGridView1.DataSource = GameData
        DataGridView1.RowHeadersVisible = False
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
        DataGridView1.Columns(4).Width = Integer.Parse(Columns4Width.ToString)
        DataGridView1.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView1.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        'For Each c As DataGridViewColumn In DataGridView1.Columns
        '    c.SortMode = DataGridViewColumnSortMode.Programmatic
        'Next c

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

        Dim C0_F As StringBuilder = New StringBuilder(300)
        Dim C1_F As StringBuilder = New StringBuilder(300)
        Dim C2_F As StringBuilder = New StringBuilder(300)
        Dim C3_F As StringBuilder = New StringBuilder(300)
        Dim C4_F As StringBuilder = New StringBuilder(300)

        Dim Last_Sort_s As StringBuilder = New StringBuilder(300)
        Dim Last_Selected_s As StringBuilder = New StringBuilder(300)

        Dim FontSize As StringBuilder = New StringBuilder(300)
        Dim Resolution_index As StringBuilder = New StringBuilder(300)
        Dim BgcolorR As StringBuilder = New StringBuilder(300)
        Dim BgcolorG As StringBuilder = New StringBuilder(300)
        Dim BgcolorB As StringBuilder = New StringBuilder(300)
        Dim Forecolor As StringBuilder = New StringBuilder(300)
        Dim Title_SB As StringBuilder = New StringBuilder(300)
        Dim Outputs As StringBuilder = New StringBuilder(300)
        Dim Scanline As StringBuilder = New StringBuilder(300)
        Dim Gamepad As StringBuilder = New StringBuilder(300)
        Dim Opacity As StringBuilder = New StringBuilder(30000)

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
        GetPrivateProfileString(" Supermodel3 UI ", "Outputs", "False", Outputs, 15, iniFileName)

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

        GetPrivateProfileString(" Supermodel3 UI ", "Dir", "C:\天上天下唯我独尊\Roms", Dir, 150, iniFileName)
        GetPrivateProfileString(" Global ", "Title", "Supermodel", Title_SB, 150, iniFileName)

        GetPrivateProfileString(" Global ", "CrosshairStyle", "vector", CrosshairStyle, 15, iniFileName)

        GetPrivateProfileString(" Supermodel3 UI ", "Columns0Width", 200, Columns0Width, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "Columns1Width", 150, Columns1Width, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "Columns2Width", 120, Columns2Width, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "Columns3Width", 50, Columns3Width, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "Columns4Width", 50, Columns4Width, 15, iniFileName)

        GetPrivateProfileString(" Supermodel3 UI ", "Columns0Sort", "False", C0_F, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "Columns1Sort", "False", C1_F, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "Columns2Sort", "False", C2_F, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "Columns3Sort", "False", C3_F, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "Columns4Sort", "False", C4_F, 15, iniFileName)

        GetPrivateProfileString(" Supermodel3 UI ", "LastSort", 0, Last_Sort_s, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "LastSelectedRow", 0, Last_Selected_s, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "FontSize", 10, FontSize, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "Resolution_index", 10, Resolution_index, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "BackColor_R", "147", BgcolorR, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "BackColor_G", "0", BgcolorG, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "BackColor_B", "80", BgcolorB, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "ForeColor", "White", Forecolor, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "BackColor_B", "80", BgcolorB, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "ForeColor", "White", Forecolor, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "Scanline", "False", Scanline, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "Gamepad", "False", Gamepad, 15, iniFileName)
        GetPrivateProfileString(" Supermodel3 UI ", "Opacity", "5", Opacity, 15, iniFileName)
        'Opacity
        Dim result As Integer
        If Integer.TryParse(Opacity.ToString, result) = False Then
            Opacity_D = 5
        Else
            Opacity_D = Integer.Parse(Opacity.ToString())
        End If
        'Scanline
        If Scanline.ToString() = "True" Or VSync.ToString() = "1" Then
            Button_hook.PerformClick()
        End If

        'Gamepad
        If Gamepad.ToString() = "True" Or VSync.ToString() = "1" Then
            Button_X.PerformClick()
        End If

        'BackColor
        Bgcolor_R = BgcolorR.ToString
        Bgcolor_G = BgcolorG.ToString
        Bgcolor_B = BgcolorB.ToString

        'ForeColor
        Forecolor_s = Forecolor.ToString

        'Columuns Sort Flag
        If C0_F.ToString() = "True" Then
            C0_Sort_F = True
        Else
            C0_Sort_F = False
        End If
        If C1_F.ToString() = "True" Then
            C1_Sort_F = True
        Else
            C1_Sort_F = False
        End If
        If C2_F.ToString() = "True" Then
            C2_Sort_F = True
        Else
            C2_Sort_F = False
        End If
        If C3_F.ToString() = "True" Then
            C3_Sort_F = True
        Else
            C3_Sort_F = False
        End If
        If C4_F.ToString() = "True" Then
            C4_Sort_F = True
        Else
            C4_Sort_F = False
        End If

        'Last_Sort
        Last_Sort = Integer.Parse(Last_Sort_s.ToString)

        'Last_Selected
        Last_SelectedRow = Integer.Parse(Last_Selected_s.ToString)
        Last_SelectedRow_bin = Integer.Parse(Last_Selected_s.ToString)

        'FontSize
        FontSize_bin = Integer.Parse(FontSize.ToString)

        'Resolution_index
        Resolution_index_bin = Integer.Parse(Resolution_index.ToString)

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
        Roms_count(Dir.ToString)
        Debug("Dir" & Dir.ToString)

        'Title
        TextBox_Title.Text = Title_SB.ToString

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

        'Outputs
        If Outputs.ToString() = "True" Or Outputs.ToString() = "1" Then
            CheckBox_outputs.Checked = True
        Else
            CheckBox_outputs.Checked = False
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
            'Debug(i & "::" & ScreenN(i))
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
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Exclamation,
                                            MessageBoxDefaultButton.Button1)
                If result = DialogResult.OK Then
                    Exit Sub
                End If
            Else
                Dim f As PosResWindow = New PosResWindow()
                'f.Left = Integer.Parse(Label_xPos.Text)
                'f.Top = Integer.Parse(Label_yPos.Text)
                f.StartPosition = FormStartPosition.CenterScreen
                If f.ShowDialog(Me) = DialogResult.OK Then
                End If
                f.Dispose()
            End If
        End If
    End Sub

    Private Sub DataGridView1_SelectCellChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellEnter
        Roms = DataGridView1.CurrentRow.Cells(2).Value
        PictureBox1.ImageLocation = "Snaps\" & Roms & ".jpg"
        Last_SelectedRow = DataGridView1.CurrentRow.Index

    End Sub

    Private Sub DataGridView1_SelectCellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Last_SelectedItem = DataGridView1.CurrentCell.Value
        Last_SelectedRow = DataGridView1.CurrentRow.Index
        Debug(Last_SelectedRow)
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
        Dim S_Split() As String = Split(S_Select, "x")
        Label_xRes.Text = S_Split(0)
        Label_yRes.Text = S_Split(1)
    End Sub

    Private Sub Button_LoadRom_Click(sender As Object, e As EventArgs) Handles Button_loadrom.Click
        Load_Roms()
    End Sub
    Private Sub Load_Roms()
        WriteIni()

        'Dim outputs_s As String = " -outputs=win"
        'If CheckBox_outputs.Checked = True Then
        '    Try
        '        Dim appPath As String = System.Windows.Forms.Application.StartupPath
        '        Dim startInfo As New ProcessStartInfo(appPath & "\Supermodel.exe ", " """ & Label_path.Text & "\" & Roms & ".zip""" & " -outputs=win")
        '        startInfo.CreateNoWindow = CheckBox_hidecmd.Checked
        '        startInfo.UseShellExecute = False
        '        Process.Start(startInfo)
        '        loading.Show()
        '    Catch ex As Exception
        '        MessageBox.Show(ex.Message.ToString, "Error",
        '            MessageBoxButtons.OK,
        '            MessageBoxIcon.Error)
        '    End Try
        'Else
        Try
            Dim appPath As String = System.Windows.Forms.Application.StartupPath
            Dim startInfo As New ProcessStartInfo(appPath & "\Supermodel.exe ", " """ & Label_path.Text & "\" & Roms & ".zip""") ')
            startInfo.CreateNoWindow = CheckBox_hidecmd.Checked
            startInfo.UseShellExecute = False
            Process.Start(startInfo)
            loading.Show()
        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString, "Error",
MessageBoxButtons.OK,
MessageBoxIcon.Error)
        End Try
        'End If

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
        fbd.SelectedPath = Label_path.Text
        fbd.ShowNewFolderButton = True
        If fbd.ShowDialog(Me) = DialogResult.OK Then
            Label_path.Text = fbd.SelectedPath
            Roms_count(fbd.SelectedPath)
        End If
    End Sub


    Private Sub Roms_count(Dir As String)
        Try
            If DT_Roms.Rows.Count > 0 Then
                DT_Roms.Rows.Clear()
            End If
            Dim Roms_count As Integer = 0
            Dim FileCount As Integer = Directory.GetFiles(Dir, "*.zip", SearchOption.TopDirectoryOnly).Length
            If FileCount > 0 Then
                Dim files As IEnumerable(Of String) = System.IO.Directory.EnumerateFiles(Dir, "*.zip", System.IO.SearchOption.TopDirectoryOnly)
                'ファイルを列挙する

                For Each f As String In files
                    Dim f_split() As String
                    f_split = Split(f, "\")
                    Dim fl As Integer = f_split.Length - 1
                    Dim f_replace As String = f_split(fl).Replace(".zip", "")
                    DT_Roms.Rows.Add(f_replace)
                Next
                'DataGridView2.DataSource = DT_Roms

                For i As Integer = 0 To GameData.Rows.Count - 1
                    GameData.Rows(i).Item("A-E") = " "
                Next



                Dim Roms_bin As String

                For i As Integer = 0 To GameData.Rows.Count - 1
                    Roms_bin = GameData.Rows(i).Item("Roms").ToString
                    For j As Integer = 0 To FileCount - 1
                        If Roms_bin = DT_Roms.Rows(j).Item(0).ToString Then
                            GameData.Rows(i).Item("A-E") = "o"
                            Roms_count += 1
                            Exit For
                        End If
                    Next
                Next
            Else
                For i As Integer = 0 To GameData.Rows.Count - 1
                    GameData.Rows(i).Item("A-E") = " "
                Next
            End If
            DataGridView1.DataSource = GameData
            Label_Roms.Text = Roms_count & " rom(s) available."
            Label_listed.Text = GameData.Rows.Count & " game(s) listed." 'DataGridView1.CurrentRow.Index + 1 & " / " &
        Catch ex As Exception

        End Try

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
        Panel_ponmi.Left = 2000
        Panel_ponmi.Top = 336
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
        Panel_ponmi.Left = 2000
        Panel_ponmi.Top = 336
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
        Panel_ponmi.Left = 2000
        Panel_ponmi.Top = 336
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
        Panel_ponmi.Left = 2000
        Panel_ponmi.Top = 336
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs)
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
        WritePrivateProfileString(" Supermodel3 UI ", "Outputs", CheckBox_outputs.Checked.ToString, iniFileName)

        WritePrivateProfileString(Section, "Network", CheckBox_network.Checked.ToString, iniFileName)
        WritePrivateProfileString(Section, "SimulateNet", CheckBox_simnetwork.Checked.ToString, iniFileName)
        WritePrivateProfileString(Section, "PortIn", TextBox_Portin.Text, iniFileName)
        WritePrivateProfileString(Section, "PortOut", TextBox_Portout.Text, iniFileName)
        WritePrivateProfileString(Section, "AddressOut", TextBox_Addressout.Text, iniFileName)

        WritePrivateProfileString(Section, "InputSystem", ComboBox_input.SelectedItem, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "HideCMD", CheckBox_hidecmd.Checked.ToString, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "Dir", Label_path.Text, iniFileName)
        If TextBox_Title.Text = "" Then
            TextBox_Title.Text = "Supermodel - PonMi"
        End If
        WritePrivateProfileString(Section, "Title", TextBox_Title.Text, iniFileName)

        WritePrivateProfileString(" Supermodel3 UI ", "Columns0Width", DataGridView1.Columns(0).Width, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "Columns1Width", DataGridView1.Columns(1).Width, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "Columns2Width", DataGridView1.Columns(2).Width, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "Columns3Width", DataGridView1.Columns(3).Width, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "Columns4Width", DataGridView1.Columns(4).Width, iniFileName)

        WritePrivateProfileString(" Supermodel3 UI ", "Columns0Sort", C0_Sort_F, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "Columns1Sort", C1_Sort_F, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "Columns2Sort", C2_Sort_F, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "Columns3Sort", C3_Sort_F, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "Columns4Sort", C4_Sort_F, iniFileName)

        WritePrivateProfileString(" Supermodel3 UI ", "LastSort", Last_Sort, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "LastSelectedRow", Last_SelectedRow, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "FontSize", FontSize_bin, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "Resolution_index", ComboBox_resolution.SelectedIndex, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "BackColor_R", Bgcolor_R, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "BackColor_G", Bgcolor_G, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "BackColor_B", Bgcolor_B, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "ForeColor", Forecolor_s, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "Scanline", Scanline_Enabled, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "Gamepad", Surround.Enabled, iniFileName)
        WritePrivateProfileString(" Supermodel3 UI ", "Opacity", Opacity_D, iniFileName)

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
        Try
            WriteIni()
        Catch

        End Try
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

    Private Sub Get_Local_IPAddress(sender As Object, e As EventArgs) Handles Button_Get_Local_IPAddress.Click
        Dim hostname As String = Dns.GetHostName()
        Dim adrList As IPAddress() = Dns.GetHostAddresses(hostname)
        Dim adrLength = adrList.Count - 1
        Label_Local_IPaddress.Text = adrList(adrLength).ToString()
    End Sub

    Private Function Get_Global_IPaddress() As String
        Try
            Dim cmd As New System.Diagnostics.Process()
            cmd.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec")
            cmd.StartInfo.UseShellExecute = False
            cmd.StartInfo.RedirectStandardOutput = True
            cmd.StartInfo.RedirectStandardInput = False
            cmd.StartInfo.CreateNoWindow = True
            cmd.StartInfo.Arguments = "/c curl inet-ip.info/ip"
            cmd.Start()
            Dim results As String = cmd.StandardOutput.ReadToEnd()
            cmd.WaitForExit()
            cmd.Close()
            Return results '.Trim() ※1
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    Private Sub Header0_Click(sender As Object, e As EventArgs) Handles Header0.Click
        If C0_Sort_F = False Then
            DataGridView1.Sort(DataGridView1.Columns(0), System.ComponentModel.ListSortDirection.Ascending)
            GameData.DefaultView.Sort = "Games ASC"
            C0_Sort_F = True
        Else
            DataGridView1.Sort(DataGridView1.Columns(0), System.ComponentModel.ListSortDirection.Descending)
            GameData.DefaultView.Sort = "Games DESC"
            C0_Sort_F = False
        End If
        Last_Sort = 0
    End Sub
    Private Sub Header1_Click(sender As Object, e As EventArgs) Handles Header1.Click
        DataGridView1.ClearSelection()
        If C1_Sort_F = False Then
            DataGridView1.Sort(DataGridView1.Columns(1), System.ComponentModel.ListSortDirection.Ascending)
            GameData.DefaultView.Sort = "Version ASC"
            C1_Sort_F = True
        Else
            DataGridView1.Sort(DataGridView1.Columns(1), System.ComponentModel.ListSortDirection.Descending)
            GameData.DefaultView.Sort = "Version DESC"
            C1_Sort_F = False
        End If
        Last_Sort = 1

    End Sub
    Private Sub Header2_Click(sender As Object, e As EventArgs) Handles Header2.Click
        If C2_Sort_F = False Then
            DataGridView1.Sort(DataGridView1.Columns(2), System.ComponentModel.ListSortDirection.Ascending)
            GameData.DefaultView.Sort = "Roms ASC"
            C2_Sort_F = True
        Else
            DataGridView1.Sort(DataGridView1.Columns(2), System.ComponentModel.ListSortDirection.Descending)
            GameData.DefaultView.Sort = "Roms DESC"
            C2_Sort_F = False
        End If
        Last_Sort = 2
    End Sub
    Private Sub Header3_Click(sender As Object, e As EventArgs) Handles Header3.Click
        If C3_Sort_F = False Then
            DataGridView1.Sort(DataGridView1.Columns(3), System.ComponentModel.ListSortDirection.Ascending)
            GameData.DefaultView.Sort = "Step ASC"
            C3_Sort_F = True
        Else
            DataGridView1.Sort(DataGridView1.Columns(3), System.ComponentModel.ListSortDirection.Descending)
            GameData.DefaultView.Sort = "Step DESC"
            C3_Sort_F = False
        End If
        Last_Sort = 3
    End Sub

    Private Sub Header4_Click(sender As Object, e As EventArgs) Handles Header4.Click
        If C4_Sort_F = False Then
            DataGridView1.Sort(DataGridView1.Columns(4), System.ComponentModel.ListSortDirection.Ascending)
            GameData.DefaultView.Sort = "A-E ASC"
            C4_Sort_F = True
        Else
            DataGridView1.Sort(DataGridView1.Columns(4), System.ComponentModel.ListSortDirection.Descending)
            GameData.DefaultView.Sort = "A-E DESC"
            C4_Sort_F = False
        End If
        Last_Sort = 4
    End Sub

    Private Sub DataGridView1_ColumnWidthChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles DataGridView1.ColumnWidthChanged
        Dim wn As Integer = 2
        Dim wp As Integer = 7
        Header0.Width = DataGridView1.Columns(0).Width - wn
        Header1.Left = DataGridView1.Columns(0).Width + wp
        Header1.Width = DataGridView1.Columns(1).Width - wn
        Header2.Left = DataGridView1.Columns(0).Width + DataGridView1.Columns(1).Width + wp
        Header2.Width = DataGridView1.Columns(2).Width - wn
        Header3.Left = DataGridView1.Columns(0).Width + DataGridView1.Columns(1).Width + DataGridView1.Columns(2).Width + wp
        Header3.Width = DataGridView1.Columns(3).Width - wn
        Header4.Left = DataGridView1.Columns(0).Width + DataGridView1.Columns(1).Width + DataGridView1.Columns(2).Width + DataGridView1.Columns(3).Width + wp
        Header4.Width = DataGridView1.Columns(4).Width - wn
    End Sub

    Private Sub ToolStripMenuItem8_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem8.Click, ToolStripMenuItem10.Click
        FontSize_bin = Integer.Parse(sender.tag.ToString)
        GetAllControls(Me, Integer.Parse(FontSize_bin))
    End Sub

    Private Sub GetAllControls(ByVal control As Control, size As Integer)
        If control.HasChildren Then
            For Each childControl As Control In control.Controls
                GetAllControls(childControl, size)
                childControl.Font = New Font("Arial", size, FontStyle.Regular)
            Next childControl
        End If
    End Sub

    Private Sub BGColorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChooseToolStripMenuItem.Click
        Dim cd As New ColorDialog()
        cd.Color = Me.BackColor
        cd.AllowFullOpen = True
        cd.SolidColorOnly = False
        If cd.ShowDialog() = DialogResult.OK Then
            Me.BackColor = cd.Color
            Label_path.BackColor = cd.Color
            Bgcolor_R = cd.Color.R.ToString
            Bgcolor_G = cd.Color.G.ToString
            Bgcolor_B = cd.Color.B.ToString
            Debug(cd.Color.R.ToString)
            Debug(cd.Color.G.ToString)
            Debug(cd.Color.B.ToString)
        End If
    End Sub

    Private Sub WhiteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WhiteToolStripMenuItem.Click
        Dim c As Object
        For Each c In Controls
            If TypeOf c Is IButtonControl Or TypeOf c Is MenuStrip Or TypeOf c Is TextBoxBase Then
                c.ForeColor = Color.Black
            Else
                c.ForeColor = Color.White
            End If
        Next
        For Each c In Panel_Video.Controls
            c.ForeColor = Color.White
            If TypeOf c Is IButtonControl Then
                c.ForeColor = Color.Black
            Else
                c.ForeColor = Color.White
            End If
        Next
        For Each c In Panel_Sound.Controls
            c.ForeColor = Color.White
        Next
        For Each c In Panel_Input.Controls
            c.ForeColor = Color.White
            If TypeOf c Is ButtonBase Then
                c.ForeColor = Color.Black
            End If
            If c.name = "CheckBox18" Or c.name = "CheckBox_outputs" Then
                c.ForeColor = Color.White
            End If
        Next
        For Each c In Panel_Network.Controls
            c.ForeColor = Color.White
            If TypeOf c Is IButtonControl Then
                c.ForeColor = Color.Black
            Else
                c.ForeColor = Color.White
            End If
        Next
        For Each c In Panel_ponmi.Controls
            c.ForeColor = Color.White
            If TypeOf c Is IButtonControl Then
                c.ForeColor = Color.Black
            Else
                c.ForeColor = Color.White
            End If
        Next
        Pub_Forecolor_s = Color.White
        Forecolor_s = "White"
    End Sub

    Private Sub BlackToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BlackToolStripMenuItem.Click
        Dim c As Object
        For Each c In Me.Controls
            If TypeOf c Is DataGridView Then
                c.ForeColor = Color.White
            Else
                c.ForeColor = Color.Black
            End If
        Next
        For Each c In Panel_Video.Controls
            c.ForeColor = Color.Black
        Next
        For Each c In Panel_Sound.Controls
            c.ForeColor = Color.Black
        Next
        For Each c In Panel_Input.Controls
            c.ForeColor = Color.Black
        Next
        For Each c In Panel_Network.Controls
            c.ForeColor = Color.Black
            If TypeOf c Is TextBoxBase Then
                c.ForeColor = Color.White
            End If
        Next
        For Each c In Panel_ponmi.Controls
            c.ForeColor = Color.Black
        Next
        Pub_Forecolor_s = Color.Black
        Forecolor_s = "Balck"
    End Sub

    Private Sub DefoultToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DefoultToolStripMenuItem.Click
        Bgcolor_R = 147
        Bgcolor_G = 0
        Bgcolor_B = 80
        Me.BackColor = Color.FromArgb(255, Bgcolor_R, Bgcolor_G, Bgcolor_B)
        Label_path.BackColor = Color.FromArgb(255, Bgcolor_R, Bgcolor_G, Bgcolor_B)
        WhiteToolStripMenuItem.PerformClick()
    End Sub

    Private Sub Button_Ponmi_Click(sender As Object, e As EventArgs) Handles Button_Ponmi.Click
        Panel_Video.Left = 2000
        Panel_Video.Top = 336
        Panel_Sound.Left = 2000
        Panel_Sound.Top = 336
        Panel_Input.Left = 2000
        Panel_Input.Top = 336
        Panel_Network.Left = 2000
        Panel_Network.Top = 336
        Panel_ponmi.Left = 612
        Panel_ponmi.Top = 336
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        System.Diagnostics.Process.Start("https://github.com/BackPonBeauty/Supermodel3-PonMi?tab=readme-ov-file#ponmi")
    End Sub

    Private Sub Surround_Tick(sender As Object, e As EventArgs) Handles Surround.Tick
        SurroundingSub1()
    End Sub

    Dim Center_i As Integer = 0
    Private Sub SurroundingSub1()
        Me.SuspendLayout()
        Dim controller = New SharpDX.XInput.Controller(SharpDX.XInput.UserIndex.One)
        If controller.IsConnected Then
            Dim state = controller.GetState()
            Dim a = state.Gamepad.Buttons

            Dim n As String = TentoTwo(a).PadLeft(16, "0")
            'TextBox1.Text = n
            Dim lever As String = Strings.Right(n, 4)
            Dim n1 = "0"
            Dim n2 = "0"
            Dim n3 = "0"
            Dim n4 = "0"
            Dim x = state.Gamepad.LeftThumbX
            Dim y = state.Gamepad.LeftThumbY

            If (y <> Center_i Or x <> Center_i) And lever = "0000" Then
                If y > Center_i Then
                    n1 = "1"
                End If
                If y < Center_i Then
                    n2 = "1"
                End If
                If x < Center_i Then
                    n3 = "1"
                End If
                If x > Center_i Then
                    n4 = "1"
                End If
                lever = n4 & n3 & n2 & n1
            End If

            joybox1.Image = My.Resources.ResourceManager.GetObject("_" & lever.ToString)

            'Shift
            Dim ss As String = n.Substring(3, 1)
            If ss = "0" Then
                shiftbox1.Image = Nothing

            Else
                shiftbox1.Image = My.Resources.sd

            End If


            'Beat
            Dim bb As String = n.Substring(1, 1)
            If bb = "0" Then
                beatbox1.Image = Nothing

            Else
                beatbox1.Image = My.Resources.bd

            End If

            'Charge
            Dim cc As String = n.Substring(0, 1)
            If cc = "0" Then
                chargebox1.Image = Nothing

            Else
                chargebox1.Image = My.Resources.cd

            End If


            'Jump
            Dim jj As String = n.Substring(6, 1)
            If jj = "0" Then
                jumpbox1.Image = Nothing

            Else
                jumpbox1.Image = My.Resources.jd

            End If


        Else
            joybox1.Image = Nothing
        End If
        Me.ResumeLayout(True)
    End Sub

    Private Function TentoTwo(ByVal value As String) As String
        If Math.Floor(value / 2) Then
            Return TentoTwo(Math.Floor(value / 2)) + CStr(value Mod 2)
        End If
        Return CStr(value Mod 2)
    End Function

    Private Sub ControlerX4_Click(sender As Object, e As EventArgs) Handles Button_X.Click
        If Surround.Enabled = True Then
            Surround.Enabled = False
            Button_X.Text = "Disabled"
            joybox1.Image = Nothing
        Else
            Surround.Enabled = True
            Button_X.Text = "Enabled"
        End If
    End Sub


    Public scanline_type As String = "PICTURE" 'Or "PICTURE"
    Dim control_F As Boolean = False
    Sub KeybordHooker1_KeyDown(sender As Object, e As KeyBoardHookerEventArgs) Handles KeyboardHooker1.KeyDown1
        Dim Tabb As String = CStr(e.vkCode)
        Label2.Text = Tabb
        If Tabb = "162" Then
            control_F = True
        End If

        If control_F Then
            If Tabb = "73" Then 'Escape key
                If Front_F = False Then
                    Front_F = True
                    Me.BringToFront()
                Else
                    Front_F = False
                    Me.SendToBack()
                End If

            End If
            If Tabb = "83" Then
                'Debug(Process.GetProcessesByName("Supermodel").Count)
                If Process.GetProcessesByName("Supermodel").Count <> 0 Then
                    If ScanLine_F = False Then
                        If scanline_type = "PICTURE" Then
                            scanline_type = "LINE1"
                        ElseIf scanline_type = "LINE1" Then
                            scanline_type = "LINE2"
                        ElseIf scanline_type = "LINE2" Then
                            scanline_type = "PICTURE"
                            ScanLine_F = True
                        End If
                        ScanLine.Width = Integer.Parse(Label_xRes.Text.ToString)
                        ScanLine.Height = Integer.Parse(Label_yRes.Text.ToString)
                        ScanLine.Top = Integer.Parse(Label_yPos.Text.ToString)
                        ScanLine.Left = Integer.Parse(Label_xPos.Text.ToString)
                        ScanLine.PictureBox1.Top = 0
                        ScanLine.PictureBox1.Left = 0
                        ScanLine.PictureBox1.Width = Integer.Parse(Label_xRes.Text.ToString)
                        ScanLine.PictureBox1.Height = Integer.Parse(Label_yRes.Text.ToString)

                        ScanLine.Top = Integer.Parse(Label_yPos.Text.ToString)
                        ScanLine.Left = Integer.Parse(Label_xPos.Text.ToString)
                        ScanLine.Draw_Scanline(scanline_type)
                        ScanLine.Show()
                    Else
                        ScanLine_F = False
                        ScanLine.Close()
                        ScanLine.Dispose()
                    End If
                Else
                    ScanLine_F = False
                    ScanLine.Close()
                    ScanLine.Dispose()
                End If
            End If
            If Tabb = "79" Then
                If Opacity_D > 1 Then
                    Opacity_D -= 1
                    ScanLine.Opacity -= 0.1
                End If
            End If
            If Tabb = "80" Then
                If Opacity_D < 10 Then
                    Opacity_D += 1
                    ScanLine.Opacity += 0.1
                End If
            End If
            End If
    End Sub

    Sub KeybordHooker1_Keyup(sender As Object, e As KeyBoardHookerEventArgs) Handles KeyboardHooker1.KeyUp1
        Dim Tabb As String = CStr(e.vkCode)
        Label2.Text = Tabb
        If Tabb = "162" Then
            control_F = False
        End If
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button_hook.Click
        If KeyboardHooker1.Hooked = False Then
            If KeyboardHooker1.MouseHookStart() = True Then
                Button_hook.Text = "Enabled"
                Scanline_Enabled = True
            End If
        Else
            If KeyboardHooker1.MouseHookEnd() = True Then
                Button_hook.Text = "Disabled"
                Scanline_Enabled = False
            End If
        End If
    End Sub

    Private Sub Button1_Click_2(sender As Object, e As EventArgs) Handles Button_Get_Global_IPAddress.Click
        Label_Global_IPaddress.Text = Get_Global_IPaddress()
    End Sub


End Class
