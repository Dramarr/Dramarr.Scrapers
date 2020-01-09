namespace Dramarr.Scrapers.EstrenosDoramas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using static Dramarr.Core.Enums.EnumsHelpers;
    using Dramarr.Data.Model;

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

        public string BaseUrl { get; set; } = "https://www.estrenosdoramas.net/";

        #endregion Properties

        #region Methods

        public Tuple<int, bool> GetStatus(string url)
        {
            return new Tuple<int, bool>(0, false);
        }

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
            var urls = Helpers.GetFiles(show.Url);

            foreach (var url in urls)
            {
                result.Add(new Episode(show.Id, url, url.Split("/").Last()) { Status = EpisodeStatus.SCRAPED });
            }

            return result;
        }

        #endregion Methods
    }
}