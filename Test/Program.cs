using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using System.Web;
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
                responseString = await client.GetStringAsync("https://itunes.apple.com1/search?term=" + HttpUtility.UrlEncode(search) + "&entity=album");
                //Асинхронное добавление кэша.
                AddCache(new Cache() { Response = responseString, Search = search });
                
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

        private static async Task AddCache(Cache cache)
        {
            using (var db = new CacheContext())
            {
                try
                {
                    
                    var item =db.Cache.SingleOrDefault(x => x.Search == cache.Search);
                    if (item != null)
                    {
                        item.Response = cache.Response;
                    }
                    else
                    {
                        db.Cache.Add(cache);
                    }
                    //Вызов в БД без блокировки основного потока
                    db.SaveChangesAsync();
                    

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    
                }
                
            }
            
        }
        private static void GetCache(string search, Action<string> finishAction)
        {
            using (var db = new CacheContext())
            {
                var item = db.Cache.SingleOrDefault(x => x.Search == search);
                if (item != null)
                    finishAction(db.Cache.Single(x => x.Search == search).Response);

            }
            

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
