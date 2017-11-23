using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kalender
{
    /// <summary>
    /// Interaktionslogik für MonthCalendar.xaml
    /// </summary>
    public partial class MonthCalendar : Window
    {
        private DateTime dat;
        private DateTime dateNow;
        private Database db;

        struct Termin
        {
            public int Id;
            public string Bezeichnung;
            public DateTime Von;
            public DateTime Bis;
        }


        public MonthCalendar()
        {
            InitializeComponent();
            dateNow = DateTime.Now;
            dat = new DateTime(dateNow.Year, dateNow.Month, 1);
            lblCurrentDate.Content = dateNow.ToString("dd.MM.yyyy");
            db = new Database();
            Refresh();
        }

        private void Refresh()
        {
            tbxDate.Text = dat.ToString("MMMM yyyy");

            int max = DateTime.DaysInMonth(dat.Year, dat.Month);
            grd_btns.Children.Clear();
            int k = (int)dat.DayOfWeek;
            if (k == 0)
                k = 7;
            k -= 1;

            GenerateLabel(k, max);
        }

        private Termin[] GenerateTerminArray(int k, int max)
        {
            Database db = new Database();
            DataSet ds = db.selectTermin();
            DataTable dt = ds.Tables["Termine"];
            Termin[] termin = new Termin[dt.Rows.Count];
            int x = 0;
            foreach(DataRow row in dt.Rows)
            {
                termin[x].Id = x;
                termin[x].Bezeichnung = row["tn_bez"].ToString();
                termin[x].Von = (DateTime)row["tn_von"];
                termin[x].Bis = (DateTime)row["tn_bis"];
                x++;
            }
            return termin;
        }

        private void GenerateLabel(int k, int max)
        {
            Termin[] termin = GenerateTerminArray(k, max);
            int cou = 0;
            for (int i = 0; i < 6 && cou < max; i++)
            {
                for (int j = 0; (j + k) < 7 && cou < max; j++)
                {
                    int x = cou + 1;
                    Label lbl = new Label();
                    if (x == dateNow.Day && dat.Month == dateNow.Month)
                    {
                        lbl.Foreground = Brushes.Red;
                    }
                    lbl.Name = "tag" + x.ToString("00");
                    if(termin[0].Von.Day == x)
                    {
                        lbl.Content = x + "\n" + termin[0].Bezeichnung;
                    }else
                    {
                        lbl.Content = x;
                    }
                    lbl.MouseLeftButtonDown += new MouseButtonEventHandler(selectDay_Click);
                    grd_btns.Children.Add(lbl);
                    Grid.SetRow(lbl, (i));
                    Grid.SetColumn(lbl, j + k);
                    cou++;
                }
                k = 0;
            }
        }

        private void selectDay_Click(object sender, RoutedEventArgs e)
        {
            Label lab = sender as Label;
            DataSet ds = db.selectTermin();
            DataTable dt = ds.Tables["Termine"];
            string termin = "";
            foreach (DataRow dr in dt.Rows)
            {
                termin = dr["tn_bez"] + "von: " + dr["tn_von"] + " bis: " + dr["tn_bis"];
            }
            int day = Convert.ToInt16(lab.Name.Substring(3));
            lblCurrentDate.Content = day.ToString("00") + "." + dat.ToString("MM.yyyy") + "\n" + termin;
            
        }

        private void btnForwrd_Click(object sender, RoutedEventArgs e)
        {
            if (dat.Month < 12)
            {
                dat = new DateTime(dat.Year, dat.Month + 1, dat.Day);
            }
            else
            {
                dat = new DateTime(dat.Year + 1, dat.Month - 11, dat.Day);
            }
            Refresh();
        }

        private void btnBackwrd_Click(object sender, RoutedEventArgs e)
        {
            if (dat.Month > 1)
            {
                dat = new DateTime(dat.Year, dat.Month - 1, dat.Day);
            }
            else
            {
                dat = new DateTime(dat.Year - 1, dat.Month + 11, dat.Day);
            }
            Refresh();
        }
    }
}
