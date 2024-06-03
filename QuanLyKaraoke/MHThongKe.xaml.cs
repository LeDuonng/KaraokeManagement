using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for MHThongKe.xaml
    /// </summary>
    public partial class MHThongKe : Window
    {
        DataTable tbHoaDon = new DataTable();
        DataTable tbSanPham= new DataTable();
        DataTable tbRom = new DataTable();
         NumberFormatInfo nfi = new CultureInfo("de", false).NumberFormat;
         public MHThongKe()
         {
             InitializeComponent();
             tbHoaDon.Columns.Add("Mã hóa đơn");
             tbHoaDon.Columns.Add("Mã đặt phòng");
             tbHoaDon.Columns.Add("Mã khách hàng");
             tbHoaDon.Columns.Add("Ngày giờ tạo");
             tbHoaDon.Columns.Add("Tổng tiền");
             ketthuc.SelectedDate = batdau.SelectedDate = DateTime.Now;
             gridHoaDon.ItemsSource = tbHoaDon.DefaultView;

             tbSanPham.Columns.Add("Mã sản phẩm");
             tbSanPham.Columns.Add("Tên Sản phẩm");
             tbSanPham.Columns.Add("Số lượng đã bán");
             tbSanPham.Columns.Add("Tiền thu được");
             gridSanPham.ItemsSource = tbSanPham.DefaultView;
            
            
         }

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Dao dao = new Dao();
            DateTime from=DateTime.Now;
            DateTime to = DateTime.Now;
            try
            {
                 from = DateTime.Parse(batdau.SelectedDate.ToString());
                 to = DateTime.Parse(ketthuc.SelectedDate.ToString());
                if (from >to )
                {
                    MessageBox.Show("Khoảng thời gian không hợp lệ !", "QuanLyKaraoke", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }

            }
            catch
            {
                MessageBox.Show("Hãy chọn khoảng thời gian", "QuanLyKaraoke", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            tbHoaDon.Rows.Clear();
            tbSanPham.Rows.Clear();
            
            dao.LayThongKeHoaDon(from, to, ref tbHoaDon);
            lbSoHoaDon.Content = tbHoaDon.Rows.Count;
            string listHD = "";
            double total=0;
            int i = 0;
            foreach (DataRow row in tbHoaDon.Rows)
            {
                if (i==0)
                {
                    listHD ="'" + row.ItemArray[0].ToString() + "'";
                }
                else
                {
                    listHD = listHD + ", " + "'" + row.ItemArray[0].ToString() + "'";
                }
                
                double value=double.Parse(row.ItemArray[4].ToString());
                total = total +value;
                i++;
               
            }
            txtTotal.Text = total.ToString("N0",nfi);
            double total2 = 0;
            dao.LayThongKeSanPham(listHD, ref tbSanPham);
            foreach (DataRow row in tbSanPham.Rows)
            {

                double value = double.Parse(row.ItemArray[7].ToString());
                total2 = total2 + value;
                i++;

            }
            txtToTalSP.Text = total2.ToString("N0", nfi);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Dao dao = new Dao();
            List<PhongHat> listPH = dao.LayDanhSachPhongHat();
            tbRom.Columns.Add("Mã phòng");
            tbRom.Columns.Add("Tên phòng");
            tbRom.Columns.Add("Trạng thái");
           for (int i=0;i<listPH.Count;i++)
           {
            
               string[] data = { listPH[i].idPhong.ToString(), listPH[i].tenPhong.ToString(), listPH[i].trangThai.ToString() };
           
               tbRom.Rows.Add(data);
           }
           gridListRom.ItemsSource = tbRom.DefaultView;
        }

        private void gridListRom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}
