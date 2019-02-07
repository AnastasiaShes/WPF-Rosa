using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections.Specialized;

namespace Rosalind
{

    /// Для грида полосы: ShowGridLines="True"

 public partial class MainWindow : Window
    {
        string connectionString;

        public MainWindow()
        {
            InitializeComponent();

            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            var artists = new List<Information>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT [Id], [Name] FROM [Information]", sqlConnection))
                {
                    DataTable dt = new DataTable();
                    sqlConnection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dt.Load(reader);

                        artists = dt.AsEnumerable().Select(se => new Information(se.Field<string>("Id"), se.Field<string>("Name")) { Name = se.Field<string>("Id"), Index = se.Field<string>("Name") }).ToList();
                    }
                }
            }

            listBox.ItemsSource = artists;
        }
    }
}

public class Information
{
    private readonly string name;
    private readonly string index;

    public Information(string name, string index)
    {
        this.name = name;
        this.index = index;
    }

    public string Name
    {
        get { return name; }
        set { }
    }

    public string Index
    {
        get { return index; }
        set { }
    }
}


