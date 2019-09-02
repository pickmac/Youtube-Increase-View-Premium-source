Public Class frm1
    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        If LinkLabel1.Text = "ENGLISH TRANS" Then
            Label5.Text = "Oh NO LOOKS LIKE WE'RE MAINTAINING 1 FEATURE
AS FOLLOWS: ADD FIREFOX TO THE SYSTEM | {END}
PLEASE BACK BACK, WE'RE REPLYING UPDATE FAST
ASKED TO SERVE YOU, THANK YOU !!!!"
            LinkLabel1.Text = "VIET NAM"
        Else

            Label5.Text = "Ồ KHÔNG CÓ VẺ NHƯ CHÚNG TÔI ĐANG BẢO TRÌ 1 SỐ TÍNH NĂNG
NHƯ SAU : THÊM FIREFOX VÀO HỆ THỐNG | {END}
VUI LÒNG QUAY LẠI SAU, CHÚNG TÔI SẼ CỐ GẮNG UPDATE NHANH
NHẤT CÓ THỂ ĐỂ PHỤ VỤ CHO BẠN, XIN CẢM ƠN !!!!
"
            LinkLabel1.Text = "ENGLISH TRANS"
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Application.Exit()
    End Sub
End Class