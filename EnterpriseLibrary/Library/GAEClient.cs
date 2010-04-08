using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;

namespace Energy.Library
{
    /// <summary>
    /// Google App Engine (GAE) client 
    /// Production mode will login to Google and get auth key, and then login to application
    /// Development mode will login with username
    /// </summary>
    public class GAEClient
    {
        private const string ExceptionMessageDefalt = "Error authorizing and getting Google Cookie";
        private const string ProductionGoogleLoginURL = "https://www.google.com/accounts/ClientLogin";  // dev does not need a google login
        private const string Application_GAELoginURL_AddOn = "/_ah/login";
        private const string Application_GAEAuthorizationURL_AddOn = "/authme";
        private GoogleAppEngineMode _appEngineMode;     // CALLER sets this
        private string _application_Name;               // CALLER sets this; myenergyuse
        private string _application_URL;                // CALLER sets this; http://myenergyuse.appspot.com or http://localhost:8081
        private string _googleUsername;                 // CALLER sets this;
        private string _googlePassword;                 // CALLER sets this;
        private string _authorizedToken;
        private string _authorizedCookie;
        private bool _authorized;
        private bool _error;
        private string _errorMsg;

        public GAEClient()
        {
            _appEngineMode = GoogleAppEngineMode.Production;    //DEBUG: Development || Production
            _application_Name = string.Empty;
            _application_URL = string.Empty;
            _googleUsername = string.Empty;
            _googlePassword = string.Empty;
            _authorizedToken = string.Empty;
            _authorizedCookie = string.Empty;
            _authorized = false;
            _error = false;
            _errorMsg = string.Empty;
        }

        public GAEClient(Configuration config, bool noReset)
        {
            _application_Name = config.GAEAppName;
            _application_URL = config.GAESiteUrl;
            _googleUsername = config.GAEGmail;
            _googlePassword = config.GAEPassword;

            if (!noReset)
            {
                _appEngineMode = GoogleAppEngineMode.Production;    //DEBUG: Development || Production
                _authorizedToken = string.Empty;
                _authorizedCookie = string.Empty;
                _authorized = false;
                _error = false;
                _errorMsg = string.Empty;
            }
        }

        #region Properties


        public string AuthorizedCookie
        {
            get { return _authorizedCookie; }
            set { _authorizedCookie = value; }
        }

        public string ErrorMessage
        {
            get { return _errorMsg; }
            set { _errorMsg = value; }
        }

        public bool IsAuthorized
        {
            get { return _authorized; }
        }


        public bool Error
        {
            get { return _error; }
            set { _error = value; }
        }

        public GoogleAppEngineMode Mode
        {
            get { return _appEngineMode; }
            set { _appEngineMode = value; }
        }

        public string GoogleUsername
        {
            get { return _googleUsername; }
            set { _googleUsername = value; }
        }

        public string GooglePassword
        {
            get { return _googlePassword; }
            set { _googlePassword = value; }
        }

        public string ApplicationURL
        {
            get { return _application_URL; }
            set { _application_URL = value; }
        }

        public string ApplicationName
        {
            get { return _application_Name; }
            set { _application_Name = value; }
        }

        #endregion

        /// <summary>
        /// Unauthorize the GAEClient
        /// </summary>
        public void Unauthorize()
        {
            _authorized = false;
        }

        /// <summary>
        /// Callers use this to login to google, login to application, 
        /// authorize for application, finally get a cookie. (dev uses dev_appserver_login)
        /// </summary>
        /// <returns></returns>
        public bool Authorize()
        {
            if (!_authorized)
            {
                LoginToGoogleAndAuthorize();
            }

            return (_authorized);
        }

        /// <summary>
        /// Google login to get auth
        /// </summary>
        /// <returns></returns>
        private bool LoginToGoogleAndAuthorize()
        {
            HttpWebRequest authRequest;
            Stream postDataStr;
            HttpWebResponse authResponse;
            Stream responseStream;
            StreamReader responseReader;
            string nextLine;
            StringBuilder postData = new StringBuilder();

            _authorized = false;
            _authorizedToken = string.Empty;

            try
            {

                if (_appEngineMode == GoogleAppEngineMode.Production)   // do not change this
                {

                    authRequest = (HttpWebRequest)HttpWebRequest.Create(ProductionGoogleLoginURL);
                    authRequest.Method = "POST";
                    authRequest.ContentType = "application/x-www-form-urlencoded";
                    authRequest.AllowAutoRedirect = false;

                    postData.Append("Email=");
                    postData.Append(HttpUtility.UrlEncode(_googleUsername));
                    postData.Append("&");
                    postData.Append("Passwd=");
                    postData.Append(HttpUtility.UrlEncode(_googlePassword));
                    postData.Append("&");
                    postData.Append("service=");
                    postData.Append(HttpUtility.UrlEncode("ah"));
                    postData.Append("&");
                    postData.Append("source=");
                    postData.Append(HttpUtility.UrlEncode(_application_Name));
                    postData.Append("&");
                    postData.Append("accountType=");
                    postData.Append(HttpUtility.UrlEncode("HOSTED_OR_GOOGLE"));

                    byte[] buffer = Encoding.ASCII.GetBytes(postData.ToString());
                    authRequest.ContentLength = buffer.Length;

                    postDataStr = authRequest.GetRequestStream();
                    postDataStr.Write(buffer, 0, buffer.Length);
                    postDataStr.Flush();
                    postDataStr.Close();

                    authResponse = (HttpWebResponse)authRequest.GetResponse();
                    responseStream = authResponse.GetResponseStream();
                    responseReader = new StreamReader(responseStream);

                    // search for auth line
                    _authorizedToken = null;
                    nextLine = responseReader.ReadLine();

                    while (nextLine != null)
                    {
                        if (nextLine.StartsWith("Auth="))
                        {
                            // remove the 'Auth=' from the start of the string
                            _authorizedToken = nextLine.Substring(5);
                        }

                        nextLine = responseReader.ReadLine();
                    }

                    responseReader.Close();
                    authResponse.Close();


                }
                else
                {
                    // dev
                    // does not need to login to google, just the application
                    _authorizedToken = "n/a";
                }

                // with authorization token set, now login to app and get cookie
                LoginToApplicationAndGetCookie();

            }
            catch (Exception ex)
            {
                _error = true;
                _errorMsg = ExceptionMessageDefalt + ex.Message;
                _authorized = false;
            }

            return (_authorized);
        }

