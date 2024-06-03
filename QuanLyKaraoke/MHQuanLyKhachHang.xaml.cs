using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for MHQuanLyKhachHang.xaml
    /// </summary>
    public partial class MHQuanLyKhachHang : Window
    {
        DataTable mytb = new DataTable();
        
        public MHQuanLyKhachHang()
        {
            InitializeComponent();
            mytb.Columns.Add("Mã KH");
            mytb.Columns.Add("Họ Tên");
            mytb.Columns.Add("Số điện thoại");
            mytb.Columns.Add("Địa chỉ");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void gridKHType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (gridKH.SelectedItem == null)
                    return;
                DataRow row = (gridKH.SelectedItem as DataRowView).Row;
                if (row == null)
                    return;
               tbid.Text = row.ItemArray[0].ToString();
                tbname.Text = row.ItemArray[1].ToString();
                tbsdt.Text = row.ItemArray[2].ToString();
                tbdc.Text = row.ItemArray[3].ToString();
               
            }
            catch
            {

            }
        }

        private void gridKH_Loaded(object sender, RoutedEventArgs e)
        {
            Dao dao = new Dao();
          
         

            List<KhachHang> listKH = dao.LayDanhSachKhachHang();
            mytb.Rows.Clear();
            foreach (KhachHang kh in listKH)
            {
                mytb.Rows.Add(kh.idKhachHang, kh.hoTen, kh.soDT, kh.diaChi);
            }

            gridKH.ItemsSource = mytb.DefaultView;
        }

        private void ThemKH(object sender, RoutedEventArgs e)
        {
            try
            {
                KhachHang kh =new KhachHang();
                kh.hoTen = tbname.Text;
                kh.soDT =  tbsdt.Text;
                kh.diaChi = tbdc.Text;

                Dao dao = new Dao();
                dao.ThemKhachHang(kh);
                MessageBox.Show("Thêm thành công", "MiniKaraOperator", MessageBoxButton.OK, MessageBoxImage.Information);
                gridKH_Loaded(null, null);
            }

            catch
            {
                MessageBox.Show("Có lỗi xảy ra, xem lại dữ liệu nhập", "MiniKaraOperator", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CapNhat(object sender, RoutedEventArgs e)
        {
            try
            {
                KhachHang kh = new KhachHang();
                kh.hoTen = tbname.Text;
                kh.soDT = tbsdt.Text;
                kh.diaChi = tbdc.Text;
                kh.idKhachHang = int.Parse(tbid.Text);
                Dao dao = new Dao();
                dao.CapNhatKhachHang(kh);
                MessageBox.Show("Cập nhật thành công", "MiniKaraOperator", MessageBoxButton.OK, MessageBoxImage.Information);
                gridKH_Loaded(null, null);
            }

            catch
            {
                MessageBox.Show("Có lỗi xảy ra, xem lại dữ liệu nhập", "MiniKaraOperator", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Xoa(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = int.Parse(tbid.Text);
                Dao dao = new Dao();
                dao.XoaKH(id);
                MessageBox.Show("Xóa thành công", "MiniKaraOperator", MessageBoxButton.OK, MessageBoxImage.Information);
                gridKH_Loaded(null, null);
            }

            catch
            {
                MessageBox.Show("Có lỗi xảy ra, Khách hàng này có liên quan đến dữ liệu hóa đơn đã lưu!!", "MiniKaraOperator", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void TraCuu(object sender, RoutedEventArgs e)
        {
            NhapSoLuong_Dialog inputdlg = new NhapSoLuong_Dialog("Nhập mã khách hàng hoặc SĐT:", "1");
            if (inputdlg.ShowDialog() == true)
            {
                Dao dao =new Dao();
                List<KhachHang> list =dao.LayDanhSachKhachHang(); 
                int idKH;
                bool isNumber = int.TryParse(inputdlg.Answer.ToString(), out idKH);
                foreach (KhachHang kh in list)
                {
                    if (kh.idKhachHang == idKH || kh.soDT == inputdlg.Answer.ToString())
                    {
                        tbid.Text = kh.idKhachHang.ToString();
                        tbname.Text = kh.hoTen;
                        tbsdt.Text = kh.soDT;
                        tbdc.Text = kh.diaChi;
                        return;
               
                    }
                }
                MessageBox.Show("Không tìm thấy khách hàng ", "MiniKaraOperator", MessageBoxButton.OK, MessageBoxImage.Information);
                
            }
        }
    }
}
