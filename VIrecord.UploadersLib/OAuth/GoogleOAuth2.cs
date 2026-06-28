#region License Information (GPL v3)

/*
    VIrecord - A program that allows you to take screenshots and share any file type
    Copyright (c) 2007-2026 VIrecord Team

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using Newtonsoft.Json;
using VIrecord.HelpersLib;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace VIrecord.UploadersLib
{
    public class GoogleOAuth2 : IOAuth2Loopback
    {
        private const string AuthorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        private const string TokenEndpoint = "https://oauth2.googleapis.com/token";
        private const string UserInfoEndpoint = "https://www.googleapis.com/oauth2/v3/userinfo";

        public OAuth2Info AuthInfo { get; private set; }
        private Uploader GoogleUploader { get; set; }
        public string RedirectURI { get; set; }
        public string State { get; set; }
        public string Scope { get; set; }

        public GoogleOAuth2(OAuth2Info oauth, Uploader uploader)
        {
            AuthInfo = oauth;
            GoogleUploader = uploader;
        }

        public string GetAuthorizationURL()
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("response_type", "code");
            args.Add("client_id", AuthInfo.Client_ID);
            args.Add("redirect_uri", RedirectURI);
            args.Add("state", State);
            args.Add("scope", Scope);

            return URLHelpers.CreateQueryString(AuthorizationEndpoint, args);
        }

        public bool GetAccessToken(string code)
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("code", code);
            args.Add("client_id", AuthInfo.Client_ID);
            args.Add("client_secret", AuthInfo.Client_Secret);
            args.Add("redirect_uri", RedirectURI);
            args.Add("grant_type", "authorization_code");

            string response = GoogleUploader.SendRequestURLEncoded(HttpMethod.POST, TokenEndpoint, args);

            return OAuth2Helper.ProcessTokenResponse(response, AuthInfo);
        }

        public bool RefreshAccessToken()
        {
            if (OAuth2Info.CheckOAuth(AuthInfo) && !string.IsNullOrEmpty(AuthInfo.Token.refresh_token))
            {
                Dictionary<string, string> args = new Dictionary<string, string>();
                args.Add("refresh_token", AuthInfo.Token.refresh_token);
                args.Add("client_id", AuthInfo.Client_ID);
                args.Add("client_secret", AuthInfo.Client_Secret);
                args.Add("grant_type", "refresh_token");

                string response = GoogleUploader.SendRequestURLEncoded(HttpMethod.POST, TokenEndpoint, args);

                return OAuth2Helper.ProcessTokenResponse(response, AuthInfo, preserveRefreshToken: true);
            }

            return false;
        }

        public bool CheckAuthorization()
        {
            return OAuth2Helper.CheckAuthorization(AuthInfo, RefreshAccessToken, GoogleUploader.Errors);
        }

        public NameValueCollection GetAuthHeaders()
        {
            return OAuth2Helper.CreateBearerAuthHeaders(AuthInfo.Token.access_token);
        }

        public OAuthUserInfo GetUserInfo()
        {
            string response = GoogleUploader.SendRequest(HttpMethod.GET, UserInfoEndpoint, null, GetAuthHeaders());

            if (!string.IsNullOrEmpty(response))
            {
                return JsonConvert.DeserializeObject<OAuthUserInfo>(response);
            }

            return null;
        }
    }
}