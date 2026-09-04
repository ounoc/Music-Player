namespace MusicPlayer;

public class Song
{
    public string? title { get; set; }
    public string? album { get; set; }
    public string? artist { get; set; }
    public string? duration { get; set; }

    public void print(string msg)
    {
        Console.WriteLine(msg);
    }   

    /*
    *
    *
    */

    public void getSongInfo()
    {
        print( $"\n {title} - {album} - {artist} - {duration}");
    }
}
