using BonesAlbumInfoApp.Model;
using Firebase.Auth;
using FireSharp.Interfaces;
using Org.Json;
using static Android.Content.ClipData;

namespace BonesAlbumInfoApp.View;

public partial class MyAccount : ContentPage
{
    //database
    IFirebaseConfig fcon = new FireSharp.Config.FirebaseConfig()
    {
        AuthSecret = "O1itSeLoiYkjfMjzk3iv9q4wK99k70VkJ0COQPMZ",
        BasePath = "https://bonesinfoapp-default-rtdb.europe-west1.firebasedatabase.app/"
    };
    //IFirebaseClient client;
    IFirebaseClient client;
    public MyAccount()
	{
		InitializeComponent();
        client = new FireSharp.FirebaseClient(fcon);
        _ = ClickEmail();
    }
    public async Task ClickEmail()
    {
        var authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey: "AIzaSyAjUEQRl2QC5-Zt0Skxk9-53rKHtw7dxkM"));
        string token = Preferences.Default.Get("token", "Unknown");
        string UID = Preferences.Default.Get("userid", "Unknown");
        var clientDb = client.Get("Users/" + UID);
        DbUser user = clientDb.ResultAs<DbUser>();
        var getUser = await authProvider.GetUserAsync(token);
        email.Text = getUser.Email;
        welcome.Text = $"Welcome {user.fullName}";
    }

    private async void btnLogout_Clicked(object sender, EventArgs e)
    {
        Preferences.Default.Clear();
        await Shell.Current.GoToAsync($"//{nameof(Account)}");
    }

    private async void btnDelete_Clicked(object sender, EventArgs e)
    {
        var authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey: "AIzaSyAjUEQRl2QC5-Zt0Skxk9-53rKHtw7dxkM"));
        string token = Preferences.Default.Get("token", "Unknown");
        var deleteUser = authProvider.DeleteUserAsync(token);
        string UID = Preferences.Default.Get("userid", "Unknown");
        var setter = client.Delete("Users/" + UID);
        Preferences.Default.Clear();
        await Shell.Current.GoToAsync($"//{nameof(Account)}");
    }

    private async void btnUpdate_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(UpdateAccount));
    }

    private void btnPrevious_Clicked(object sender, EventArgs e)
    {

    }

    private void btnPrevious_Clicked_1(object sender, EventArgs e)
    {

    }

    private void btnPrevious_Clicked_2(object sender, EventArgs e)
    {

    }

    private void btnPrevious_Clicked_3(object sender, EventArgs e)
    {

    }

    private void btnPrevious_Clicked_4(object sender, EventArgs e)
    {

    }

    private void btnPrevious_Clicked_5(object sender, EventArgs e)
    {

    }

    private void Button_Clicked(object sender, EventArgs e)
    {

    }
}