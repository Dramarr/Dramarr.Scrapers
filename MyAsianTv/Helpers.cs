namespace Dramarr.Scrapers.MyAsianTv
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;

    public class Helpers
    {
        #region Methods

        public static List<string> GetAllShows(string baseUrl)
        {
            List<string> shows = new List<string>();
            var url = $"{baseUrl}drama/page-REPLACE/?selOrder=0&selCat=0&selCountry=0&selYear=0&btnFilter=Submit";
            bool finished = false;
            int i = 1;
            do
            {
                try
                {
                    var auxUrl = url.Replace("REPLACE", i.ToString());
                    WebClient wb = new WebClient() { Encoding = Encoding.UTF8 };

                    var inner = wb.DownloadString(auxUrl);

                    var aux = inner.Split(new string[] { "<div id=\"list-1\" class=\"list\">" }, StringSplitOptions.None)[1];
                    aux = aux.Split(new string[] { "<ul class=\"pagination\"><" }, StringSplitOptions.None)[0];

                    var hrefs = aux.Split(new string[] { $"<h2><a href=\"{baseUrl}drama/" }, StringSplitOptions.None).ToList();
                    hrefs.RemoveAt(0);

                    if (hrefs.Count() == 0)
                    {
                        throw new Exception();
                    }

                    foreach (var item in hrefs)
                    {
                        var href = item.Split(new string[] { "/" }, StringSplitOptions.None)[0];
                        var finalurl = $"{baseUrl}drama/{href}";
                        shows.Add(finalurl);
                    }

                    i++;
                }
                catch (Exception)
                {
                    finished = true;
                }

            } while (!finished);

            return shows;
        }

        public static List<string> GetLatestShows(string baseUrl)
        {
            List<string> shows = new List<string>();
            var url = $"{baseUrl}drama/page-REPLACE/?selOrder=0&selCat=0&selCountry=0&selYear=0&btnFilter=Submit";

            var auxUrl = url.Replace("REPLACE", "1");
            WebClient wb = new WebClient() { Encoding = Encoding.UTF8 };

            var inner = wb.DownloadString(auxUrl);

            var aux = inner.Split(new string[] { "<div id=\"list-1\" class=\"list\">" }, StringSplitOptions.None)[1];
            aux = aux.Split(new string[] { "<ul class=\"pagination\"><" }, StringSplitOptions.None)[0];

            var hrefs = aux.Split(new string[] { $"<h2><a href=\"{baseUrl}drama/" }, StringSplitOptions.None).ToList();
            hrefs.RemoveAt(0);

            if (hrefs.Count() == 0)
            {
                throw new Exception();
            }

            foreach (var item in hrefs)
            {
                var href = item.Split(new string[] { "/" }, StringSplitOptions.None)[0];
                var finalurl = $"{baseUrl}drama/{href}";
                shows.Add(finalurl);
            }

            return shows;
        }

        public static List<string> GetAzVideoFiles(string url)
        {
            var result = new List<string>();

            using (var wc = new WebClient())
            {
                var aux = wc.DownloadString(url).Split(new string[] { "http://azvideo.net/" }, StringSplitOptions.None).ToList();
                aux.RemoveAt(0);

                foreach (var item in aux)
                {
                    var newUlr = item.Split(new string[] { "\"" }, StringSplitOptions.None)[0];
                    result.Add($"http://azvideo.net/{newUlr}");
                }

                return result;
            }
        }

        public static Tuple<int, bool> GetStatus(string baseUrl, string urlTitle)
        {
            var result = new List<string>();
            var realUrl = $"{baseUrl}drama/{urlTitle}/";

            using (var wc = new WebClient())
            {
                var aux = wc.DownloadString(realUrl);

                var status = aux.Split(new string[] { "<strong>Status:</strong>" }, StringSplitOptions.None)[1]
                    .Split(new string[] { "<span>" }, StringSplitOptions.None)[1]
                    .Split(new string[] { "</" }, StringSplitOptions.None)[0];

                var isCompleted = status == "Complete";

                var episodeCount = aux.Split(new string[] { $"{realUrl}episode-" }, StringSplitOptions.None).ToList();

                episodeCount.RemoveAt(0);

                foreach (var item in episodeCount)
                {
                    var newUlr = item.Split(new string[] { "\"" }, StringSplitOptions.None)[0];
                    result.Add($"http://azvideo.net/{newUlr}");
                }

                return new Tuple<int, bool>(result.Count, isCompleted);
            }
        }

        public static Tuple<string, string> GetDownloadFile(string url)
        {
            using (var wc = new WebClient())
            {
                try
                {
                    var inner = wc.DownloadString(url);

                    var aux = inner.Split(new string[] { "https://adf.ly/" }, StringSplitOptions.None)[1];
                    aux = aux.Split(new string[] { "https://" }, StringSplitOptions.None)[1];
                    aux = aux.Split(new string[] { "\"" }, StringSplitOptions.None)[0];

                    var name = inner.Split(new string[] { "<label>File name:</label>" }, StringSplitOptions.None)[1];
                    name = name.Split(new string[] { "</li>" }, StringSplitOptions.None)[0];

                    return new Tuple<string, string>(name, $"https://{aux}");
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        #endregion Methods
    }
}