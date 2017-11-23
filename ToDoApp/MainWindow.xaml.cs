using System.Data;
using System.Windows;

namespace Kalender
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private Database db;
        public MainWindow()
        {
            InitializeComponent();
            db = new Database();
            DataSet ds = db.selectTermin();
            DataTable dt = ds.Tables["Termine"];

            foreach (DataRow dr in dt.Rows)
            {
                liTermin.Items.Add(dr["tn_bez"] + "von: " + dr["tn_von"] + " bis: " + dr["tn_bis"]);
            }
            
        }

        private void miClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void miDbSettings_Click(object sender, RoutedEventArgs e)
        {
            DbSettings dbset = new DbSettings();
            dbset.Show();
        }

        private void miCalendar_Click(object sender, RoutedEventArgs e)
        {
            MonthCalendar monthCalendar = new MonthCalendar();
            monthCalendar.Show();
        }
    }
}
