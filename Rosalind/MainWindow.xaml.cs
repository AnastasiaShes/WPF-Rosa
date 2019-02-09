using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections.Specialized;

namespace Rosalind
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {

           InitializeComponent();

           FillingLB(); //Заполнение списка

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) //Выбор ячейки
        {
            if (listBox.SelectedItem is Information _info)
            {
                TB_1.Text = _info.Name;
                Tb_id.Text = _info.Index;
                TB_Condition.Text = "Условие: " +  _info.Condition;
                EXP_text.Text = _info.Text;
            }            
        }

        private void FillingLB() //Метод для заполнения списка
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            var Info = new List<Information>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM [Information]", sqlConnection))
                {
                    DataTable dt = new DataTable();
                    sqlConnection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dt.Load(reader);

                        Info = dt.AsEnumerable().Select(se => new Information(se.Field<string>("Name"), se.Field<string>("Id"), se.Field<string>("Condition"), se.Field<string>("Text")) { Name = se.Field<string>("Id"), Index = se.Field<string>("Name"), Condition = se.Field<string>("Condition"), Text = se.Field<string>("Text") }).ToList();
                    }
                }
            }
            listBox.ItemsSource = Info;
        }
    }
}



