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
        /// Generics the rest call using http client for getting web api results.
        /// we can use all type of Web Api Method like GET,POST,PUT,DELETE with is single web service method
        /// </summary>

        public static ServiceResponse<String> GenericRestCallUsingHttpClient<T, Tr>(string requestURL, HttpMethod method, Tr content)
        {
            var serviceResponse = new ServiceResponse<String> { IsSuccess = false };
            string returnValue = string.Empty;
            try
            {
                //using Httpclient for calling WEB API
                using (var client = new HttpClient())
                {
                    //Assining web Api URL 
                    client.BaseAddress = new Uri(requestURL);
                    client.DefaultRequestHeaders.Accept.Clear();

                    //Assining web api request/response format is JSON
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = null;

                       //Check parameter  if  method is GET/DELETE 
                        if (method == HttpMethod.Get || method == HttpMethod.Delete)
                        {
                            //if method is get then call GetAsync  
                            if (method == HttpMethod.Get)
                            {
                            response = client.GetAsync(requestURL).Result;

                            }
                           else //if method is delete then call DeleteAsync  
                            {
                            response = client.DeleteAsync(requestURL).Result;

                            }
                           // if success code is 200
                            if (response.IsSuccessStatusCode)
                            {
                              // if success code is not 200 then assign response true 
                                serviceResponse.IsSuccess = true;
                            // we can get the json format string by calling ReadAsStringAsync
                                serviceResponse.Data = response.Content.ReadAsStringAsync().Result;
                            }
                            else
                            {
                               // if success code is not 200 then assign response is fail to get data 
                                serviceResponse.IsSuccess = false;
                            // we can get actuall error from the response
                                serviceResponse.Message = response.Content.ReadAsStringAsync().Result;
                            }
                        }
                        else
                        {

                        //if method is POST/PUT  then set Body(parameter) for web api
                            string Body = JsonConvert.SerializeObject(content, Formatting.None,
                                                                                    new JsonSerializerSettings
                                                                                    {
                                                                                        NullValueHandling = NullValueHandling.Ignore
                                                                                    });

                            switch (method.Method)
                            {
                            case "POST": //if method is POST then call PostAsync  
                                response = client.PostAsync(requestURL, new StringContent(Body, Encoding.UTF8, "application/json")).Result;
                                    break;
                            case "PUT": //if method is PUT then call PutAsync  
                                response = client.PutAsync(requestURL, new StringContent(Body, Encoding.UTF8, "application/json")).Result;
                                    break;
                            }
                            if (response.IsSuccessStatusCode)
                            {
                              //if success code is not 200 then assign response true 
                                serviceResponse.IsSuccess = true;
                               //we can get the json format string by calling ReadAsStringAsync
                                serviceResponse.Data = response.Content.ReadAsStringAsync().Result;
                            }
                            else
                            {
                               //if success code is not 200 then assign response is fail to get data 
                                serviceResponse.IsSuccess = false;
                              //we can get actuall error from the response
                                serviceResponse.Message = response.Content.ReadAsStringAsync().Result;
                            }
                        }

                    
                }
            }
            catch (Exception ex)
            {
                serviceResponse.IsSuccess = false;
                serviceResponse.Message = "Exception generated: " + ex.Message;
            }

            return serviceResponse;
        }
    }
}
