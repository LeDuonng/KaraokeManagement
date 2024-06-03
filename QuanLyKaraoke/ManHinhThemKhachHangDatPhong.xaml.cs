using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for ManHinhThemKhachHangDatPhong.xaml
    /// </summary>
    public partial class ManHinhThemKhachHangDatPhong : Window
    {
        public ManHinhThemKhachHangDatPhong()
        {
            InitializeComponent();
        }

        private void Them_Click(object sender, RoutedEventArgs e)
        {
            KhachHang kh = new KhachHang();
            kh.hoTen = txt_HoTen.Text;
            kh.soDT = txt_sDT.Text;
            kh.diaChi = txt_DiaChi.Text;
            Dao dao = new Dao();
            if(dao.ThemKhachHang(kh))
            {
                MessageBox.Show("Lưu thông tin thành công!!!");
            }
            else
            {
                MessageBox.Show("Không lưu được thông tin!!!");
            }
        }

        private void Huy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
