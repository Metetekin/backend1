using BullBeez.Core.Helper;
using BullBeez.Core.ResponseDTO;

using OAuth;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Api.Operation
{
    public class TwitterClient
    {
        string cKey = null;
        string cSecret = null;
        string aToken = null;
        string tSecret = null;
        string baseUrl = null;



        public TwitterClient(string _baseUrl, string _cKey, string _cSecret, string _aToken, string _tSecret)
        {
            baseUrl = _baseUrl;
            cKey = _cKey;
            cSecret = _cSecret;
            aToken = _aToken;
            tSecret = _tSecret;
        }

        public List<TweetResponse> GetHomeTimeline(ulong sinceId)
        {
            StringBuilder apiPath = new StringBuilder(baseUrl);
            apiPath.Append("1.1/statuses/home_timeline.json");
            apiPath.AppendFormat("?count={0}", 100);
            apiPath.AppendFormat("&include_entities={0}", 0);

            if (sinceId > 0)
            {
                apiPath.AppendFormat("&since_id={0}", sinceId);
            }

            string REQUEST_URL = apiPath.ToString();

            OAuthRequest oAuth = OAuthRequest.ForProtectedResource("GET", cKey, cSecret, aToken, tSecret);
            oAuth.RequestUrl = REQUEST_URL;
            string auth = oAuth.GetAuthorizationHeader();


            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(oAuth.RequestUrl);
                request.Headers.Add("Authorization", auth);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream dataStream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(dataStream))
                            {
                                string strResponse = reader.ReadToEnd();
                                
                                return strResponse.DeserializeObject<List<TweetResponse>>();
                            }
                        }
                    }

                }

            }
            catch (Exception)
            {

                return null;
            }

            return null;

        }


    }
}