        /// <summary>
        /// GAE login to get cookie
        /// </summary>
        private void LoginToApplicationAndGetCookie()
        {
            HttpWebRequest cookieRequest;
            StringBuilder req = new StringBuilder();
            HttpWebResponse cookieResponse;

            try
            {
                if (_appEngineMode == GoogleAppEngineMode.Production)   // do not change this
                {
                    req.Append(_application_URL + Application_GAELoginURL_AddOn);
                    req.Append("?");
                    req.Append("continue=");
                    req.Append(HttpUtility.UrlEncode(_application_URL + Application_GAEAuthorizationURL_AddOn));
                    req.Append("&");
                    req.Append("auth=");
                    req.Append(HttpUtility.UrlEncode(_authorizedToken));

                }
                else
                {
                    // dev

                    req.Append(_application_URL + Application_GAELoginURL_AddOn);
                    req.Append("?");
                    req.Append("email=");
                    req.Append(_googleUsername);
                    req.Append("&");
                    req.Append("action=Login");
                    req.Append("&");
                    req.Append("continue=");
                    req.Append(HttpUtility.UrlEncode(_application_URL));

                }

                cookieRequest = (HttpWebRequest)WebRequest.Create(req.ToString());
                cookieRequest.Method = "GET";
                cookieRequest.ContentType = "application/x-www-form-urlencoded";
                cookieRequest.AllowAutoRedirect = false;

                cookieResponse = (HttpWebResponse)cookieRequest.GetResponse();

                _authorizedCookie = cookieResponse.Headers["Set-Cookie"];

                // does cookie exist
                if (_authorizedCookie != null)
                {
                    if (_authorizedCookie != String.Empty)
                    {
                        _authorized = true;
                    }
                }

            }
            catch
            {
                _authorized = false;

                throw;
            }

        }

        /// <summary>
        /// Connects and sends json chirp to GAE application
        /// Example WebRequest code from 
        /// http://blogs.x2line.com/al/archive/2008/08/29/3544.aspx
        /// </summary>
        /// <param name="geaReadContainer"></param>
        /// <param name="url"></param>
        public void SendJSONToGAEService(GAEReadContainer geaReadContainer, string url)
        {
            const int constTimeoutInMilliseconds = 10000;

            SendJSONToGAEService(geaReadContainer, url, constTimeoutInMilliseconds);
        }

        public void SendJSONToGAEService(GAEReadContainer geaReadContainer, string url, int timeoutInMilliseconds)
        {
            string json = string.Empty;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            string responseData = String.Empty;
            StreamReader reader = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json; charset:utf-8";
                request.Headers["Cookie"] = _authorizedCookie;
                request.Timeout = timeoutInMilliseconds; 

                json = JSONHelper.Serialize<GAEReadContainer>(geaReadContainer);

                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                byte[] data = enc.GetBytes(json);

                if ((data != null) && (data.Length > 0))
                {
                    using (Stream strm = request.GetRequestStream())
                    {
                        strm.Write(data, 0, data.Length);
                    }
                }

                response = (HttpWebResponse)request.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                responseData = reader.ReadToEnd().Trim();

                // if the data is sent okay, 
                // the response from the service is simply the string "success"
                // if not, unauthorize this GAEClient
                if (responseData != "success")
                {
                    this._authorized = false;
                }

            }
            catch
            {
                // fatal error communicating; unauthorize the GAEClient
                this._authorized = false;
            }
            finally
            {
                if (request != null)
                {
                    request = null;
                }

                if (response != null)
                {
                    response.Close();
                    response = null;
                }
            }
    
        }
    }

    /// <summary>
    /// App Engine Mode
    /// </summary>
    public enum GoogleAppEngineMode
    {
        Production = 0,
        Development = 1
    }

}
