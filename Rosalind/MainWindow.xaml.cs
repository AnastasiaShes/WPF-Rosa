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

        #region Событие при выборе ячейки
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            if (listBox.SelectedItem is Information _info)
            {
                TB_Headline.Text = _info.Name;
                TB_Condition.Text = "Условие: " +  _info.Condition;
                EXP_text.Text = _info.Text;
            }            
        }
        #endregion

        #region Кнопка "Решить"
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            logic log = new logic();

            if (listBox.SelectedItem is Information _info)
            {
                TB_Answer.Text = log.Decision(_info.Index, TB_Variable.Text);
            }

        }
        #endregion

        #region Метод для заполнения списка
        private void FillingLB()
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

        #endregion

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
           TB_Headline.Text = "Заголовок";
           TB_Condition.Text = "Условие";
           TB_Variable.Text = "";
           TB_Answer.Text = "";
           EXP_text.Text = "";
           listBox.SelectedIndex = -1;
        }
    } 

}



