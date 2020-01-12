namespace Dramarr.Scrapers.Kshow
{
    using System.Collections.Generic;

    using static Dramarr.Core.Enums.EpisodeHelpers;
    using Dramarr.Data.Model;
    using System;

    public class Manager
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="baseUrl"></param>
        public Manager(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Manager()
        {
        }

        #endregion Constructors

        #region Properties

        public string BaseUrl { get; set; } = "https://kshow.to/";

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets all shows
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllShows()
        {
            return Helpers.GetAllShows(BaseUrl);
        }

        /// <summary>
        /// Gets latest shows
        /// </summary>
        /// <returns></returns>
        public List<string> GetLatestShows()
        {
            return Helpers.GetLatestShows(BaseUrl);
        }

        /// <summary>
        /// Gets episodes of a show
        /// </summary>
        /// <param name="episodes"></param>
        /// <param name="show"></param>
        /// <returns></returns>
        public List<Episode> GetEpisodes(List<Episode> episodes, Show show)
        {
            List<Episode> result = new List<Episode>();
            var urls = Helpers.GetAzVideoFiles(BaseUrl, show.Url);

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
        /// Get status of a show
        /// </summary>
        /// <param name="urlTitle"></param>
        /// <returns></returns>
        public Tuple<int, bool> GetStatus(string urlTitle)
        {
            return Helpers.GetStatus(BaseUrl, urlTitle);
        }

        /// <summary>
        /// Gets metadata of show
        /// </summary>
        /// <param name="urlTitle"></param>
        /// <returns></returns>
        public Metadata GetMetadata(string urlTitle)
        {
            return Helpers.GetMetadata(BaseUrl, urlTitle);
        }

        #endregion Methods
    }
}