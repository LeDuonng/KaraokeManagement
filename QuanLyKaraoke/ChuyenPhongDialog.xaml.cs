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
    /// Interaction logic for ChuyenPhongDialog.xaml
    /// </summary>
    public partial class ChuyenPhongDialog : Window
    {
        public ChuyenPhongDialog(string question, string defaultAnswer)
        {
            InitializeComponent();
            lblQuestion.Content = question;
            Dao dao =new Dao();
            List<PhongHat> listPH=dao.LayDanhSachPhongHat();
         
            foreach (PhongHat ph in listPH)
            {
                if (ph.trangThai != false)
                {
                    cbRoom.Items.Add(ph);
                    
                }
            }
            
            
            }

            private void btnDialogOk_Click(object sender, RoutedEventArgs e)
            {
                if (cbRoom.SelectedItem!=null)
                this.DialogResult = true;
            }

            private void Window_ContentRendered(object sender, EventArgs e)
            {
                
            }

            public PhongHat Answer
            {
                get { return cbRoom.SelectedItem as PhongHat; }
            }
        }
    }

