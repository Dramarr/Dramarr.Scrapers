﻿namespace Dramarr.Scrapers.MyAsianTv
{
    using Dramarr.Data.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;

    public class Helpers
    {
        #region Methods

        /// <summary>
        /// Gets all shows
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
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

                    using (var wc = new WebClient() { Encoding = Encoding.UTF8 })
                    {
                        wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36 Edg/79.0.309.60");
                        wc.Headers.Add("referer", $"{baseUrl}");

                        var inner = wc.DownloadString(auxUrl);

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
                }
                catch (Exception)
                {
                    finished = true;
                }

            } while (!finished);

            return shows;
        }

        /// <summary>
        /// Gets latest shows
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        public static List<string> GetLatestShows(string baseUrl)
        {
            List<string> shows = new List<string>();
            var url = $"{baseUrl}drama/page-REPLACE/?selOrder=0&selCat=0&selCountry=0&selYear=0&btnFilter=Submit";

            var auxUrl = url.Replace("REPLACE", "1");

            using (var wc = new WebClient() { Encoding = Encoding.UTF8 })
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36 Edg/79.0.309.60");
                wc.Headers.Add("referer", $"{baseUrl}");

                var inner = wc.DownloadString(auxUrl);

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
            }



            return shows;
        }

        /// <summary>
        /// Gets az video files
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static List<string> GetAzVideoFiles(string url)
        {
            var result = new List<string>();

            using (var wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36 Edg/79.0.309.60");
                wc.Headers.Add("referer", $"{url}");

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

        /// <summary>
        /// Gets status of show
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="urlTitle"></param>
        /// <returns></returns>
        public static Tuple<int, bool> GetStatus(string baseUrl, string urlTitle)
        {
            var result = new List<string>();
            var realUrl = $"{baseUrl}drama/{urlTitle}/";

            using (var wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36 Edg/79.0.309.60");
                wc.Headers.Add("referer", $"{baseUrl}");

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

        /// <summary>
        /// Gets metadata of show
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="urlTitle"></param>
        /// <returns></returns>
        public static Metadata GetMetadata(string baseUrl, string urlTitle)
        {
            var result = new List<string>();
            var realUrl = $"{baseUrl}drama/{urlTitle}/";

            using (var wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36 Edg/79.0.309.60");
                wc.Headers.Add("referer", $"{baseUrl}");

                var aux = wc.DownloadString(realUrl);

                var imageUrl = aux.Split(new string[] { "<img class=\"poster\" src=\"" }, StringSplitOptions.None)[1]
                .Split('"')[0];

                var plot = aux.Split(new string[] { "<div class=\"right\">" }, StringSplitOptions.None)[1]
                    .Split(new string[] { $"<div class=\"info\">" }, StringSplitOptions.None)[1]
                    .Split(new string[] { "<hr>" }, StringSplitOptions.None)[0]
                    .Replace("<p>", "")
                    .Replace("</p>", "");


                var cast = aux.Split(new string[] { "<div class=\"left\">" }, StringSplitOptions.None)[1]
                    .Split(new string[] { $"<strong>Cast:</strong>" }, StringSplitOptions.None)[1]
                    .Split(new string[] { "<span>" }, StringSplitOptions.None)[1]
                    .Split(new string[] { "<" }, StringSplitOptions.None)[0];

                var language = "English";

                return new Metadata(imageUrl, plot, cast, language);
            }
        }

        /// <summary>
        /// Gets download files of azfile
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Tuple<string, string> GetDownloadFile(string url)
        {
            using (var wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36 Edg/79.0.309.60");

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