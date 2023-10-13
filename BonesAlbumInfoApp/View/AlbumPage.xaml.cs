using BonesAlbumInfoApp.Class;
using BonesAlbumInfoApp.Model;
using BonesAlbumInfoApp.PassObjects;
using BonesAlbumInfoApp.ViewModel;
using CsvHelper;
using System.Globalization;
using System.Reflection;
using Button = Microsoft.Maui.Controls.Button;
namespace BonesAlbumInfoApp;

public partial class MainPage : ContentPage
{
    Methods m = new Methods();
    public MainPage(AlbumPageViewModel albumPageViewModel)
	{
        InitializeComponent();
        BindingContext = albumPageViewModel;
    }
    private void Button_Clicked(object sender, EventArgs e)
    {
        SearchBar searchBar = (SearchBar)sender;
        searchCommand.Command.Execute(searchBar);
    }
    private async void btnDownload_Clicked(object sender, EventArgs e)
    {
        await progressBar.ProgressTo(0, 1, Easing.CubicOut);
        progressBar.IsVisible = true;
        await progressBar.ProgressTo(0.05, 300, Easing.CubicOut);
        Button bs = (Button)sender;
        Album album = (Album)bs.BindingContext;
        AlbumInfoObjectPass.passedAlbum = album;
        bs.BackgroundColor = Colors.Orange;
        await progressBar.ProgressTo(0.10, 500, Easing.Linear);
        progressBar.ProgressTo(0.75, 95000, Easing.CubicOut);
        await Task.Run(() => m.DownloadProcess());
        await progressBar.ProgressTo(1, 400, Easing.Linear);
        bs.BackgroundColor = Colors.Green;
        bs.TextColor = Colors.White;
        bs.Text = "✓";
        progressBar.IsVisible = false;
    }
}

