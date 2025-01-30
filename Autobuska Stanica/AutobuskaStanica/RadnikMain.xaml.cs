using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ComboBox = System.Windows.Controls.ComboBox;

namespace AutobuskaStanica
{
    public partial class RadnikMain : Window
    {

        public static int StanicaID, RadnikID;
        public static string Jezik;
        
        ProdajaKarata prodaja;
        public RadnikMain(int a, int b)
        {
            StanicaID = a;
            RadnikID = b;
            prodaja = new ProdajaKarata(StanicaID, RadnikID);
            this.Content = prodaja;
            InitializeComponent();
            ucitajPodesavanja();
        }

        public RadnikMain()
        {
            InitializeComponent();
            prodaja = new ProdajaKarata(StanicaID, RadnikID);
            this.Content = prodaja;
            InitializeComponent();
            ucitajPodesavanja();
        }


        private void ucitajPodesavanja()
        {
            // Putanja do fajla Radnici.txt
            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Radnici.txt");

            // Proveravamo da li fajl postoji
            if (File.Exists(filePath))
            {
                // Čitamo sve linije iz fajla
                string[] lines = File.ReadAllLines(filePath);

                // Prolazimo kroz svaku liniju
                foreach (string line in lines)
                {
                    // Podela linije na delove: RadnikID, Sifra, Tema, Font, Jezik
                    string[] parts = line.Split(' ');

                    if (parts.Length == 5 && parts[0] == RadnikID.ToString())
                    {
                        // Proveravamo da li je tema "Svijetla" ili "Tamna"
                        string tema = parts[2].Trim();
                        string fontNumber = parts[3].Trim(); // Učitavamo broj za font iz fajla
                        string jezik = parts[4].Trim(); // Učitavamo jezik iz fajla
                        Jezik = jezik;
                        // Postavljanje teme (ovo ostaje nepromenjeno)
                        if (tema.Equals("Svijetla", StringComparison.OrdinalIgnoreCase) || tema.Equals("Light", StringComparison.OrdinalIgnoreCase))
                        {
                            SetPrimaryColor(Colors.LightBlue);  // Postavljanje svetle teme
                        }
                        else if (tema.Equals("Tamna", StringComparison.OrdinalIgnoreCase) || tema.Equals("Dark", StringComparison.OrdinalIgnoreCase))
                        {
                            SetPrimaryColor(Colors.DarkBlue);  // Postavljanje tamne teme
                        }
                        else if (tema.Equals("Plava", StringComparison.OrdinalIgnoreCase) || tema.Equals("Blue", StringComparison.OrdinalIgnoreCase))
                        {
                            SetPrimaryColor(Colors.Blue);  // Postavljanje plave teme
                        }

                        // Postavljanje fonta na osnovu broja
                        FontFamily selectedFont = fontNumber switch
                        {
                            "1" => new FontFamily("Segoe UI"),
                            "2" => new FontFamily("Arial"),
                            "3" => new FontFamily("Calibri"),
                            "4" => new FontFamily("Lionel Classic"),
                            _ => new FontFamily("Segoe UI") // Default font
                        };

                        // Postavljamo font u aplikaciji
                        System.Windows.Application.Current.Resources["ApplicationFont"] = selectedFont;

                        // Takođe možemo postaviti font u ComboBox za font (ako postoji)
                        foreach (ComboBoxItem item in comboFont.Items)
                        {
                            if (item.Content.ToString() == selectedFont.Source)
                            {
                                comboFont.SelectedItem = item;
                                break;
                            }
                        }

                        // Postavljanje jezika u aplikaciji
                        ResourceDictionary dictionary = new ResourceDictionary();
                        if (jezik.Equals("Srpski", StringComparison.OrdinalIgnoreCase) || jezik.Equals("Serbian", StringComparison.OrdinalIgnoreCase))
                        {
                            dictionary.Source = new("..\\Strings.srp.xaml", UriKind.Relative);
                        }
                        else if (jezik.Equals("Engleski", StringComparison.OrdinalIgnoreCase) || jezik.Equals("English", StringComparison.OrdinalIgnoreCase))
                        {
                            dictionary.Source = new("..\\Strings.en.xaml", UriKind.Relative);
                        }
                        this.Resources.MergedDictionaries.Add(dictionary);
                        prodaja.Resources.MergedDictionaries.Add(dictionary);


                        // Postavljamo jezik u ComboBox
                        foreach (ComboBoxItem item in comboJezik.Items)
                        {
                            if (item.Content.ToString() == jezik)
                            {
                                comboJezik.SelectedItem = item;
                                break;
                            }
                        }


                        // Postavljanje teme u ComboBox
                        foreach (ComboBoxItem item in comboTema.Items)
                        {
                            if (item.Content.ToString().Equals(tema, StringComparison.OrdinalIgnoreCase))
                            {
                                comboTema.SelectedItem = item;
                                break;
                            }
                        }
                        
                        

                    }
                }
            }
            else
            {
                // Ako fajl ne postoji, prikazujemo poruku
                MessageBox.Show("Fajl Radnici.txt nije pronađen.");
            }
        }

        

