using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using TestTask.Data;
namespace TestTask
{
    public partial class Form1 : Form
    {
        SqliteReadWorkers read;
        SqliteConnection conn;
        Worker[] workers;
        AddView frm;
        public Form1()
        {
            
            InitializeComponent();
            
            
            conn = new SqliteConnection();
            read = new SqliteReadWorkers();
            read.ReadWorkers("SELECT * FROM Workers");
            workers = read.Worker;
            ShowBase();
            
            
        }
        private void Form1_FormClosed(Object sender, FormClosedEventArgs e)
        {
            read.ReadWorkers("SELECT * FROM Workers");
            workers = read.Worker;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            ShowBase();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            frm = new AddView();
            frm.FormClosed += new FormClosedEventHandler(this.Form1_FormClosed);
            this.frm.Show();
            
        }
        private void ShowBase()
        {
            try
            {
                read.ReadWorkers("SELECT * FROM Workers");
                this.workers = read.Worker;
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось очистить или прочитать рабочих в таблице");
            }
            using (var connection = new SQLiteConnection("Data Source = Workersdata.db"))
            {
               
                connection.Open();
                Div.Items.Clear();
                SQLiteCommand command = new SQLiteCommand("SELECT DISTINCT NameDiv FROM Workers ", connection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        
                        while (reader.Read())
                        {
                            Div.Items.Add(reader.GetString(0));

                        }
                    }
                }
            }
            int i = 0;
            foreach (Worker worker in workers)
            {      
                int k = 0;
                for (k = 0; k < Post.Items.Count; k++)
                    if (Post.Items[k].ToString() == worker.Post.ToString())
                        break;

                dataGridView1.Rows.Add();
                dataGridView1[0,i].Value = worker.Id;
                dataGridView1[1, i].Value = worker.Name;
                dataGridView1[2, i].Value = worker.Birthday;
                dataGridView1[3, i].Value = worker.Gender;
                dataGridView1[4, i].Value = worker.NameDiv;
                dataGridView1.Rows[i].Cells[5].Value = Post.Items[k];
                dataGridView1[6, i].Value = worker.Info;

                i++;
            }
            comboBox1.Items.Clear();
            for (i = 0; i < Div.Items.Count; i++)
                comboBox1.Items.Add(Div.Items[i].ToString());
            comboBox2.Items.Clear();
            for (i = 0; i < Post.Items.Count; i++)
                comboBox2.Items.Add(Post.Items[i]);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                try
                {
                    if (!conn.Delete(Convert.ToInt32(dataGridView1[0,row.Index].Value)))
                    {
                        MessageBox.Show("Ошибка!");
                    }
                    dataGridView1.Rows.RemoveAt(row.Index);
                    

                }
                catch
                {
                    MessageBox.Show("Выбранная пустая строка");
                }
            }
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1[e.ColumnIndex, e.RowIndex].Value != null)
            {
                conn.Edit(dataGridView1.Rows[e.RowIndex],Convert.ToInt32(dataGridView1[0, e.RowIndex].Value), e.ColumnIndex);
            }
            ShowBase();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            String nameDiv = comboBox1.Text;
            String posts = comboBox2.Text;
            DataTable dt = new DataTable();
            using (var connection = new SQLiteConnection("Data Source = Workersdata.db"))
            {
                connection.Open();
               
                SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM Workers", connection);
                dt = conn.SelectData($"SELECT * FROM Workers" +
                            $" WHERE Posts = '{posts}' AND NameDiv = '{nameDiv}'");
                
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dataGridView1.Rows.Add();
                for(int j = 0; j < dt.Columns.Count; j++)
                    dataGridView1[j,i].Value = dt.Rows[i][j].ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            ShowBase();
        }
    }
}
