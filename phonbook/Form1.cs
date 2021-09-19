using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace phonbook
{
    public partial class Form1 : Form
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter adabtor = new SqlDataAdapter();
        DataSet dataset1 = new DataSet();
        CurrencyManager crm;
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Asus\Desktop\phonbock\phonbook\phonbook\Telbook.mdf;Integrated Security=True";
            connection.Open();
            Fillgrid();
            crm = (CurrencyManager)this.BindingContext[dataset1, "T1"];
            btnSave.Enabled = false;
            readOnlyTextbox(true, "");
          
        }

        void Fillgrid(string text = "select * from Tbltell")
        {
            dataset1.Clear();
            command.CommandText = text;
            command.Connection = connection;
            adabtor.SelectCommand = command;
            adabtor.Fill(dataset1, "T1");
            dataGridView1.DataBindings.Clear();
            dataGridView1.DataBindings.Add("datasource", dataset1, "T1");
            txtName.DataBindings.Clear();
            txtName.DataBindings.Add("text", dataset1, "T1.FirstName");
            txtFamliy.DataBindings.Clear();
            txtFamliy.DataBindings.Add("text", dataset1, "T1.Lastname");
            txtTell.DataBindings.Clear();
            txtTell.DataBindings.Add("text", dataset1, "T1.Phoneno");
            txtAddress.DataBindings.Clear();
            txtAddress.DataBindings.Add("text", dataset1, "T1.address");
            pib.DataBindings.Clear();
            pib.DataBindings.Add("ImageLocation", dataset1, "T1.Imageurl");

        }
        #region chenge curentrows
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                changeCurentrows(1);
                
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.SelectedRows[0].Index;
            }

        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {


                changeCurentrows(-1);
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.SelectedRows[0].Index;
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {

            changeCurentrows(0);
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.SelectedRows[0].Index;
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            changeCurentrows(dataGridView1.Rows.Count - 1);
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.SelectedRows[0].Index;
        }
        private void changeCurentrows(int num)
        {
            if (num == 0 || num == dataGridView1.Rows.Count - 1)
            {
                crm.Position = num;
            }
            else
            {
                crm.Position += num;
            }
            for (int a = 0; a < dataGridView1.Rows.Count; a++)
            {
                dataGridView1.Rows[a].Selected = false;
            }
            //dataGridView1.Rows[(crm.Position - num)].Selected = false;

            dataGridView1.Rows[(crm.Position)].Selected = true;

        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            crm.Position = dataGridView1.CurrentCell.RowIndex;

        }
        #endregion

        private void btnNew_Click(object sender, EventArgs e)
        {
            btnselect.Enabled = true;
            btnSave.Enabled = true;
            readOnlyTextbox(false, "new");
            txtName.Text = "";
            txtFamliy.Text = "";
            txtTell.Text = "";
            txtAddress.Text = "";
            if (dataGridView1.Rows.Count > 0)
                dataGridView1.CurrentCell.Selected = false;

        }

        private void readOnlyTextbox(bool isenable, string btnName)
        {
            txtAddress.ReadOnly = isenable;
            txtFamliy.ReadOnly = isenable;
            txtName.ReadOnly = isenable;
            if (btnName == "edit")
                return;
            txtTell.ReadOnly = isenable;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (txtTell.Text == "")
            {
                MessageBox.Show("phon number cant emply");
                return;
            }
            string i = txtTell.Text;
            addAndEdit("new");
            btnSave.Enabled = false;
            //........................................................

            int rowIndex = -1;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[2].Value.ToString().Equals(i))
                {
                    rowIndex = row.Index;
                    break;
                }
            }
            dataGridView1.Rows[0].Selected = false;
            dataGridView1.Rows[rowIndex].Selected = true;
            if (rowIndex >= 0)
            {
                crm.Position = rowIndex;
            }

            readOnlyTextbox(true, "");
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.SelectedRows[0].Index;
            btnselect.Enabled = false;
        }
        private void addAndEdit(string txtbutton, string txtname = "Unknown", string txtfamily = "Unknown", string txtaddress = "Unknown")
        {


            if (txtName.Text != "")
                txtname = txtName.Text;
            if (txtAddress.Text != "")
                txtaddress = txtAddress.Text;
            if (txtFamliy.Text != "")
                txtfamily = txtFamliy.Text;


            SqlCommand com = new SqlCommand();
            com.Connection = connection;
            //VALUES('"+"@p1"+"', "+ "@p2"+"'," + "@p3" + "', "+ "@p4" + "',)"
            com.CommandText = "INSERT INTO Tbltell(Firstname ,Lastname ,Phoneno ,address,Imageurl) VALUES(@p1,@p2,@p3,@p4,@p5)";
            if (txtbutton == "edit")
                com.CommandText = " UPDATE Tbltell set Firstname = @p1 ,Lastname = @p2 ,address = @p4, Imageurl = @p5 WHERE phoneno = @p3 ";

            //            {
            //            UPDATE table_name
            //SET column1 = value1, column2 = value2, ...
            // UPDATE condition;
            //            }
            com.Parameters.AddWithValue("@p1", txtname);
            com.Parameters.AddWithValue("@p2", txtfamily);
            com.Parameters.AddWithValue("@p3", txtTell.Text);
            com.Parameters.AddWithValue("@p4", txtaddress);
            com.Parameters.AddWithValue("@p5", pib.ImageLocation);
            com.ExecuteNonQuery();
            Fillgrid();
        }
        bool isedit1 = true;
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count < 1)
                return;
            int a = dataGridView1.CurrentCell.RowIndex;

            if (dataGridView1.SelectedCells.Count == 0)
            {
                MessageBox.Show("selected row...");
                return;
            }
            if (isedit1)
            {
                readOnlyTextbox(false, "edit");
                isedit1 = false;
                btnEdit.Text = "save";
                btnselect.Enabled = true;
                return;

            }
            addAndEdit("edit");
            isedit1 = true;
            btnEdit.Text = "Edit";
            dataGridView1.Rows[0].Selected = false;
            dataGridView1.Rows[a].Selected = true;
            crm.Position = a;
            readOnlyTextbox(true, "");
            btnselect.Enabled = false;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedCells.Count == 0)
            {
                MessageBox.Show("selected row...");
                return;
            }
            SqlCommand com = new SqlCommand();
            com.Connection = connection;
            com.CommandText = "DELETE FROM Tbltell where Phoneno = @man";
            com.Parameters.AddWithValue("@man", txtTell.Text);

            com.ExecuteNonQuery();
            Fillgrid();
        }



        private void btnSearch_Click(object sender, EventArgs e)
        {
            string i;
            if (cmbSearchBy.SelectedItem != null)
            {
                i = cmbSearchBy.SelectedItem.ToString();
            }
            else
            {
                i = "Firstname";
            }

            string a = "select * from Tbltell where " + i + " LIKE N'%" + txtSearchFor.Text + "%';";
            Fillgrid(a);




        }

        private void txtSearchFor_KeyUp(object sender, KeyEventArgs e)
        {
            btnSearch_Click(null, null);
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Up)
            {
                btnPre_Click(null, null);
            }
            else if(e.KeyCode == Keys.Down)
            {
                btnNext_Click(null, null);
            }
        }


       private void selectImage()
        {
            
            DialogResult dr = new DialogResult();
           dr = openFileDialog1.ShowDialog();
            string fliname1 = openFileDialog1.FileName;
            string filnamecopy = Application.StartupPath + @"\image\" + txtTell.Text +"."+ fliname1.Split('.')[fliname1.Split('.').Length - 1];
            if (System.IO.File.Exists(filnamecopy))
                System.IO.File.Delete(filnamecopy);
                
            if (System.IO.Directory.Exists(Application.StartupPath + @"\image") == false)
            {
                System.IO.Directory.CreateDirectory((Application.StartupPath + @"\image"));
            }
           
            if (dr == DialogResult.OK)
            {
                System.IO.File.Copy(fliname1, filnamecopy);
                pib.ImageLocation = filnamecopy;
               
            }
        }

        private void btnselect_Click(object sender, EventArgs e)
        {
            selectImage();
        }
    }
}
