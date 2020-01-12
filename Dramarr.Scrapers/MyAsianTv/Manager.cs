namespace Dramarr.Scrapers.MyAsianTv
{
    using System;
    using System.Collections.Generic;

    using static Dramarr.Core.Enums.EnumsHelpers;
    using Dramarr.Data.Model;

    public class Manager
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="episodeUrl"></param>
        /// <param name="allShowsUrl"></param>
        /// <param name="latestEpisodesUrl"></param>
        public Manager(string episodeUrl, string allShowsUrl, string latestEpisodesUrl)
        {
            EpisodeUrl = episodeUrl;
            AllShowsUrl = allShowsUrl;
            LatestEpisodesUrl = latestEpisodesUrl;
        }

        /// <summary>
        /// Constructor
        /// </summary>
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

        /// <summary>
        /// Gets all shows
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllShows()
        {
            return Helpers.GetAllShows(AllShowsUrl);
        }

        /// <summary>
        /// Gets latest shows
        /// </summary>
        /// <returns></returns>
        public List<string> GetLatestShows()
        {
            return Helpers.GetLatestShows(AllShowsUrl);
        }

        /// <summary>
        /// Gets episodes of show
        /// </summary>
        /// <param name="episodes"></param>
        /// <param name="show"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets status of show
        /// </summary>
        /// <param name="urlTitle"></param>
        /// <returns></returns>
        public Tuple<int, bool> GetStatus(string urlTitle)
        {
            return Helpers.GetStatus(AllShowsUrl, urlTitle);
        }

        /// <summary>
        /// Gets metadata of show
        /// </summary>
        /// <param name="urlTitle"></param>
        /// <returns></returns>
        public Metadata GetMetadata(string urlTitle)
        {
            return Helpers.GetMetadata(AllShowsUrl, urlTitle);
        }

        #endregion Methods
    }
}