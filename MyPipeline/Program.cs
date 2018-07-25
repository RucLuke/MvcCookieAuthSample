using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyPipeline
{
    class Program
    {
        public static List<Func<RequestDelegate, RequestDelegate>>
            _list = new List<Func<RequestDelegate, RequestDelegate>>();

        static void Main(string[] args)
        {
            //Use(next =>
            //{
            //    return context =>
            //    {
            //        Console.WriteLine("1");
            //        return next.Invoke(context);
            //    };
            //});

            //Use(next =>
            //{
            //    return context =>
            //    {
            //        Console.WriteLine("2");
            //        return next.Invoke(context);
            //    };
            //});

            //RequestDelegate end = (context) =>
            //{
            //    Console.WriteLine("end...");
            //    return Task.CompletedTask;
            //};

            //foreach (var middleWare in _list)
            //{
            //    end = middleWare.Invoke(end);
            //}

            //end.Invoke(new Context());
            //Console.ReadLine();


            using (var httpClient = new HttpClient())
            {
                var url = "http://ldxf.enorth.com.cn/learnstate/LearnStateAction!saveStudyTimePerMinute.do";
                var paramList = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("svo.userId", "363045"),
                    new KeyValuePair<string, string>("svo.chapterId", "311"),
                    new KeyValuePair<string, string>("svo.levelId", "86"),
                    new KeyValuePair<string, string>("isMouseStop", "0"),
                    new KeyValuePair<string, string>("tips", "387ea78a3ca83a65ccca1ae6208929f5"),
                    new KeyValuePair<string, string>("startTime", "1532077261863")
                };
                for (var i = 0; i < 9; i++)
                {
                    var response = httpClient.PostAsync(new Uri(url), new FormUrlEncodedContent(paramList)).Result;
                    var result = response.Content.ReadAsStringAsync().Result;
                    Console.Write(result);
                }


                Console.ReadKey();
            }
        }

        public static void Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            _list.Add(middleware);
        }
    }
}
