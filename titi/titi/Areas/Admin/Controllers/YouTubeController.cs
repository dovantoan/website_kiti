using Google.YouTube;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.Diagnostics;
using System.Threading;
using System.Reflection;

namespace titi.Areas.Admin.Controllers
{
    public class YouTubeController : Controller
    {
        public static string videoId = "";
        public static string statusMessage = "Booting";
        public static string totalSize = "0";
        public static string totalSent = "0";
        public async Task<JsonResult> YouTubeUpload(FormCollection frm)
        {
            try
            {
                string ProductName = frm["ProductName"].ToString();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    Stream fileStream = file.InputStream;
                    await Run(fileStream, ProductName);
                    break;
                }
                return Json(new
                {
                    Success = string.IsNullOrEmpty(videoId) ? false : true,
                    Message = string.IsNullOrEmpty(videoId) ? "Upload video lỗi!" : "Upload video thành công!",
                    Data = videoId
                });
            }
            catch(Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Upload video lỗi: " + ex.Message,
                    Data = ""
                });
            }
        }
        public async Task Run(Stream fileStream,string ProductName)
        {
            //UserCredential credential;
            //using (var stream = new FileStream(Server.MapPath("~/Areas/Admin/Scripts/Youtube/code_secret_client.json"), FileMode.Open, FileAccess.Read))
            //{
            //    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            //        GoogleClientSecrets.Load(stream).Secrets,
            //        new[] { YouTubeService.Scope.YoutubeUpload },
            //        "user",
            //        CancellationToken.None
            //    );
            //}
            //var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            //{
            //    HttpClientInitializer = credential,
            //    ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
            //});
            string CLIENT_ID = "683141981090-pns4i5cf1ijisl7j825m5in669tdqf6k.apps.googleusercontent.com";  // Replace with your client id
            string CLIENT_SECRET = "9XaN_Hh6Xzf-KnZ3ISsFHKbn";  // Replace with your secret

            var youtubeService = AuthenticateOauth(CLIENT_ID, CLIENT_SECRET, "user");


            var video = new Google.Apis.YouTube.v3.Data.Video
            {
                Snippet = new VideoSnippet
                {
                    Title = "KTShopHouse "+ ProductName,
                    Description = ProductName,
                    Tags = new string[] { "tag1", "tag2" },
                    CategoryId = "22" // See https://developers.google.com/youtube/v3/docs/videoCategories/list
                },
                Status = new VideoStatus
                {
                    PrivacyStatus = "unlisted" // or "private" or "public"
                }
            };
            //var filePath = @"E:\video\11730239_736377929817874_1430326728_n.mp4"; // Replace with path to actual movie file.
            
            using (fileStream)
            {
                var videosInsertRequest = youtubeService.Videos.Insert(video, "snippet,status", fileStream, "video/*");
                videosInsertRequest.ProgressChanged += VideosInsertRequest_ProgressChanged;
                videosInsertRequest.ResponseReceived += VideosInsertRequest_ResponseReceived;

                await videosInsertRequest.UploadAsync();
            }
        }
        void VideosInsertRequest_ProgressChanged(Google.Apis.Upload.IUploadProgress progress)
        {
            switch (progress.Status)
            {
                case UploadStatus.Starting:
                    Debug.WriteLine("Starting");
                    new Task(() => { UpdateUIAsync(progress, "starting"); }).Start();
                    break;
                case UploadStatus.Uploading:
                    Debug.WriteLine(progress.BytesSent + " bytes sent. Please wait.");
                    new Task(() => { UpdateUIAsync(progress, "uploading"); }).Start();
                    break;
                case UploadStatus.Completed:
                    Debug.WriteLine("Upload completed on YouTube.");
                    new Task(() => { UpdateUIAsync(progress, "completed"); }).Start();
                    break;
                case UploadStatus.Failed:
                    Debug.WriteLine("An error prevented the upload from completing.\n" + progress.Exception);
                    new Task(() => { UpdateUIAsync(progress, "failed"); }).Start();
                    break;
            }
        }

        void VideosInsertRequest_ResponseReceived(Google.Apis.YouTube.v3.Data.Video video)
        {
            videoId = video.Id;
            Console.WriteLine("Video id '{0}' was successfully uploaded.", video.Id);
        }
        private void UpdateUIAsync(object obj, string type)
        {
            if (type == "starting")
            {
                IUploadProgress progress = (IUploadProgress)obj;
                totalSent = progress.BytesSent.ToString();
                statusMessage = "Upload Starting";
            }

            if (type == "uploading")
            {
                IUploadProgress progress = (IUploadProgress)obj;
                totalSent = progress.BytesSent.ToString();
                statusMessage = "Video uploading";
            }

            if (type == "completed")
            {
                IUploadProgress progress = (IUploadProgress)obj;
                totalSent = progress.BytesSent.ToString();
                statusMessage = "Completed";
            }

            if (type == "failed")
            {
                statusMessage = "Completed";
            }

            if (type == "done")
            {
                Google.Apis.YouTube.v3.Data.Video video = (Google.Apis.YouTube.v3.Data.Video)obj;
                statusMessage = "Done";
            }
        }

        public static YouTubeService AuthenticateOauth(string clientId, string clientSecret, string userName)
        {

            string[] scopes = new string[] { YouTubeService.Scope.Youtube,
                                             YouTubeService.Scope.YoutubeForceSsl,
                                             YouTubeService.Scope.Youtubepartner,
                                             YouTubeService.Scope.YoutubepartnerChannelAudit,
                                             YouTubeService.Scope.YoutubeReadonly,
                                             YouTubeService.Scope.YoutubeUpload};

            try
            {
                // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%
                UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret }
                                                                                             , scopes
                                                                                             , userName
                                                                                             , CancellationToken.None
                                                                                             , new FileDataStore("Daimto.YouTube.Auth.Store")).Result;

                YouTubeService service = new YouTubeService(new YouTubeService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "YouTube Data API Sample"
                });

                return service;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return null;
            }
        }
    }
}