using CommunityToolkit.Mvvm.ComponentModel;
using System;
using AndroidApp = Android.App.Application;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Android.Media;
using static Android.Content.ClipData;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static Java.Util.Jar.Attributes;
using static System.Net.Mime.MediaTypeNames;
using AndroidX.ConstraintLayout.Helper.Widget;
using BonesAlbumInfoApp.PassObjects;
using BonesAlbumInfoApp.Model;
using Bumptech.Glide.Load.Engine;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
using Microsoft.Maui.Controls;
using Android.Media.Session;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;
using static Android.Gms.Common.Apis.Api;
using System.Diagnostics;
using static Android.Renderscripts.ScriptGroup;
using Kotlin.Text;

namespace BonesAlbumInfoApp.ViewModel
{
    public partial class MusicPlayerViewModel : INotifyPropertyChanged
    {
        public int albumIndex { get; set; }
        public int songIndex { get; set; }
        public bool isPlaying = true;
        public bool badFolderDir = false;
        public bool coverFromFilesFound = false;
        public string foundCover { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        MediaPlayer player;
        string path = AndroidApp.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
        private string theCurrentSong1;
        private string currentCover1;
        private string startStop = "||";
        private int duration1;
        private int songPosition1;
        private LastfmClient currentClient;
        public string currentCover
        {
            get => currentCover1;
            set
            {
                currentCover1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(currentCover)));
            }
        }
        public string theCurrentSong
        {
            get => theCurrentSong1;
            set
            {
                theCurrentSong1 = value.Remove(0, value.IndexOf("-") + 1);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(theCurrentSong)));
            }
        }
        public string StartStop
        {
            get => startStop; set
            {
                startStop = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartStop)));
            }
        }
        public int duration
        {
            get => duration1; set
            {
                duration1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(duration)));
            }
        }
        public int songPosition
        {
            get => songPosition1; set
            {
                songPosition1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(songPosition)));
            }
        }
        public string lovedTrack
        {
            get => lovedTrack1; set
            {
                lovedTrack1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(lovedTrack)));
            }
        }


        private void Current_NotificationActionTapped(NotificationActionEventArgs e)
        {
            switch (e.ActionId)
            {
                case 100:
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        PreviousSong("");
                    });
                    break;

                case 101:
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        StartSong("");
                    });
                    break;
                case 102:
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        NextSong();
                    });
                    break;
            }
        }

        public void StartPlayer(String filePath)
        {
            try
            {
                if (player == null)
                {
                    player = new MediaPlayer();
                    player.Reset();
                    player.SetDataSource(filePath);
                    player.Prepare();
                    player.Start();
                    player.Completion += delegate
                    {
                        scrobbleSong();
                        NextSong();
                    };
                    updateCover();
                    duration = player.Duration;
                    carouselSwitch(b, System.EventArgs.Empty);
                }
                else
                {
                    player.Reset();
                    player.SetDataSource(filePath);
                    player.Prepare();
                    player.Start();
                    updateCover();
                    duration = player.Duration;
                    carouselSwitch(b, System.EventArgs.Empty);
                }
            }
            catch (Exception)
            {

            }
        }


        Button b = new Button();
        //mainpage
        public ObservableCollection<string> DownloadedAlbums { get; } = new();
        public ObservableCollection<string> SongList { get; } = new();
        public MusicPlayerViewModel()
        {
            //delete downloaded rar files for cleaning
            foreach (var item in Directory.GetFiles(path))
            {
                FileInfo files = new FileInfo(item);
                string ext = files.Extension;
                if (ext == ".rar")
                {
                    File.Delete(item);
                }
            }
            //getdata
            GetDataCommand.Execute("");
            //music control notification delegate
            LocalNotificationCenter.Current.NotificationActionTapped += Current_NotificationActionTapped;
        }
        [RelayCommand]
        public async Task ReloadItems()
        {
            //getdata
            GetDataCommand.Execute("");
            //music control notification delegate
            LocalNotificationCenter.Current.NotificationActionTapped += Current_NotificationActionTapped;
        }
        public void updateEverySecond()
        {
            songPosition = player.CurrentPosition;
        }
        private async void carouselSwitch(object sender, EventArgs e)
        {
            try
            {
                await WaitAndExecute(1000, () => updateEverySecond());
            }
            catch (Exception)
            {
                songPosition = 0;
            }
        }
        private async Task WaitAndExecute(int milisec, Action actionToExecute)
        {
            await Task.Delay(milisec);
            actionToExecute();
            carouselSwitch(b, System.EventArgs.Empty);
        }

        [RelayCommand]
        public async Task GetData()
        {
            try
            {
                DownloadedAlbums.Clear();
                foreach (var item in Directory.GetDirectories(path))
                {
                    string theFaultyDir = String.Empty;
                    badFolderDir = false;
                    foreach (var lowerdir in Directory.GetDirectories(item))
                    {
                        string original = (lowerdir.Remove(0, path.Length + 1)).Substring(0, 3);
                        string compare = (lowerdir.Remove(0, path.Length + 1)).Remove(0, lowerdir.IndexOf("/"));
                        string subdir = compare.Substring(compare.IndexOf("/") + 1).Substring(0, 3);
                        if (original == subdir)
                        {
                            badFolderDir = true;
                            theFaultyDir = lowerdir;
                        }
                    }
                    if (badFolderDir == true)
                    {
                        DownloadedAlbums.Add(theFaultyDir.Remove(0, path.Length + 1).ToUpper());
                    }
                    else if (badFolderDir == false)
                    {
                        DownloadedAlbums.Add(item.Remove(0, path.Length + 1).ToUpper());
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            if (DownloadedAlbums.Count == 0)
            {
                DownloadedAlbums.Clear();
            }
        }
        [RelayCommand]
        public async void GetAlbumTracks(string text)
        {
            try
            {
                SongList.Clear();
                albumIndex = DownloadedAlbums.IndexOf(text);
                string fullpath = $"{path}/{DownloadedAlbums[albumIndex]}/";
                foreach (var item in Directory.GetFiles(fullpath))
                {
                    string songNameNoPath = item.Remove(0, path.Length + 1 + DownloadedAlbums[albumIndex].Length + 1).ToUpper();
                    if (songNameNoPath == "COVER.JPG")
                    {
                        foundCover = item;
                    }
                    if (songNameNoPath != ".DS_STORE" && songNameNoPath != "DESKTOP.INI" && songNameNoPath != "COVER.JPG")
                    {
                        SongList.Add(songNameNoPath);
                    }
                }
                //sort songlist by track number
                List<string> sortedList = SongList.OrderBy(o => o).ToList();
                SongList.Clear();
                foreach (var item in sortedList)
                {
                    
                    SongList.Add(item);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
        }
        public async void PlayingNotification()
        {
            var request = new NotificationRequest
            {
                NotificationId = 1337,
                Title = $"{DownloadedAlbums[albumIndex]}",
                Subtitle = $"{theCurrentSong}",
                Description = "Playing Music",
                BadgeNumber = 42,
                CategoryType = NotificationCategoryType.Status,
                Image = new NotificationImage
                {
                    FilePath = foundCover,
                },
                Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
                {
                }
            };
            await LocalNotificationCenter.Current.Show(request);
        }
        public async void scrobbleSong()
        {
            try
            {
                if (currentClient.Auth.Authenticated)
                {
                    string album = DownloadedAlbums[albumIndex].Remove(0, DownloadedAlbums[albumIndex].IndexOf("-") + 2);
                    //string artist = DownloadedAlbums[albumIndex].Remove(DownloadedAlbums[albumIndex].IndexOf("-")); //artist is always bones on lastfm not two collab
                    string song = SongList[songIndex].Remove(0, SongList[songIndex].IndexOf("-") + 2);
                    string songg = new String(song.Where(c => c != '-' && (c < '0' || c > '9')).ToArray()); //remove extra numbers
                    string songExt = songg.Remove(songg.IndexOf(".")); //remove extension
                    string songSpace = songExt.Remove(0, songExt.IndexOf(" ") + 1);
                    Scrobble scr = new Scrobble("Bones", album, songExt, DateTimeOffset.Now);
                    await currentClient.Scrobbler.ScrobbleAsync(scr);
                }
            }
            catch (Exception)
            {

            }
        }
        [RelayCommand]
        public async void PlaySong(string text)
        {
            try
            {
                songIndex = SongList.IndexOf(text);
                string fullpath = $"{path}/{DownloadedAlbums[albumIndex]}/{SongList[songIndex]}/";
                StartPlayer(fullpath);
                player.Start();
                DisplayCurrentSong(songIndex);
                PlayingNotification();
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Error!", "Nothing is currently playing", "OK");
            }
        }
        [RelayCommand]
        public async void PreviousSong(string text)
        {
            try
            {
                songIndex--;
                string fullpath = $"{path}/{DownloadedAlbums[albumIndex]}/{SongList[songIndex]}/";
                StartPlayer(fullpath);
                player.Start();
                DisplayCurrentSong(songIndex);
                PlayingNotification();
            }
            catch (Exception)
            {
            }
        }
        [RelayCommand]
        public async void StartSong(string text)
        {
            try
            {
                if (isPlaying == true)
                {
                    isPlaying = false;
                    player.Pause();
                    StartStop = "▶︎";
                    var request = new NotificationRequest
                    {
                        NotificationId = 1337,
                        Title = $"{DownloadedAlbums[albumIndex]}",
                        Subtitle = $"{theCurrentSong}",
                        Description = "Paused Music",
                        BadgeNumber = 42,
                        CategoryType = NotificationCategoryType.Status,
                        Image = new NotificationImage
                        {
                            FilePath = foundCover,
                        },
                        Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
                        {
                            IsProgressBarIndeterminate = false,
                            ProgressBarMax = duration,
                            ProgressBarProgress = songPosition,
                        }
                    };
                    await LocalNotificationCenter.Current.Show(request);
                }
                else
                {
                    isPlaying = true;
                    player.Start();
                    StartStop = "| |";
                    LocalNotificationCenter.Current.Clear();

                    PlayingNotification();
                }

            }
            catch (Exception)
            {

            }
            DisplayCurrentSong(songIndex);
        }
        [RelayCommand]
        public async void NextSong()
        {
            try
            {
                songIndex++;
                string fullpath = $"{path}/{DownloadedAlbums[albumIndex]}/{SongList[songIndex]}/";
                StartPlayer(fullpath);
                player.Start();
                DisplayCurrentSong(songIndex);
                PlayingNotification();
            }
            catch (Exception)
            {
            }

        }
        public void DisplayCurrentSong(int index)
        {
            try
            {
                theCurrentSong = SongList[index];

                string song = SongList[songIndex].Remove(0, SongList[songIndex].IndexOf("-") + 2);
                string songg = new String(song.Where(c => c != '-' && (c < '0' || c > '9')).ToArray()); //remove extra numbers
                string songExt = songg.Remove(songg.IndexOf(".")); //remove extension
                string songSpace = songExt.Remove(0, songExt.IndexOf(" ") + 1);
                bool thisLoved = false;
                foreach (var item in lovedTracks)
                {
                    if (item.Name.ToUpper() == songSpace)
                    {
                        thisLoved = true;
                        lovedTrack = "filledheart.png";
                    }
                }
                if (thisLoved == false)
                {
                    lovedTrack = "heart.png";
                }
                else if (thisLoved == true)
                {
                    lovedTrack = "filledheart.png";
                }
            }
            catch (Exception)
            {

            }
        }
        public void updateCover()
        {
            try
            {
                bool found = false;
                for (int i = 0; i < AlbumInfoObjectPass.Albums.Count; i++)
                {
                    string search = AlbumInfoObjectPass.Albums[i].AlbumName;
                    string what = DownloadedAlbums[albumIndex];
                    if (what.Contains(search))
                    {
                        currentCover = AlbumInfoObjectPass.Albums[i].Image;
                        found = true;
                    }
                }
                if (found == false)
                {
                    currentCover = foundCover;
                }
            }
            catch (Exception ex)
            {

            }
        }
        [RelayCommand]
        public async void DeleteAlbum(string title)
        {
            int index = DownloadedAlbums.IndexOf(title);
            string fullpath = $"{path}/{DownloadedAlbums[index]}/";
            foreach (var item in Directory.GetFiles(fullpath))
            {
                File.Delete(item);
            }
            foreach (var item in Directory.GetDirectories(fullpath))
            {

                foreach (var items in Directory.GetFiles(item))
                {
                    File.Delete(items);
                }
                foreach (var items in Directory.GetDirectories(item))
                {
                    foreach (var itemss in Directory.GetFiles(items))
                    {
                        File.Delete(itemss);
                    }
                    Directory.Delete(items);
                }
                Directory.Delete(item);
            }
            Directory.Delete(fullpath);
            DownloadedAlbums.Clear();
            await GetData();
            SongList.Clear();
        }
        [RelayCommand]
        public void SkipBack()
        {
            player.SeekTo(player.CurrentPosition - 10000);
        }
        [RelayCommand]
        public void SkipForwd()
        {
            player.SeekTo(player.CurrentPosition + 10000);
        }
        List<LastTrack> lovedTracks = new List<LastTrack>();
        private string lovedTrack1;

        [RelayCommand]
        public async void LastFm(LastfmClient client)
        {
            currentClient = client;
            string apikey = "64d5c7295b78ec1719b3eb7e664231ee";
            var loved = await currentClient.User.GetLovedTracks(currentClient.Auth.UserSession.Username);
            for (int i = 1; i < loved.TotalPages + 1; i++)
            {
                var love = await currentClient.User.GetLovedTracks(currentClient.Auth.UserSession.Username, i, 20);
                foreach (var item in love.Content)
                {
                    lovedTracks.Add(item);
                }
            }
        }
        [RelayCommand]
        public async void Logout()
        {
            currentClient.Dispose();
        }
        [RelayCommand]
        public async void Heart(string heartInfo)
        {
            try
            {
                string artist = DownloadedAlbums[albumIndex].Remove(DownloadedAlbums[albumIndex].IndexOf("-"));
                string song = SongList[songIndex].Remove(0, SongList[songIndex].IndexOf("-") + 2);
                string songg = new String(song.Where(c => c != '-' && (c < '0' || c > '9')).ToArray()); //remove extra numbers
                string songExt = songg.Remove(songg.IndexOf(".")); //remove extension
                string songSpace = songExt.Remove(0, songExt.IndexOf(" ") + 1);
                if (isPlaying == true)
                {
                    if (heartInfo == "heart")
                    {
                        await currentClient.Track.LoveAsync(songSpace, artist);
                    }
                    else if (heartInfo == "filledheart")
                    {
                        await currentClient.Track.UnloveAsync(songSpace, artist);
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
