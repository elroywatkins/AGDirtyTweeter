using System;
using System.IO; 
using System.Collections.Generic;
using System.Linq;

namespace Tweeter
{
    public class ConsoleApp
    {
        private readonly string srcPath = @"C:\Deviloa\AG Interview\";
        private bool IsDebugMode { get; set; }        
        private List<User> UserList;
        private List<Tweet> TweetList;
        public ConsoleApp(bool isDebugMode)
        {
            this.IsDebugMode = isDebugMode;           
        }

        public SystemMessage ReadFromSource()
        {
            if (IsDebugMode)
            {
                Console.WriteLine("Reading from Source");
                Console.WriteLine("-------------------");
            }

            string userPath = srcPath + "user.txt";
            string tweetPath = srcPath + "tweet.txt";
            List<string> userList = ReadFileToStringList(userPath);
            
            UserList = MapListToUsers(userList);
            
            // foreach(var user in users)
            // {
            //     Console.WriteLine($"user name: {user.Name}");
            //     Console.WriteLine($"followers : {string.Join(",",user.followers)}");
            // }

            List<string> tweetList = ReadFileToStringList(tweetPath);
            TweetList = MapListToTweets(tweetList);
            // foreach(var tweet in tweets)
            // {
            //     Console.WriteLine($"user name: {tweet.TweeterName}");
            //     Console.WriteLine($"message : {tweet.TweetMessage}");
            // }

            return new SystemMessage(MessageType.Information);
        }
                
        public List<User> MapListToUsers(List<string> inputList)
        {
            //bug martin not found
            //test if same name appears with diff followers
            var resultList = new List<User>();
            string delimiter = "follows"; //todo read from settings
            foreach(var item in inputList)
            {
                var chrIdx = item.IndexOf(delimiter);                
                string userName = item.Substring(0,chrIdx-1);
                var strLength = item.Length-chrIdx-delimiter.Length-1;
                string followersList = item.Substring(chrIdx+delimiter.Length+1,strLength);
                var user = new User()
                {
                    Name = userName,                    
                    Followers = followersList.Split(", ").ToList() //todo make this dynamic splitter from settings and sort out space issue
                };
                resultList.Add(user);
                foreach(var follower in followersList.Split(", ").ToList())
                {
                    if (!resultList.Select(x=>x.Name).Contains(follower))
                    {
                        resultList.Add(new User(){Name=follower});
                    }
                }
            }
            return resultList;
        }

        public List<Tweet> MapListToTweets(List<string> inputList)
        {
             var resultList = new List<Tweet>();
            string delimiter = @">"; //todo read from settings
            int tweetOrder = 0;
            foreach(var item in inputList)
            {
                var chrIdx = item.IndexOf(delimiter);                
                string userName = item.Substring(0,chrIdx);
                var strLength = item.Length-chrIdx-delimiter.Length-1;
                string tweetMessage = item.Substring(chrIdx+delimiter.Length+1,strLength);
                var tweet = new Tweet()
                {
                    TweeterName = userName,
                    TweetMessage = tweetMessage,
                    TweetOrder = tweetOrder
                };
                tweetOrder++;
                resultList.Add(tweet);
            }
            return resultList;
        }

        public List<string> ReadFileToStringList(string filePath)
        {
            var resultList = new List<string>();
            try
            {                
                //TODO Read Files Async
                string line;
                
                using (StreamReader reader =  new StreamReader(filePath,  System.Text.Encoding.ASCII))
                {
                    while((line = reader.ReadLine()) != null)
                    {
                        resultList.Add(line.Trim());// add error checking
                    }
                }
                
            }
            catch (System.Exception)
            {
                
                throw;
            }
            return resultList;

        }
        public SystemMessage ProcessTweets()
        {
            if (IsDebugMode)
            {
                Console.WriteLine("Processing Tweets");
                Console.WriteLine("-----------------");
            }

            foreach(var user in UserList.OrderBy(x => x.Name))
            {
                Console.WriteLine(user.Name);
                PrintAllUserTweets(user);
            }

            return new SystemMessage(MessageType.Information);
        }

        public void PrintAllUserTweets(User user)
        {
            var tweets = TweetList.Where(x=>x.TweeterName==user.Name).ToList();

            if (user.Followers != null)
            {     
                foreach(var followerTweet in TweetList.Where(x=> user.Followers.Contains(x.TweeterName)))
                {       
                    tweets.Add(followerTweet);
                }
            }
 
         //   var tweets = TweetList.Where(x=>x.TweeterName==user.Name).OrderBy(y=>y.TweetOrder).ToList();
            foreach(var tweet in tweets)
            {
                Console.WriteLine($"@{tweet.TweeterName}:{tweet.TweetMessage}");
            }
        }

        public SystemMessage WriteToOutput()
        {
            if (IsDebugMode)
            {
                Console.WriteLine("Writing to Console");
            }
            return new SystemMessage(MessageType.Information);
        }

        public void Run()
        {
            ReadFromSource();
            ProcessTweets();
            WriteToOutput();
        }
    }
}