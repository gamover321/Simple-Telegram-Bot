using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;


namespace ConsoleApplication12
{
    class Program
    {
        static void Main(string[] args)
        {


           // var results = new Bot.InlineQueryResultArray();
           // results.QueryArray = new List<Bot.InlineQueryResult>
           //{
           //     new Bot.InlineQueryResultArticle
           //     {
           //         type = "article",
           //         id = "0",
           //         title = "title",
           //         url = "google.com",
           //         input_message_content = new Bot.InputMessageContent
           //         {
           //             message_text = "message"
           //         }
           //     }
           //};

           // var options = new Dictionary<string, object>();
           // options.Add("inline_query_id", "1");
           // options.Add("results", results);
           // var optionsString = "?" + string.Join("&", options.Select(i => i.Key + "=" + Convert.ChangeType(i.Value, i.Value.GetType()).ToString()));
            

           // Console.WriteLine(optionsString);
           // Console.WriteLine("___");







            var phrazeList = new List<string>
            {
                "Ублюдок, мать твою",
            "а ну иди сюда",
    "говно собачье",
    "с дури решил ко мне лезть, ты",
    "засранец вонючий",
                "мать твою, а?",
                "Ну иди сюда?",
            "попробуй меня трахнуть, я тебя сам трахну",
                "ублюдок, онанист чертов",
                "будь ты проклят",
            "иди, идиот, трахать тебя и всю твою семью",
    "говно собачье, жлоб вонючий",
    "дерьмо, сука, падла",
    "иди сюда, мерзавец",
    "негодяй, гад",
                "иди сюда﻿ ты, говно, жопа!"
            };

           

            var phrazeProgressByUser = new Dictionary<int, int>();

            //Bot.GetMe();

            while (true)
            {
                var updates = Bot.GetUpdates();
                if (updates.Any())
                    foreach (var update in updates)
                    {

                        if (update.message == null && update.inline_query!=null)
                        {
                            Console.WriteLine("inline!");
                            Bot.AnswerInlineQuery();
                            continue;
                        }

                        var message = "";
                        if (!phrazeProgressByUser.ContainsKey(update.message.chat.id))
                        {
                            phrazeProgressByUser.Add(update.message.chat.id, 0);
                        }
                        int order;
                        phrazeProgressByUser.TryGetValue(update.message.chat.id, out order);

                        if (order >= phrazeList.Count)
                        {
                            order = 0;
                        }

                        message = phrazeList[order];

                        phrazeProgressByUser[update.message.chat.id] = ++order;


                        Bot.SendMessage(update.message.chat.id, "", string.Format(message, update.message.chat.first_name));
                        Console.WriteLine("Sent to {3}({0}) : ({1})->({2})", update.message.chat.id, update.message.text, message, update.message.chat.first_name);
                    }
                System.Threading.Thread.Sleep(2500);
            }


            Console.ReadKey();
        }
    }




}
