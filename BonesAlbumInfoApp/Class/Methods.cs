using Plugin.DownloadManager.Abstractions;
using Plugin.DownloadManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if __ANDROID__
using AndroidApp = Android.App.Application;
#endif
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using SharpCompress.Archives;
using CodeHollow.FeedReader;
using Newtonsoft.Json.Linq;
using System.Xml;
using BonesAlbumInfoApp.Model;
using BonesAlbumInfoApp.PassObjects;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BonesAlbumInfoApp.Class
{
    public class Methods
    {
        public void extractRarAndroid(string pathOs)
        {
            using (var archive = RarArchive.Open(pathOs))
            {
                foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                {
                    entry.WriteToDirectory(AndroidApp.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads).AbsolutePath, new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                }
            }
        }
        public async Task DownloadProcess()
        {
            //https://github.com/SimonSimCity/Xamarin-CrossDownloadManager
            
            string path = string.Empty;
            CrossDownloadManager.Current.PathNameForDownloadedFile = new Func<IDownloadFile, string>(file =>
            {
                string fileName = AlbumInfoObjectPass.passedAlbum.AlbumName + ".rar";
                path = Path.Combine(AndroidApp.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads).AbsolutePath, fileName);
                return path;
            });
            bool IsDownloading(IDownloadFile file)
            {
                if (file == null) return false;

                switch (file.Status)
                {
                    case DownloadFileStatus.INITIALIZED:
                    case DownloadFileStatus.PAUSED:
                    case DownloadFileStatus.PENDING:
                    case DownloadFileStatus.RUNNING:
                        return true;

                    case DownloadFileStatus.COMPLETED:
                    case DownloadFileStatus.CANCELED:
                    case DownloadFileStatus.FAILED:
                        return false;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            await Task.Run(async () =>
            {
                var downloadManager = CrossDownloadManager.Current;
                var file = downloadManager.CreateDownloadFile(AlbumInfoObjectPass.passedAlbum.DownloadLink);
                downloadManager.Start(file);
                bool isDownloading = true;
                while (isDownloading)
                {
                    isDownloading = IsDownloading(file);
                }
            });
            extractRarAndroid(path);
        }
        public List<string> availability()
        {
            var availability = new List<string>
            {
                "out of stock",
                "in stock",
                "preorder",
                "all"
            };
            return availability;
        }

        public async Task GetCurrency(string cur)
        {
            using var client = new HttpClient();
            var content = await client.GetStringAsync($"https://api.exchangerate.host/convert?from=USD&to={cur}");
            var details = JObject.Parse(content);
            var please = details["result"];
            if (cur == "EUR")
            {
                DataPass.eurExchange = double.Parse(please.ToString());
            }
            else if (cur == "GBP")
            {
                DataPass.gbpExchange = double.Parse(please.ToString());
            }
            else if (cur == "RUB")
            {
                DataPass.rubExchange = double.Parse(please.ToString());
            }
        }

        public async Task FillProductListCustSource()
        {
            rssChannelItems.Clear();
            DataPass.loadedMerch.Clear();
            Sources s = new Sources();
            foreach (var item in DataPass.whichOneRemove)
            {
                try {await ReadXml(s.AddLinks()[item]);}
                catch (Exception){}
            }
            foreach (var item in rssChannelItems)
            {
                DataPass.loadedMerch.Add(item);
            };
        }

        ObservableCollection<rssChannelItem> rssChannelItems = new ObservableCollection<rssChannelItem>();
        public async Task ReadXml(string link)
        {
            var feed = await FeedReader.ReadAsync(link);
            rssChannelItem r = new rssChannelItem();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(feed.OriginalDocument);
            for (int i = 0; i < doc.GetElementsByTagName("item").Count; i++)
            {
                XmlNodeList nl = doc.GetElementsByTagName("item")[i].ChildNodes;
                rssChannelItem localItem = new rssChannelItem()
                {
                    idField = int.Parse(nl[0].InnerText),
                    titleField = nl[1].InnerText.ToUpper(),
                    descriptionField = nl[2].InnerText,
                    priceField = nl[3].InnerText,
                    linkField = nl[4].InnerText,
                    image_linkField = nl[5].InnerText,
                    additional_image_linkField = nl[6].InnerText,
                    mpnField = int.Parse(nl[7].InnerText),
                    conditionField = nl[8].InnerText,
                    availabilityField = nl[9].InnerText
                };
                rssChannelItems.Add(localItem);
            }
        }
        public async Task LoadMerch()
        {
            await GetCurrency("GBP");
            await GetCurrency("EUR");
            await GetCurrency("RUB");
            rssChannelItems.Clear();
            DataPass.loadedMerch.Clear();
            Sources s = new Sources();
            for (int i = 0; i < s.AddLinks().Count; i++)
            {
                try{await ReadXml(s.AddLinks()[i]);}
                catch (Exception){}
            }
            foreach (var item in rssChannelItems)
            {
                DataPass.loadedMerch.Add(item);
            };
        }
    }
}
