using BullBeez.Api.Operation;
using BullBeez.Core.Entities;
using BullBeez.Core.Helper;
using BullBeez.Core.ResponseDTO;
using BullBeez.Core.Services;
using BullBeez.Data.Context;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BullBeez.Api.Batch
{
    public class TwitterBatch : IHostedService, IDisposable
    {
        private Timer _timer;
        private string apiUrl = "https://api.twitter.com/";
        //public TwitterBatch()
        //{
        //}

        private readonly ICommonService _commonService;
        private readonly IServiceScopeFactory _scopeFactory;
        public TwitterBatch(IServiceScopeFactory scopeFactory)
        {
            this._scopeFactory = scopeFactory;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            //_logger.LogInformation("Timed Hosted Service running.");
            Thread.Sleep(2000);

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(5));

            return Task.CompletedTask;

        }

        private void DoWork(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BullBeezDBContext>();
                var data = dbContext.BullBeezConfig.FirstOrDefault();
                
                var twitterAcc = data.JsonData.DeserializeObject<ConfigResponse>();


                twitterAcc.TwitterAccount.ForEach(x =>
                {
                    TwitterClient twitterClient = new TwitterClient(apiUrl, x.CKey, x.CSecret, x.AToken, x.TSecret);
                    var tweets = twitterClient.GetHomeTimeline(x.LastTweetId);

                    if (tweets != null && tweets.Count > 0)
                    {
                        var posts = ConvertTweets2Post(tweets);

                        if (posts != null && posts.Count() > 0)
                        {
                            foreach (var item in posts)
                            {
                                dbContext.Posts.AddAsync(new Posts
                                {
                                    Text = item.Text,
                                    UserName = item.UserName,
                                    UserLink = item.UserLink,
                                    MediaLink = item.MediaLink,
                                    PostLink = item.PostLink,
                                });
                            }

                            var maxid = posts.Max(i => i.Id);
                            x.LastTweetId = maxid;

                        }

                    }
                });

                data.JsonData = twitterAcc.SerializeObject();
#if !DEBUG
                dbContext.BullBeezConfig.Update(data);
                dbContext.SaveChanges();
#endif

            }
        }

        private List<PostModel> ConvertTweets2Post(List<TweetResponse> tweets)
        {
            List<PostModel> posts = new List<PostModel>();

            foreach (var tweet in tweets.OrderBy(t => t.id))
            {
                try
                {
                    var index = tweet.text.LastIndexOf("http");

                    posts.Add(new PostModel()
                    {
                        Id = tweet.id,
                        Text = tweet.text.Substring(0, index),
                        UserName = tweet.user.name,
                        UserLink = tweet.user.profile_image_url_https,
                        MediaLink = tweet.extended_entities?.media.FirstOrDefault().media_url_https ?? "",
                        PostLink = tweet.text.Substring(index)
                    });
                }
                catch (Exception ex)
                {

                    //_logger.LogWarning("Tweet Parsing erorr: " + ex.Message);
                }

            }

            return posts;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

       
    }
}
