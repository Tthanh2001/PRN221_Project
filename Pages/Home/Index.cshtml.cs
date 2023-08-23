using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace PRN221_Project.Pages
{
    public class IndexModel : PageModel
    {


        private readonly HttpClient client;
        private readonly IConfiguration _configuration;
        private readonly string PopularFilm;


        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            PopularFilm = "https://api.themoviedb.org/3/movie/popular?api_key=e9e9d8da18ae29fc430845952232787c&append_to_response=videos"
                ;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            HttpResponseMessage resp = await client.GetAsync(PopularFilm);

            if (!resp.IsSuccessStatusCode)
            {
                return Page();
            }

            var strData = await resp.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };


            //var response = JsonSerializer.Deserialize<ListFilm>(strData, options);
            //List<Genres> listGenres = response.Genres;

            //ListAuthor = listGenres;
            return Page();
        }

        public class ListFilm
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string backdrop_path { get; set; }

            public int genre_ids { get; set; }


        }

    }
}

