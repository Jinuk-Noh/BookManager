using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookManager
{
    public partial class Form2 : Form
    {
        DBO dbo = new DBO();
        public Form2()
        {
            InitializeComponent();
            Text = "도서 관리";

            dataGridView_book.DataSource = dbo.Query_Select("bookinfo").DataSource;
            dataGridView_book.DataMember = dbo.Query_Select("bookinfo").DataMember;
        }

        private void button_add_Click(object sender, EventArgs e)
        {


            string Isbn = textBox_isbn.Text;
            string Name = textBox_bookName.Text;
            string Publisher = textBox_publisher.Text;
            int Page = int.Parse(textBox_page.Text);

            dbo.Query_Insert("bookinfo", Isbn, Name, Publisher, Page);
            dataGridView_book.DataSource = dbo.Query_Select("bookinfo").DataSource;
        }



        private void button_modify_Click(object sender, EventArgs e)
        {
            string changeName = textBox_bookName.Text;
            string changePublisher = textBox_publisher.Text;
            string changePage = textBox_page.Text;
            dataGridView_book.DataSource = dbo.Query_SelectIsbn("bookinfo", textBox_isbn.Text).DataSource;
            try
            {
                string update_isbn = dataGridView_book.Rows[0].Cells[0].FormattedValue.ToString();
                if (changeName.Trim() != "" && changePublisher.Trim() != "" && changePage.Trim() != "")
                {
                    dbo.Query_Update("bookinfo", update_isbn, changeName, changePublisher, changePage);
                }
                else
                {
                    MessageBox.Show("책이름, 출판사, 페이지 수를 입력하였는지 확인해주세요");
                }
            }
            catch
            {
                MessageBox.Show("존재하지 않는 도서입니다.");
            }
            dataGridView_book.DataSource = dbo.Query_Select("bookinfo").DataSource;
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            string delete_isbn = textBox_isbn.Text;
            string delete_book = textBox_bookName.Text;
            dataGridView_book.DataSource = dbo.Query_SelectIsbn("bookinfo", textBox_isbn.Text).DataSource;
            try
            {
                string a = dataGridView_book.CurrentRow.Cells[0].Value.ToString();
                MessageBox.Show(a);
                DialogResult result = MessageBox.Show($"ISBN : {delete_isbn}, 제목 : {delete_book}  정보를 삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    dbo.Query_DeleteB("bookinfo", delete_isbn);
                }
            }
            catch
            {
                MessageBox.Show("Isbn을 다시한번 확인해주세요");
            }
            dataGridView_book.DataSource = dbo.Query_Select("bookinfo").DataSource;
        }




        private void dataGridView_book_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                textBox_isbn.Text = dataGridView_book.CurrentRow.Cells[0].Value.ToString();
                textBox_bookName.Text = dataGridView_book.CurrentRow.Cells[1].Value.ToString();
                textBox_publisher.Text = dataGridView_book.CurrentRow.Cells[2].Value.ToString();
                textBox_page.Text = dataGridView_book.CurrentRow.Cells[3].Value.ToString();
            }
            catch (Exception)
            {

            }

        }
    }
}
