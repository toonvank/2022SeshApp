using BonesAlbumInfoApp.Model;
using Firebase.Auth;
using FireSharp.Response;

namespace BonesAlbumInfoApp.View;

public partial class Account : ContentPage
{
    public Account()
	{
		InitializeComponent();
        _ = LoginCheck();
    }

    private async void btnRegister_Clicked(object sender, EventArgs e)
    {
        if (email.Text == null)
        {
            await Shell.Current.GoToAsync(nameof(Register));
        }
        else
        {
            await Shell.Current.GoToAsync(nameof(Register), true, new Dictionary<string, object>
            {
                {"userEmail", email.Text}
            });
        }
        email.Text = string.Empty;
        password.Text = string.Empty;
    }

    private async void btnLogin_Clicked(object sender, EventArgs e)
    {
        try
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey: "AIzaSyAjUEQRl2QC5-Zt0Skxk9-53rKHtw7dxkM"));
            var auth = await authProvider.SignInWithEmailAndPasswordAsync(email.Text, password.Text);
            string token = auth.FirebaseToken;
            Preferences.Default.Set("token", token);
            Preferences.Default.Set("userid", auth.User.LocalId);
            await Shell.Current.GoToAsync(nameof(MyAccount));
        }
        catch (Exception ex)
        {
            string baseException = ex.Message.Substring(ex.Message.IndexOf("\n\n"));
            string shortExeption = baseException.Remove(0, baseException.IndexOf("\n\n")+2);
            await DisplayAlert("Failed login", shortExeption, "OK");
        }
        email.Text = string.Empty;
        password.Text = string.Empty;
    }
    private async Task LoginCheck()
    {
        string token = Preferences.Default.Get("token", "Unknown");
        if (token != "Unknown")
        {
            await Shell.Current.GoToAsync(nameof(MyAccount));
        }
    }
}