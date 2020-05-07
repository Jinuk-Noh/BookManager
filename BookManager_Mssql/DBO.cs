using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookManager
{
    class DBO
    {
        public static SqlConnection conn = new SqlConnection();

        //DB연결
        private static void ConnetDB()
        {
            conn.ConnectionString = string.Format("Data Source=({0});" +
                 "Initial Catalog = {1};" +
                 "Integrated Security= {2};" +
                 "Timeout = 3"
                 , "local", "BOOKMAN", "SSPI");
            conn = new SqlConnection(conn.ConnectionString);
            conn.Open();
        }

        //전체 조회
        public DataGridView Query_Select(String text)
        {
            DataGridView data = new DataGridView();
            ConnetDB();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select * from " + text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds, text);
            data.DataSource = ds;
            data.DataMember = text;
            conn.Close();
            return data;
        }

        //특정 조건 조회
        public DataGridView Query_SelectId(String text, String tb_id)
        {
            DataGridView data = new DataGridView();

            ConnetDB();
            string sqlcommand = "select * from " + text + " where id = @p";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@p", tb_id);
            cmd.CommandText = sqlcommand;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            da.Fill(ds, text);
            data.DataSource = ds;
            data.DataMember = text;
            conn.Close();                  
            return data;
        }

        public DataGridView Query_SelectIsbn(String text, String tb_isbn)
        {
            DataGridView data = new DataGridView();

            ConnetDB();
            string sqlcommand = "select * from " + text + " where isbn = @p";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@p", tb_isbn);
            cmd.CommandText = sqlcommand;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            da.Fill(ds, text);
            data.DataSource = ds;
            data.DataMember = text;
            conn.Close();
            return data;
        }

        //회원 추가
        public void Query_Insert(String table, String tb_id, String tb_name)
        {
            try
            {
                ConnetDB();
                string sqlcommand = "insert into " + table + " values (@p1, @p2)";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@p1", tb_id);
                cmd.Parameters.AddWithValue("@p2", tb_name);
                cmd.CommandText = sqlcommand;
                cmd.ExecuteNonQuery();
                conn.Close();           
            }
            catch
            {
                MessageBox.Show("중복된 ID넘버입니다.");
                conn.Close();
            }
        }
        // 책 추가
        public void Query_Insert(String table, String tb_isbn, String tb_name,String publisher, int page )
        {
            try
            {
                ConnetDB();
                string sqlcommand = "insert into " + table + "(isbn, name, publisher, page, isborrowed) values (@p1, @p2,@p3, @p4, 0)";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@p1", tb_isbn);
                cmd.Parameters.AddWithValue("@p2", tb_name);
                cmd.Parameters.AddWithValue("@p3", publisher);
                cmd.Parameters.AddWithValue("@p4", page);
                cmd.CommandText = sqlcommand;
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch
            {
                MessageBox.Show("중복된 Isbn입니다.");
                conn.Close();
            }
        }

        //회원정보 수정
        public void Query_Update(String table, String tb_id, String tb_name)
        {
            try
            {
                ConnetDB();
                string sqlcommand = $"update {table} set name = @p2 where id = @p1";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@p1", tb_id);
                cmd.Parameters.AddWithValue("@p2", tb_name);
                cmd.CommandText = sqlcommand;
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch
            {             
                conn.Close();
            }
        }
        //책정보 수정
        public void Query_Update(String table, String tb_id, String tb_name, String publisher, String page)
        {
            try
            {
                ConnetDB();
                string sqlcommand = $"update {table} set name = @p2, publisher =@p3, page =@p4 where isbn = @p1";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@p1", tb_id);
                cmd.Parameters.AddWithValue("@p2", tb_name);
                cmd.Parameters.AddWithValue("@p3", publisher);
                cmd.Parameters.AddWithValue("@p4", page);
                cmd.CommandText = sqlcommand;
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch
            {
                conn.Close();
            }
        }
        //대여 반납
        public void Query_Update_br(String table,String isbn ,String userid, String username)
        {
            try
            {
                ConnetDB();
                string sqlcommand = $"update {table} set userid = @p2, username =@p3, isborrowed =1, borrwedat=@p4 where isbn = @p1";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@p1", isbn);
                cmd.Parameters.AddWithValue("@p2", userid);
                cmd.Parameters.AddWithValue("@p3", username);

                string borrwedat = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                cmd.Parameters.AddWithValue("@p4", borrwedat);
                cmd.CommandText = sqlcommand;
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch
            {
                conn.Close();
            }
        }

        public void Query_Update_br(String table, String isbn)
        {
            try
            {
                ConnetDB();
                string sqlcommand = $"update {table} set userid = null , username =null, isborrowed =0, borrwedat=@p4 where isbn = @p1";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@p1", isbn);               
                string borrwedat ="";
                cmd.Parameters.AddWithValue("@p4", borrwedat);
                cmd.CommandText = sqlcommand;
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch
            {
                conn.Close();
            }
        }

        //정보 삭제
        public void Query_DeleteU(String table, String tb_id)
        {
            try
            {
                ConnetDB();
                string sqlcommand = $"delete from {table} where id =@p";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@p", tb_id);
                cmd.CommandText = sqlcommand;
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch
            {
                conn.Close();
            }
        }
        //책정보 삭제
        public void Query_DeleteB(String table, String tb_isbn)
        {
            try
            {
                ConnetDB();
                string sqlcommand = $"delete from {table} where isbn =@p";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@p", tb_isbn);
                cmd.CommandText = sqlcommand;
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch
            {
                conn.Close();
            }
        }







    }
}
