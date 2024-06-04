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
    /// Interaction logic for ManhHinhDatPhong.xaml
    /// </summary>
    public partial class ManhHinhDatPhong : Window
    {
        PhongHat phongHat;
        public ManhHinhDatPhong(PhongHat _phongHat)
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
            roomPrice.Content = loaiPhong.gia + "/Giờ";
        }

        private void ChapNhan_Click(object sender, RoutedEventArgs e)
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
            // Bắt đầu lưu thông tin đặt phòng và tính giờ
            QuanLyDatphong.Instance.ThemThongTinDatPhong(phongHat.idPhong, 5);
            //Khóa phòng và kiểm tra kết quả
            if (dao.KhoaPhong(phongHat.idPhong, false))
            {
                MessageBox.Show("Đặt phòng thành công !", "QuanLyKaraoke",MessageBoxButton.OK,MessageBoxImage.Information);

            }
            else
            {
                MessageBox.Show("Lỗi không lấy được phòng!", "QuanLyKaraoke",MessageBoxButton.OK,MessageBoxImage.Warning);
            }
            
        }

        private void Huy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void kiemtra_Click(object sender, RoutedEventArgs e)
        {
            Dao dao = new Dao();
            List<KhachHang> ds = new List<KhachHang>();
            List<KhachHang> ds_khachhang = new List<KhachHang>();
            ds = dao.LayDanhSachKhachHang();
            for (int i = 0; i < ds.Count; i++)
            {
                if (ds[i].hoTen.Contains(nameCustomer.Text) || phoneNumbers.Text == ds[i].soDT)
                {
                    ds_khachhang.Add(ds[i]);
                    
                }
            }
            if (ds_khachhang.Count > 0)
            {
                ds_KhachHang.DataContext = ds_khachhang;
            }
            else
                MessageBox.Show("Khách hàng mới!!!");
        }

        private void themmoi_Click(object sender, RoutedEventArgs e)
        {
            // Các thao tác kỹ thuật để lấy đối tượng phongHat từ nút trên màn hình
            //MenuItem menuItem = sender as MenuItem;
           // ContextMenu cm = menuItem.Parent as ContextMenu;
           // RadioButton roomButton = cm.PlacementTarget as RadioButton;
           // PhongHat phongHat = (roomButton.Tag as PhongHat);

            Window manHinhThemThongTinDatPhong = new ManHinhThemKhachHangDatPhong();
            //manHinhDatPhong.Closed += new EventHandler(ThemPhongHat_Closed);
            manHinhThemThongTinDatPhong.ShowDialog();
        }

    }
}
