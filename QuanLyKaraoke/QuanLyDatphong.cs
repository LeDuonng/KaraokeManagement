using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKaraoke
{
    // Lớp singleton quản lý đặt phòng
    public class QuanLyDatphong
    {

        private static QuanLyDatphong _quanlydatphong;
        List<ThongTinDatPhong> listThongTinDatPhong= new List<ThongTinDatPhong>();

        private QuanLyDatphong()
        {

        }
        //Singleton Instance
        public static QuanLyDatphong Instance
        {
            get
            {
                if (_quanlydatphong == null)
                {
                    _quanlydatphong = new QuanLyDatphong();

                }
                return _quanlydatphong;
            }
        }
        //Lấy thong tin đặt phòng của một phòng bất kỳ
        public ThongTinDatPhong LayThongTinDatPhong (int idPhongHat)
        {
            foreach (ThongTinDatPhong ttph in listThongTinDatPhong)
            {
                if (ttph.IDphongHat == idPhongHat)
                    return ttph;
            }
            return null;
        }
        // Thêm thông tin đặt phòng của một phòng hát vào danh sách.
        public bool ThemThongTinDatPhong(int idPhongHat, int idKhachHang)
        {
            Dao dao = new Dao();
            PhongHat ph = dao.LayPhongHatByID(idPhongHat);

            if (ph.trangThai == false)
                return false;

            ThongTinDatPhong ttph = new ThongTinDatPhong(ph.idPhong, idKhachHang);
            ttph.IDkhachHang = idKhachHang;
            ttph.thoiGianRa = ttph.thoiGianVao = DateTime.Now;
            DateTime.Now.ToString();
            ttph.IDdatPhong = DateTime.Now.ToString("hhMMyyyyddmmss");
            while ( !dao.LuuThongTinDatPhong(ttph) )
            {
               ttph.IDdatPhong = Guid.NewGuid().ToString();
            }

            listThongTinDatPhong.Add(ttph);
            return true;

            
        }
        // Gọi thêm sản phẩm vào phòng hát
        public bool GoiSanPham (int idPhongHat, int idSanPham, int soLuong)
        {
            ThongTinDatPhong ttph = LayThongTinDatPhong(idPhongHat);
            return ttph.GoiSanPham(idSanPham, soLuong);
        }

        public void ChuyenPhong (int idPhongCu, int idPhongMoi)
        {
            ThongTinDatPhong ttdp = LayThongTinDatPhong(idPhongCu);
            ttdp.IDphongHat = idPhongMoi;
            Dao dao=new Dao();
            dao.KhoaPhong(idPhongCu, true);
            dao.KhoaPhong(idPhongMoi, false);
        }
       
    }
    // Sản phẩm sử dụng trong phòng hát
    public class SanPhamSuDung
    {
        public int idSanpham;
        public int soLuong;

        public SanPhamSuDung(int idSanPham, int soLuong1)
        {
            // TODO: Complete member initialization
            this.idSanpham = idSanPham;
            this.soLuong = soLuong1;
        }
        public double ThanhTien ()
        {
            Dao dao = new Dao();
            SanPham sanPham = dao.LaySanPhamByID(idSanpham);
            if (sanPham != null)
             return (double)soLuong * sanPham.donGia;
            return 0;
        }
        public double DonGia()
        {
            Dao dao = new Dao();
            SanPham sanPham = dao.LaySanPhamByID(idSanpham);
            if (sanPham != null)
                return  sanPham.donGia;
            return 0;
        }
    }


    // Lớp thông tin đặt phòng
    // Gồm các thông tin: phòng hát, danh sách sản phẩm đã dùng
    // thời gian vào
    //thời gian ra, tiền phòng, phụ thu, giảm giá, tổng tiền
    public class ThongTinDatPhong
    {

        public string IDdatPhong;
        public int IDphongHat;
        public int IDkhachHang;
        public List<SanPhamSuDung> listSanPham = null;
        public DateTime thoiGianVao;
        public DateTime thoiGianRa;
        public double tienPhong;
        public double tienSanPham;
        public double phuThu;
        public double tongPhu;
        public double disCount;
        public double tongTien;
        public double giaPhong;
        public bool traPhong =false;

        public ThongTinDatPhong(int IDph,int idKhachHang)
        {
            // TODO: Complete member initialization
            this.IDphongHat = IDph;
            tienPhong = 0;
            phuThu = 0;
            disCount = 0;
            tongTien = 0;
            tienSanPham = 0;
            listSanPham = new List<SanPhamSuDung>();

            Dao dao = new Dao();

            PhongHat ph = dao.LayPhongHatByID(IDphongHat);
            LoaiPhong loaiPhong = dao.LayLoaiPhongByID(ph.idLoaiPhong);

            giaPhong = (double)loaiPhong.gia;
        }

        // Tính tổng tiền: tiền phòng + sản phẩm + phụ thu;
        public double TinhTien ()
        {
            TinhTienPhong();
            tongPhu = tienPhong + tienSanPham + phuThu;
            tongTien = tongPhu - tongPhu * disCount/100;
            return tongTien;

        }

        public double TinhTienPhong ()
        {
           
            TimeSpan duration = DateTime.Now - thoiGianVao;
            double min = duration.Minutes;
            double hours = (double)duration.Hours + min*1.0/60.0;

            tienPhong = giaPhong * hours;
            tienPhong=Math.Round(tienPhong);
            return tienPhong;
        }
        // Gọi thêm sản phẩm vào phòng hát
        public bool GoiSanPham (int idSanPham, int soLuong)
        {
            Dao dao = new Dao();
            SanPham sanPham = dao.LaySanPhamByID(idSanPham);
        
            foreach (SanPhamSuDung sp in listSanPham)
            {
                if (sp.idSanpham == idSanPham)
                {
                    sp.soLuong += soLuong;
                    tienSanPham += sanPham.donGia * soLuong;
                    return true;
                }
               
            }
            listSanPham.Add(new SanPhamSuDung(idSanPham, soLuong));
            tienSanPham += sanPham.donGia * soLuong;
            return true;
        }

        public bool XoaSanPham(int idSanPham)
        {
            Dao dao = new Dao();
            SanPham sanPham = dao.LaySanPhamByID(idSanPham);

            foreach (SanPhamSuDung sp in listSanPham)
            {
                if (sp.idSanpham == idSanPham)
                {
                    listSanPham.Remove(sp);
                    tienSanPham -= sanPham.donGia * sp.soLuong;
                    return true;
                }

            }
            
           
            return true;
        }

    }
}
