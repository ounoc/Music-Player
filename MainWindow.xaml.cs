using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

        private void a()
        {
        
        }

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

                //Canciones_Temporal.TextAlignment = TextAlignment.Left;
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

                    //Canciones_Temporal.Text = "";

                    for (int i = 0; i < fileEntries.Length; i++)
                    {
                        SongTitle.Text = songName(fileEntries[i]); // Aca el titulo

                        currentSong = fileEntries[i]; // Almacena path de la cancion para despues procesarlo y guardar el nombre en una variable

                        TagLib.File audio = TagLib.File.Create(fileEntries[i]); // Aca

                        TimeSpan dur = audio.Properties.Duration;
                        int totalMinutes = (int)dur.TotalMinutes;
                        duration = $"{totalMinutes:D2}:{dur.Seconds:D2}";

                        BtnAddSong();

                        addVisualSongs(songs[i].title, songs[i].album, songs[i].artist, songs[i].duration, i + 1);
                    }

                    break;

                case 2: //manual

                    /// Cuando termina de procesar todas las canciones manualmente
                    /// Por ahora solo le agrega el titulo al textbox del medio

                    if (songToAdd == fileEntries.Length)
                    {
                        songToAdd = 0; // resetea el index de las canciones

                        for (int i = 0; i < songs.Count; i++) // Escribe el texto de las canciones en el texbox
                        {
                            addVisualSongs(songs[i].title, songs[i].album, songs[i].artist, songs[i].duration, i + 1);
                        }

                        return;
                    }

                    /// Aca procesa las canciones agregadas manualmente
                    /// TabLib.File audio es crea una variable que almacena la cancion que se esta procesando en el momento
                    /// y por ahora solo lee la duracion

                    if (songToAdd < fileEntries.Length)
                    {
                        try
                        {
                            TagLib.File audio = TagLib.File.Create(fileEntries[songToAdd]); // Aca

                            TimeSpan dur = audio.Properties.Duration;

                            int totalMinutes = (int)dur.TotalMinutes;

                            duration = $"{totalMinutes:D2}:{dur.Seconds:D2}";
                        }
                        catch (Exception ex) { MessageBox.Show($"{ex.Message}, Error"); }

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

        /*
        *
        *
        */

        /// Funcion para crear o asignar visualmente las canciones a la pantalla principal
        /// nada eso

        private void addVisualSongs(string name, string album, string artist, string duration, int id)
        {
            var converter = new BrushConverter();
            var font = new FontFamily("Nanitos");

            StackPanel cancion = new StackPanel
            {
                Background = (Brush)converter.ConvertFromString("#33FFFFFF"),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(10),
                Width = 1060,
                Height = 50
            };

            TextBlock cancion_Numero = new TextBlock
            {
                Foreground = (Brush)converter.ConvertFromString("White"),
                Margin = new Thickness(12.5, 0, 0, 0),
                TextAlignment = TextAlignment.Center,
                FontFamily = FontFamily,
                FontSize = 20,
                Height = 30,
                Width = 55,
                Text = id.ToString()
            };

            TextBlock cancion_Title = new TextBlock
            {
                Foreground = (Brush)converter.ConvertFromString("White"),
                Margin = new Thickness(30, 0, 0, -10),
                TextAlignment = TextAlignment.Left,
                FontFamily = FontFamily,
                FontSize = 20,
                Height = 30,
                Width = 385,
                Text = name
            };

            TextBlock cancion_Album = new TextBlock
            {
                Foreground = (Brush)converter.ConvertFromString("White"),
                Margin = new Thickness(40, 0, 0, -10),
                TextAlignment = TextAlignment.Left,
                FontFamily = FontFamily,
                FontSize = 20,
                Height = 30,
                Width = 185,
                Text = "album " + id
            };

            TextBlock cancion_Artist = new TextBlock
            {
                Foreground = (Brush)converter.ConvertFromString("White"),
                Margin = new Thickness(40, 0, 0, -10),
                TextAlignment = TextAlignment.Left,
                FontFamily = FontFamily,
                FontSize = 20,
                Height = 30,
                Width = 165,
                Text = "artist" + id
            };

            TextBlock cancion_Duration = new TextBlock
            {
                Foreground = (Brush)converter.ConvertFromString("White"),
                Margin = new Thickness(40, 0, 0, -10),
                TextAlignment = TextAlignment.Center,
                FontFamily = FontFamily,
                FontSize = 20,
                Height = 30,
                Width = 85,
                Text = duration 
            };

            cancion.Children.Add(cancion_Numero);
            cancion.Children.Add(cancion_Title);
            cancion.Children.Add(cancion_Album);
            cancion.Children.Add(cancion_Artist);
            cancion.Children.Add(cancion_Duration);

            Canciones_Temporal.Children.Add(cancion);
        }

        //********************* Funciones Backend *********************//
    }
}

// ********************* Como usar el pinche taglib sharp ********************* //

// var tfile = TagLib.File.Create(@"C:\My picture.jpg");
// string title = tfile.Tag.Title;
// var tag = tfile.Tag as TagLib.Image.CombinedImageTag;
// ? snapshot = tag.DateTime;
// Console.WriteLine("Title: {0}, snapshot taken on {1}", title, snapshot);

//change title in the file
// .Tag.Title = "my new title";
// tfile.Save();
