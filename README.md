# Project C# Mobile 2022-2023
Name and email: **Van Kimmenade Anton (anton.vanKimmenade@STUDENT.PXL.BE)**

Application Title: **SESHApp**

The SESHAPP is an app for fans of TEAMSESH, especially for fans of the most famous member, BONES.
In the app, users can download and listen to all BONES albums in FLAC format, and they can also stay up to date with their clothing line. The idea is based on an existing app that is currently no longer functional (see play store link in sources). The music player is built from scratch and uses only Android.Media.MediaPlayer to play media. Although the app itself does not offer the ability to purchase clothing, it can redirect users to the official website where they can make clothing purchases.

**The SESHAPP provides a convenient and easy way for TEAMSESH fans to listen to their favorite music and see the latest updates from the group's clothing line.**

List of key features and components of the application:
* Download BONES albums locally in high quality
* Listen to downloaded albums and scrobble them with <a href="https://www.last.fm" target="_blank">LastFm<a>
* Overview of clothing line
* Create an account to keep track of likes

# Logbook
* 29/9/2022: Project created
* 2/10/2022: Basic album download functionality completed
* 3/10/2022: Basic search function created
* 5/10/2022: Attempted to make it compatible with iOS & Windows (unsuccessful)
* 10/10/2022: Basic UI for clothing home page
* 11/10/2022: Added CarouselView
* 15/10/2022: Clothing Detail Page completed & icons/splash screen created
* 19/10/2022: Live currency adjustment
* 20/10/2022: Favorites list
* 30/10/2022: Music player completed
* 02/11/2022: Created MVVM folder structure
* 12/11/2022: Created MVVM folder structure for merchandise
* 13/11/2022: Finished music player notifications
* 14/11/2022: Finished Last.fm scrobbling
* 15/11/2022: Improved scrobbling method & two-way likes
* 22/11/2022: Basic login functionality
* 27/11/2022: Registration
* 28/11/2022: Second database with additional personal data & account update started
* 5/12/2022: Loading albums via Firebase
* 19/12/2022: Converting likes to Firebase
* 29/12/2022 - 04/01/2023: Final bug fixes, polishing of features, and video recording
<br><br>

# Optional: Screenshots
*Include a few screenshots of your app in action if possible.*
<div style="display: flex;flex-wrap: wrap">
  <p><img src="Screenshots/1.png" width="250"></p>
  <p><img src="Screenshots/2.png" width="250"></p>
  <p><img src="Screenshots/3.png" width="250"></p>
  <p><img src="Screenshots/13.png" width="250"></p>
  <p><img src="Screenshots/12.png" width="250"></p>
  <p><img src="Screenshots/4.png" width="250"></p>
  <p><img src="Screenshots/5.png" width="250"></p>
  <p><img src="Screenshots/7.png" width="250"></p>
  <p><img src="Screenshots/6.png" width="250"></p>
  <p><img src="Screenshots/11.png" width="250"></p>
  <p><img src="Screenshots/8.png" width="250"></p>
  <p><img src="Screenshots/9.png" width="250"></p>
  <p><img src="Screenshots/10.png" width="250"></p>
  <p><img src="Screenshots/14.png" width="250"></p>
</div>

## Click the link below to watch the video!
https://youtu.be/-TMKff5HjkA

# Sources
* CommunityToolkit https://github.com/CommunityToolkit/Maui
* Inflatable.LastFM https://github.com/inflatablefriends/lastfm
* Plugin.LocalNotification https://github.com/thudugala/Plugin.LocalNotification
* SharpCompress https://github.com/adamhathcock/sharpcompress
* Xamarin CrossDownloadManager https://github.com/SimonSimCity/Xamarin-CrossDownloadManager
* CodeHollow.FeedReader https://github.com/arminreiter/FeedReader
* Icons https://labs.openai.com/
* Album covers https://www.discogs.com/artist/1635814-Bones-28
* Albums https://drive.google.com/drive/folders/1jXsM7lOWBs_cO5qVnOADachit38UaPgY?usp=share_link
* Albums downloaded via https://deemix.app/gui, https://github.com/yaronzz/Tidal-Media-Downloader, https://www.freezerapk.pro/
* XML Data Sources
  * https://teamsesh.bigcartel.com/products.xml
  * https://teamseshmerchscraps.bigcartel.com/products.xml
  * https://www.supporttrees.com/products.xml
  * https://endorphinfitnesswear.bigcartel.com/
* Firebase.Authentication.Net https://github.com/step-up-labs/firebase-authentication-dotnet
* Firesharp https://github.com/ziyasal/FireSharp
* Plugin.LocalNotification https://github.com/thudugala/Plugin.LocalNotification
* Currency Exchange API https://api.exchangerate.host
* Watched video for local push notifications https://youtu.be/dWdXXGa1_hI

# Based on
SESHAPP (music player and more, app not functional): https://play.google.com/store/apps/details?id=org.teamsesh.seshapp <br>
SESHstation (radio and clothing only): https://play.google.com/store/apps/details?id=team.sesh.teamsesh

# Future Work
* I would like to integrate my own radio into the app.
* Ability to purchase clothing through the app (not possible because I am not the owner of the store).
* App available on iOS & Windows.
* Chat function between users.
