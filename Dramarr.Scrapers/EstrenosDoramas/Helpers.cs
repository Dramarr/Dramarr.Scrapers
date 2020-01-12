namespace Dramarr.Scrapers.EstrenosDoramas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Dramarr.Data.Model;
    using Newtonsoft.Json;

    using RestSharp;

    public static class Helpers
    {
        #region Methods

        public static List<string> GetAllShows(string url)
        {
            var result = new List<string>();

            using (var wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36 Edg/79.0.309.60");
                wc.Headers.Add("referer", $"https://www.estrenosdoramas.net");

                var inner = wc.DownloadString(url);

                var aux = inner.Split(new string[] { "<ul id=\"dramaslist\">" }, StringSplitOptions.None)[1];
                aux = aux.Split(new string[] { "</ul>" }, StringSplitOptions.None)[0];

                var splitHref = aux.Split(new string[] { "<a href=\"" }, StringSplitOptions.None).ToList();
                splitHref.RemoveAt(0);

                foreach (var item in splitHref)
                {
                    var showUrl = item.Split(new string[] { "\"" }, StringSplitOptions.None)[0];
                    result.Add(showUrl);
                }
            }
            return result;
        }

        public static List<string> GetLatestShows(string url)
        {
            var result = new List<string>();

            using (var wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36 Edg/79.0.309.60");
                wc.Headers.Add("referer", $"https://www.estrenosdoramas.net");

                var inner = wc.DownloadString(url);

                var aux = inner.Split(new string[] { "<h3 class=\"widgetitulo\">Últimas Series</h3>" }, StringSplitOptions.None)[1];
                aux = aux.Split(new string[] { "<div class=\"clear\"></div>" }, StringSplitOptions.None)[0];

                var splitHref = aux.Split(new string[] { "<a href=\"" }, StringSplitOptions.None).ToList();
                splitHref.RemoveAt(0);

                foreach (var item in splitHref)
                {
                    var showUrl = item.Split(new string[] { "\"" }, StringSplitOptions.None)[0];
                    result.Add(showUrl);
                }
            }
            return result;
        }

        public static List<string> GetFiles(string url)
        {
            var episodes = GetEpisodesPreDownloadFiles(url);
            var result = GetDownloadFiles(episodes);
            return result;
        }

        private static List<string> GetDownloadFiles(List<string> episodes)
        {
            var result = new List<string>();

            foreach (var item in episodes)
            {
                using (WebClient wc = new WebClient())
                {
                    var first = wc.DownloadString(item);

                    var key = first.Split(new string[] { "https://repro3.estrenosdoramas.us/repro/amz/examples/pi7.php?key=" }, StringSplitOptions.None)[1]
                        .Split(new string[] { "\"" }, StringSplitOptions.None)[0];

                    var a = wc.DownloadString($"https://repro3.estrenosdoramas.us/repro/amz/examples/pi7.php?key={key}");

                    var id = a.Split(new string[] { "id: '" }, StringSplitOptions.None)[1].Split(new string[] { "'" }, StringSplitOptions.None)[0];
                    var tk = a.Split(new string[] { "tk: '" }, StringSplitOptions.None)[1].Split(new string[] { "'" }, StringSplitOptions.None)[0];

                    var client = new RestClient("https://repro3.estrenosdoramas.us/repro/amz/examples/pejnko202.php");
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Length", "72");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Host", "repro3.estrenosdoramas.us");
                    request.AddHeader("Postman-Token", "2e33f51e-8feb-40b6-861b-a5f1e537338e,029c4cbc-9206-4ffc-b9c9-6e8ed6267f7a");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Accept", "*/*");
                    request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36 Edg/79.0.309.60 Chrome/69.0.3497.100 Safari/537.36");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    request.AddHeader("Orig", "https://repro3.estrenosdoramas.us");
                    request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                    request.AddParameter("undefined", $"acc=token&id={id}&tk={tk}", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);

                    var myOjbect = JsonConvert.DeserializeObject(response.Content).ToString();

                    var link = myOjbect.Split(new string[] { "{\r\n  \"urlremoto\": \"" }, StringSplitOptions.None)[1].Split(new string[] { "\"" }, StringSplitOptions.None)[0];
                    var name = link.Split('/').Last();

                    result.Add(link);
                }
            }
            return result;
        }

        private static List<string> GetEpisodesPreDownloadFiles(string url)
        {
            var res = new List<string>();

            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36 Edg/79.0.309.60");
                wc.Headers.Add("referer", $"https://www.estrenosdoramas.net");

                var inner = wc.DownloadString($"https://www.estrenosdoramas.net{url}")
                    .Split(new string[] { "<div class=\"listanime\">" }, StringSplitOptions.None)[1]
                    .Split(new string[] { "<li class=\"current\">" }, StringSplitOptions.None)[0]
                    .Split(new string[] { "href=\"" }, StringSplitOptions.None).ToList();

                inner.RemoveAt(0);

                foreach (var item in inner)
                {
                    var episodeUlr = item.Split(new string[] { "\"" }, StringSplitOptions.None)[0];
                    res.Add(episodeUlr);
                }
            }

            return res;
        }

        public static Tuple<int, bool> GetStatus(string url)
        {
            var episodesCount = GetEpisodesPreDownloadFiles(url);
            var isCompleted = false;

            using (var wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36 Edg/79.0.309.60");
                wc.Headers.Add("referer", $"https://www.estrenosdoramas.net");

                var aux = wc.DownloadString($"https://www.estrenosdoramas.net{url}");
                isCompleted = !aux.Contains("<b>Emisión</b>");
            }

            return new Tuple<int, bool>(episodesCount.Count, isCompleted);
        }


        public static Metadata GetMetadata(string urlTitle)
        {
            using (var wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36 Edg/79.0.309.60");
                wc.Headers.Add("referer", $"https://www.estrenosdoramas.net");

                var aux = wc.DownloadString($"https://www.estrenosdoramas.net{urlTitle}");

                var imageUrl = aux.Split(new string[] { "<img style=\"float: left; margin-bottom: 1em; margin-right: 1em;\" src=\"" }, StringSplitOptions.None)[1]
                .Split('"')[0];

                var plot = aux.Split(new string[] { "<b>Sinopsis:</b>" }, StringSplitOptions.None)[1]
                    .Split(new string[] { "/>" }, StringSplitOptions.None)[1]
                    .Split(new string[] { "<" }, StringSplitOptions.None)[0]
                    .Replace("\n", "");

                var cast = "";

                var language = "Spanish";

                return new Metadata(imageUrl, plot, cast, language);
            }
        }


        #endregion Methods
    }
}