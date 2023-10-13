using BonesAlbumInfoApp.Class;
using BonesAlbumInfoApp.PassObjects;
using BonesAlbumInfoApp.ViewModel;
using ICSharpCode.SharpZipLib.Tar;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using System.IO;
namespace BonesAlbumInfoApp;
using AndroidApp = Android.App.Application;


public partial class AlbumInfo : ContentPage
{
    Methods m = new Methods();
    public AlbumInfo(AlbumInfoViewModel albumInfoViewModel)
	{
		InitializeComponent();
        BindingContext = albumInfoViewModel;
    }
    private async void Button_Clicked(object sender, EventArgs e)
    {
        await progressBar.ProgressTo(0, 1, Easing.CubicOut);
        progressBar.IsVisible = true;
        await progressBar.ProgressTo(0.05, 300, Easing.CubicOut);
        downloadButton.Text = "Download started!";
        downloadButton.BackgroundColor = Colors.Orange;
        await progressBar.ProgressTo(0.10, 500, Easing.Linear);
        progressBar.ProgressTo(0.75, 95000, Easing.CubicOut);
        await Task.Run(() => m.DownloadProcess());
        await progressBar.ProgressTo(1, 400, Easing.Linear);
        downloadButton.Text = "Succesfully downloaded!";
        downloadButton.BackgroundColor = Colors.Green;
        downloadButton.TextColor = Colors.White;
        progressBar.IsVisible = false;
    }

    private async void backButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}