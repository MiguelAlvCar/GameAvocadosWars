using System;
using System.Collections.Generic;
using DTO_Models;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using BasicElements;
using Ninject;
using BasicElements.AbstractServerInternetCommunication;
using System.Net.Sockets;
using System.Linq;

namespace InternetCommunication.Server
{
    public class CommunicationWithServer: ICommunicationWithServer
    {
        private string WebAddress = Properties.Settings.Default.Test ? Properties.Settings.Default.WebSiteTest : Properties.Settings.Default.WebSite;

        #region User

        public bool InternetLogin(LoginModel details, out IEnumerable<OnlineGameDTO> gamesPool, out bool connectionSucceeded)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(WebAddress + "/api/loggeduser");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(details);
                streamWriter.Write(json);
            }

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                string jsonString;
                using (Stream stream = httpResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                    jsonString = reader.ReadToEnd();
                }
                LoginDTO loginDTO = JsonConvert.DeserializeObject<LoginDTO>(jsonString);

                gamesPool = null;
                if (loginDTO.LoggedIn)
                {
                    string setCookieHeader = httpResponse.Headers["Set-Cookie"];
                    Cookie cookie = new Cookie();
                    string nameValue = setCookieHeader.Split(';')[0];
                    cookie.Name = nameValue.Split('=')[0];
                    cookie.Value = nameValue.Split('=')[1];
                    cookie.Path = "/";
                    cookie.HttpOnly = true;

                    int rootPathUri = httpResponse.ResponseUri.AbsoluteUri.IndexOf(httpResponse.ResponseUri.AbsolutePath);
                    string domainURL = httpResponse.ResponseUri.AbsoluteUri.Substring(0, rootPathUri);
                    Uri target = new Uri(domainURL);
                    cookie.Domain = target.Host;
                    //there was some type of problem: a cookie had to have two point in the domain
                    if (cookie.Domain == "localhost")
                        cookie.Domain = ".app.localhost";

                    IInternetCommunicationMainModelView firstMV = BasicMechanisms.Kernel.Get<IInternetCommunicationMainModelView>();

                    IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

                    firstMV.LoggedPlayer = new UserPlayer(loginDTO.User);
                    firstMV.LoggedPlayer.Player.LocalIPs = host.AddressList.Select(x => new LocalIPDTO() { IP = x.ToString()}).ToArray();
                    firstMV.LoggedPlayer.IdentityCookie = cookie;

                    gamesPool = GetPoolGames(firstMV.LoggedPlayer.Player);
                }

                connectionSucceeded = true;
                return loginDTO.LoggedIn;
            }
            catch (WebException e)
            {
                connectionSucceeded = false;
                gamesPool = null;
                return false;
            }
        }

        public bool EditUser(EditUserDTO editUserDTO)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(WebAddress + "/api/loggeduser");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PUT";

            httpWebRequest.CookieContainer = new CookieContainer();
            IInternetCommunicationMainModelView firstMV = BasicMechanisms.Kernel.Get<IInternetCommunicationMainModelView>();
            httpWebRequest.CookieContainer.Add(firstMV.LoggedPlayer.IdentityCookie);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(editUserDTO);
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string jsonString;
            using (Stream stream = httpResponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<bool>(jsonString);
        }

        #endregion

        #region Games

        public int PostHostGame(OnlineGameDTO onlineGameDTO)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(WebAddress + "/api/GamesPool2");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            httpWebRequest.CookieContainer = new CookieContainer();
            IInternetCommunicationMainModelView firstMV = BasicMechanisms.Kernel.Get<IInternetCommunicationMainModelView>();
            httpWebRequest.CookieContainer.Add(firstMV.LoggedPlayer.IdentityCookie);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(onlineGameDTO);
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string jsonString;
            using (Stream stream = httpResponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<int>(jsonString);
        }

        public IEnumerable<OnlineGameDTO> GetPoolGames(PlayerDTO player)
        {
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(WebAddress + "/api/GamesPool");

            WebReq.ContentType = "application/json";
            WebReq.Method = "POST";

            WebReq.CookieContainer = new CookieContainer();
            IInternetCommunicationMainModelView firstMV = BasicMechanisms.Kernel.Get<IInternetCommunicationMainModelView>();
            if (firstMV.LoggedPlayer.IdentityCookie != null)
                WebReq.CookieContainer.Add(firstMV.LoggedPlayer.IdentityCookie);

            using (var streamWriter = new StreamWriter(WebReq.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(player);
                streamWriter.Write(json);
            }

            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

            string jsonString;
            using (Stream stream = WebResp.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<IEnumerable<OnlineGameDTO>>(jsonString);
        }

        public bool DeleteGame(int currentGameID)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(WebAddress + "/api/GamesPool/" + currentGameID);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "DELETE";

            httpWebRequest.CookieContainer = new CookieContainer();
            IInternetCommunicationMainModelView firstMV = BasicMechanisms.Kernel.Get<IInternetCommunicationMainModelView>();
            httpWebRequest.CookieContainer.Add(firstMV.LoggedPlayer.IdentityCookie);

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string jsonString;
            using (Stream stream = httpResponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<bool>(jsonString);
        }

        public bool PutHostGame(OnlineGameDTO onlineGameDTO)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(WebAddress + "/api/GamesPool");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PUT";

            httpWebRequest.CookieContainer = new CookieContainer();
            IInternetCommunicationMainModelView firstMV = BasicMechanisms.Kernel.Get<IInternetCommunicationMainModelView>();
            httpWebRequest.CookieContainer.Add(firstMV.LoggedPlayer.IdentityCookie);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(onlineGameDTO);
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string jsonString;
            using (Stream stream = httpResponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<bool>(jsonString);
        }

        public void MaintainConnection(int currentGameID)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(WebAddress + "/api/GamesPool/" + currentGameID);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "HEAD";

            httpWebRequest.GetResponse();
        }

        public void Victory(BattleDTO battleDTO)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(WebAddress + "/api/Battles");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            httpWebRequest.CookieContainer = new CookieContainer();
            IInternetCommunicationMainModelView firstMV = BasicMechanisms.Kernel.Get<IInternetCommunicationMainModelView>();
            httpWebRequest.CookieContainer.Add(firstMV.LoggedPlayer.IdentityCookie);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(battleDTO);
                streamWriter.Write(json);
            }

            httpWebRequest.GetResponse();
        }
        #endregion

    }
}
