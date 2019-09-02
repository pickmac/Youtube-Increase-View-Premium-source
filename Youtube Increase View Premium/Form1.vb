Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Threading
Imports Microsoft.VisualBasic.CompilerServices
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium.Support.UI
Imports OpenQA.Selenium.Support.UI.SelectElement
Imports WindowScrape
Imports WindowScrape.Types
Imports OpenQA.Selenium.Firefox
'lasted build...
'pub source on github by Nguyễn Đắc Tài
'chúc người phát triển lại mã nguồn này thành công
'hơn mình ... <3
Public Class Form1
    Private randomprxy As New Random
    Private randomugclient As New Random
    Dim useproxy As Boolean = False
    Dim autodeltrack As Boolean = False
    Private currentThread As List(Of Thread) = New List(Of Thread)()
    Dim stopthred As Boolean = False
    Private _reporterWin As frm1 = New frm1()
    Dim randomvitri As Random = New Random
    Dim screenWidth As Integer = Screen.PrimaryScreen.Bounds.Width - 70
    Dim screenHeight As Integer = Screen.PrimaryScreen.Bounds.Height - 70
    Dim loaitrinhduyet As String = "firefox"
    Sub New()
        InitializeComponent()
        Control.CheckForIllegalCrossThreadCalls = False
        MetroSetTabPage1.BaseColor = Color.FromArgb(30, 30, 30)
        MetroSetTabPage2.BaseColor = Color.FromArgb(30, 30, 30)
        btnopenproxy.IconZoom = 60
        btnopenug.IconZoom = 60
        btnstart.IconZoom = 60
        btnstop.IconZoom = 60
        btnopenug.IconVisible = True
        btnopenproxy.IconVisible = True
        btnstart.IconVisible = True
        btnstop.IconVisible = True
        txtlink.BackColor = Color.FromArgb(30, 30, 30)
        txtthread.BackColor = Color.FromArgb(30, 30, 30)
        txttimeview.BackColor = Color.FromArgb(30, 30, 30)
        txtlog.ScrollBars = ScrollBars.Vertical
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lbug.Text = ListBox2.Items.Count
        txtlog.AppendText(DateTime.Now & " CHÀO BẠN NGÀY MỚI CHÚC BẠN NGÀY MỚI VUI VẺ, CẢM ƠN BẠN ĐÃ SỬ DỤNG TOOL CỦA MÌNH NHÉ ! YÊU BẠN <3" & vbCrLf)
        'Thread.Sleep(TimeSpan.FromSeconds(4))
        txtlog.AppendText(DateTime.Now & " LOADED " & lbug.Text & " USER AGENT FROM TOOL" & vbCrLf)
        cmbtimeview.SelectedIndex = 0
        If Not System.IO.File.Exists("chromedriver.exe") Then
            Try
                Dim b() As Byte = My.Resources.chromedriver
                System.IO.File.WriteAllBytes("chromedriver.exe", b)
            Catch ex As Exception
                txtlog.AppendText(DateTime.Now & " GHI ĐÈ FILE EXE THẤT BẠI VUI LÒNG CHẠY VỚI QUYỀN ADMIN | FAILED WRITE EXE FILE PLEASE RUN AS ADMIN" & vbCrLf)
                MsgBox("GHI ĐÈ FILE CHROMEDRIVER.EXE THẤT BẠI VUI LÒNG CHẠY LẠI VỚI QUYỀN ADMIN !")
            End Try
        Else
        End If
    End Sub

    Private Sub MetroSetTabPage1_Click(sender As Object, e As EventArgs) Handles MetroSetTabPage1.Click

    End Sub

    Private Sub Txtlog_TextChanged(sender As Object, e As EventArgs) Handles txtlog.TextChanged

    End Sub

    Private Sub Btnopenproxy_Click(sender As Object, e As EventArgs) Handles btnopenproxy.Click
        useproxy = True
        swuseproxy.Switched = True
        ListBox1.Items.Clear()
        On Error Resume Next
        Dim openfile As New OpenFileDialog
        openfile.Title = "Proxy List File |*.txt"
        openfile.ShowDialog()
        Dim txtline() As String = IO.File.ReadAllLines(openfile.FileName)
        ListBox1.Items.AddRange(txtline)
        lbproxy.Text = ListBox1.Items.Count
        txtlog.AppendText(DateTime.Now & " LOADED " & lbproxy.Text & " PROXY FROM " & openfile.FileName & vbCrLf)
    End Sub

    Private Sub Btnopenug_Click(sender As Object, e As EventArgs) Handles btnopenug.Click
        ListBox2.Items.Clear()
        On Error Resume Next
        Dim openfile As New OpenFileDialog
        openfile.Title = "User Agent List File |*.txt"
        openfile.ShowDialog()
        Dim txtline() As String = IO.File.ReadAllLines(openfile.FileName)
        ListBox2.Items.AddRange(txtline)
        If ListBox2.Items.Count = 0 Then
            txtlog.AppendText(DateTime.Now & " ĐỌC FILE USER AGENT THẤT BẠI ! :< | FAILED READ USER AGENT FILE PLEASE TRY AGAIN " & vbCrLf)
        Else
            txtlog.AppendText(DateTime.Now & " LOADED " & lbug.Text & " USER AGENT FROM " & openfile.FileName & vbCrLf)
            lbug.Text = ListBox2.Items.Count
        End If

    End Sub

    Private Sub Btnstart_Click(sender As Object, e As EventArgs) Handles btnstart.Click
        If Not txtlink.Text.Contains("youtube.com") Then
            MsgBox("CHỈ HỖ TRỢ YOUTBE THÔI NHÉ BRO :3 VUI LÒNG ĐIỀN ĐÚNG LINK NÀO")
            txtlog.AppendText(DateTime.Now & " PLEASE CHECK YOUR LINK, ERROR FORMAT YOUTUBE LINK " & txtlink.Text & vbCrLf)
        Else
        End If
        If ListBox1.Items.Count = 0 Then
            useproxy = False
            txtlog.AppendText(DateTime.Now & " BẠN CHƯA THÊM LIST PROXY NÊN CHÚNG TÔI ĐÃ TỰ ĐỘNG TẮT PROXY ! " & vbCrLf)
            swuseproxy.Switched = False
        End If
        If ListBox2.Items.Count = 0 Then
            MsgBox("THIẾU USER AGENT RỒI VUI LÒNG THÊM LIST VÀO ĐỂ CHẠY NHÉ ! LMAO")
            txtlog.AppendText(DateTime.Now & " THIẾU LIST USER AGENT RỒI .-. " & txtlink.Text & vbCrLf)
        Else
        End If
        Try
            stopthred = False
            If stopthred = False Then
                txtlog.AppendText(DateTime.Now & " LINK YOUTBE = " & txtlink.Text & vbCrLf)
                txtlog.AppendText(DateTime.Now & " ---> STARTED RUN WITH " & txtthread.Text & " THREAD " & vbCrLf)
                lbstatus.Text = "RUNNING"
                lbstatus.ForeColor = Color.Lime
                Dim luong1 As String = txtthread.Text
                Dim delay1 As String = txtdelay.Text
                Dim thoigian1 As Integer = Integer.Parse(delay1)
                Dim thrSTART1 As ThreadStart
                Task.Run(Sub()
                             While True
                                 Dim soluong1 As Integer = Integer.Parse(luong1)
                                 For i As Integer = 0 To soluong1 - 1
                                     Dim thrd As ThreadStart = thrSTART1
                                     Dim start As ThreadStart = thrd
                                     If thrd Is Nothing Then
                                         Dim threadstart As ThreadStart = Sub()
                                                                              Me.viewyoutube(randomvitri.Next(1, screenWidth), randomvitri.Next(1, screenHeight))
                                                                          End Sub
                                         Dim threadstart2 As ThreadStart = threadstart
                                         thrSTART1 = threadstart
                                         start = threadstart2
                                     End If
                                     Dim t As Thread = New Thread(start) With {.IsBackground = True}
                                     t.Start()
                                     Me.currentThread.Add(t)
                                 Next
                                 For Each item As Thread In Me.currentThread
                                     item.Join()
                                 Next
                                 Control.CheckForIllegalCrossThreadCalls = False
                                 System.Threading.Thread.Sleep(thoigian1)
                                 System.Threading.Thread.Sleep(TimeSpan.FromSeconds(txtdelay.Text))
                                 txtlog.AppendText(DateTime.Now & " ĐANG NGHỈ TRONG THỜI GIAN " & txtdelay.Text & " GIÂY TRƯỚC KHI BẮT ĐẦU LẠI" & vbCrLf)
                                 If stopthred = True Then
                                     txtlog.AppendText(DateTime.Now & " ---> đã dừng chạy ! " & vbCrLf)
                                     Exit While
                                     Exit Sub
                                 End If

                             End While
                         End Sub)
            Else
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub viewyoutube(height As Integer, witth As Integer)
        Dim rdps As New Random
        Dim rdps2 As New Random
        Dim randomug As Integer = ListBox2.Items.Count
        Dim ugclient As String = ListBox2.Items(randomugclient.Next(randomug)).ToString()
        If stopthred = False Then
            Dim service As ChromeDriverService = ChromeDriverService.CreateDefaultService()
            service.HideCommandPromptWindow = True
            Dim options As ChromeOptions = New ChromeOptions()
            If useproxy = True Then
                Dim rdproxy As Integer = ListBox1.Items.Count
                Dim proxyclient As String = ListBox1.Items(randomprxy.Next(rdproxy)).ToString()
                txtlog.AppendText(DateTime.Now & " SET PROXY WITH " & proxyclient & vbCrLf)
                options.AddArgument("--proxy-server=" & proxyclient)
            Else
            End If
            options.AddArgument("--window-size=" & txtdai.Text & "," & txtrong.Text)
            options.AddArgument("--user-agent=" & ugclient)
            txtlog.AppendText(DateTime.Now & " SET USER AGENT WITH " & ugclient & vbCrLf)
            Dim webDriver As IWebDriver = New ChromeDriver(service, options, TimeSpan.FromSeconds(60.0))
            webDriver.Manage().Window.Position = New Point(Width, height)
            webDriver.Navigate().GoToUrl(txtlink.Text)
            Dim wait As IWait(Of IWebDriver) = New WebDriverWait(webDriver, TimeSpan.FromSeconds(30.0))
            wait.Until(Of Boolean)(Function(driver1 As IWebDriver) CType(webDriver, IJavaScriptExecutor).ExecuteScript("return document.readyState", New Object(-1) {}).Equals("complete"))
            If cmbtimeview.SelectedIndex = 0 Then
                txtlog.AppendText(DateTime.Now & " ĐANG XEM VIDEO TRONG " & txttimeview.Text & " GIÂY" & vbCrLf)
                Thread.Sleep(TimeSpan.FromSeconds(txttimeview.Text))
                txtlog.AppendText(DateTime.Now & " ĐÃ XEM XONG, ĐANG KHỞI ĐỘNG LẠI CHROME VÀ XEM TIẾP !" & vbCrLf)
                If autodeltrack = True Then
                    webDriver.Manage().Cookies.DeleteAllCookies()
                    Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 2")
                    Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 1")
                    txtlog.AppendText(DateTime.Now & " ĐÃ XÓA COOKIE VÀ MÃ THEO DÕI !" & vbCrLf)
                Else

                End If
                webDriver.Quit()

            End If
            If cmbtimeview.SelectedIndex = 1 Then
                txtlog.AppendText(DateTime.Now & " ĐANG XEM VIDEO TRONG " & txttimeview.Text & " PHÚT" & vbCrLf)
                Thread.Sleep(TimeSpan.FromMinutes(txttimeview.Text))
                txtlog.AppendText(DateTime.Now & " ĐÃ XEM XONG, ĐANG KHỞI ĐỘNG LẠI CHROME VÀ XEM TIẾP !" & vbCrLf)
                If autodeltrack = True Then
                    webDriver.Manage().Cookies.DeleteAllCookies()
                    Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 2")
                    Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 1")
                    txtlog.AppendText(DateTime.Now & " ĐÃ XÓA COOKIE VÀ MÃ THEO DÕI !")
                Else

                End If
                webDriver.Quit()
            End If
            If cmbtimeview.SelectedIndex = 2 Then
                txtlog.AppendText(DateTime.Now & " ĐANG XEM VIDEO TRONG " & txttimeview.Text & " GIỜ" & vbCrLf)
                Thread.Sleep(TimeSpan.FromHours(txttimeview.Text))
                txtlog.AppendText(DateTime.Now & " ĐÃ XEM XONG, ĐANG KHỞI ĐỘNG LẠI CHROME VÀ XEM TIẾP !" & vbCrLf)
                If autodeltrack = True Then
                    webDriver.Manage().Cookies.DeleteAllCookies()
                    Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 2")
                    Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 1")
                    txtlog.AppendText(DateTime.Now & " ĐÃ XÓA COOKIE VÀ MÃ THEO DÕI !")
                Else

                End If
                webDriver.Quit()
            End If

        Else
            stopthred = True
            For Each item As Thread In Me.currentThread
                item.Abort()
            Next

            Dim chromeDriverProcesses As Process() = Process.GetProcessesByName("chromedriver")
            For Each chromeDriverProcess In chromeDriverProcesses
                chromeDriverProcess.Kill()
            Next
            txtlog.AppendText(DateTime.Now & " ĐÃ DỪNG CÁC LUỒNG CHẠY !" & vbCrLf)
        End If
    End Sub


    Private Sub viewoutbefirefox()
        Dim fireDriverService = FirefoxDriverService.CreateDefaultService()
        Dim profileManager = New FirefoxProfileManager()
        Dim proxy = New Proxy()
        fireDriverService.HideCommandPromptWindow = True
        Dim [option] As FirefoxOptions = New FirefoxOptions()
        proxy.HttpProxy = "89.208.212.2:80"
        [option].Profile = profileManager.GetProfile("Selenium")
        [option].Profile.SetProxyPreferences(proxy)
        Dim Driver = New FirefoxDriver(fireDriverService, [option])
        Driver.Navigate().GoToUrl("http://whatismyip.host/")
        'Driver.Quit()
        'Driver.Quit()
        'Driver.Quit()
        For Each item As Thread In Me.currentThread
            item.Abort()
        Next
    End Sub
    Private Sub BunifuImageButton1_Click(sender As Object, e As EventArgs) Handles BunifuImageButton1.Click
        Dim value As Integer
        If Int32.TryParse(txtthread.Text, value) Then
            value = value + 1
            txtthread.Text = value.ToString()
            lbthread.Text = txtthread.Text
            txtlog.AppendText(DateTime.Now & " THREAD = " & lbthread.Text & vbCrLf)
        Else
            MsgBox("Số luồng phải là số không phải chữ nhaaa !", MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub BunifuImageButton2_Click(sender As Object, e As EventArgs) Handles BunifuImageButton2.Click
        Dim value As Integer
        If Int32.TryParse(txtthread.Text, value) Then
            value = value - 1
            txtthread.Text = value.ToString()
            If txtthread.Text < 1 Then
                MessageBox.Show("SỐ LUỒNG NHỎ NHẤT LÀ 1 !")
                txtthread.Text = "1"
            End If
            lbthread.Text = txtthread.Text
            txtlog.AppendText(DateTime.Now & " THREAD = " & lbthread.Text & vbCrLf)
        Else
            MessageBox.Show("Số luồng phải là số không chữ nhaaa !")
        End If
    End Sub

    Private Sub Txtthread_OnValueChanged(sender As Object, e As EventArgs) Handles txtthread.OnValueChanged
        Try
            lbthread.Text = txtthread.Text
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Swuseproxy_SwitchedChanged(sender As Object) Handles swuseproxy.SwitchedChanged
        If swuseproxy.Switched = True Then
            useproxy = True
            Label7.Text = "PROXY : ON"
            txtlog.AppendText(DateTime.Now & " USE PROXY = ON " & vbCrLf)
        Else
            useproxy = False
            Label7.Text = "PROXY : OFF"
            txtlog.AppendText(DateTime.Now & " USE PROXY = OFF " & vbCrLf)
        End If
    End Sub

    Private Sub Swdeltrack_SwitchedChanged(sender As Object) Handles swdeltrack.SwitchedChanged
        If swdeltrack.Switched = True Then
            autodeltrack = True
            Label14.Text = "AUTO DELETE COOKIE : ON"
            txtlog.AppendText(DateTime.Now & " AUTO DELETE COOKIE = ON " & vbCrLf)
        Else
            autodeltrack = False
            Label14.Text = "AUTO DELETE COOKIE : OFF"
            txtlog.AppendText(DateTime.Now & " AUTO DELETE COOKIE = OFF " & vbCrLf)
        End If
    End Sub

    Private Sub Txttimeview_OnValueChanged(sender As Object, e As EventArgs) Handles txttimeview.OnValueChanged
        If cmbtimeview.SelectedIndex = 0 Then
            txtlog.AppendText(DateTime.Now & " THỜI GIAN XEM VIDEO : " & txttimeview.Text & " GIÂY" & vbCrLf)
        End If
        If cmbtimeview.SelectedIndex = 1 Then
            txtlog.AppendText(DateTime.Now & " THỜI GIAN XEM VIDEO : " & txttimeview.Text & " PHÚT" & vbCrLf)
        End If
        If cmbtimeview.SelectedIndex = 2 Then
            txtlog.AppendText(DateTime.Now & " THỜI GIAN XEM VIDEO : " & txttimeview.Text & " GIỜ" & vbCrLf)
        End If
    End Sub

    Private Sub Cmbtimeview_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbtimeview.SelectedIndexChanged
        If cmbtimeview.SelectedIndex = 0 Then
            txtlog.AppendText(DateTime.Now & " THỜI GIAN XEM VIDEO : " & txttimeview.Text & " GIÂY" & vbCrLf)
        End If
        If cmbtimeview.SelectedIndex = 1 Then
            txtlog.AppendText(DateTime.Now & " THỜI GIAN XEM VIDEO : " & txttimeview.Text & " PHÚT" & vbCrLf)
        End If
        If cmbtimeview.SelectedIndex = 2 Then
            txtlog.AppendText(DateTime.Now & " THỜI GIAN XEM VIDEO : " & txttimeview.Text & " GIỜ" & vbCrLf)
        End If
    End Sub

    Private Sub BunifuImageButton4_Click(sender As Object, e As EventArgs) Handles BunifuImageButton4.Click
        Dim value As Integer
        If Int32.TryParse(txtthread.Text, value) Then
            value = value + 1
            txttimeview.Text = value.ToString()
        Else
            MsgBox("Số thời gian view phải là số không phải chữ nhaaa !", MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub BunifuImageButton3_Click(sender As Object, e As EventArgs) Handles BunifuImageButton3.Click
        Dim value As Integer
        If Int32.TryParse(txtthread.Text, value) Then
            value = value - 1
            txttimeview.Text = value.ToString()
            If txttimeview.Text < 1 Then
                MessageBox.Show("SỐ THỜI GIAN VIEW NHỎ NHẤT LÀ 1 !")
                txttimeview.Text = "1"
            End If
        Else
            MessageBox.Show("Số thời gian view phải là số không chữ nhaaa !")
        End If
    End Sub

    Private Sub Txtlink_OnValueChanged(sender As Object, e As EventArgs) Handles txtlink.OnValueChanged

    End Sub

    Private Sub Btnstop_Click(sender As Object, e As EventArgs) Handles btnstop.Click
        Try
            stopthred = True
            For Each item As Thread In Me.currentThread
                item.Abort()
                lbstatus.Text = "STOPPED"
                lbstatus.ForeColor = Color.Red
            Next
            Dim chromeDriverProcesses As Process() = Process.GetProcessesByName("chromedriver")
            For Each chromeDriverProcess In chromeDriverProcesses
                chromeDriverProcess.Kill()
            Next
            txtlog.AppendText(DateTime.Now & " ĐÃ DỪNG CÁC LUỒNG CHẠY !" & vbCrLf)
        Catch ex As Exception

        End Try


    End Sub



    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        txtlog.Text = ""
    End Sub

    Private Sub PictureBox1_MouseHover(sender As Object, e As EventArgs) Handles PictureBox1.MouseHover
        Label27.Location = New Point(248, 240)
        Label27.Visible = True
    End Sub

    Private Sub PictureBox1_MouseLeave(sender As Object, e As EventArgs) Handles PictureBox1.MouseLeave
        Label27.Location = New Point(248, 240)
        Label27.Visible = False
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        MsgBox("ĐÂY LÀ 1 CÔNG CỤ MIỄN PHÍ DO NHÀ PHÁT HÀNH TOOL
https://tienichmmo.net ( TIỆN ÍCH MMO x TN STUDIO ) xây dựng
cho những ai làm bên youtube, mục đích sử dụng tool làm gì?
tool được sử dụng để tăng lượt xem youtube thông qua proxy và
user agent, khi bạn cho nó xem 1 video dài trên 2 tiếng trong kênh
của bạn thì bạn có thể nhận ra giờ xem lên rất nhanh !, tool tự thiết kế
hoàn toàn tự động từ việc xem đến khi xem xong tự động xóa các cookie
và set lại proxy mới user agent mới sau đó xem lại tiếp ! tool được chăm chút
rất kĩ từ phần giao diện tối ( Dark theme, đến cách sắp xếp nội dung hiển thị
màu nhẹ, chữ to rõ dễ đọc ) đến phần chức năng được tự động hóa hoàn toàn
bằng công nghệ selenium và trí thông minh nhân tạo ( AI ) thật ra là if else nhiều
thôi à :v, đây là 1 công cụ hoàn toàn miễn phí nên chúng tôi không khuyến khích
bạn vì 1 chút lợi nhuận cá nhân mà đem đi bán nhé ! buồn lắm đấy nếu công cụ này
giúp ích cho bạn thì bạn có thể bỏ ra vài ngàn lẻ để donate cho nhà phát triển nhé !
đó sẽ là 1 động lực rất lớn để tui phát triển thêm nhiều tính năng hay hơn nữa !
cảm ơn bạn đã đọc đến đây, chúc bạn thật nhiều sức khỏe và may mắn trong life !
", MsgBoxStyle.Information)
    End Sub

    Private Sub BunifuImageButton5_Click(sender As Object, e As EventArgs) Handles BunifuImageButton5.Click
        Process.Start("https://www.facebook.com/100025696380578")
    End Sub

    Private Sub BunifuImageButton6_Click(sender As Object, e As EventArgs) Handles BunifuImageButton6.Click
        Process.Start("https://www.youtube.com/channel/UCFkKjznKpg3eZUDSrcw_Psw")
    End Sub

    Private Sub BunifuImageButton7_Click(sender As Object, e As EventArgs) Handles BunifuImageButton7.Click
        Process.Start("https://tienichmmo.net")
    End Sub

    Private Sub BunifuImageButton8_Click(sender As Object, e As EventArgs) Handles BunifuImageButton8.Click
        MsgBox("Discord : tn16201
#0761")
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        Process.Start("https://www.paypal.me/nguyendactai")
    End Sub
End Class
'Dim rc As Integer = ListBox1.Items.Count
'txtlog.AppendText(ListBox1.Items(randomprxy.Next(rc)).ToString() & vbCrLf)
