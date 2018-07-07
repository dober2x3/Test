using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace Test
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            var search = Console.ReadLine();
            while (search != "")
            {
                callWebApi(search).Wait();
                search = Console.ReadLine();
                
            }

        }

        static private async Task callWebApi(string search)
        {
            var responseString = "";
            try
            {
                responseString = await client.GetStringAsync("https://itunes.apple.com/search?term=" + search + "&entity=album");
                AddCache(new Data() { Response = responseString, Search = search, date = DateTime.Now });
                Debug.WriteLine("main {0}", DateTime.Now);
                Show(responseString);
            }
            catch (Exception e)
            {
                GetCache(search, Show);
            }



        }

        static private void Show(string json)
        {
            try
            {
                if (!String.IsNullOrEmpty(json))
                {
                    var x = JsonConvert.DeserializeObject<Result>(json);
                    x.results.ForEach(album =>
                        Console.WriteLine("Артист:{0}, Альбом:{1}", album.artistName, album.collectionName));
                }
                else
                {
                    Console.WriteLine("Сервис недоступен");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        private static async Task AddCache(Data cache)
        {
            using (var db = new Cache())
            {
                var item =await db.Data.SingleOrDefaultAsync(x => x.Search == cache.Search);
                if (item!=null)
                {
                   item.Response = cache.Response;
                    item.date = DateTime.Now;
                }
                else
                {
                    db.Data.Add(cache);
                }
                db.SaveChangesAsync();
            }
            Debug.WriteLine("add {0}",DateTime.Now);
        }
        private static async Task GetCache(string search, Action<string> finishAction)
        {
            using (var db = new Cache())
            {
                var item = await db.Data.SingleOrDefaultAsync(x => x.Search == search);
                if (item!=null)
                    finishAction(db.Data.Single(x => x.Search == search).Response);
              
            }
            Debug.WriteLine("Get {0}", DateTime.Now);
            
        }
    }

    public class Result
    {
        public int resultCount { get; set; }

        public List<SearchAlbum> results { get; set; }
    }
    public class SearchAlbum
    {
        public String wrapperType { get; set; }
        public String artistType { get; set; }
        public String artistName { get; set; }
        public String artistId { get; set; }
        public String amgArtistId { get; set; }
        public String collectionTyp { get; set; }
        public String collectionId { get; set; }
        public String collectionName { get; set; }
        public String collectionCensoredName { get; set; }
        public String artistViewUrl { get; set; }
        public String collectionViewUrl { get; set; }
        public String artworkUrl60 { get; set; }
        public String collectionPrice { get; set; }
        public String collectionExplicitness { get; set; }
        public String trackCount { get; set; }
        public String releaseDate { get; set; }
        public String primaryGenreName { get; set; }
    }
}
