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
    /// Interaction logic for ManHinhChinhSuaPhongHat.xaml
    /// </summary>
    public partial class ManHinhChinhSuaPhongHat : Window
    {
        NumberFormatInfo nfi = new CultureInfo("de", false).NumberFormat;
        PhongHat phongHat;
        public ManHinhChinhSuaPhongHat(PhongHat _phongHat)
        {
            InitializeComponent();
            phongHat = _phongHat;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<LoaiPhong> dsLoaiPhong;
            Dao dao = new Dao();
            // Lấy danh sách loại phòng
            dsLoaiPhong = dao.LayDanhSachLoaiPhong();
            if (dsLoaiPhong.Count > 0)
            {
                cbRoomType.ItemsSource = dsLoaiPhong;
                cbRoomType.SelectedIndex = 0;
            }
            cbRoomType.SelectedIndex = phongHat.idLoaiPhong - 1;
          
            roomName.Text = phongHat.tenPhong;
        }

        private void cbRoomType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoaiPhong loaiPhong = cbRoomType.SelectedItem as LoaiPhong;
            roomPrice.Content = loaiPhong.gia.ToString("N0", nfi) + "/Giờ";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChapNhan(object sender, RoutedEventArgs e)
        {
            // Accept

            phongHat.tenPhong = roomName.Text.ToString().Trim();
            if (phongHat.tenPhong != "" && cbRoomType.SelectedItem != null)
            {
                phongHat.idLoaiPhong = (cbRoomType.SelectedItem as LoaiPhong).idLoaiPhong;
            }
            else
                return;
            Dao dao = new Dao();
            if (dao.CapNhatPhongHat(phongHat))
            {
                MessageBox.Show("Đã lưu thông tin chỉnh sửa !");

                this.Close();
            }
            else
            {
                MessageBox.Show("Lỗi, kiểm tra trùng tên !");

            }
        }
    }
}
