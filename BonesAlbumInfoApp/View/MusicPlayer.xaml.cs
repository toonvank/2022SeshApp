using Android.Media;
using AndroidX.Lifecycle;
using BonesAlbumInfoApp.ViewModel;
using CommunityToolkit.Mvvm.Input;
using IF.Lastfm.Core;
using IF.Lastfm.Core.Api;
using Plugin.LocalNotification;
using static Android.Gms.Common.Apis.Api;
using AndroidApp = Android.App.Application;
namespace BonesAlbumInfoApp;

public partial class MusicPlayer : ContentPage
{
    public MusicPlayer(MusicPlayerViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        stubbornLabel.Text = "Please select an album from the list";
        loginOnStartup();
    }

    private void albumSelection_Clicked(object sender, EventArgs e)
    {
        Button button = sender as Button;
        GetAlbumTracksCommand.Command.Execute(button.Text);
        scrollTracks.IsVisible = true;
        scrollTracks.ScrollToAsync(0, 0, true);
    }

    private void PlaySongCommand_Clicked(object sender, EventArgs e)
    {
        Button button = sender as Button;
        PlaySongCommand.Command.Execute(button.Text);
        Controls.IsVisible = true;

        //automatisch dichtklappen
        img.HeightRequest = 250;
        Controls.Margin = new Thickness(0, 100, 0, 0);
        scrollTracks.IsVisible = false;
        albumCol.IsVisible = false;
    }

    private void collapseAlbums_Clicked(object sender, EventArgs e)
    {
        if (albumCol.IsVisible == true)
        {
            albumCol.IsVisible = false;
            img.HeightRequest = 250;
        }
        else
        {
            albumCol.IsVisible = true;
            img.HeightRequest = 150;
        }
        if (albumCol.IsVisible == false && scrollTracks.IsVisible == false)
        {
            Controls.Margin = new Thickness(0, 100, 0, 0);
        }
        else
        {
            Controls.Margin = new Thickness(0, 0, 0, 0);
        }
    }

    private void collapseTracks_Clicked(object sender, EventArgs e)
    {
        if (scrollTracks.IsVisible == true)
        {
            scrollTracks.IsVisible = false;
            img.HeightRequest = 250;
        }
        else
        {
            scrollTracks.IsVisible = true;
            img.HeightRequest = 150;
        }
    }
    bool loggedIn = false;
    private async void btnSave_Clicked(object sender, EventArgs e)
    {
        if (username.Text!=null && password.Text != null)
        {
            string apikey = "64d5c7295b78ec1719b3eb7e664231ee";
            string apisecret = "e591faf7794d5a5f91fc5a5009a36002";
            var client = new LastfmClient(apikey, apisecret);
            Preferences.Default.Set("username", username.Text);
            Preferences.Default.Set("password", password.Text);
            var response = await client.Auth.GetSessionTokenAsync(username.Text, password.Text);
            if (response.Success == true)
            {
                username.Text = String.Empty;
                password.Text = String.Empty;
                loginLayout.IsVisible = false;
                loggedIn = true;
                lastFmCommand.Command.Execute(client);
                lastFm.Source = "fmlog.png";
            }
            else
            {
                await DisplayAlert("Alert", "Could not log in. Incorrect username/password", "OK");
                username.Text = String.Empty;
                password.Text = String.Empty;
                loggedIn = false;
            }
        }
        else
        {
            await DisplayAlert("Alert", "Please fill out everything", "OK");
        }
    }

    private void lastFm_Clicked(object sender, EventArgs e)
    {
        loginLayout.IsVisible = true;
        if (loggedIn == true)
        {
            itemContent.IsVisible = false;
            logout.IsVisible = true;
        }
        else
        {
            itemContent.IsVisible = true;
            logout.IsVisible = false;
        }
    }

    private void close_Clicked(object sender, EventArgs e)
    {
        loginLayout.IsVisible = false;
    }
    private async void loginOnStartup()
    {
        string apikey = "64d5c7295b78ec1719b3eb7e664231ee";
        string apisecret = "e591faf7794d5a5f91fc5a5009a36002";
        var client = new LastfmClient(apikey, apisecret);
        string username = Preferences.Default.Get("username", "Unknown");
        string password = Preferences.Default.Get("password", "Unknown");
        if (username != "Unknown")
        {
            var response = await client.Auth.GetSessionTokenAsync(username, password);
            if (response.Success == true)
            {
                loginLayout.IsVisible = false;
                loggedIn = true;
                lastFmCommand.Command.Execute(client);
                lastFm.Source = "fmlog.png";
            }
        }
    }

    private void logoutButton_Clicked(object sender, EventArgs e)
    {
        if (loggedIn == true)
        {
            itemContent.IsVisible = true;
            logout.IsVisible = false;
            Preferences.Default.Clear();
            loggedIn = false;
            lastFm.Source = "fm.png";
            logoutCommand.Command.Execute("");
        }
    }

    private async void loveBtn_Clicked(object sender, EventArgs e)
    {
        if (loggedIn == true)
        {
            ImageButton send = (ImageButton)sender;
            string noExt = string.Empty;
            try
            {
                noExt = send.Source.ToString().Remove(0, send.Source.ToString().IndexOf(" ") + 1).Remove(send.Source.ToString().Remove(0, send.Source.ToString().IndexOf(" ") + 1).IndexOf("."));

            }
            catch (Exception)
            {
                noExt = send.Source.ToString().Remove(0,send.Source.ToString().IndexOf(" ")+1);
            }
            HeartCommand.Command.Execute(noExt);
            if (noExt == "heart")
            {
                loveBtn.Source = "filledheart";
            }
            else if (noExt == "filledheart")
            {
                loveBtn.Source = "heart";
            }

        }
        else
        {
            loginLayout.IsVisible = true;
        }
    }

    private void fetch_Clicked(object sender, EventArgs e)
    {
        ReloadItemsCommand.Command.Execute("");
    }
}