        private void ListBoxItem_Selected_2(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Putnici(StanicaID, RadnikID);
        }

        private void odjavaLog(string radnikID)
        {
            // Putanja do fajla Radnici.txt
            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Radnici.txt");

            if (File.Exists(filePath))
            {
                // Čitamo sve linije iz fajla
                string[] lines = File.ReadAllLines(filePath);

                // Selektovane vrednosti iz ComboBox-ova
                string tema = (comboTema.SelectedItem as ComboBoxItem)?.Content.ToString();
                string font = (comboFont.SelectedItem as ComboBoxItem)?.Content.ToString();
                string jezik = (comboJezik.SelectedItem as ComboBoxItem)?.Content.ToString();

                // Prolazimo kroz svaku liniju i tražimo odgovarajući RadnikID
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(' ');
                    if (parts[0] == radnikID)
                    {
                        // Zamenjujemo vrednosti za temu, font i jezik
                        parts[2] = tema;   // Tema (npr. Svijetla, Tamna)
                        parts[3] = GetFontNumber(font); // Font (npr. 1, 2, 3, 4)
                        parts[4] = jezik;  // Jezik (npr. Srpski, Engleski)

                        // Ponovno sastavljamo liniju sa novim vrednostima
                        lines[i] = string.Join(" ", parts);
                        break;
                    }
                }

                // Upisujemo izmenjene linije nazad u fajl
                File.WriteAllLines(filePath, lines);
            }
            else
            {
                // Ako fajl ne postoji, prikazujemo poruku
                MessageBox.Show("Fajl Radnici.txt nije pronađen.");
            }
        }

        // Funkcija za konvertovanje fonta u broj
        private string GetFontNumber(string font)
        {
            return font switch
            {
                "Segoe UI" => "1",
                "Arial" => "2",
                "Calibri" => "3",
                "Lionel Classic" => "4",
                _ => "1"  // Podrazumevani font
            };
        }

        private void ListBoxItem_Selected_4(object sender, RoutedEventArgs e)
        {
            odjavaLog(RadnikID.ToString());
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();

        }

