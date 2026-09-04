using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using TagLib.Mpeg;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int auto_Manual, songToAdd = 0;
        private string path, currentSong, duration = "";
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

        /// Para mover la pestaña

        private void Border_MouseDown(object sender, MouseButtonEventArgs e) 
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        /*
        *
        *
        */

        /// Para minimizar la pestaña

        private void Minimize(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        /*
        *
        *
        */

        /// Para cerrar la pestaña

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /*
        *
        *
        */

        /// Funcion auxiliar para esconder la alerta de agregar cancion con un boton de la interfaz

        private void HideAlertBtn(object sender, RoutedEventArgs e)
        {
            HideAlert();
        }

        /*
        *
        *
        */

        /// Funcion originial para esconder la alerta de agregar cancion

        private void HideAlert()
        {
            Alerta_Canciones.Visibility = Visibility.Hidden;
        }

        /*
        *
        *
        */

        /// Funcion que maneja el agregado de canciones 

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

        /// En caso de no querer agregar la cancion sugerida
        /// Pasa a la siguiente

        private void declineSongs(object sender, RoutedEventArgs e)
        {
            HideAlert();

            addSongsAUX();
        }

        /*
        *
        *
        */

        /// Abre una ventana para seleccionar un directorio de canciones

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

        /// Funcion secundaria que maneja el modo en el cual se relodea el directorio (Manual o Automatico)
        /// El manual te permite elegir una por una las canciones que se quiere agregar.
        /// El automatico simplemente agrega todos los archivos

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

        /// Agrega las funciones a una lista dinamica 

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

        /// Maneja el agregado de las funciones con el boton YES de la interfaz

        private void BtnAddSong()
        {
            //ADD SONGS ALGORITHM

            string name = songName(currentSong);

            addSong(name, "unnkown", "unnkown", duration);
        }

        /*
        *
        *
        */

        /// No se porque le puse AUX xd
        /// Pero hace todo el recorrido de como se deberian agregar las canciones dependiendo el modo de agregado que se eligio
        /// (Manual o automatico)

        private void addSongsAUX()
        {
            // Extensiones de audio

            string[] extensionesAudio = { ".mp3", ".wav", ".m4a", ".wma", ".flac", ".ogg" };

            Music_Path.Text = "Path: " + path;

            string[] fileEntries = Directory.GetFiles(path);

            //Cuando llega al final del recorrido

            switch (auto_Manual)
            {
                case 1: //auto

                    Canciones_Temporal.Text = "";

                    for (int i = 0; i < fileEntries.Length; i++)
                    {
                        Canciones_Temporal.Text = Canciones_Temporal.Text + "\n" + songName(fileEntries[i]);

                        //mas tarde
                    }

                    break;

                case 2: //manual

                    /// Cuando termina de procesar todas las canciones manualmente
                    /// Por ahora solo le agrega el titulo al textbox del medio

                    if (songToAdd == fileEntries.Length)
                    {
                        songToAdd = 0; // resetea el index de las canciones

                        Canciones_Temporal.Text = ""; // Borra el texto que dice q no hay canciones

                        for (int i = 0; i < songs.Count; i++) Canciones_Temporal.Text = Canciones_Temporal.Text + "\n" + songs[i].title; // Escribe el texto de las canciones en el texbox

                        return;
                    }

                    /// Aca procesa las canciones agregadas manualmente
                    /// TabLib.File audio es crea una variable que almacena la cancion que se esta procesando en el momento
                    /// y por ahora solo lee la duracion

                    if (songToAdd < fileEntries.Length)
                    {
                        TagLib.File audio = TagLib.File.Create(fileEntries[songToAdd]); // Aca

                        duration = audio.Properties.Duration.ToString(); // Lee la duracion

                        SongTitle.Text = songName(fileEntries[songToAdd]); // Aca el titulo

                        currentSong = fileEntries[songToAdd]; // Almacena path de la cancion para despues procesarlo y guardar el nombre en una variable

                        Alerta_Canciones.Visibility = Visibility.Visible; // Activa la alerta visual 

                        songToAdd++; // Va a la siguiente cancion
                    }

                    break;
            }
        }

        /*
        *
        *
        */

        /// Algoritmo simple que toma un path y devuelve el nombre del archivo
        /// Se encarga de delvolver el nombre de la cancion

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
