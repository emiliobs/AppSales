namespace Sales.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Sales.Models;

    public class ApiService
    {
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
