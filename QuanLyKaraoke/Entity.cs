using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKaraoke
{
    public class PhongHat
    {
        public int idPhong { get; set; }
        public string tenPhong { get; set; }
        public bool trangThai { get; set; }

        public int idLoaiPhong { get; set; }


        public PhongHat()
        {

        }
        public PhongHat(int _idPhong, string _tenPhong, bool _trangThai, int _LoaiPhong)
        {
            idPhong = _idPhong;
            tenPhong = _tenPhong;
            trangThai = _trangThai;
            idLoaiPhong = _LoaiPhong;
        }
        public override string ToString()
        {
            return tenPhong;
        }
    }
    public class LoaiPhong
    {
        public int idLoaiPhong { get; set; }
        public string ten { get; set; }
        public double gia { get; set; }

        public override string ToString()
        {

            return ten;
        }

    }
    public class SanPham
    {
        public int idSanPham { get; set; }
        public string tenSP { get; set; }
        public string moTa { get; set; }
        public double donGia { get; set; }

        public bool hienDung { get; set; }
        public override string ToString()
        {

            return tenSP;
        }
    }

    public class KhachHang
    {
        public int idKhachHang { get; set; }
        public string hoTen { get; set; }
        public string soDT { get; set; }
        public string diaChi { get; set; }

    }

}