        private void ComboBox_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {
            ResourceDictionary dictionary = new ResourceDictionary();

            // Dohvati izabrani jezik
            var comboBox = sender as ComboBox;
            string jezik = (comboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            Jezik = jezik;

            // Postavi odgovarajući ResourceDictionary
            switch (jezik)
            {
                case "Srpski":
                case "Serbian":
                    dictionary.Source = new("..\\Strings.srp.xaml", UriKind.Relative);
                    break;

                case "Engleski":
                case "English":
                    dictionary.Source = new("..\\Strings.en.xaml", UriKind.Relative);
                    break;

                default:
                    dictionary.Source = new("..\\Strings.srp.xaml", UriKind.Relative);
                    break;
            }

            // Dodaj jezičke resurse globalno
            this.Resources.MergedDictionaries.Clear();
            this.Resources.MergedDictionaries.Add(dictionary);

            // Ažuriraj trenutnu stranicu u MainFrame bez menjanja sadržaja
            if (MainFrame.Content is Page currentPage)
            {
                currentPage.Resources.MergedDictionaries.Clear();
                currentPage.Resources.MergedDictionaries.Add(dictionary);

                // Osveži trenutnu stranicu ako ima posebnu metodu za inicijalizaciju
                switch (currentPage)
                {
                    case ProdajaKarata prodajaKarataPage:
                        prodajaKarataPage = new ProdajaKarata(StanicaID, RadnikID);
                        ApplyLanguageToPage(prodajaKarataPage);
                        MainFrame.Content = prodajaKarataPage;
                        prodajaKarataPage.InitRuteWithFilters("", "");
                        break;

                    case Putnici putniciPage:
                        putniciPage = new Putnici(StanicaID, RadnikID);
                        ApplyLanguageToPage(putniciPage);
                        MainFrame.Content = putniciPage;
                        putniciPage.InitPutnik();
                        break;

                    case IzdateKarte izdateKartePage:
                        izdateKartePage = new IzdateKarte(StanicaID, RadnikID);
                        ApplyLanguageToPage(izdateKartePage);
                        MainFrame.Content = izdateKartePage;
                        izdateKartePage.InitKarteWithAllFilters();
                        break;
                }
            }



            
        }


        private void ApplyLanguageToPage(Page page)
        {
            // Dobij trenutni jezik iz ComboBox-a ili postavi podrazumevani
            string selectedLanguage = Jezik;

            // Kreiraj novi ResourceDictionary za izabrani jezik
            ResourceDictionary dictionary = new ResourceDictionary();
            switch (selectedLanguage)
            {
                case "Srpski":
                case "Serbian":
                    dictionary.Source = new Uri("..\\Strings.srp.xaml", UriKind.Relative);
                    break;
                case "Engleski":
                case "English":
                    dictionary.Source = new Uri("..\\Strings.en.xaml", UriKind.Relative);
                    break;
                default:
                    dictionary.Source = new Uri("..\\Strings.srp.xaml", UriKind.Relative);
                    break;
            }

            // Primeni resurse na stranicu
            page.Resources.MergedDictionaries.Clear();
            page.Resources.MergedDictionaries.Add(dictionary);


        }


        private void ListBoxItem_Selected_5(object sender, RoutedEventArgs e)
        {
            IzdateKarte str = new IzdateKarte(StanicaID, RadnikID);
            ApplyLanguageToPage(str);
            MainFrame.Content = str;
            str.InitKarteWithAllFilters();
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            Pomoc pm = new Pomoc();
            MainFrame.Content = pm;

        }

        private void ListBoxItem_Selected_3(object sender, RoutedEventArgs e)
        {
            Putnici str = new Putnici(StanicaID, RadnikID);
            ApplyLanguageToPage(str);
            MainFrame.Content = str;
            str.InitPutnik();
        }

        private void ListBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {
            // Kreiraj instancu stranice ProdajaKarata
            ProdajaKarata prodajaKarataPage = new ProdajaKarata(StanicaID, RadnikID);
            
            // Primeni trenutni jezik
            ApplyLanguageToPage(prodajaKarataPage);
            
            // Postavi stranicu u MainFrame
            MainFrame.Content = prodajaKarataPage;
            prodajaKarataPage.InitRuteWithFilters("", "");
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;

            // Dohvatanje selektovanog fonta iz ComboBox-a
            string selectedFont = (comboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Mapa za konvertovanje fonta u broj
            string fontNumber = string.Empty;
            switch (selectedFont)
            {
                case "Segoe UI":
                    fontNumber = "1";  // 1 za Segoe UI
                    break;
                case "Arial":
                    fontNumber = "2";  // 2 za Arial
                    break;
                case "Calibri":
                    fontNumber = "3";  // 3 za Calibri
                    break;
                case "Lionel Classic":
                    fontNumber = "4";  // 4 za Lionel Classic
                    break;
                default:
                    fontNumber = "1";  // Default font je Segoe UI
                    break;
            }
            System.Windows.Application.Current.Resources["ApplicationFont"] = new FontFamily(selectedFont);
        }


        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            

            // Dohvatanje selektovane teme
            string selectedTheme = (comboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Na osnovu selektovane teme pozivamo funkciju za promenu boje
            switch (selectedTheme)
            {
                case "Svijetla":
                    SetPrimaryColor(Colors.MediumTurquoise);  // Svetla tema
                    System.Windows.Application.Current.Resources["FrameBackgroundColor"] = new SolidColorBrush(Colors.LightBlue);
                    break;

                case "Light":
                    SetPrimaryColor(Colors.MediumTurquoise);  // Svetla tema
                    System.Windows.Application.Current.Resources["FrameBackgroundColor"] = new SolidColorBrush(Colors.LightBlue);
                    break;

                case "Tamna":
                    SetPrimaryColor(Colors.DarkBlue);  // Tamna tema
                    System.Windows.Application.Current.Resources["FrameBackgroundColor"] = new SolidColorBrush(Colors.Cyan);
                    break;

                case "Dark":
                    SetPrimaryColor(Colors.DarkBlue);  // Tamna tema
                    System.Windows.Application.Current.Resources["FrameBackgroundColor"] = new SolidColorBrush(Colors.Cyan);
                    break;

                case "Plava":
                    SetPrimaryColor(Colors.Blue);   // Plava tema
                    System.Windows.Application.Current.Resources["FrameBackgroundColor"] = new SolidColorBrush(Colors.PaleTurquoise);
                    break;

                case "Blue":
                    SetPrimaryColor(Colors.Blue);   // Plava tema
                    System.Windows.Application.Current.Resources["FrameBackgroundColor"] = new SolidColorBrush(Colors.PaleTurquoise);
                    break;
                default:
                    SetPrimaryColor(Colors.White);  // Podrazumevana svetla tema
                    break;
            }
        }

        

        private void SetPrimaryColor(Color primaryColor)
        {
            // Ažuriraj globalnu boju
            System.Windows.Application.Current.Resources["PrimaryBackgroundColor"] = new SolidColorBrush(primaryColor);

        }

    }
}
