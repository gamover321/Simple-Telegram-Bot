using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using ConsoleApplication12.Extensions;

namespace ConsoleApplication12
{
    public class Bot
    {

        public class CommandEn
        {
            public static readonly string GetUpdates = "getUpdates";
            public static readonly string AnswerInlineQuery = "answerInlineQuery";
            public static readonly string SendMessage = "sendMessage";
            public static readonly string GetMe = "getMe";
        }

        private static readonly string token = "216229289:AAH5j3Ozf0CAANsCkF-hMIX52oEyznAFc4Q";
        private static Int64 Offset { get; set; }

        public static T UploadString<T>(string command, Dictionary<string, object> options = null)
        {
            var optionsString = "";
            if (options != null)
            {
                optionsString = "?" +
                                string.Join("&",
                                    options.Select(
                                        i => i.Key + "=" + Convert.ChangeType(i.Value, i.Value.GetType()).ToString()));
            }
            var url = string.Format("https://api.telegram.org/bot{0}/{1}{2}", token, command, optionsString);
            WebRequest req = WebRequest.Create(url);
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string s = sr.ReadToEnd();
            sr.Close();

            var result = Extensions.Extensions.Deserialize<T>(s);

            return result;
        }

        public static void UploadString(string command, Dictionary<string, object> options = null)
        {
            try
            {


                var optionsString = "";
                if (options != null && options.Any())
                {
                    optionsString = "?" + string.Join("&", options.Select(i => i.Key + "=" + i.Value.ToString()+""));
                }
                var url = string.Format("https://api.telegram.org/bot{0}/{1}{2}", token, command, optionsString);
                WebRequest req = WebRequest.Create(url);
                WebResponse resp = req.GetResponse();
                Stream stream = resp.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                string s = sr.ReadToEnd();
                sr.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
               
            }
        }

        public static void GetMe()
        {
            UploadString(CommandEn.GetMe);
        }

        public static List<Update> GetUpdates()
        {
            //var options = Offset != 0 ? "offset=" + Offset : "";
            var options = new Dictionary<string, object>();
            options.Add("offset", Offset.ToString());

            var result = UploadString<UpdateList>(CommandEn.GetUpdates, options);
            if (Offset == 0 && result.result.Any())
            {
                Offset = result.result.FirstOrDefault().update_id;
            }
            Offset += result.result.Count();

            return result.result.ToList();
        }

        //public static List<Update> GetUpdates_old()
        //{
        //    WebRequest req = WebRequest.Create("https://api.telegram.org/bot" + token + "/getUpdates" + (Offset != 0 ? "?offset=" + Offset : ""));
        //    WebResponse resp = req.GetResponse();
        //    Stream stream = resp.GetResponseStream();
        //    StreamReader sr = new StreamReader(stream);
        //    string s = sr.ReadToEnd();
        //    sr.Close();

        //    var result = Extensions.Extensions.Deserialize<UpdateList>(s);
        //    if (Offset == 0 && result.result.Any())
        //    {
        //        Offset = result.result.FirstOrDefault().update_id;
        //    }
        //    Offset += result.result.Count();

        //    return result.result.ToList();
        //}

        public static void AnswerInlineQuery()
        {
            var results = new InlineQueryResultArray();
            results.QueryArray = new List<InlineQueryResult>
            {
                new InlineQueryResultArticle
                {

                    id = "1",
                    title = "Title",
                    input_message_content = new InputMessageContent
                    {
                        message_text = "message"
                    },
                }
            };

            //var options = $"inline_query_id=1&reply_to_message_id={replyToMessageId}&text={text}";
            var options = new Dictionary<string, object>();
            options.Add("inline_query_id", "2");
            options.Add("results", results);

            UploadString(CommandEn.AnswerInlineQuery, options);
        }

        public static void SendMessage(int chatId, string replyToMessageId, string text)
        {
            //var options = $"chat_id={chatId}&reply_to_message_id={replyToMessageId}&text={text}";
            var options = new Dictionary<string, object>();
            options.Add("chat_id", chatId);
            //options.Add("reply_to_message_id", replyToMessageId);
            options.Add("text", text);

            UploadString(CommandEn.SendMessage, options);
        }
        
        public class UpdateList
        {
            public Update[] result;
        }

        public class Update
        {
            public int update_id { get; set; }
            public Message message { get; set; }
            public InlineQuery inline_query { get; set; }
        }
        public class Message
        {
            public int message_id { get; set; }
            public MessageFrom from { get; set; }
            public MessageChat chat { get; set; }
            public int date { get; set; }
            public string text { get; set; }
        }

        public class User
        {
            public string id { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string username { get; set; }
        }

        public class InlineQuery
        {
            public string id { get; set; }
            public User from { get; set; }
            public string query { get; set; }
            public string offset { get; set; }
        }

        public class MessageFrom
        {
            public int ind { get; set; }
            public string first_name { get; set; }
            public string username { get; set; }
        }
        public class MessageChat
        {
            public int id { get; set; }
            public string first_name { get; set; }
            public string username { get; set; }
        }

        public class InputMessageContent
        {
            public string message_text { get; set; }
        }

        public abstract class InlineQueryResult
        {
            public abstract string type { get; }
            public string id { get; set; }
            public string title { get; set; }
            //public string caption { get; set; }
            public InputMessageContent input_message_content { get; set; }
        }

        public class InlineQueryResultArray
        {
            public List<InlineQueryResult> QueryArray { get; set; }

            public InlineQueryResultArray()
            {
                QueryArray = new List<InlineQueryResult>();
            }

            public override string ToString()
            {
                var result =QueryArray.SerializeToJson();
                return result;
            }
        }

        public class InlineQueryResultArticle : InlineQueryResult
        {
            public string url { get; set; }
            public override string type => "article";
        }
    }
}
