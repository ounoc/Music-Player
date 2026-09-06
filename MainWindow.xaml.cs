using System.Windows;
using System.Windows.Input;

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
