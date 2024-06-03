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
    /// Interaction logic for ChiTietSp.xaml
    /// </summary>
    public partial class ChiTietSp : Window
    {
        SanPham sp;
        NumberFormatInfo nfi = new CultureInfo("de", false).NumberFormat;
        public ChiTietSp(SanPham _sp)
        {
            InitializeComponent();
            sp = _sp;
            if (sp!=null)
            {
                this.Title = sp.tenSP;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (sp!=null)
            {
                spDes.Text = sp.moTa;
                spPrice.Content = sp.donGia.ToString("N0", nfi);
                spName.Content = sp.tenSP;

            }
        }
    }
}
