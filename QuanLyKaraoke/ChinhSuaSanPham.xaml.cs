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
    /// Interaction logic for ChinhSuaSanPham.xaml
    /// </summary>
    public partial class ChinhSuaSanPham : Window
    {
        SanPham sp;
        bool editFlag;
        public ChinhSuaSanPham(SanPham _sp)
        {
            InitializeComponent();
            sp = _sp;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (sp != null)
            {
                editFlag = true;
                spDes.Text = sp.moTa;
                spPrice.Text = sp.donGia.ToString();
                spName.Text = sp.tenSP;
            }
            else
            {
                sp = new SanPham();
                editFlag = false;
                this.Title = "Thêm sản phẩm";
                labelName.Content = "Thêm sản phẩm";
            }
        }

        private void LuuChinhSua(object sender, RoutedEventArgs e)
        {
            sp.moTa = spDes.Text;
            sp.donGia = double.Parse(spPrice.Text);
            sp.hienDung = true;
            sp.tenSP = spName.Text.Trim();
            if (sp.tenSP == "")
                return;
            else if (editFlag)
            {
                Dao dao=new Dao();
                if (dao.CapNhatSanPham(sp)) 
                {
                    MessageBox.Show("Cập nhật thành công !");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lỗi, Kiểm tra tên, hoặc các giá trị nhập vào!");
                }
            }
            else
            {
                Dao dao = new Dao();
                if (dao.ThemSanPham(sp))
                {
                    MessageBox.Show("Thêm thành công !");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lỗi, Kiểm tra tên, hoặc các giá trị nhập vào!");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
