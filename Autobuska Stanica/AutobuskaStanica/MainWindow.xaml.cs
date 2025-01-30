using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace AutobuskaStanica
{

    public partial class MainWindow : Window
    {
        public static int RadnikID, StanicaID;
        string connectionString = "Server=127.0.0.1;Database=autobuskastanica;Uid=root;Pwd=;";
        
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (RadniciComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedText = selectedItem.Content.ToString();
                string[] parts = selectedText.Split(new string[] { " - " }, StringSplitOptions.None);

                if (parts.Length == 2 && int.TryParse(parts[1], out int radnikID))
                {
                    string sifra = PasswordBox.Password;

                    try
                    {
                        string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Radnici.txt");
                        string[] lines = System.IO.File.ReadAllLines(filePath);

                        bool isSifraValid = false;

                        foreach (string line in lines)
                        {
                            string[] fileParts = line.Split(' '); 
                            if (fileParts.Length == 5)
                            {
                                int fileRadnikID = int.Parse(fileParts[0]); 
                                string fileSifra = fileParts[1];

                                if (fileRadnikID == radnikID && fileSifra == sifra)
                                {
                                    RadnikID = radnikID;
                                    isSifraValid = true;
                                    break; 
                                }
                            }
                        }

                        if (isSifraValid && RadnikID!=1)
                        {
                            RadnikMain radnik = new RadnikMain(StanicaID,RadnikID);
                            radnik.Show(); 
                            this.Close(); 
                        }
                        else if (isSifraValid && RadnikID == 1)
                        {
                            //ADMIN
                            AdminMain admin = new AdminMain(StanicaID, RadnikID);
                            admin.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Pogrešna šifra!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error); 
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Greška pri čitanju fajla: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Nevalidan format za RadnikID!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Morate odabrati radnika!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        private void Init()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT StanicaID, NazivStanice FROM autobuskastanica"; 
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ComboBoxItem item = new ComboBoxItem();
                        item.Content = reader["NazivStanice"].ToString();  
                        item.Tag = reader["StanicaID"];  

                        StaniceComboBox.Items.Add(item);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri povezivanju s bazom: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StaniceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StaniceComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                StanicaID = (int)selectedItem.Tag;
                PopuniRadnike();
            }
        }

        private void PopuniRadnike()
        {
            RadniciComboBox.Items.Clear(); 

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Ime, Prezime, RadnikID FROM radnik WHERE StanicaID = @StanicaID AND Status=1"; 
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@StanicaID", StanicaID);

                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string imePrezime = reader["Ime"].ToString() + " " + reader["Prezime"].ToString();
                        int radnikID = Convert.ToInt32(reader["RadnikID"]); 

                        
                        ComboBoxItem item = new ComboBoxItem();
                        item.Content = imePrezime + " - " + radnikID.ToString(); 
                        item.Tag = radnikID; 

                        
                        RadniciComboBox.Items.Add(item);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri preuzimanju radnika: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}