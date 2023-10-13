using BonesAlbumInfoApp.Model;
using Firebase.Auth;
using FireSharp.Interfaces;
using Java.Security;

namespace BonesAlbumInfoApp.View;

public partial class UpdateAccount : ContentPage
{
    IFirebaseConfig fcon = new FireSharp.Config.FirebaseConfig()
    {
        AuthSecret = "O1itSeLoiYkjfMjzk3iv9q4wK99k70VkJ0COQPMZ",
        BasePath = "https://bonesinfoapp-default-rtdb.europe-west1.firebasedatabase.app/"
    };
    IFirebaseClient client;

    public UpdateAccount()
    {
        InitializeComponent();
        client = new FireSharp.FirebaseClient(fcon);
        _ = loadUpdate();
    }

    private async void btnUpdate_Clicked(object sender, EventArgs e)
	{try
        {
            if (username.Text == null)
            {
                await DisplayAlert("Failed update", "Reason: no username", "OK");
            }
            else
            {
                var authProvider = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(apiKey: "AIzaSyAjUEQRl2QC5-Zt0Skxk9-53rKHtw7dxkM"));
                string token = Preferences.Default.Get("token", "Unknown");
                var register = await authProvider.UpdateProfileAsync(token,email.Text, username.Text);
                var login = await authProvider.SignInWithEmailAndPasswordAsync(email.Text, password.Text);
                DbUser updatedUser = new DbUser();
                updatedUser.fullName = fullname.Text;
                var setter = client.Update("Users/" + login.User.LocalId, updatedUser);
                await Shell.Current.GoToAsync(nameof(MyAccount));
            }
        }
        catch (Exception ex)
        {
            string baseException = ex.Message.Substring(ex.Message.IndexOf("\n\n"));
            string shortExeption = baseException.Remove(0, baseException.IndexOf("\n\n") + 2);
            await DisplayAlert("Failed update", shortExeption, "OK");
        }
    }
    public async Task loadUpdate()
    {
        var authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey: "AIzaSyAjUEQRl2QC5-Zt0Skxk9-53rKHtw7dxkM"));
        string token = Preferences.Default.Get("token", "Unknown");
        string UID = Preferences.Default.Get("userid", "Unknown");
        var clientDb = client.Get("Users/" + UID);
        DbUser user = clientDb.ResultAs<DbUser>();
        var getUser = await authProvider.GetUserAsync(token);
        email.Text = getUser.Email;
        username.Text = getUser.DisplayName;
        fullname.Text = user.fullName;
    }
}