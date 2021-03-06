﻿namespace Dramarr.Scrapers.Kshow
{
    using Dramarr.Data.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    public class Helpers
    {
        #region Methods

        /// <summary>
        /// Gets all shows
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static List<string> GetAllShows(string url)
        {
            var result = new List<string>();
            result.AddRange(GetShowsFromPageWithNumber(url));
            return result;
        }

        /// <summary>
        /// Gets latest shows
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static List<string> GetLatestShows(string url)
        {
            var result = new List<string>();
            result.AddRange(GetShowsFromFirstPage(url));
            return result;
        }

        /// <summary>
        /// Gets azvideo files
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static List<string> GetAzVideoFiles(string baseUrl, string url)
        {
            var result = new List<string>();

            using (var wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36 Edg/79.0.309.60");
                wc.Headers.Add("referer", $"{baseUrl}");

                var inner = wc.DownloadString($"{baseUrl}shows/{url}");

                var aux = inner
                    .Split(new string[] { "<tbody class=\"list-episode\">" }, StringSplitOptions.None)[1]
                    .Split(new string[] { "</table>" }, StringSplitOptions.None)[0];

                var split = aux.Split(new string[] { $"href=\"{baseUrl}shows/" }, StringSplitOptions.None).ToList();
                split.RemoveAt(0);

                var latestEpisodeNumber = Int32.Parse(split[0].Split(new string[] { "episode-" }, StringSplitOptions.None)[1].Split(new string[] { "/" }, StringSplitOptions.None)[0]);

                for (int i = latestEpisodeNumber; i >= 1; i--)
                {
                    try
                    {
                        var episodeUrl = $"{baseUrl}shows/{url}/episode-{i}";
                        var episodeInner = wc.DownloadString(episodeUrl);
                        var azUrl = episodeInner.Split(new string[] { "http://azvideo.net/" }, StringSplitOptions.None)[1].Split(new string[] { "\"" }, StringSplitOptions.None)[0];
                        result.Add($"http://azvideo.net/{azUrl}");
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets download files
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

        /// <summary>
        /// Gets shows from a specificied page number
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        private static List<string> GetShowsFromPageWithNumber(string baseUrl)
        {
            var result = new List<string>();
            var count = 1;
            var isFinished = false;
            var pages = new List<string>() { "shows/latest/", "shows/popular/" };

            foreach (var page in pages)
            {
                do
                {
                    try
                    {
                        var pagedUrl = $"{baseUrl}{page}page-{count}";

                        using (var wc = new WebClient())
                        {
                            wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36 Edg/79.0.309.60");
                            wc.Headers.Add("referer", $"{baseUrl}");

                            var inner = wc.DownloadString(pagedUrl);

                            var split = inner.Split(new string[] { "<div class=\"col-md-8\" id=\"main\">" }, StringSplitOptions.None)[1]
                                .Split(new string[] { "<ul class=\"pagination pull-right\">" }, StringSplitOptions.None)[0];

                            var shows = split.Split(new string[] { $"<a href=\"{baseUrl}shows/" }, StringSplitOptions.None).ToList();

                            if (shows.Count == 1)
                            {
                                throw new Exception();
                            }

                            shows.RemoveAt(0);

                            foreach (var item in shows)
                            {
                                try
                                {
                                    var res = item.Split(new string[] { "\"" }, StringSplitOptions.None)[0].Split("/")[0];
                                    if (!result.Contains(res))
                                    {
                                        result.Add($"{baseUrl}shows/{res}");
                                    }
                                }
                                catch
                                {

                                }
                            }
                        }
                        count++;
                    }
                    catch
                    {
                        isFinished = true;
                    }

                } while (!isFinished);
            }

            return result;
        }

        /// <summary>
        /// Get shows from first page
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        private static List<string> GetShowsFromFirstPage(string baseUrl)
        {
            var result = new List<string>();
            var pages = new List<string>() { "shows/latest/", "shows/popular/" };

            foreach (var page in pages)
            {
                var pagedUrl = $"{baseUrl}{page}page-1";

                using (var wc = new WebClient())
                {
                    wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36 Edg/79.0.309.60");
                    wc.Headers.Add("referer", $"{baseUrl}");

                    var inner = wc.DownloadString(pagedUrl);

                    var split = inner.Split(new string[] { "<div class=\"col-md-8\" id=\"main\">" }, StringSplitOptions.None)[1]
                        .Split(new string[] { "<ul class=\"pagination pull-right\">" }, StringSplitOptions.None)[0];

                    var shows = split.Split(new string[] { $"<a href=\"{baseUrl}shows/" }, StringSplitOptions.None).ToList();

                    if (shows.Count == 1)
                    {
                        throw new Exception();
                    }

                    shows.RemoveAt(0);

                    foreach (var item in shows)
                    {
                        try
                        {
                            var res = item.Split(new string[] { "\"" }, StringSplitOptions.None)[0].Split("/")[0];
                            if (!result.Contains(res))
                            {
                                result.Add($"{baseUrl}shows/{res}");
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }

            return result;
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
            var realUrl = $"{baseUrl}shows/{urlTitle}/";

            var episodesCount = GetAzVideoFiles(baseUrl, urlTitle);
            var isCompleted = false;

            using (var wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36 Edg/79.0.309.60");
                wc.Headers.Add("referer", $"{baseUrl}");

                var aux = wc.DownloadString(realUrl);
                isCompleted = aux.Contains("Complete");
            }

            return new Tuple<int, bool>(episodesCount.Count, isCompleted);
        }

        /// <summary>
        /// Gets metadata 
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="urlTitle"></param>
        /// <returns></returns>
        public static Metadata GetMetadata(string baseUrl, string urlTitle)
        {
            var realUrl = $"{baseUrl}shows/{urlTitle}/";

            using (var wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36 Edg/79.0.309.60");
                wc.Headers.Add("referer", $"{baseUrl}");

                var aux = wc.DownloadString(realUrl);

                var imageUrl = aux.Split(new string[] { "<img class=\"media-object\" src=\"" }, StringSplitOptions.None)[1]
                .Split('"')[0];

                var realImageUrl = baseUrl.Remove(baseUrl.Length - 1, 1) + imageUrl;

                var plot = "";

                try
                {
                    plot = aux.Split(new string[] { "<span class=\"pl-header-description-text\">" }, StringSplitOptions.None)[1]
                        .Split(new string[] { "<" }, StringSplitOptions.None)[0];
                }
                catch (Exception)
                {
                    plot = aux.Split(new string[] { "<div class=\"desc\">" }, StringSplitOptions.None)[1]
                        .Split(new string[] { "<p>" }, StringSplitOptions.None)[1]
                        .Split(new string[] { "<" }, StringSplitOptions.None)[0];
                }


                var cast = aux.Split(new string[] { "<strong>Cast:</strong>" }, StringSplitOptions.None)[1]
                    .Split(new string[] { $"</p>" }, StringSplitOptions.None)[0];

                var language = "English";

                return new Metadata(realImageUrl, plot, cast, language);
            }
        }

        #endregion Methods
    }
}