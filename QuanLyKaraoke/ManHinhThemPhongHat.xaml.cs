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
    /// Interaction logic for ManHinhThemPhongHat.xaml
    /// </summary>
    public partial class ManHinhThemPhongHat : Window
    {
        public ManHinhThemPhongHat()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List <LoaiPhong> dsLoaiPhong;
            Dao dao=new Dao();
            // Lấy danh sách loại phòng
            dsLoaiPhong=dao.LayDanhSachLoaiPhong();
            if (dsLoaiPhong.Count >0)
            {
                cbRoomType.ItemsSource = dsLoaiPhong;
                cbRoomType.SelectedItem = 0;
            }
           
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Khi thay đổi loại phòng
        private void cbRoomType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoaiPhong loaiPhong = cbRoomType.SelectedItem as LoaiPhong;
            roomPrice.Content = loaiPhong.gia + "/Giờ";
        }

        // Button Chấp nhận
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
            PhongHat phongHat = new PhongHat();
            phongHat.tenPhong = roomName.Text.ToString().Trim();
            if (phongHat.tenPhong != "" && cbRoomType.SelectedItem != null)
            {
                phongHat.idLoaiPhong = (cbRoomType.SelectedItem as LoaiPhong).idLoaiPhong;
            }
            else
                return;
            Dao dao = new Dao();
           if ( dao.ThemPhongHat(phongHat) )
           {
               MessageBox.Show("Đã thêm phòng thành công !");
               
               this.Close();
           }
            else
           {
               MessageBox.Show("Lỗi, kiểm tra trùng tên !");

           }

            


        }
    }
}
