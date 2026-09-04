using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int auto_Manual, songToAdd = 0;
        private string path, currentSong = "";
        public bool flag = false;

        Library library = new Library();

        private List<Song> songs = new List<Song>();

        //********************* Funciones de botones *********************//
        public MainWindow()
        {
            InitializeComponent();
        }

        /*
        *
        *
        */

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        /*
        *
        *
        */

        private void Minimize(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        /*
        *
        *
        */

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /*
        *
        *
        */

        private void HideAlertBtn(object sender, RoutedEventArgs e)
        {
            HideAlert();
        }

        /*
        *
        *
        */

        private void HideAlert()
        {
            Alerta_Canciones.Visibility = Visibility.Hidden;
        }

        /*
        *
        *
        */

        private void addSongs(object sender, RoutedEventArgs e)
        {
            HideAlert();

            BtnAddSong();

            addSongsAUX();
        }

        /*
        *
        *
        */

        private void declineSongs(object sender, RoutedEventArgs e)
        {
            HideAlert();

            addSongsAUX();
        }

        /*
        *
        *
        */

        private void BtnSeleccionarCarpeta(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialogCarpeta = new OpenFolderDialog();

            dialogCarpeta.Title = "Seleccione la Carpeta de destino";

            dialogCarpeta.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);

            if (dialogCarpeta.ShowDialog() == true)
            {
                path = dialogCarpeta.FolderName;

                addSongsAUX();

                Canciones_Temporal.TextAlignment = TextAlignment.Left;
                Path_Button.Content = "Reload Path";
            }
        }

        /*
        *
        *
        */

        private void toggleButtons(object sender, RoutedEventArgs e)
        {
            switch (auto_Manual)
            {
                case 0:

                    if (Auto_Toggle_Button.IsChecked == true) auto_Manual = 1;

                    if (Manual_Toggle_Button.IsChecked == true) auto_Manual = 2;

                    break;

                case 1:

                    if (Manual_Toggle_Button.IsChecked == true)
                    {
                        Auto_Toggle_Button.IsChecked = false;

                        auto_Manual = 2;
                    }
                    else if (Auto_Toggle_Button.IsChecked == false) auto_Manual = 0;

                    break;

                case 2:

                    if (Auto_Toggle_Button.IsChecked == true)
                    {
                        Manual_Toggle_Button.IsChecked = false;

                        auto_Manual = 1;
                    }
                    else if (Manual_Toggle_Button.IsChecked == false) auto_Manual = 0;

                    break;
            }

            if (auto_Manual == 1 || auto_Manual == 2) Path_Button.IsEnabled = true;
            else Path_Button.IsEnabled = false;
        }

        //********************* Funciones de botones *********************//



        //********************* Funciones Backend *********************//

        public void addSong(string title, string album, string artist, string duration)
        {
            Song songToAdd = new Song();

            songToAdd.title = title;
            songToAdd.album = album;
            songToAdd.artist = artist;
            songToAdd.duration = duration;

            songs.Add(songToAdd);
        }

        /*
        *
        *
        */

        private void BtnAddSong()
        {
            //ADD SONGS ALGORITHM

            string name = songName(currentSong);

            addSong(name, "unnkown", "unnkown", "unnkown");
        }

        /*
        *
        *
        */

        private void addSongsAUX()
        {
            // Extensiones de audio

            string[] extensionesAudio = { ".mp3", ".wav", ".m4a", ".wma", ".flac", ".ogg" };

            Music_Path.Text = "Path: " + path;

            string[] fileEntries = Directory.GetFiles(path);

            //Cuando llega al final del recorrido

            if (songToAdd == fileEntries.Length)
            { 
                songToAdd = 0;

                Canciones_Temporal.Text = "";

                for (int i = 0; i < songs.Count; i++) Canciones_Temporal.Text = Canciones_Temporal.Text + "\n" + songs[i].title;

                return;
            }

            //Agregando Canciones

            if (songToAdd < fileEntries.Length)
            {
                string ext = Path.GetExtension(fileEntries[songToAdd]).ToLower();
                
                SongTitle.Text = songName(fileEntries[songToAdd]);

                currentSong = fileEntries[songToAdd];

                Alerta_Canciones.Visibility = Visibility.Visible;

                songToAdd++;
            }
        }

        /*
        *
        *
        */

        public string songName(string song)
        {
            string songNameDef = "";

            foreach (char car in song)
            {
                if (flag && (car == '/' || car == '\\')) flag = false;

                if (flag) songNameDef += car;
                else songNameDef = "";

                if (car == '/' || car == '\\') flag = true;
            }
            flag = false;

            return songNameDef;
        }

        //********************* Funciones Backend *********************//
    }
}
