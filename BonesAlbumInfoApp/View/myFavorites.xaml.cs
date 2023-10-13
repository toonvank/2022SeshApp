namespace TeamSeshMerchAppMaui;

using BonesAlbumInfoApp.Model;
using BonesAlbumInfoApp.PassObjects;
using BonesAlbumInfoApp.ViewModel;
using Firebase.Auth;
using FireSharp.Interfaces;
using AndroidApp = Android.App.Application;

public partial class myFavorites : ContentPage
{
    IFirebaseConfig fcon = new FireSharp.Config.FirebaseConfig()
    {
        AuthSecret = "O1itSeLoiYkjfMjzk3iv9q4wK99k70VkJ0COQPMZ",
        BasePath = "https://bonesinfoapp-default-rtdb.europe-west1.firebasedatabase.app/"
    };
    IFirebaseClient client;
    string UID = Preferences.Default.Get("userid", "Unknown");
    public myFavorites(myFavoritesViewModel mf)
	{
		InitializeComponent();
        BindingContext = mf;
        client = new FireSharp.FirebaseClient(fcon);
    }

	private async void tapFrame_Tapped(object sender, EventArgs e)
	{
        Frame bs = (Frame)sender;
        rssChannelItem a = (rssChannelItem)bs.BindingContext;
        try
        {
            Uri uri = new Uri(a.linkField);
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Something went wrong opening link", ex.Message, "OK");
        }
    }
    private void Button_Clicked(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        Grid g = (Grid)btn.Parent;
        Frame f = (Frame)g.Children[0];
        rssChannelItem a = (rssChannelItem)f.BindingContext;
        var authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey: "AIzaSyAjUEQRl2QC5-Zt0Skxk9-53rKHtw7dxkM"));
        var setter = client.Get($"Users/{UID}/Likes/");
        string userLikes = setter.Body.Substring(1, setter.Body.Length - 2);
        string[] userLikesArray = userLikes.Split(',');
        List<int> userLikesList = new List<int>();
        if (userLikesArray.Contains<string>("null"))
        {
            // put in array where not equal to "null" and then use that array
            foreach (string item in userLikesArray)
            {
                if (item != "null")
                {
                    userLikesList.Add(int.Parse(item));
                }
            }
        }
        else
        {
            foreach (string item in userLikesArray)
            {
                userLikesList.Add(int.Parse(item));
            }
        }
        userLikesList.Remove(a.idField);
        var remove = client.Delete($"Users/{UID}/Likes/");
        var setterr = client.Set($"Users/{UID}/" + "Likes", userLikesList);
        RefreshDeleteCommand.Command.Execute("");
    }
}