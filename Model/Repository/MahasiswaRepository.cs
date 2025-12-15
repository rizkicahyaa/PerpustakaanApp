using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;
using PerpustakaanApp.Model.Entity;
using PerpustakaanApp.Model.Context;

namespace PerpustakaanApp.Model.Repository
{
    public class MahasiswaRepository
    {
        // deklarasi objek connection
        private SQLiteConnection _conn;

        // constructor
        public MahasiswaRepository(DbContext context)
        {
            // inisialisasi objek connection
            _conn = context.Conn;
        }

        public int Create(Mahasiswa mhs)
        {
            int result = 0;
            // deklarasi perintah SQL
            string sql = @"insert into mahasiswa (nim, nama, angkatan) values (@nim, @nama, @angkatan)";
            // membuat objek command menggunakan blok using
            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                // mendaftarkan parameter dan mengeset nilainya
                cmd.Parameters.AddWithValue("@nim", mhs.Nim);
                cmd.Parameters.AddWithValue("@nama", mhs.Nama);
                cmd.Parameters.AddWithValue("@angkatan", mhs.Jurusan);
                try
                {
                    // jalankan perintah INSERT dan tampung hasilnya ke dalam variabel result
                    result = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("Create error: {0}", ex.Message);
                }
            }
            return result;
        }

        public int Update(Mahasiswa mhs)
        {
            int result = 0;

            string sql = @"update mahasiswa set nama = @nama, jurusan = @jurusan where nim = @nim";

            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@nim", mhs.Nim);
                cmd.Parameters.AddWithValue("@nama", mhs.Nama);
                cmd.Parameters.AddWithValue("@jurusan", mhs.Jurusan);

                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("Update error: {0}", ex.Message);
                }
            }

            return result;
        }

        public int Delete(Mahasiswa mhs)
        {
            int result = 0;

            string sql = @"delete from mahasiswa
                           where nim = @nim";

            using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@nim", mhs.Nim);

                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("Delete error: {0}", ex.Message);
                }
            }

            return result;
        }

        public List<Mahasiswa> ReadAll()
        {
            List<Mahasiswa> list = new List<Mahasiswa>();

            try
            {
                string sql = @"select nim, nama, jurusan
                               from mahasiswa 
                               order by nama";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
                {
                    using (SQLiteDataReader dtr = cmd.ExecuteReader())
                    {
                        while (dtr.Read())
                        {
                            Mahasiswa mhs = new Mahasiswa();
                            mhs.Nim = dtr["nim"].ToString();
                            mhs.Nama = dtr["nama"].ToString();
                            mhs.Jurusan = dtr["jurusan"].ToString();

                            list.Add(mhs);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("ReadAll error: {0}", ex.Message);
            }

            return list;
        }

        public List<Mahasiswa> ReadByNama(string nama)
        {
            List<Mahasiswa> list = new List<Mahasiswa>();

            try
            {
                string sql = @"select nim, nama, jurusan
                               from mahasiswa 
                               where nama like @nama
                               order by nama";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, _conn))
                {
                    cmd.Parameters.AddWithValue("@nama", "%" + nama + "%");

                    using (SQLiteDataReader dtr = cmd.ExecuteReader())
                    {
                        while (dtr.Read())
                        {
                            Mahasiswa mhs = new Mahasiswa();
                            mhs.Nim = dtr["nim"].ToString();
                            mhs.Nama = dtr["nama"].ToString();
                            mhs.Jurusan = dtr["jurusan"].ToString();

                            list.Add(mhs);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("ReadByNama error: {0}", ex.Message);
            }

            return list;
        }
    }
}
