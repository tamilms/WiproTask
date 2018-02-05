using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WiproTask
{
    public static class ServiceManager
    {
        /// <summary>
        /// Generics the rest call using http client.
        /// </summary>
        /// <returns>The rest call using http client.</returns>
        /// <param name="requestURL">Request URL.</param>
        /// <param name="method">Method.</param>
        /// <param name="content">Content.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <typeparam name="Tr">The 2nd type parameter.</typeparam>
        public static ServiceResponse<String> GenericRestCallUsingHttpClient<T, Tr>(string requestURL, HttpMethod method, Tr content)
        {
            var serviceResponse = new ServiceResponse<String> { IsSuccess = false };
            string returnValue = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(requestURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = null;
                        if (method == HttpMethod.Get || method == HttpMethod.Delete)
                        {

                            if (method == HttpMethod.Get)
                            {
                            response = client.GetAsync(requestURL).Result;

                            }
                            else
                            {
                            response = client.DeleteAsync(requestURL).Result;

                            }

                            if (response.IsSuccessStatusCode)
                            {
                                serviceResponse.IsSuccess = true;
                                serviceResponse.Data = response.Content.ReadAsStringAsync().Result;
                            }
                            else
                            {
                                //serviceResponse.Data = response.Content.ReadAsStringAsync().Result;

                                serviceResponse.IsSuccess = false;
                                serviceResponse.Message = response.Content.ReadAsStringAsync().Result;
                            }
                        }
                        else
                        {

                            string Body = JsonConvert.SerializeObject(content, Formatting.None,
                                                                                    new JsonSerializerSettings
                                                                                    {
                                                                                        NullValueHandling = NullValueHandling.Ignore
                                                                                    });

                            switch (method.Method)
                            {
                                case "POST":
                                response = client.PostAsync(requestURL, new StringContent(Body, Encoding.UTF8, "application/json")).Result;
                                    break;
                                case "PUT":
                                response = client.PutAsync(requestURL, new StringContent(Body, Encoding.UTF8, "application/json")).Result;
                                    break;
                            }
                            if (response.IsSuccessStatusCode)
                            {
                                serviceResponse.IsSuccess = true;
                                serviceResponse.Data = response.Content.ReadAsStringAsync().Result;
                            }
                            else
                            {
                                //serviceResponse.Data = response.Content.ReadAsStringAsync().Result;

                                serviceResponse.IsSuccess = false;
                                serviceResponse.Message = response.Content.ReadAsStringAsync().Result;
                            }
                        }

                    
                }
            }
            catch (Exception ex)
            {
                serviceResponse.IsSuccess = false;
                serviceResponse.Message = "Exception generated: " + ex.Message;
                //returnValue = "Exception generated: " + ex.Message; //report the exception message if one was hit
            }
            return serviceResponse;
        }
    }
}
