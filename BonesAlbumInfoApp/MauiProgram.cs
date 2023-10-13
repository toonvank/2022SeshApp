using BonesAlbumInfoApp.View;
using BonesAlbumInfoApp.ViewModel;
using CommunityToolkit.Maui;
using Plugin.LocalNotification;
using TeamSeshMerchAppMaui;

namespace BonesAlbumInfoApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseLocalNotification(config =>
            {
                config.AddCategory(new NotificationCategory(NotificationCategoryType.Status)
                {
                    ActionList = new HashSet<NotificationAction>(new List<NotificationAction>()
                                {
                                    new NotificationAction(100)
                                    {
                                            Title = "Previous",
                                    },
                                    new NotificationAction(101)
                                    {
                                            Title = "Pause/start",
                                    },
                                    new NotificationAction(102)
                                    {
                                            Title = "Forward",
                                    }
                                })
                });
            })
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			}).UseMauiCommunityToolkit();
        builder.Services.AddTransient<MusicPlayerViewModel>();
        builder.Services.AddTransient<MusicPlayer>();
        builder.Services.AddTransient<AlbumPageViewModel>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<AlbumInfoViewModel>();
        builder.Services.AddTransient<AlbumInfo>();
        builder.Services.AddTransient<Merch>();
        builder.Services.AddTransient<MerchViewModel>();
        builder.Services.AddTransient<Account>();
        builder.Services.AddTransient<Register>();
        builder.Services.AddTransient<MyAccount>();
        builder.Services.AddTransient<UpdateAccount>();
        builder.Services.AddTransient<myFavoritesViewModel>();
        builder.Services.AddTransient<myFavorites>();
        builder.UseMauiApp<App>().UseMauiCommunityToolkit();
        return builder.Build();
    }
}
