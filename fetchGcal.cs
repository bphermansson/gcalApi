using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Text.Json;

namespace GcalData
{
    public class fetchGcal
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/calendar-dotnet-quickstart.json
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "Google Calendar API .NET Quickstart";
        static List<OneItem> AllEvents = new List<OneItem>();
        static string jsonString;

        public static string gcalfetch()
        {
            UserCredential credential;
            AllEvents.Clear();
            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            
            CalendarListResource.ListRequest clr = service.CalendarList.List();
            CalendarList cals = clr.Execute();
            foreach (CalendarListEntry calId in cals.Items) {
                //Console.WriteLine("Id: " + calId.Id.ToString());
                var oneCal = calId.Id.ToString();
                listEvents(service, oneCal);
            }
            Console.WriteLine("All done");
            try {
                //Console.WriteLine(AllEvents.Count().ToString());
                /*foreach (var t in AllEvents)
                {
                    //Console.WriteLine("t: " + t.Summary);
                    Console.WriteLine($"{t.Starttime}: {t.Summary} Creator:{t.Creator}");                
                }*/
                AllEvents.Sort((x, y) => DateTime.Compare(x.Starttime, y.Starttime));
                foreach (var t in AllEvents)
                {
                   Console.WriteLine($"{t.Starttime.ToString("yyyy-MM-dd hh:mm")} - {t.Endtime.ToString("yyyy-MM-dd hh:mm")}: {t.Summary} Creator:{t.Creator}");  
                }
                jsonString = JsonSerializer.Serialize(AllEvents);
                //Console.WriteLine($"{jsonString}");
            }
            catch{}
            Console.WriteLine($"{jsonString}");
            return jsonString;
        }
        static Events listEvents(CalendarService service, string calendar){
            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List(calendar);
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
        
            // List events.
            Events events = request.Execute();
            //Console.WriteLine("Upcoming events:");
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    string when = eventItem.Start.DateTime.ToString();
                    if (String.IsNullOrEmpty(when))
                    {
                        when = eventItem.Start.Date;
                    }
                    string et = eventItem.End.DateTime.ToString();
                    if (String.IsNullOrEmpty(et))
                    {
                        et = eventItem.End.Date;
                    }
                    //Console.WriteLine("{0} ({1})", eventItem.Summary, when);

                    var cultureInfo = new CultureInfo("sv-SE");
                    DateTime dtWhen = DateTime.Parse(when, cultureInfo);
                    
                    OneItem oneitem = new OneItem();
                    oneitem.Summary = eventItem.Summary;
                    oneitem.Starttime = dtWhen;
                    oneitem.Endtime = DateTime.Parse(et);
                    oneitem.Creator = eventItem.Creator.DisplayName;
                    AllEvents.Add(oneitem);
                }
            }
            else
            {
                //Console.WriteLine("No upcoming events found.");
            }
            //Console.WriteLine("Done.");     
            //Console.WriteLine(AllEvents.Count);            
            return events;
        }
    }
}