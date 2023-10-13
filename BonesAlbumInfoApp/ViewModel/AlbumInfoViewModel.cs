using BonesAlbumInfoApp.Model;
using BonesAlbumInfoApp.PassObjects;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Icu.Text.CaseMap;
using static Java.Util.Jar.Attributes;

namespace BonesAlbumInfoApp.ViewModel
{
    [QueryProperty("Album", "selectedAlbum")]
    public partial class AlbumInfoViewModel : ObservableObject
    {
        [ObservableProperty]
        private string releaseDate;
        [ObservableProperty]
        private string image;
        [ObservableProperty]
        private string title;
        [ObservableProperty]
        private string artist;

        private Album album;
        public Album Album
        {
            set
            {
                album = value;
                ReleaseDate = album.ReleaseDate;
                Image = album.Image;
                Title = album.AlbumName;
                Artist = album.Artist;
                AlbumInfoObjectPass.passedAlbum = album;
            }
        }
    }
}
