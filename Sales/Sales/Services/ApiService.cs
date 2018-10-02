namespace Sales.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using Sales.Models;

    public class ApiService
    {
        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Please turn on your internet setting.",
                    //Message = Languages.InternetSetting,
                };
            }

            //aqui hace una especie de ping a google
            var response = await CrossConnectivity.Current.IsRemoteReachable("google.com");
            if (!response)
            {
                return new Response()
                {
                    IsSuccess = false,
                    Message = "No internet connection,",
                    //Message = Languages.NoInternet,
                };
            }

            return new Response()
            {
                IsSuccess = true,
                Message = "OK",
            };
        }

        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}";
                var Response = await client.GetAsync(url);

                //aqui leo esto que llega lo leo como un string por que llega como un json:
                var answer = await Response.Content.ReadAsStringAsync();

                if (!Response.IsSuccessStatusCode)
                {
                    return new Response()
                    {
                       IsSuccess = false,
                       Message = answer,
                    };
                }

                //desarializar es convertir de string a objeto:
                var list = JsonConvert.DeserializeObject<List<T>>(answer);

                return new Response()
                {
                   IsSuccess = true,
                   Result = list,
                };

              //serializar es de objecto a string:
            }
            catch (Exception ex)
            {

                return new Response()
                {
                   IsSuccess= false,
                   Message = ex.Message,
                };
            }
        }
    }
}
