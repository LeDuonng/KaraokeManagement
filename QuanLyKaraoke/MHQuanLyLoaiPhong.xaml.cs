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
    /// Interaction logic for MHQuanLyLoaiPhong.xaml
    /// </summary>
    public partial class MHQuanLyLoaiPhong : Window
    {
        LoaiPhong loaiPhong = new LoaiPhong();
        
        public MHQuanLyLoaiPhong()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Dao dao = new Dao();
            List<LoaiPhong> listRoomType = dao.LayDanhSachLoaiPhong();
            DataTable tb = new DataTable();
            tb.Columns.Add("ID");
            tb.Columns.Add("Tên Loại");
            tb.Columns.Add("Giá");
            foreach (LoaiPhong lp in listRoomType)
            {
                tb.Rows.Add(new object[] { lp.idLoaiPhong, lp.ten, lp.gia });
            }
            gridRoomType.ItemsSource = tb.DefaultView;
          
        }

       

        private void gridRoomType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (gridRoomType.SelectedItem == null)
                    return;
                DataRow row = (gridRoomType.SelectedItem as DataRowView).Row;
                if (row == null)
                    return;
                loaiPhong.idLoaiPhong = int.Parse(row.ItemArray[0].ToString());
                loaiPhong.ten = row.ItemArray[1].ToString();
                loaiPhong.gia = double.Parse(row.ItemArray[2].ToString());
                tbName.Text = loaiPhong.ten;
                tbPrice.Text = loaiPhong.gia.ToString();
            }
           catch
            {

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                string ten = tbName.Text;
                double gia = double.Parse(tbPrice.Text);

                Dao dao = new Dao();
                dao.ThemLoaiPhongHat(ten, gia);
                MessageBox.Show("Thêm thành công", "MiniKaraOperator", MessageBoxButton.OK, MessageBoxImage.Information);
                Grid_Loaded(null, null);
            }
            catch
            {
                MessageBox.Show("Lỗi không thêm được", "MiniKaraOperator", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dao dao = new Dao();
                int id = loaiPhong.idLoaiPhong;
                string ten = tbName.Text;
                double gia = double.Parse(tbPrice.Text);
                dao.CapNhatLoaiPhong(id, ten, gia);
                MessageBox.Show("Cập nhật thành công", "MiniKaraOperator", MessageBoxButton.OK, MessageBoxImage.Information);
                Grid_Loaded(null, null);
            }
            catch
            {
                MessageBox.Show("Lỗi không thêm được", "MiniKaraOperator", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }


    }
}
