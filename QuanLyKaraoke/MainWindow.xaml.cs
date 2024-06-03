using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;

namespace QuanLyKaraoke
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string ROOMBUTTON_GROUPNAME = "roomItems";
        const int ROOMBUTTON_SIZE = 100;
        NumberFormatInfo nfi = new CultureInfo("de", false).NumberFormat;

        ThongTinDatPhong currentRoom = null;
        public MainWindow()
        {
            InitializeComponent();

        }

        // Hàm này tạo đối tượng Panel của đối tượng phòng hát để hiển thị lên màn hình
        public StackPanel GetRoomItem(PhongHat phongHat)
        {
            //Tạo Stackpanel và set Style
            StackPanel roomItem = new StackPanel();
            Style style = this.FindResource("RoomStackStyle") as Style;
            roomItem.Style = style;

            //Tạo label từ tên
            Label roomLabel = new Label();
            roomLabel.Content = phongHat.tenPhong;
            roomLabel.HorizontalAlignment = HorizontalAlignment.Center;
            roomLabel.FontWeight = FontWeights.Bold;

            // Clone roomButtonTemplate và gán cho radio button
            RadioButton roomButton = Clone(roomButtonTemplate) as RadioButton;
            roomButton.Name = "";
            roomButton.GroupName = ROOMBUTTON_GROUPNAME;
            roomButton.Click += new RoutedEventHandler(roomButtonTemplate_Checked);

            roomButton.Tag = phongHat;
            //roombutton context menu
            ContextMenu contextMenu = (phongHat.trangThai ? this.FindResource("roomMenuActive") : this.FindResource("roomMenuUnActive")) as ContextMenu;
            roomButton.ContextMenu = contextMenu;
            //Image 
            BitmapImage image;
            if (phongHat.trangThai == true)
            {
                image = new BitmapImage(new Uri("img/roomFree.png", UriKind.Relative));
            }
            else
            {
                image = new BitmapImage(new Uri("img/roomBusy.png", UriKind.Relative));
            }
            Image roomImage = new Image();
            roomImage.Source = image;

            //Set image roomButton
            roomButton.Content = roomImage;

            //Thêm roomLabel và roomButton vào panel roomItem
            roomItem.Children.Add(roomButton);
            roomItem.Children.Add(roomLabel);
            return roomItem;
        }

        //Hàm tạo panel sản phẩm để hiển thị
        public StackPanel GetSanPhamItem(SanPham sp, int i)
        {
            // Tạo button chứa toàn bộ thông tin sản phẩm
            Button button = new Button();
            StackPanel motherPanel = new StackPanel(); //stackpanel 
            StackPanel itemPanel = Clone(itemPanelTemplate) as StackPanel; // Clone tempate panel
            itemPanel.Name = "";

            // Chỉnh sửa các label
            Label stt = itemPanel.Children[0] as Label;
            Label tenSP = itemPanel.Children[1] as Label;
            Label donGia = itemPanel.Children[2] as Label;

            //...............................
            stt.Content = i.ToString();
            tenSP.Content = sp.tenSP;
            donGia.Content = sp.donGia.ToString("N0", nfi); ;

            // Gắn content menu
            button.Tag = sp;
            button.ContextMenu = this.FindResource("itemMenu") as ContextMenu;
            button.Background = new SolidColorBrush(Colors.Transparent);
            button.Content = itemPanel;
            motherPanel.Children.Add(button);

            return motherPanel;

        }

        public StackPanel GetItemInRoom(SanPham sp, int i, int soLuong)
        {
            Button button = new Button();
            StackPanel motherPanel = new StackPanel();
            StackPanel itemPanel = Clone(roomItemInRoom) as StackPanel;
            itemPanel.Name = "";

            Label stt = itemPanel.Children[0] as Label;
            Label tenSP = itemPanel.Children[1] as Label;
            Label donGia = itemPanel.Children[2] as Label;
            Label lbSoLuong = itemPanel.Children[3] as Label;
            Label lbThanhTien = itemPanel.Children[4] as Label;


            stt.Content = i.ToString();
            tenSP.Content = sp.tenSP;
            donGia.Content = sp.donGia.ToString("N0", nfi); 
            lbSoLuong.Content = soLuong.ToString();
            lbThanhTien.Content = (sp.donGia * soLuong).ToString("N0", nfi); 

            button.Tag = sp;
            button.Background = new SolidColorBrush(Colors.Transparent);
            button.ContextMenu = this.FindResource("itemỈnRoom") as ContextMenu;
            button.Content = itemPanel;
            motherPanel.Children.Add(button);
            itemPanel.Background = new SolidColorBrush(Colors.Transparent);
            return motherPanel;

        }

        private bool KiemTraKetNoi ()
        {
            Dao dao = new Dao();
            return dao.CheckConnect();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (KiemTraKetNoi()==false)
            {
                MessageBox.Show("Không thể tìm thấy file CSDL (KaraDB.mdb);  Chương trình sẽ thoát !!!", "Lỗi nghiêm trọng", MessageBoxButton.OK, MessageBoxImage.Warning);
               
            }
            KhoiDongDanhSachPhongHat();
            KhoiDongDanhSachSanPham();

            TaiTaoThonTinDatPhong();
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0,0,1), DispatcherPriority.Normal, delegate 
                {
                    this.lbclock.Content = DateTime.Now.ToString("hh:mm:ss tt");
                }, this.Dispatcher);
        
        }

        private void KhoiDongDanhSachPhongHat()
        {
            Dao dao = new Dao();
            roomShowList.Children.Clear();
            List<PhongHat> dsPhong = dao.LayDanhSachPhongHat();
            foreach (PhongHat phongHat in dsPhong)
            {
                roomShowList.Children.Add(GetRoomItem(phongHat));
            }
        }
        
        private void TaiTaoThonTinDatPhong()
        {
            Dao dao = new Dao();
            roomShowList.Children.Clear();
            List<PhongHat> dsPhong = dao.LayDanhSachPhongHat();
            foreach (PhongHat phongHat in dsPhong)
            {
                dao.KhoaPhong(phongHat.idPhong, true);
                phongHat.trangThai = true;
                roomShowList.Children.Add(GetRoomItem(phongHat));
            }
        }
        private void KhoiDongDanhSachSanPham()
        {
            listSanPham.Children.Clear();
            Dao dao = new Dao();
            List<SanPham> dsSanPham = dao.LayDanhSachSanPham();
            int i = 0;
            foreach (SanPham sp in dsSanPham)
            {
                if (sp.hienDung)
                {
                    listSanPham.Children.Add(GetSanPhamItem(sp, ++i));
                }
            }
        }

        // Hàm clone wpf control bằng xml
        private object Clone(object e)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(XamlWriter.Save(e));
            return (object)XamlReader.Load(new XmlNodeReader(document));
        }



        private void LayPhongHat(object sender, RoutedEventArgs e)
        {
            // Các thao tác kỹ thuật để lấy đối tượng phongHat từ nút trên màn hình
            MenuItem menuItem = sender as MenuItem;
            ContextMenu cm = menuItem.Parent as ContextMenu;
            RadioButton roomButton = cm.PlacementTarget as RadioButton;
            PhongHat phongHat = (roomButton.Tag as PhongHat);

            //Window manHinhDatPhong = new ManhHinhDatPhong(phongHat);
            //manHinhDatPhong.Closed += new EventHandler(ThemPhongHat_Closed);
            //manHinhDatPhong.Show();
            Dao dao = new Dao();

            // Bắt đầu lưu thông tin đặt phòng và tính giờ
            QuanLyDatphong.Instance.ThemThongTinDatPhong(phongHat.idPhong, 5);
            //Khóa phòng và kiểm tra kết quả
            if (dao.KhoaPhong(phongHat.idPhong, false))
            {
                MessageBox.Show("Đặt phòng thành công !", "QuanLyKaraoke", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
            {
                MessageBox.Show("Lỗi không lấy được phòng!", "QuanLyKaraoke", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            KhoiDongDanhSachPhongHat();

        }

        private void TraPhong(object sender, RoutedEventArgs e)
        {
            // Các thao tác kỹ thuật để lấy đối tượng phongHat từ nút trên màn hình
            MenuItem menuItem = sender as MenuItem;
            ContextMenu cm = menuItem.Parent as ContextMenu;
            RadioButton roomButton = cm.PlacementTarget as RadioButton;
            PhongHat phongHat = (roomButton.Tag as PhongHat);
            Dao dao = new Dao();

            // Mở khóa phòng và kiểm tra kết quá
            if (dao.KhoaPhong(phongHat.idPhong, true))
            {
                //
            }
            else
            {
                MessageBox.Show("Lỗi không trả được phòng!");
            }
            KhoiDongDanhSachPhongHat();
        }

        private void TaiLaiDanhSachPhong(object sender, RoutedEventArgs e)
        {
            KhoiDongDanhSachPhongHat();
        }

        private void ThemPhongHat(object sender, RoutedEventArgs e)
        {
            Window manHinhThemPhong = new ManHinhThemPhongHat();
            manHinhThemPhong.Closed += new EventHandler(ThemPhongHat_Closed);
            manHinhThemPhong.Show();

        }

        private void ThemPhongHat_Closed(object sender, EventArgs e)
        {
            KhoiDongDanhSachPhongHat();
        }

        private void DoiTenPhong(object sender, RoutedEventArgs e)
        {

        }


        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        private void ChinhSuaPhong(object sender, RoutedEventArgs e)
        {
            // Các thao tác kỹ thuật để lấy đối tượng phongHat từ nút trên màn hình
            MenuItem menuItem = sender as MenuItem;
            ContextMenu cm = menuItem.Parent as ContextMenu;
            RadioButton roomButton = cm.PlacementTarget as RadioButton;
            PhongHat phongHat = (roomButton.Tag as PhongHat);

            Window manHinhChinhSua = new ManHinhChinhSuaPhongHat(phongHat);
            manHinhChinhSua.Closed += new EventHandler(ThemPhongHat_Closed);
            manHinhChinhSua.Show();


        }


        private void itemPanelTemplate_MouseEnter(object sender, MouseEventArgs e)
        {
            StackPanel myPanel = sender as StackPanel;
            myPanel.Background = new SolidColorBrush(Colors.Aqua);
        }

        // Thêm sản phẩm vào một phòng bất kỳ
        private void ThemVaoPhong(object sender, RoutedEventArgs e)
        {
            // Nếu chưa nếu chưa có phòng được chọn thì return;
            if (currentRoom == null)
            {
                MessageBox.Show("Hãy chọn phòng trước khi thêm sản phẩm ", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;

            }
            // Các thao tác kỹ thuật để lấy đối tượng SanPham từ nút trên màn hình
            MenuItem menuItem = sender as MenuItem;
            ContextMenu cm = menuItem.Parent as ContextMenu;
            Button spButton = cm.PlacementTarget as Button;
            SanPham sp = spButton.Tag as SanPham;
            NhapSoLuong_Dialog inputdlg = new NhapSoLuong_Dialog("Nhập số lượng sản phẩm:", "1");
            if (inputdlg.ShowDialog() == true)
            {
                int soluong;
                bool isNumber = int.TryParse(inputdlg.Answer.ToString(), out soluong);
                if (isNumber)
                    currentRoom.GoiSanPham(sp.idSanPham, soluong);
            }

            LoadListItemInRoom(currentRoom);
            CapNhatHienThiPhong();
        }

        private void ChiTietSanPham(object sender, RoutedEventArgs e)
        {
            // Các thao tác kỹ thuật để lấy đối tượng SanPham từ nút trên màn hình
            MenuItem menuItem = sender as MenuItem;
            ContextMenu cm = menuItem.Parent as ContextMenu;
            Button spButton = cm.PlacementTarget as Button;
            SanPham sp = spButton.Tag as SanPham;

            Window chiTietSP = new ChiTietSp(sp);
            chiTietSP.Show();
        }

        private void ChinhSuaSanPham(object sender, RoutedEventArgs e)
        {
            // Các thao tác kỹ thuật để lấy đối tượng SanPham từ nút trên màn hình
            MenuItem menuItem = sender as MenuItem;
            ContextMenu cm = menuItem.Parent as ContextMenu;
            Button spButton = cm.PlacementTarget as Button;
            SanPham sp = spButton.Tag as SanPham;

            Window chinhSuaSp = new ChinhSuaSanPham(sp);
            chinhSuaSp.Closed += new EventHandler(chinhSuaSp_done);

            chinhSuaSp.Show();
        }

        private void chinhSuaSp_done(object sender, EventArgs e)
        {
            KhoiDongDanhSachSanPham();
        }

        private void XoaSanPham(object sender, RoutedEventArgs e)
        {
            // Xac dinh phong hat duoc chon
            MenuItem menuItem = sender as MenuItem;
            ContextMenu cm = menuItem.Parent as ContextMenu;
            Button spButton = cm.PlacementTarget as Button;
            SanPham sanpham = spButton.Tag as SanPham;

            Dao dao = new Dao();
            if (XacNhanXoa())
            {
                if (dao.XoaSanPham(sanpham) == false)
                {
                    MessageBox.Show("Lỗi !!! Không xóa được !");
                }
                else
                {
                    MessageBox.Show(" Xóa thành công !!!");
                }
            }
            else
            {

            }
            KhoiDongDanhSachSanPham();
        }

        private void ThemSanPham(object sender, RoutedEventArgs e)
        {
            Window chinhSuaSp = new ChinhSuaSanPham(null);
            chinhSuaSp.Closed += new EventHandler(chinhSuaSp_done);

            chinhSuaSp.Show();
        }

        //Sử lý sự kiện phòng được nhấp chọn
        private void roomButtonTemplate_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton btnPhong = sender as RadioButton;
            if (btnPhong != null)
            {

                PhongHat ph = btnPhong.Tag as PhongHat;
                selectedRoomName.Content = ph.tenPhong;
                currentRoom = QuanLyDatphong.Instance.LayThongTinDatPhong(ph.idPhong);
                LoadListItemInRoom(currentRoom);
                CapNhatHienThiPhong();

            }


        }

        private void CapNhatHienThiPhong()
        {
            if (currentRoom != null)
            {
                currentRoom.TinhTien();
                timeIn.Content = currentRoom.thoiGianVao.ToString();
                lbTienPhong.Content = currentRoom.tienPhong.ToString("N0", nfi);
                lbTienSanPham.Content = currentRoom.tienSanPham.ToString("N0", nfi);
                lbPhuThu.Content = currentRoom.phuThu.ToString("N0", nfi); 
                lbTongPhu.Content = (currentRoom.tienSanPham + currentRoom.tienPhong + currentRoom.phuThu).ToString("N0", nfi);
                lbKhuyenMai.Content = (currentRoom.disCount/100 * currentRoom.tongPhu).ToString("N0", nfi);
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
        // Load danh sách sản phẩm của phòng hát

        private void LoadListItemInRoom(ThongTinDatPhong ttph)
        {

            if (ttph == null)
            {
                ItemInRoomPanel.Children.Clear();
                return;
            }

            int i = 0;
            Dao dao = new Dao();
            ItemInRoomPanel.Children.Clear();
            foreach (SanPhamSuDung spsd in ttph.listSanPham)
            {   
                SanPham sp = dao.LaySanPhamByID(spsd.idSanpham);
                ItemInRoomPanel.Children.Add(GetItemInRoom(sp, i + 1, spsd.soLuong));
                i++;
            }
        }

        private void XoaPhongHat(object sender, RoutedEventArgs e)
        {
            // Xac dinh phong hat duoc chon
            MenuItem menuItem = sender as MenuItem;
            ContextMenu cm = menuItem.Parent as ContextMenu;
            RadioButton roomButton = cm.PlacementTarget as RadioButton;
            PhongHat phat = roomButton.Tag as PhongHat;

            Dao dao = new Dao();
            if (XacNhanXoa())
            {
                if (dao.XoaPhongHat(phat) == false)
                {
                    MessageBox.Show("Lỗi !!! Không xóa được !");
                }
                else
                {
                    MessageBox.Show("Xóa thành công !!!");
                }
            }
            else
            {

            }
            KhoiDongDanhSachPhongHat();
        }

        public bool XacNhanXoa()
        {
            MessageBoxResult result = MessageBox.Show("Bạn có thật sự muốn xóa không ?", "Thông Báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                return false;
            }
            return true;
        }




        private void txtKhuyenMai_DataContextChanged(object sender, RoutedEventArgs e)
        {
            if (currentRoom == null)
                return;
            try
            {
                double discount = double.Parse(txtKhuyenMai.Text);
                currentRoom.disCount = discount;

                CapNhatHienThiPhong();
            }
            catch
            {

            }
        }

        private void TinhTien(object sender, RoutedEventArgs e)
        {
            // Xac dinh phong hat duoc chon
            MenuItem menuItem = sender as MenuItem;
            ContextMenu cm = menuItem.Parent as ContextMenu;
            RadioButton roomButton = cm.PlacementTarget as RadioButton;
            PhongHat phat = roomButton.Tag as PhongHat;

            ThongTinDatPhong ttph = QuanLyDatphong.Instance.LayThongTinDatPhong(phat.idPhong);
            if (ttph == null)
                return;
            ManHinhTinhTien mhtt = new ManHinhTinhTien(ttph);
            mhtt.Show();
            ttph.listSanPham.Clear();
            timeIn.Content = "...";
            lbTienPhong.Content = "...";
            lbTienSanPham.Content = "...";
            txtKhuyenMai.Text = "0";
            lbPhuThu.Content = "...";
            lbTongPhu.Content = "...";
            lbKhuyenMai.Content = "...";
            lbTong.Content = "...";
            LoadListItemInRoom(ttph);
        }

        private void XemThongKe(object sender, RoutedEventArgs e)
        {
            MHThongKe thongKe = new MHThongKe();
            thongKe.Show();
        }

        private void QuanLoaiPhongClick(object sender, RoutedEventArgs e)
        {
            MHQuanLyLoaiPhong qllp = new MHQuanLyLoaiPhong();
            qllp.Show();
            qllp.Closed += new EventHandler(ThemPhongHat_Closed);
        }

        private void PhuThu(object sender, RoutedEventArgs e)
        {
            if (currentRoom == null)
            {
                MessageBox.Show("Hãy chọn phòng đang hoạt động ", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            NhapSoLuong_Dialog inputdlg = new NhapSoLuong_Dialog("Nhập số phụ thu:", currentRoom.phuThu.ToString());
            if (inputdlg.ShowDialog() == true)
            {
                double soluong;
                bool isNumber = double.TryParse(inputdlg.Answer.ToString(), out soluong);
                if (isNumber)
                {
                    if (currentRoom != null)
                    {
                        currentRoom.phuThu = soluong;
                        CapNhatHienThiPhong();
                    }
                }
                else
                {
                    MessageBox.Show("Số tiền không hợp lệ ", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private void XoaSanPhamKhoiPhong(object sender, RoutedEventArgs e)
        {
            if (currentRoom!=null)
            {
                // Các thao tác kỹ thuật để lấy đối tượng SanPham từ nút trên màn hình
                MenuItem menuItem = sender as MenuItem;
                ContextMenu cm = menuItem.Parent as ContextMenu;
                Button spButton = cm.PlacementTarget as Button;
                SanPham sp = spButton.Tag as SanPham;

                currentRoom.XoaSanPham(sp.idSanPham);
                MessageBox.Show("Đã xóa !", "QuanLyKaraoke", MessageBoxButton.OK, MessageBoxImage.Information);
                CapNhatHienThiPhong();
            }
                
        }
         private void ChuyenPhong(object sender, RoutedEventArgs e)
        {
            // Các thao tác kỹ thuật để lấy đối tượng phongHat từ nút trên màn hình
            MenuItem menuItem = sender as MenuItem;
            ContextMenu cm = menuItem.Parent as ContextMenu;
            RadioButton roomButton = cm.PlacementTarget as RadioButton;
            PhongHat phongHat = (roomButton.Tag as PhongHat);

            ChuyenPhongDialog inputdlg = new ChuyenPhongDialog("Chọn phòng để chuyển:", "1");
            if (inputdlg.ShowDialog() == true)
            {
               
                PhongHat ph = inputdlg.Answer;
                QuanLyDatphong.Instance.ChuyenPhong(phongHat.idPhong,ph.idPhong);
                KhoiDongDanhSachPhongHat();
                MessageBox.Show("Đã chuyển phòng " + phongHat.tenPhong + " sang phòng " + ph.tenPhong  +" !", "QuanLyKaraoke", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
     
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MHQuanLyKhachHang qlkh = new MHQuanLyKhachHang();
            qlkh.Show();
        }
    }
}
