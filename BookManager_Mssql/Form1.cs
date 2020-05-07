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
    public partial class Form1 : Form
    {
        DBO dbo = new DBO();
        public Form1()
        {
            InitializeComponent();
            Text = "도서관 관리";
            label5.Text = DateTime.Now.ToString("HH:mm");
            // timer1.Enabled = false; <<나중에 주석 풀것



            dataGridView_BookManager.CurrentCellChanged += DataGridView_BookManager_CurrentCellChanged;

            //전체 도서 수
            label_allBookCount.Text = (dataGridView_BookManager.Rows.Count).ToString();
            //사용자 수
            label_allUserCount.Text = (dataGridView_UserManager.Rows.Count).ToString();
            //대출중인 도서의 수
            label_allBorrowedBook.Text = DataManager.Books.Where((x) => x.isBorrowed).Count().ToString();
            //연체중인 도서의 수
            label_allDelayedBook.Text = DataManager.Books.Where((x) =>
            {
                return x.isBorrowed && x.BorrowedAt.AddDays(7) < DateTime.Now;
            }).Count().ToString();

            //데이터 그리드 설정
            dataGridView_BookManager.DataSource = dbo.Query_Select("bookinfo").DataSource;
            dataGridView_BookManager.DataMember = dbo.Query_Select("bookinfo").DataMember;
            dataGridView_UserManager.DataSource = dbo.Query_Select("userinfo").DataSource;
            dataGridView_UserManager.DataMember = dbo.Query_Select("userinfo").DataMember;


        }

        private void DataGridView_BookManager_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                textBox_isbn.Text = dataGridView_BookManager.CurrentRow.Cells[0].Value.ToString();
                textBox_bookName.Text = dataGridView_BookManager.CurrentRow.Cells[1].Value.ToString();
            }
            catch (Exception)
            {

            }
        }

        private void dataGridView_UserManager_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                textBox_id.Text = dataGridView_UserManager.CurrentRow.Cells[0].Value.ToString();
            }
            catch (Exception)
            {

            }
        }
        private void button_Borrow_Click(object sender, EventArgs e)
        {
            if (textBox_isbn.Text.Trim() == "")
            {
                MessageBox.Show("Isbn을 입력해주세요.");
            }
            else if (textBox_id.Text.Trim() == "")
            {
                MessageBox.Show("사용자 Id를 입력해주세요.");
            }
            else
            {
                string brId = textBox_id.Text;
                string brIsbn = textBox_isbn.Text;
                try
                {
                    dataGridView_BookManager.DataSource = dbo.Query_SelectIsbn("bookinfo", brIsbn).DataSource;
                    Boolean borrowed = Boolean.Parse(dataGridView_BookManager.CurrentRow.Cells[6].Value.ToString());
                    if (borrowed)
                    {
                        MessageBox.Show("이미 대여중인 도서입니다.");
                    }
                    else
                    {
                        dataGridView_UserManager.DataSource = dbo.Query_SelectId("userinfo", brId).DataSource;
                        string brName = dataGridView_UserManager.CurrentRow.Cells[1].Value.ToString();


                        dbo.Query_Update_br("bookinfo", brIsbn, brId, brName);
                        MessageBox.Show("\"" + brIsbn + "\"이/가\"" + brName + "\"님께 대여되었습니다.");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("존재하지 않는 도서 또는 사용자입니다.");
                }
                dataGridView_UserManager.DataSource = dbo.Query_Select("userinfo").DataSource;
                dataGridView_BookManager.DataSource = dbo.Query_Select("bookinfo").DataSource;
            }
        }

        private void button_Return_Click(object sender, EventArgs e)
        {
            if (textBox_isbn.Text.Trim() == "")
            {
                MessageBox.Show("Isbn을 입력해주세요.");
            }
            else
            {
                string brIsbn = textBox_isbn.Text;
                try
                {
                    dataGridView_BookManager.DataSource = dbo.Query_SelectIsbn("bookinfo", brIsbn).DataSource;
                    string bookname = dataGridView_BookManager.CurrentRow.Cells[1].Value.ToString();
                    Boolean borrowed = Boolean.Parse(dataGridView_BookManager.CurrentRow.Cells[6].Value.ToString());
                    if (borrowed)
                    {
                        DateTime oldDay = DateTime.Parse(dataGridView_BookManager.CurrentRow.Cells[7].Value.ToString());
                        TimeSpan timeDiff = DateTime.Now - oldDay;
                        int diffDays = timeDiff.Days;
                        if (diffDays > 7)
                        {
                            MessageBox.Show("\"" + bookname + "\"이/가 연체 상태로 반납되었습니다.");
                            dbo.Query_Update_br("bookinfo", brIsbn);
                        }
                        else
                        {
                            MessageBox.Show("\"" + bookname + "\"이/가 반납되었습니다.");
                            dbo.Query_Update_br("bookinfo", brIsbn);
                        }
                    }

                    else
                    {
                        MessageBox.Show("대여상태가 아닙니다.");
                    }
                }

                catch (Exception)
                {
                    MessageBox.Show("존재하지 않는 도서 또는 사용자입니다.");
                }
                dataGridView_UserManager.DataSource = dbo.Query_Select("userinfo").DataSource;
                dataGridView_BookManager.DataSource = dbo.Query_Select("bookinfo").DataSource;
            }
        }

        private void 도서관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // new Form2().ShowDialog();
            Form2 temp = new Form2();
            temp.ShowDialog();
            dataGridView_BookManager.DataSource = dbo.Query_Select("bookinfo").DataSource;


        }

        private void 사용자관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form3().ShowDialog();

            dataGridView_UserManager.DataSource = dbo.Query_Select("userinfo").DataSource;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label5.Text = DateTime.Now.ToString("HH:mm");
        }
    }
}
