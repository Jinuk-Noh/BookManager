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
    public partial class Form3 : Form
    {
        DBO dbo = new DBO();
        public Form3()
        {

            InitializeComponent();

            Text = "사용자관리";

           
            try
            {
                dataGridView_Users.DataSource = dbo.Query_Select("userinfo").DataSource;             
                dataGridView_Users.DataMember = dbo.Query_Select("userinfo").DataMember;

            }
            catch { }
            dataGridView_Users.CurrentCellChanged += DataGridView_Users_CurrentCellChanged;

            //=> 람다 
            button_Add.Click += (sender, e) =>
            {
                dbo.Query_Insert("userinfo", textBox_ID.Text, textBox_Name.Text);
                dataGridView_Users.DataSource = dbo.Query_Select("userinfo").DataSource;
            };


            button_Modify.Click += (sender, e) => {
                string changeName = textBox_Name.Text;
                if (changeName.Trim() ==  "")
                {
                    MessageBox.Show("변경할 이름을 입력해주세요");
                }
                else
                {
                    dataGridView_Users.DataSource = dbo.Query_SelectId("userinfo", textBox_ID.Text).DataSource;
                    try
                    {
                        string a = dataGridView_Users.Rows[0].Cells[0].FormattedValue.ToString();
                        dbo.Query_Update("userinfo", textBox_ID.Text, changeName);
                    }
                    catch
                    {
                        MessageBox.Show("존재하지 않는 ID 번호입니다.");
                    }
                    dataGridView_Users.DataSource = dbo.Query_Select("userinfo").DataSource;
                }
            };



            button_Delete.Click += (sender, e) =>
            {
                string deleteId = textBox_ID.Text;
                dataGridView_Users.DataSource = dbo.Query_SelectId("userinfo", textBox_ID.Text).DataSource;
                try
                {
                    string a = dataGridView_Users.Rows[0].Cells[0].FormattedValue.ToString();
                    DialogResult result = MessageBox.Show($"삭제하는 ID가 {deleteId}가 맞습니까?", "확인", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        dbo.Query_DeleteU("userinfo", textBox_ID.Text);
                    }
                }
                catch
                {
                    MessageBox.Show("존재하지 않는 ID 번호입니다.");
                }               
                dataGridView_Users.DataSource = dbo.Query_Select("userinfo").DataSource;
            };

        }

        private void DataGridView_Users_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                textBox_ID.Text = dataGridView_Users.CurrentRow.Cells[0].Value.ToString();
                textBox_Name.Text = dataGridView_Users.CurrentRow.Cells[1].Value.ToString();
            }
            catch (Exception)
            {

            }
        }
    }
}
