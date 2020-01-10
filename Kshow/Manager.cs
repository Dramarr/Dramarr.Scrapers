namespace Dramarr.Scrapers.Kshow
{
    using System.Collections.Generic;

    using static Dramarr.Core.Enums.EnumsHelpers;
    using Dramarr.Data.Model;
    using System;

    public class Manager
    {
        #region Constructors

        public Manager(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public Manager()
        {
        }

        #endregion Constructors

        #region Properties

        public string BaseUrl { get; set; } = "https://kshow.to/";

        #endregion Properties

        #region Methods

        public List<string> GetAllShows()
        {
            return Helpers.GetAllShows(BaseUrl);
        }

        public List<string> GetLatestShows()
        {
            return Helpers.GetLatestShows(BaseUrl);
        }

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

        public Tuple<int, bool> GetStatus(string urlTitle)
        {
            return Helpers.GetStatus(BaseUrl, urlTitle);
        }

        #endregion Methods
    }
}