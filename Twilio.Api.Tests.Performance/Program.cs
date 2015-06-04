using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Disposables;


namespace Twilio.Api.Tests.Performance
{
    class Program
    {
        static void Main(string[] args)
        {
            var run = new Runner();
            run.Run().GetAwaiter().GetResult();
        }

    }

    public class Runner {
        public async Task Run() {
            //bool tick = true;
            //StringBuilder log = new StringBuilder();
            //Dictionary<string, int> statuscodes = new Dictionary<string, int>();

           // DateTime start = DateTime.Now;

            int loopSize = 1000;

            // This is important.  This sets the number of network 
            // connections that Windows will allow.  The default value 
            // is 2 based on RFC2616 (see Section 8.1.4).  Here 
            // I have increased it to 100
            ServicePointManager.DefaultConnectionLimit = 100;

            string accountsid = "";
            string authtoken = "";

            // Build all of the strings manually
            //string uri = string.Format("https://api.twilio.com/2010-04-01/Accounts/{0}/SMS/Messages", accountsid);
            //string data = string.Format("From={0}&To={1}&Body={2}", "%2B13142875471", "%2B15005550009", "Test Message");
            //string token = Convert.ToBase64String(Encoding.Default.GetBytes(accountsid + ":" + authtoken));

            // Tell the TPL to make the loop parallelism
            // the same size as the number of items in the loop
            //var options = new ParallelOptions();
            //options.MaxDegreeOfParallelism = loopSize;

            // Just create one HttpClient
            //var client = new HttpClient();

            //var spin = new ConsoleSpinner();
            //Console.Write("Working ");

            //Task.Factory.StartNew(() =>
            //{
                //Start the spinner
                //while (tick)
                //{
                //    spin.Turn();
                //}
            //});


            // Put the loop into a Task to ensure that it happens on a new thread
            //Task.Factory.StartNew(() =>
            //{

                // Build a collection of tasks.
                // Preset the List size otherwise there is a chance
                // the first item will not get added to it (weird!)
                //var tasks = new List<Task>(loopSize);

                TwilioRestClient client = new TwilioRestClient(accountsid, authtoken);

                foreach (var r in client.ListAllMessages(new MessageListRequest()))
                {
                    foreach (var m in r.Messages)
                    {
                        Console.WriteLine("SID: {0}, DateCreated: {1}", m.Sid, m.DateCreated.ToString());
                    }
                }


                //IObservable<MessageResult> result = client.ListAllMessagesAsync(new MessageListRequest() { Count = 10 });
                //var observer = Observer.Create<MessageResult>(
                //    x => {
                //        foreach (var m in x.Messages)
                //        {
                //            Console.WriteLine("SID: {0}, DateCreated: {1}", m.Sid, m.DateCreated.ToString());
                //        }
                //    },
                //    ex => { 
                //        Console.WriteLine("OnError: {0}", ex);
                //    },
                //    () => { 
                //        Console.WriteLine("OnCompleted"); 
                //    });
                //result.Subscribe(observer);

                Console.Read();
            
                //for (int i = 0; i < loopSize; i++)
                //{
                //    //Console.WriteLine("Send");
                //    var msg = await client.SendMessageAsync("+15005550006", "+13144586142", ".NET Unit Test Message");
                //}
                //var result = Parallel.For(0, loopSize, async b =>
                //{
                //    var msg = await client.SendMessageAsync("+15005550006", "+13144586142", ".NET Unit Test Message");

                    // Unfortunately you have to generate both the StringContent and HttpRequestMessage objects
                    // each time because HttpClient immediately disposes of them as soon as it uses them
                    //HttpContent content = new StringContent(data);

                    // HttpContent should default to having a 'Connection: keep-alive' and 
                    // an Expect: 100-continue' header, so no need to explicitly add
                    //content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                    //var message = new HttpRequestMessage(HttpMethod.Post, uri);
                    //message.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);

                    //message.Content = content;

                    // Add all of the tasks to the array.   Use the 
                    //continuation to check the HTTP Request results
                    //tasks.Add(client.SendAsync(message)
                    //            .ContinueWith(t =>
                    //            {
                    //                if (t.IsFaulted)
                    //                {
                    //                    log.Append("Task Faulted\r\n");
                    //                    t.Exception.Handle(e =>
                    //                    {
                    //                        log.Append("\t" + e.Message + "\r\n");
                    //                        return true;
                    //                    });
                    //                }
                    //                else
                    //                {
                    //                    var r = t.Result;
                    //                    if (!r.IsSuccessStatusCode)
                    //                    {
                    //                        string key = string.Format("{0} ({1})", r.StatusCode.ToString(), ((int)r.StatusCode).ToString());

                    //                        if (!statuscodes.ContainsKey(key))
                    //                        {
                    //                            statuscodes.Add(key, 0);
                    //                        }

                    //                        int currentcount = statuscodes[key];
                    //                        Interlocked.Increment(ref currentcount);
                    //                        statuscodes[key] = currentcount;
                    //                    }
                    //                }
                    //            })
                    //);

                //});

                // For this sample I'm telling the TPL to wait here
                // until all of the tasks in the array are done.
                // You might do something different here.

                // Also note that if you use this technique, you may need
                // to specify the tasks Timeout method parameter.  I was getting
                // task timeouts when I bumped the loopSize up to 10000
            //    Task.WaitAll(tasks.ToArray());

            //});
            
            //.ContinueWith(t =>
            //{

            //    tick = false; //stop ticking please

            //    var ts = DateTime.Now.Subtract(start);

            //    Console.SetCursorPosition(0, 0);

            //    Console.WriteLine("Requests completed in {0}ms", ts.TotalMilliseconds);

            //    double calc = 1000 / ts.TotalSeconds;

            //    Console.WriteLine("Reqs/Sec: {0}", calc);

            //    Console.WriteLine(" \r\n --- Faults Log ---");
            //    if (log.Length > 0)
            //        Console.WriteLine(log.ToString());
            //    else
            //        Console.WriteLine("No Tasks Faulted");

            //    Console.WriteLine(" \r\n --- HTTP Request Failure Log ---");

            //    foreach (var item in statuscodes)
            //    {
            //        Console.WriteLine("{0}: {1}", item.Key, item.Value);
            //    }

            //    Console.WriteLine("\r\n\r\nPress any key to close.");

            //});

            //Console.Read();
        }
    }
}
