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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Asus\Desktop\phonbock\phonbook\phonbook\Telbook.mdf;Integrated Security=True";
            connection.Open();
            Fillgrid();
        }

        void Fillgrid(string text = "select * from Tbltell")
        {
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

        }

    }
}
