using MvcApiManagement.Models;
using System.Net.Http.Headers;
using System.Web;

namespace MvcApiManagement.Services
{
    public class ServiceApiManagement
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApiEmpleados;
        private string UrlApiDepartamentos;

        public ServiceApiManagement(IConfiguration configuration)
        {
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
            this.UrlApiEmpleados = configuration.GetValue<string>("ApiUrls:ApiEmpleados");
            this.UrlApiDepartamentos = configuration.GetValue<string>("ApiUrls:ApiDepartamentos");
        }

        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                //Debemos enviar una cadena vacia al final del request
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                string request = "data?" + queryString;
                //NO SE UTILIZA BASEADDRESS
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.CacheControl = CacheControlHeaderValue.Parse("no-cache");
                //La peticion se realiza en conjunto, (url y request)
                HttpResponseMessage response = await client.GetAsync(this.UrlApiEmpleados + request);
                if(response.IsSuccessStatusCode)
                {
                    List<Empleado> data = await response.Content.ReadAsAsync<List<Empleado>>();
                    return data;
                }
                else
                {
                    return null;
                }
            }
        }
        public async Task<List<Departamento>> GetDepartamentosAsync(string subscripcion)
        {
            using (HttpClient client = new HttpClient())
            {
                //Debemos enviar una cadena vacia al final del request
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                string request = "/api/Departamentos?" + queryString;
                //NO SE UTILIZA BASEADDRESS
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.CacheControl = CacheControlHeaderValue.Parse("no-cache");
                //Debemos añadir la subscripcion mediante una key
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscripcion);
                //La peticion se realiza en conjunto, (url y request)
                HttpResponseMessage response = await client.GetAsync(this.UrlApiDepartamentos + request);
                if (response.IsSuccessStatusCode)
                {
                    List<Departamento> data = await response.Content.ReadAsAsync<List<Departamento>>();
                    return data;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
