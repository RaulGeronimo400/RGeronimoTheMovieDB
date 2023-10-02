using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace PL.Controllers
{
    public class MovieController : Controller
    {
        string urlAPI = System.Configuration.ConfigurationManager.AppSettings["API_URI"];
        string api_key = "99bbeb7cf76c2afa5d5896c91060e092";
        string imagekey = "https://image.tmdb.org/t/p/original/";
        string api_token = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI5OWJiZWI3Y2Y3NmMyYWZhNWQ1ODk2YzkxMDYwZTA5MiIsInN1YiI6IjY0ZmU1MmU5ZGI0ZWQ2MTA0MzA4MGY0ZiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.zmf0DOgw0DQpabrkVcpnL6A5-uBnItSPg-xqnNq7q0Q";
        string language = "es-MX";

        string session_id = "9c1d65c9b226cba1a78ec04119f0f6b036a19e64";
        string account_id = "20421989";

        // GET: Movie
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetPopular()
        {
            ML.Movie movie = new ML.Movie();
            movie.Peliculas = new List<object>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlAPI);
                var responseTask = client.GetAsync("movie/popular?language=" + language + "&api_key=" + api_key);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    dynamic json = JObject.Parse(readTask.Result.ToString());

                    foreach (var res in json.results)
                    {
                        ML.Movie ListaPelicula = new ML.Movie();
                        ListaPelicula.IdPelicula = res.id;
                        ListaPelicula.Titulo = res.title;
                        ListaPelicula.Sinopsis = res.overview;
                        ListaPelicula.Puntuacion = res.vote_average;
                        ListaPelicula.Portada = imagekey + res.poster_path;

                        movie.Peliculas.Add(ListaPelicula);
                    }

                }
                return View(movie);
            }
        }

        [HttpGet]
        public ActionResult AddFavorite(ML.Favorito favorito)
        {
            favorito.media_type = "movie";
            favorito.favorite = true;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlAPI);
                var responseTask = client.PostAsJsonAsync<ML.Favorito>("account/" + account_id + "/favorite?api_key=" + api_key + "&session_id=" + session_id, favorito);
                responseTask.Wait();

                var result = responseTask.Result;

                //if (result.IsSuccessStatusCode)
                //{
                //    ViewBag.Message = "Se agrego a favoritos";
                //}
                //else
                //{
                //    ViewBag.Message = "Ocurrio un problema al añadir a favoritos";
                //}
                //return PartialView("Modal");
                return RedirectToAction("GetPopular", "Movie");
            }
        }

        [HttpGet]
        public ActionResult DeleteFavorite(ML.Favorito favorito)
        {
            favorito.media_type = "movie";
            favorito.favorite = false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlAPI);
                var responseTask = client.PostAsJsonAsync<ML.Favorito>("account/" + account_id + "/favorite?api_key=" + api_key + "&session_id=" + session_id, favorito);
                responseTask.Wait();

                var result = responseTask.Result;

                //if (result.IsSuccessStatusCode)
                //{
                //    ViewBag.Message = "Se elimino de favoritos";
                //}
                //else
                //{
                //    ViewBag.Message = "Ocurrio un problema al eliminar de favoritos";
                //}
                //return PartialView("Modal");
                return RedirectToAction("GetFavorite", "Movie");
            }
        }

        [HttpGet]
        public ActionResult GetFavorite()
        {
            ML.Movie movie = new ML.Movie();
            movie.Peliculas = new List<object>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlAPI);

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + api_token);
                client.DefaultRequestHeaders.Add("accept", "application/json");

                var responseTask = client.GetAsync("account/" + account_id + "/favorite/movies?language=" + language + "&api_key=" + api_key);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    dynamic json = JObject.Parse(readTask.Result.ToString());

                    foreach (var res in json.results)
                    {
                        ML.Movie ListaPelicula = new ML.Movie();
                        ListaPelicula.IdPelicula = res.id;
                        ListaPelicula.Titulo = res.title;
                        ListaPelicula.Sinopsis = res.overview;
                        ListaPelicula.Puntuacion = res.vote_average;
                        ListaPelicula.Portada = imagekey + res.poster_path;


                        movie.Peliculas.Add(ListaPelicula);
                    }
                }
                return View(movie);
            }
        }

        //public string GetToken()
        //{
        //    string token = string.Empty;
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(urlAPI);
        //        var responseTask = client.GetAsync("authentication/token/new?api_key=" + api_key);
        //        responseTask.Wait();

        //        var result = responseTask.Result;
        //        if (result.IsSuccessStatusCode)
        //        {
        //            var readTask = result.Content.ReadAsStringAsync();
        //            dynamic json = JObject.Parse(readTask.Result.ToString());
        //            token = json.request_token;
        //        }
        //    }
        //    return token;
        //}

        //public string GetSesionID(string token)
        //{
        //    string sessionId = string.Empty;
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(urlAPI);

        //        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + api_token);
        //        client.DefaultRequestHeaders.Add("accept", "application/json");

        //        var postTask = client.PostAsJsonAsync("authentication/session/new?", token);
        //        postTask.Wait();

        //        var result = postTask.Result;
        //        if (result.IsSuccessStatusCode)
        //        {
        //            var readTask = result.Content.ReadAsStringAsync();
        //            dynamic json = JObject.Parse(readTask.Result.ToString());
        //            sessionId = json.request_token;
        //        }
        //    }
        //    return sessionId;
        //}
    }
}