using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace QuanLyKaraoke
{
    public class Dao
    {
        string strConection = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"KaraDB.mdb\"";

        public List<PhongHat> LayDanhSachPhongHat()
        {
            List<PhongHat> dsPhong = new List<PhongHat>();

            //Tạo kết nối
            OleDbConnection connection = KetNoi();

            //Query
            string sql = "SELECT * FROM PHONGHAT";
            OleDbCommand command = new OleDbCommand(sql, connection);
            OleDbDataReader reader = command.ExecuteReader();


            while (reader.Read())
            {
                if ((bool)reader["HienDung"])
                {
                    PhongHat phongHat = new PhongHat();
                    phongHat.idPhong = (int)reader["IDPhongHat"];
                    phongHat.tenPhong = reader["TenPhong"].ToString();
                    phongHat.idLoaiPhong = (int)reader["IDLoaiPhong"];
                    phongHat.trangThai = (bool)reader["TrangThai"];
                    dsPhong.Add(phongHat);
                }
            }

            reader.Close();
            connection.Close();
            return dsPhong;
        }

        public bool KhoaPhong(int idPhong, bool lockValue)
        {
            OleDbConnection conn;
            string sql = "UPDATE  PHONGHAT SET TrangThai=@true WHERE IDPhongHat=@idPhong";


            try
            {
                conn = KetNoi();
                OleDbCommand command = new OleDbCommand(sql, conn);
                command.Parameters.AddWithValue("@true", lockValue);
                command.Parameters.AddWithValue("@idPhong", idPhong);
                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public List<LoaiPhong> LayDanhSachLoaiPhong()
        {
            List<LoaiPhong> dsPhong = new List<LoaiPhong>();

            //Tạo kết nối
            OleDbConnection connection = KetNoi();

            //Query
            string sql = "SELECT * FROM LOAIPHONGHAT";
            OleDbCommand command = new OleDbCommand(sql, connection);
            OleDbDataReader reader = command.ExecuteReader();


            while (reader.Read())
            {
                LoaiPhong loaiPhong = new LoaiPhong();
                loaiPhong.idLoaiPhong = (int)reader["IDLoaiPhong"];
                loaiPhong.ten = (string)reader["TenLoaiPhong"];
                loaiPhong.gia = double.Parse(reader["Gia"].ToString());
                dsPhong.Add(loaiPhong);
            }

            reader.Close();
            connection.Close();
            return dsPhong;
        }

        public bool ThemPhongHat(PhongHat phongHat)
        {
            string sql = "INSERT INTO PHONGHAT(TenPhong,IDLoaiPhong,TrangThai,HienDung) VALUES(@tenPhong,@loaiPhong,@trangThai,true)";
            OleDbConnection conn = KetNoi();

            OleDbCommand command = new OleDbCommand(sql, conn);
            command.Parameters.AddWithValue("@tenPhong", phongHat.tenPhong);
            command.Parameters.AddWithValue("@loaiPhong", phongHat.idLoaiPhong);
            command.Parameters.AddWithValue("@trangThai", true);

            try
            {
                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (OleDbException)
            {
                conn.Close();
                return false;
            }
        }


        public bool XoaPhongHat(PhongHat phongHat)
        {
            string sql = "UPDATE PHONGHAT "
                        + " SET HienDung=@HienDung "
                        + " WHERE IDPhongHat=" + phongHat.idPhong.ToString();
            OleDbConnection conn = KetNoi();

            OleDbCommand command = new OleDbCommand(sql, conn);

            command.Parameters.AddWithValue("@HienDung", false);



            try
            {
                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch 
            {
                conn.Close();
                return false;
            }
        }
        private OleDbConnection KetNoi()
        {
            //Tạo kết nối
            OleDbConnection connection = new OleDbConnection(strConection);
            try
            {
                connection.Open();
                return connection;
            }
            catch (OleDbException)
            {
                
                return null;

            }
        }

        public bool CheckConnect()
        {
            OleDbConnection conn = KetNoi();
            if (conn == null)
                return false;
            else
            {
                conn.Close();
                return true;
            }
               
        }
        public bool CapNhatPhongHat(PhongHat phongHat)
        {
            string sql = "UPDATE PHONGHAT "
                        + " SET TenPhong=@tenPhong, IDLoaiPhong=@loaiPhong "
                        + " WHERE IDPhongHat=" + phongHat.idPhong.ToString();
            OleDbConnection conn = KetNoi();

            OleDbCommand command = new OleDbCommand(sql, conn);

            command.Parameters.AddWithValue("@tenPhong", phongHat.tenPhong);
            command.Parameters.AddWithValue("@loaiPhong", phongHat.idLoaiPhong);


            try
            {
                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (OleDbException)
            {
                conn.Close();
                return false;
            }
        }

        public List<SanPham> LayDanhSachSanPham()
        {
            List<SanPham> dsSanPham = new List<SanPham>();

            //Tạo kết nối
            OleDbConnection connection = KetNoi();

            //Query
            string sql = "SELECT * FROM SANPHAM";
            OleDbCommand command = new OleDbCommand(sql, connection);
            OleDbDataReader reader = command.ExecuteReader();


            while (reader.Read())
            {
                SanPham SPham = new SanPham();
                SPham.idSanPham = (int)reader["IDSanPham"];
                SPham.tenSP = reader["TenSanPham"].ToString();
                SPham.moTa = reader["MoTa"].ToString();
                SPham.donGia = double.Parse(reader["DonGia"].ToString());
                SPham.hienDung = (bool)reader["HienDung"];
                dsSanPham.Add(SPham);
            }

            reader.Close();
            connection.Close();
            return dsSanPham;

        }

        public bool XoaSanPham(SanPham sanpham)
        {
            string sql = "UPDATE SANPHAM "
                        + " SET HienDung = @HienDung "
                        + " WHERE IDSanPham=" + sanpham.idSanPham.ToString();
            OleDbConnection conn = KetNoi();

            OleDbCommand command = new OleDbCommand(sql, conn);

            command.Parameters.AddWithValue("@HienDung", false);


            try
            {
                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch 
            {
                conn.Close();
                return false;
            }
        }
        //--------------
        public bool ThemSanPham(SanPham sanpham)
        {
            string sql = "INSERT INTO SANPHAM(TenSanPham,Mota,DonGia,HienDung) VALUES(@TenSanPham,@MoTa,@DonGia,@HienDung)";
            OleDbConnection conn = KetNoi();

            OleDbCommand command = new OleDbCommand(sql, conn);

            command.Parameters.AddWithValue("@TenSanPham", sanpham.tenSP);
            command.Parameters.AddWithValue("@MoTa", sanpham.moTa);
            command.Parameters.AddWithValue("@DonGia", sanpham.donGia);
            command.Parameters.AddWithValue("@HienDung", true);
            try
            {
                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (OleDbException)
            {
                conn.Close();
                return false;
            }
        }
        public bool CapNhatSanPham(SanPham sanpham)
        {
            string sql = "UPDATE SANPHAM "
                        + " SET TenSanPham=@TenSanPham, MoTa=@MoTa, DonGia=@DonGia"
                        + " WHERE IDSanPham=" + sanpham.idSanPham.ToString();
            OleDbConnection conn = KetNoi();

            OleDbCommand command = new OleDbCommand(sql, conn);


            command.Parameters.AddWithValue("@TenSanPham", sanpham.tenSP);
            command.Parameters.AddWithValue("@MoTa", sanpham.moTa);
            command.Parameters.AddWithValue("@DonGia", sanpham.donGia);


            try
            {
                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (OleDbException)
            {
                conn.Close();
                return false;
            }
        }

        public void CapNhatKhachHang(KhachHang kh)
        {

            string sql = "UPDATE KHACHHANG "
                        + " SET HoTen=@ten, Sdt=@sdt, DiaChi=@dc"
                        + " WHERE IDKhachHang=" + kh.idKhachHang.ToString();
            OleDbConnection conn = KetNoi();

            OleDbCommand command = new OleDbCommand(sql, conn);
            command.Parameters.AddWithValue("@ten", kh.hoTen);
            command.Parameters.AddWithValue("@sdt", kh.soDT);
            command.Parameters.AddWithValue("@dc", kh.diaChi);
           command.ExecuteNonQuery();
            conn.Close();
           
        }


        // Lấy đối tượng phòng hát theo id
        // Lấy đối tượng phòng hát theo id

        internal PhongHat LayPhongHatByID(int IDphongHat)
        {
            PhongHat phat = new PhongHat();
            string sql = "SELECT * FROM PHONGHAT WHERE IDPhongHat = " + IDphongHat.ToString();
            OleDbConnection conn = KetNoi();
            OleDbCommand command = new OleDbCommand(sql, conn);

            OleDbDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                phat.idPhong = (int)read["IDPhongHat"];
                phat.tenPhong = read["TenPhong"].ToString();
                phat.idLoaiPhong = (int)read["IDLoaiPhong"];
                phat.trangThai = (bool)read["TrangThai"];
            }

            return phat;
        }

        internal LoaiPhong LayLoaiPhongByID(int idloaiphong)
        {
            LoaiPhong lphong = new LoaiPhong();
            string sql = "SELECT * FROM LOAIPHONGHAT WHERE IDLoaiPhong = " + idloaiphong.ToString();
            OleDbConnection conn = KetNoi();
            OleDbCommand command = new OleDbCommand(sql, conn);

            OleDbDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                lphong.idLoaiPhong = (int)read["IDLoaiPhong"];
                lphong.ten = read["TenLoaiPhong"].ToString();
                lphong.gia = double.Parse(read["Gia"].ToString());
            }


            conn.Close();

            return lphong;
        }

        internal SanPham LaySanPhamByID(int idSanpham)
        {
            SanPham sanpham = new SanPham();
            string sql = "SELECT * FROM SANPHAM WHERE IDSanPham = " + idSanpham.ToString();
            OleDbConnection conn = KetNoi();
            OleDbCommand command = new OleDbCommand(sql, conn);


            OleDbDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                sanpham.idSanPham = (int)read["IDSanPham"];
                sanpham.tenSP = read["TenSanPham"].ToString();
                sanpham.moTa = read["MoTa"].ToString();
                sanpham.donGia = double.Parse(read["DonGia"].ToString());
            }
            conn.Close();


            return sanpham;
        }


        public bool LuuThongTinDatPhong(ThongTinDatPhong ttph)
        {
              

           
            if (ttph.traPhong == false)
            {

                string sql = "INSERT INTO DATPHONG(IDDatPhong, IDKhachhang, IDPhong, ThoiGianVao) VALUES(@idDatPhong, @idKhachHang, @idPhong, @thoigianvao)";
                OleDbConnection conn = KetNoi();
                OleDbCommand command = new OleDbCommand(sql, conn);
                command.Parameters.AddWithValue("@idDatPhong", ttph.IDdatPhong);
                command.Parameters.AddWithValue("@idKhachHang", ttph.IDkhachHang);
                command.Parameters.AddWithValue("@idPhong", ttph.IDphongHat);
                command.Parameters.AddWithValue("@thoigianvao", ttph.thoiGianVao.ToString());

                try
                {
                    command.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
                catch (OleDbException)
                {
                    conn.Close();
                    return false;
                }
            }
            else
            {
                string sql = "UPDATE DATPHONG "
                        + " SET ThoiGianRa =@thoiGianra, TienPhong =@tienPhong"
                        + " WHERE IDDatPhong ='" + ttph.IDdatPhong +"'";
                OleDbConnection conn = KetNoi();
                OleDbCommand command = new OleDbCommand(sql, conn);
                command.Parameters.AddWithValue("@thoiGianra", ttph.thoiGianRa.ToString());
                command.Parameters.AddWithValue("@tienPhong", ttph.tienPhong.ToString());

                try
                {
                    command.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
                catch (OleDbException)
                {
                    conn.Close();
                    return false;
                }
            }
 
        }


        public List<KhachHang> LayDanhSachKhachHang()
        {
            List<KhachHang> dsKhachHang = new List<KhachHang>();

            //Tạo kết nối
            OleDbConnection connection = KetNoi();

            //Query
            string sql = "SELECT * FROM KHACHHANG";
            OleDbCommand command = new OleDbCommand(sql, connection);
            OleDbDataReader reader = command.ExecuteReader();


            while (reader.Read())
            {
                KhachHang KHang = new KhachHang();
                KHang.idKhachHang = (int)reader["IDKhachHang"];
                KHang.hoTen = reader["HoTen"].ToString();
                KHang.soDT = reader["Sdt"].ToString();
                KHang.diaChi = reader["DiaChi"].ToString();               
                dsKhachHang.Add(KHang);
            }

            reader.Close();
            connection.Close();
            return dsKhachHang;

        }


        public bool ThemKhachHang(KhachHang khachhang)
        {
            string sql = "INSERT INTO KHACHHANG(HoTen,Sdt,DiaChi) VALUES(@hoTen,@sdt,@diaChi)";
            OleDbConnection conn = KetNoi();
            OleDbCommand command = new OleDbCommand(sql, conn);
            command.Parameters.AddWithValue("@hoTen", khachhang.hoTen);
            command.Parameters.AddWithValue("@Sdt", khachhang.soDT);
            command.Parameters.AddWithValue("@diaChi", khachhang.diaChi);
            try
            {
                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (OleDbException)
            {
                conn.Close();
                return false;
            }
        }

        public bool GhiNhanHoaDon(string maHoaDon, ThongTinDatPhong currentRoom, string ghiChu, bool trangThai = true)
        {
            currentRoom.traPhong = true;
            if(LuuThongTinDatPhong(currentRoom))
            {
                string sql = "INSERT INTO HOADON(IDHoaDon,IDKhachHang,IDDatPhong,NgayGioTao,GhiChu,SubTotal,Discount,Total,TrangThai,PhuThu) " +
                            "VALUES(@idHoaDon,@idKhachHang,@idDatPhong,@ngayGioTao,@ghiChu,@SubTotal,@Discount,@Total,@TrangThai,@PhuThu)";
               
                OleDbConnection conn = KetNoi();
                OleDbCommand command = new OleDbCommand(sql, conn);
                command.Parameters.AddWithValue("@idHoaDon", maHoaDon);
                command.Parameters.AddWithValue("@idKhachHang", currentRoom.IDkhachHang);
                command.Parameters.AddWithValue("@idDatPhong", currentRoom.IDdatPhong);
                command.Parameters.AddWithValue("@ngayGioTao", DateTime.Now.ToString());
                command.Parameters.AddWithValue("@ghiChu", ghiChu);
                command.Parameters.AddWithValue("@SubTotal", currentRoom.tongPhu);
                command.Parameters.AddWithValue("@Discount", currentRoom.disCount);
                command.Parameters.AddWithValue("@Total", currentRoom.tongTien);
                command.Parameters.AddWithValue("@TrangThai", trangThai);
                command.Parameters.AddWithValue("@PhuThu", currentRoom.phuThu);
                try
                {
                    command.ExecuteNonQuery();
                 
                    foreach (SanPhamSuDung spsd in currentRoom.listSanPham)
                    {
                        string sql2 = "INSERT INTO HOADONSP(IDHoaDon,IDSanPham,SoLuong,ThanhTien) VALUES(@IDHoaDon,@IDSanPham,@SoLuong,@ThanhTien)";

                        OleDbCommand command2 = new OleDbCommand(sql2, conn);

                        command2.Parameters.AddWithValue("@IDHoaDon", maHoaDon);
                        command2.Parameters.AddWithValue("@IDSanPham", spsd.idSanpham);
                        command2.Parameters.AddWithValue("@SoLuong", spsd.soLuong);
                        command2.Parameters.AddWithValue("@ThanhTien", spsd.ThanhTien());

                        command2.ExecuteNonQuery();
                    }

                    conn.Close();
                    return true;
                }
                catch (OleDbException)
                {
                    conn.Close();
                    return false;
                }


            }
            else
            {
                return false;
            }
           
        }

        public bool LuuHoaDonSanPham(string idHoaDon, string idSanPham,  int soLuong, double thanhTien)
        {
            string sql = "INSERT INTO HOADONSP(IDHoaDon,IDSanPham,SoLuong,ThanhTien) VALUES(@IDHoaDon,@IDSanPham,@SoLuong,@ThanhTien)";
            OleDbConnection conn = KetNoi();
            OleDbCommand command = new OleDbCommand(sql, conn);

            command.Parameters.AddWithValue("@IDHoaDon", idHoaDon);
            command.Parameters.AddWithValue("@IDSanPham", idSanPham);
            command.Parameters.AddWithValue("@SoLuong", soLuong);
            command.Parameters.AddWithValue("@ThanhTien", thanhTien);
            try
            {
                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (OleDbException)
            {
                conn.Close();
                return false;
            }

        }

        public DataTable DocBang(String tenBang, DataSet duLieu, String Where = "")
        {
            DataTable Kq = new DataTable(tenBang);
            duLieu.Tables.Add(Kq);
            String query = "Select * From " + tenBang;
            if (Where != "")
                query += " Where " + Where;
            OleDbDataAdapter Bo_thich_ung = new OleDbDataAdapter(query, strConection);
            Bo_thich_ung.Fill(Kq);
            Bo_thich_ung.FillSchema(Kq, SchemaType.Source);
            return Kq;
        }

        public void LayThongKeHoaDon(DateTime from, DateTime to,ref DataTable  dulieu)
        {
           
            string sql = "select * from HOADON where NgayGioTao between @from and @to";
            to = to.AddDays(1);
            OleDbConnection conn = KetNoi();
            OleDbCommand command = new OleDbCommand(sql, conn);
            command.Parameters.AddWithValue("@from", from);
            command.Parameters.AddWithValue("@to", to);


            OleDbDataReader read = command.ExecuteReader();
           
            while (read.Read())
            {
                dulieu.Rows.Add(read["IDHoaDon"], read["IDDatPhong"], read["IDKhachHang"], read["NgayGioTao"], read["Total"].ToString());
               
            }
            conn.Close();
            

            
        }

        public DataTable LayThongKeSanPham (string idHoaDon, ref DataTable Kq)
        {
          
          
            // masp, tensp, so luong, don gia, thu nhap
            DataTable dataTable= new DataTable();
            string sql = "select  HOADONSP.IDSanPham,TenSanPham,SUM(SoLuong), SUM(ThanhTien) from HOADONSP,SANPHAM where IDHoaDon in (" + idHoaDon + ") and HOADONSP.IDSanPham=SANPHAM.IDSanPham GROUP BY HOADONSP.IDSanPham,TenSanPham";
            OleDbDataAdapter Bo_thich_ung = new OleDbDataAdapter(sql, strConection);
            try
            {
                Bo_thich_ung.Fill(Kq);
                Bo_thich_ung.FillSchema(Kq, SchemaType.Source);
                foreach (DataRow row in Kq.Rows)
                {
                    string[] data = { row.ItemArray[4].ToString(), row.ItemArray[5].ToString(), row.ItemArray[6].ToString(), row.ItemArray[7].ToString() };
                    row.ItemArray = data;
                }

                return Kq;
            }
        
            catch
            {
                return null;
            }

            
        }

        public void ThemLoaiPhongHat(string ten, double gia)
        {
            string sql = "insert into LOAIPHONGHAT(TenLoaiPhong,Gia) Values(@ten,@gia)";
            OleDbConnection conn= KetNoi();
            OleDbCommand command = new OleDbCommand(sql, conn);
            command.Parameters.AddWithValue("@ten", ten);
            command.Parameters.AddWithValue("@gia", gia);
            command.ExecuteNonQuery();
            conn.Close();

        }

        public void CapNhatLoaiPhong(int id, string ten, double gia)
        {
            string sql = "UPDATE LOAIPHONGHAT SET TenLoaiPhong=@ten, Gia=@gia WHERE IDLoaiPhong=" + id.ToString() ;
            OleDbConnection conn = KetNoi();
            OleDbCommand command = new OleDbCommand(sql, conn);
            command.Parameters.AddWithValue("@ten", ten);
            command.Parameters.AddWithValue("@gia", gia);
            command.ExecuteNonQuery();
            conn.Close();
        }

        public void XoaKH(int id)
        {
            string sql = "DELETE FROM KHACHHANG "
                        + " WHERE IDKhachHang =" + id.ToString();
            OleDbConnection conn = KetNoi();

            OleDbCommand command = new OleDbCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
          
        }
    }
}

