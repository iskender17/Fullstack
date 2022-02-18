using Fullstack.Models;
using Fullstack.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fullstack.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            List<User> model = new List<User>();

            string apiUrl = "https://fbuweb4rentacar.azurewebsites.net/api/user/getusersbypage/" + (pageNumber == null ? "1" : pageNumber.ToString());




            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var table = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Data.DataTable>(data);
                    User y = new User();
                    foreach (DataRow x in table.Rows)
                    {
                        y = new User
                        {
                            UserID = Convert.ToInt32(x[0]),
                            Email = x[1].ToString(),
                            Password = x[2].ToString(),
                            RePassword = x[3].ToString(),
                            RoleID = Convert.ToInt32(x[4])
                        };
                        model.Add(y);
                    }
                }
            }

            string apiUrl2 = "https://fbuweb4rentacar.azurewebsites.net/api/user/getuserscount";
            int recordNumber = 0;

            using (HttpClient client2 = new HttpClient())
            {
                client2.BaseAddress = new Uri(apiUrl);
                client2.DefaultRequestHeaders.Accept.Clear();
                client2.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client2.GetAsync(apiUrl2);
                if (response.IsSuccessStatusCode)
                {
                    var data2 = await response.Content.ReadAsStringAsync();
                    recordNumber = Newtonsoft.Json.JsonConvert.DeserializeObject<Int32>(data2);
                }
            }


            ViewBag.recordNumber = recordNumber;

            return View(model);
        }
        public async Task<IActionResult> Index2(string qs,int pageNumber = 1)
        {
            UserPaginatedViewModel model = new UserPaginatedViewModel();

            HttpClient client = new HttpClient();
            //active sayfadaki kullanıcıları çeken apicall
            string apiUrl = "https://fbuweb4rentacar.azurewebsites.net/api/user/getusersbypage/" + pageNumber.ToString();
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            string responseBody = await response.Content.ReadAsStringAsync();
            //toplam  kullanıcı sayısını veren apicall 
            string apiurl2 = "https://fbuweb4rentacar.azurewebsites.net/api/user/getuserscount";
            HttpResponseMessage response2 = await client.GetAsync(apiurl2);
            //model atamaları 
            model.Users = JsonConvert.DeserializeObject<List<User>>(responseBody).ToList();
            model.TotalRowsNumber = Convert.ToInt32(await response2.Content.ReadAsStringAsync());
            model.ActivePageNumber = pageNumber;


            model.TotalPageNumber = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(model.TotalRowsNumber) / 10));
            model.NextPage = model.ActivePageNumber == model.TotalPageNumber ? false : true;
            model.LastPage=model.ActivePageNumber == model.TotalPageNumber ? false : true;
            model.PreviousPage=model.ActivePageNumber == 1 ?  false : true;
            model.FirstPage=model.ActivePageNumber == 1 ? false : true;
            model.FirstRowNumber = (model.ActivePageNumber - 1) * 10 + 1;
            model.LastRowNumber = model.ActivePageNumber * 10;

            model.Users = model.Users.Where(a => a.Email==qs || qs==null).ToList();
            return View(model);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}