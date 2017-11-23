using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;

namespace Kalender
{
    /// <summary>
    /// Interaktionslogik für DbSettings.xaml
    /// </summary>
    public partial class DbSettings : Window
    {
        private XDocument dom;
        private const string xmlFile = @"settings\DbSettings.xml";

        public DbSettings()
        {
            InitializeComponent();
            dom = XDocument.Load(xmlFile);
            setCmdHost();        
        }


        private void setCmdHost()
        {
            lbxDatabase.Items.Clear();
            var result = from database in dom.Descendants("database")
                         select database;

            foreach (var info in result)
            {
                string list = string.Format("{0},{1},{2},{3},{4},{5},{6}",
                    (string)info.Attribute("id"),
                    (string)info.Attribute("typ"),
                    (string)info.Attribute("host"),
                    (string)info.Attribute("port"),
                    (string)info.Attribute("user"),
                    (string)info.Attribute("password"),
                    (string)info.Attribute("active"));
                lbxDatabase.Items.Add(list);
            }
           
        }

        private void lbxDatabase_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
            if(item != null)
            {
                string[] itemArray = item.Content.ToString().Split(',');
                txtId.Text = itemArray[0];
                txtTyp.Text = itemArray[1];
                txtHost.Text = itemArray[2];
                txtPort.Text = itemArray[3];
                txtUser.Text = itemArray[4];
                txtPass.Text = itemArray[5];
                if (itemArray[6] == "1")
                {
                    radioButton_Yes.IsChecked = true;
                }
                else
                {
                    radioButton_No.IsChecked = true;
                }
            }
            
        }

        private void changeActive()
        {
            var active = dom.Descendants("database")
                .Where(order => order.Attribute("active").Value == "1")
                .ToArray();
            foreach (var value in active)
            {
                value.Attribute("active").SetValue("0");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var id = txtId.Text;
            changeActive();

            var insert = dom.Descendants("database")
                         .Where(order => order.Attribute("id").Value == id)
                         .SingleOrDefault();
           
            insert.Attribute("typ").SetValue(txtTyp.Text);
            insert.Attribute("host").SetValue(txtHost.Text);
            insert.Attribute("port").SetValue(txtPort.Text);
            insert.Attribute("user").SetValue(txtUser.Text);
            insert.Attribute("password").SetValue(txtPass.Text);
            if(radioButton_Yes.IsChecked == true)
            {
                insert.Attribute("active").SetValue(1);
            }
            else
            {
                insert.Attribute("active").SetValue(0);
            }
            dom.Save(xmlFile);
            setCmdHost();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var act = 0;
            if (radioButton_Yes.IsChecked == true)
            {
                act = 1;
                changeActive();
            }

            if (dom.Descendants("database") != null)
            {
                var count = dom.Descendants("database").Count();
                var newDatabase = new XElement("database", new XAttribute("id", count + 1),
                                                           new XAttribute("typ", txtTyp.Text),
                                                           new XAttribute("host", txtHost.Text),
                                                           new XAttribute("port", txtPort.Text),
                                                           new XAttribute("user", txtUser.Text),
                                                           new XAttribute("password", txtPass.Text),
                                                           new XAttribute("active", act));
                
                dom.Root.Add(newDatabase);
                dom.Save(xmlFile);
                setCmdHost();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var id = txtId.Text;
            dom.Descendants("database")
                .Where(order => order.Attribute("id").Value == id)
                .Remove();
            dom.Save(xmlFile);
            setCmdHost();
        }
    }
}
