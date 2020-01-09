namespace Dramarr.Scrapers.Kshow
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    public class Helpers
    {
        #region Methods

        public static List<string> GetAllShows(string url)
        {
            var result = new List<string>();
            result.AddRange(GetShowsFromPageWithNumber(url));
            return result;
        }

        public static List<string> GetAzVideoFiles(string baseUrl, string url)
        {
            var result = new List<string>();

            using (var wc = new WebClient())
            {
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

        #endregion Methods
    }
}