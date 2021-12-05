using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace GoogleParsing.Models
{
    /// <summary>
    /// Represents reference's class
    /// </summary>
    public class Reference : INotifyPropertyChanged
    {
        /// <summary>
        /// Represents field for query string
        /// </summary>
        private string queryString;
        private IEnumerable<string> searchResult;

        /// <summary>
        /// Represents result of Google searching
        /// </summary>
        public IEnumerable<string> SearchResult
        {
            get => searchResult;

            private set
            {
                searchResult = value;
                OnPropertyChanged(nameof(SearchResult));
            }
        }

        /// <summary>
        /// Constructor returns new instance of Reference
        /// </summary>
        /// <param name="queryString">Query string for searching in Google</param>
        public Reference(string queryString = "ask the google")
        {
            this.queryString = queryString;
        }

        /// <summary>
        /// Method parses Google output
        /// </summary>
        /// <returns>Returns nothing (void)</returns>
        public async Task GoToGoogleAsync()
        {
            var queries = queryString.Split(' ');
            string stringQueries = String.Empty;
            for(int i = 0; i < queries.Length; i++)
            {
                stringQueries += "+" + queries[i];
            }

            WebRequest request = WebRequest.Create($"https://www.google.com/search?q={stringQueries}");
            WebResponse response = await request.GetResponseAsync();

            string data = String.Empty;
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    data = await reader.ReadToEndAsync();
                }
            }
            response.Close();

            var html = new HtmlDocument();
            html.LoadHtml(data);
            var document = html.DocumentNode;
            var refs = document.QuerySelectorAll("a > h3");

            SearchResult = refs.ToList().Select(item => this.RecursiveHtmlDecode(item.InnerText));
        }

        /// <summary>
        /// Method decodes html string
        /// </summary>
        /// <param name="str">Html string</param>
        /// <returns>Html string</returns>
        private string RecursiveHtmlDecode(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;
            var tmp = HttpUtility.HtmlDecode(str);
            while (tmp != str)
            {
                str = tmp;
                tmp = HttpUtility.HtmlDecode(str);
            }
            return str;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
