using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestTask.Data;
using System.Data.SQLite;
namespace TestTask
{
    public partial class AddView : Form
    {
        Worker worker;
        public AddView()
        {
            InitializeComponent();
            using (var connection = new SQLiteConnection("Data Source = Workersdata.db"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand("SELECT DISTINCT NameDiv FROM Workers ",connection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            comboBox3.Items.Add(reader.GetString(0));
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            worker = new Worker();
            worker.Name = textBox1.Text;
            worker.Birthday = dateTimePicker1.Text;
            worker.Gender = comboBox2.Text;
            worker.Post = comboBox1.Text;
            worker.NameDiv = comboBox3.Text;
            SqliteConnection add = new SqliteConnection();
            bool flag = add.Add(worker);
            if (flag)
                this.Close();
            else
                MessageBox.Show("Неправильно введены значения");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
