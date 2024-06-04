using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for ManHinhTinhTien.xaml
    /// </summary>
    public partial class ManHinhTinhTien : Window
    {
        ThongTinDatPhong currentRoom = null;
        NumberFormatInfo nfi = new CultureInfo("de", false).NumberFormat;
        public ManHinhTinhTien(ThongTinDatPhong ttph)
        {
            InitializeComponent();
            currentRoom = ttph;
            
        }
        private void CapNhatHienThiPhong()
        {
            if (currentRoom != null)
            {
                currentRoom.TinhTien();
                Dao dao = new Dao();
                lbRoomName.Content = dao.LayPhongHatByID(currentRoom.IDphongHat).tenPhong;

                timeIn.Content = currentRoom.thoiGianVao.ToString();
                lbTienPhong.Content = currentRoom.tienPhong.ToString("N0", nfi);
                lbTienSanPham.Content = currentRoom.tienSanPham.ToString("N0", nfi);
                lbPhuThu.Content = currentRoom.phuThu.ToString("N0", nfi);
                lbTongPhu.Content = (currentRoom.tienSanPham + currentRoom.tienPhong + currentRoom.phuThu).ToString("N0", nfi);
                lbKhuyenMai.Content = (currentRoom.disCount / 100 * currentRoom.tongPhu).ToString("N0", nfi);
                txtKhuyenMai.Text = currentRoom.disCount.ToString();
                lbTong.Content = currentRoom.tongTien.ToString("N0", nfi);
            }
            else
            {
                timeIn.Content = "...";
                lbTienPhong.Content = "...";
                lbTienSanPham.Content = "...";
                txtKhuyenMai.Text = "0";
                lbPhuThu.Content = "...";
                lbTongPhu.Content = "...";
                lbKhuyenMai.Content = "...";
                lbTong.Content = "...";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CapNhatHienThiPhong();
        }

        //Button tinh tien
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Dao dao = new Dao();
            currentRoom.thoiGianRa=DateTime.Now;
            currentRoom.TinhTien();
            string maHoaDon = GenID();
            if (dao.GhiNhanHoaDon(maHoaDon, currentRoom, "test",true))
            {
                MessageBox.Show("Đã lưu hóa đơn thanh toán mã:  " + maHoaDon, "QuanLyKaraoke", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Lỗi lưu dữ liệu. Vui lòng thử lại", "QuanLyKaraoke", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            CapNhatHienThiPhong();
            this.Close();

        }

        private string GenID ()
        {
            DateTime date = DateTime.Now;
            string mahd = date.ToString("ddMMyyyyhhmmss");
           
            return mahd;
        }

        private void HoaDon(object sender, RoutedEventArgs e)
        {
            string hoadon = "";
            hoadon += "------------HOA DON KARAOKE------------\n\n";
            hoadon += "Ma Hoa Don: " + currentRoom.IDdatPhong +"\n";
            hoadon += "Thoi Gian Tao: " + DateTime.Now.ToString()+"\n";
            hoadon += "Thoi gian Hat: " + (DateTime.Now - currentRoom.thoiGianVao).ToString();
            hoadon += "\n\nTien Gio: " + currentRoom.tienPhong.ToString("N0",nfi);
            hoadon += "\nTien San Pham: " + currentRoom.tienSanPham.ToString("N0", nfi);
            hoadon += "\nPhu Thu: " + currentRoom.phuThu.ToString("N0", nfi);
            hoadon += "\nTong Tam: " + currentRoom.tongPhu.ToString("N0", nfi);
            hoadon += "\nChiet Khau : " + currentRoom.disCount.ToString() + "%";
            hoadon += "\n\nTONG CONG : " + currentRoom.tongTien.ToString("N0", nfi);
            hoadon += "\n------------Cam on qui khach-------------\n\n";

            tbhoadon.Text = hoadon;
        }

        private void InHoaDon(object sender, RoutedEventArgs e)
        {
            PrintDialog lg = new PrintDialog();
            lg.ShowDialog();
        }
    }
}
