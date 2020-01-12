namespace Dramarr.Scrapers.EstrenosDoramas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using static Dramarr.Core.Enums.EpisodeHelpers;
    using Dramarr.Data.Model;

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

        public string BaseUrl { get; set; } = "https://www.estrenosdoramas.net/";

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets status
        /// </summary>
        /// <param name="urlTitle"></param>
        /// <returns></returns>
        public Tuple<int, bool> GetStatus(string urlTitle)
        {
            return Helpers.GetStatus(urlTitle);
        }

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
        /// Gets episodes
        /// </summary>
        /// <param name="episodes"></param>
        /// <param name="show"></param>
        /// <returns></returns>
        public List<Episode> GetEpisodes(List<Episode> episodes, Show show)
        {
            List<Episode> result = new List<Episode>();
            var urls = Helpers.GetFiles(show.Url);

            foreach (var url in urls)
            {
                result.Add(new Episode(show.Id, url, url.Split("/").Last()) { Status = EpisodeStatus.SCRAPED });
            }

            return result;
        }

        /// <summary>
        /// Gets metadata
        /// </summary>
        /// <param name="urlTitle"></param>
        /// <returns></returns>
        public Metadata GetMetadata(string urlTitle)
        {
            return Helpers.GetMetadata(urlTitle);
        }

        #endregion Methods
    }
}