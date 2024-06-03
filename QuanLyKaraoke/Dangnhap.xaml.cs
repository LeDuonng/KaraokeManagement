using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace QuanLyKaraoke
{
    /// <summary>
    /// Interaction logic for Dangnhap.xaml
    /// </summary>
    public partial class Dangnhap : ThemedWindow
    {
        public Dangnhap()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Không được để trống tài khoản hoặc mật khẩu !", "Đăng nhập thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (username == "quanly" && password == "1")
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Window.GetWindow(this).Close();
            }
            else if (username == "nhanvien" && password == "1")
            {
                MainWindowNV mainWindownv = new MainWindowNV();
                mainWindownv.Show();
                Window.GetWindow(this).Close();
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu sai. Vui lòng thử lại !.", "Đăng nhập thất bại", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
