using BonesAlbumInfoApp.Class;
using BonesAlbumInfoApp.PassObjects;
using BonesAlbumInfoApp.View;
using BonesAlbumInfoApp.ViewModel;
using TeamSeshMerchAppMaui;

namespace BonesAlbumInfoApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(AlbumInfo), typeof(AlbumInfo));
        Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage));
        Routing.RegisterRoute(nameof(myFavorites), typeof(myFavorites));
        Routing.RegisterRoute(nameof(Register), typeof(Register));
        Routing.RegisterRoute(nameof(Account), typeof(Account));
        Routing.RegisterRoute(nameof(MyAccount), typeof(MyAccount));
        Routing.RegisterRoute(nameof(UpdateAccount), typeof(UpdateAccount));
        Routing.RegisterRoute(nameof(MusicPlayer), typeof(MusicPlayer));

        Methods m = new Methods();
        DataPass.currencyIndex = Preferences.Default.Get("currency", 2);
        _ = m.LoadMerch();
        
    }
}
