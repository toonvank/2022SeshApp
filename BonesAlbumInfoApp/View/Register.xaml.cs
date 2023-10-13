using BonesAlbumInfoApp.Model;
using Firebase.Auth;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace BonesAlbumInfoApp.View;
[QueryProperty("Email", "userEmail")]
public partial class Register : ContentPage
{
    IFirebaseConfig fcon = new FireSharp.Config.FirebaseConfig()
    {
        AuthSecret = "O1itSeLoiYkjfMjzk3iv9q4wK99k70VkJ0COQPMZ",
        BasePath = "https://bonesinfoapp-default-rtdb.europe-west1.firebasedatabase.app/"
    };
    IFirebaseClient client;
    int i = 0;
    private string recievedEmail;
    public string Email
    {
        set
        {
            recievedEmail = value;
            email.Text = value;
        }
    }
    public Register()
	{
		InitializeComponent();
        client = new FireSharp.FirebaseClient(fcon);
    }

    private async void btnRegister_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (username.Text == null)
            {
                await DisplayAlert("Failed register", "Reason: no username", "OK");
            }
            else
            {
                var authProvider = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(apiKey: "AIzaSyAjUEQRl2QC5-Zt0Skxk9-53rKHtw7dxkM"));
                var register = await authProvider.CreateUserWithEmailAndPasswordAsync(email.Text, password.Text, username.Text);
                var login = await authProvider.SignInWithEmailAndPasswordAsync(email.Text, password.Text);
                string token = login.FirebaseToken;
                Preferences.Default.Set("token", token);
                Preferences.Default.Set("userid", login.User.LocalId);
                DbUser user = new DbUser() { UID = login.User.LocalId, fullName = fullname.Text };
                var setter = client.Set("Users/" + login.User.LocalId, user);
                await Shell.Current.GoToAsync(nameof(MyAccount));
            }
        }
        catch (Exception ex)
        {
            string baseException = ex.Message.Substring(ex.Message.IndexOf("\n\n"));
            string shortExeption = baseException.Remove(0, baseException.IndexOf("\n\n") + 2);
            await DisplayAlert("Failed register", shortExeption, "OK");
        }
    }
}