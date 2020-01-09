namespace Dramarr.Scrapers.MyAsianTv
{
    using System;
    using System.Collections.Generic;

    using static Dramarr.Core.Enums.EnumsHelpers;
    using Dramarr.Data.Model;

    public class Manager
    {
        #region Constructors

        public Manager(string episodeUrl, string allShowsUrl, string latestEpisodesUrl)
        {
            EpisodeUrl = episodeUrl;
            AllShowsUrl = allShowsUrl;
            LatestEpisodesUrl = latestEpisodesUrl;
        }

        public Manager()
        {
        }

        #endregion Constructors

        #region Properties

        public string AllShowsUrl { get; set; } = $"https://myasiantv.to/";

        public string EpisodeUrl { get; set; } = $"https://myasiantv.to/drama/<dorama>/download/";

        public string LatestEpisodesUrl { get; set; } = $"https://myasiantv.to/";

        #endregion Properties

        #region Methods

        public List<string> GetAllShows()
        {
            return Helpers.GetAllShows(AllShowsUrl);
        }

        public List<string> GetLatestShows()
        {
            return Helpers.GetLatestShows(AllShowsUrl);
        }

        public List<Episode> GetEpisodes(List<Episode> episodes, Show show)
        {
            List<Episode> result = new List<Episode>();
            var urls = Helpers.GetAzVideoFiles(EpisodeUrl.Replace("<dorama>", show.Url));

            foreach (var url in urls)
            {
                var data = Helpers.GetDownloadFile(url);
                if (data != null)
                {
                    result.Add(new Episode(show.Id, data.Item2, data.Item1) { Status = EpisodeStatus.SCRAPED });
                }
            }

            return result;
        }

        public Tuple<int, bool> GetStatus(string url)
        {
            return new Tuple<int, bool>(0, false);
        }

        #endregion Methods
    }
}