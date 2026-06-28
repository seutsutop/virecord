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
using System;
using System.Collections.Specialized;

namespace VIrecord.UploadersLib
{
    public static class OAuth2Helper
    {
        public static NameValueCollection CreateBearerAuthHeaders(string accessToken)
        {
            NameValueCollection headers = new NameValueCollection();
            headers.Add("Authorization", "Bearer " + accessToken);
            return headers;
        }

        public static bool ProcessTokenResponse(string response, OAuth2Info authInfo, bool updateExpireDate = true, bool preserveRefreshToken = false)
        {
            if (!string.IsNullOrEmpty(response))
            {
                OAuth2Token token = JsonConvert.DeserializeObject<OAuth2Token>(response);

                if (token != null && !string.IsNullOrEmpty(token.access_token))
                {
                    if (updateExpireDate)
                    {
                        token.UpdateExpireDate();
                    }

                    if (preserveRefreshToken && authInfo.Token != null && string.IsNullOrEmpty(token.refresh_token))
                    {
                        token.refresh_token = authInfo.Token.refresh_token;
                    }

                    authInfo.Token = token;
                    return true;
                }
            }

            return false;
        }

        public static bool CheckAuthorization(OAuth2Info authInfo, Func<bool> refreshAccessToken, UploaderErrorManager errors, string serviceName = null)
        {
            if (OAuth2Info.CheckOAuth(authInfo))
            {
                if (authInfo.Token.IsExpired && !refreshAccessToken())
                {
                    errors.Add("Refresh access token failed.");
                    return false;
                }
            }
            else
            {
                string message = string.IsNullOrEmpty(serviceName) ? "Login is required." : $"{serviceName} login is required.";
                errors.Add(message);
                return false;
            }

            return true;
        }
    }
}
