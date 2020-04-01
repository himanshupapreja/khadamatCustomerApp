using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Khadamat_CustomerApp.Helpers;

namespace Khadamat_CustomerApp.Services.ApiService
{
    public class WebApiRestClient
    {
        private readonly JsonSerializerSettings _jsonSettings;
        HttpClient client = new HttpClient();
        public WebApiRestClient()
        {
            _jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include
            };

            if (Xamarin.Forms.Application.Current.Properties.ContainsKey("AppLocale") && !string.IsNullOrEmpty(Xamarin.Forms.Application.Current.Properties["AppLocale"].ToString()))
            {
                var languageculture = Xamarin.Forms.Application.Current.Properties["AppLocale"].ToString();
                client.DefaultRequestHeaders.Add("Accept-Language", languageculture);
            }
            else
            {
                client.DefaultRequestHeaders.Add("Accept-Language", "en-US");
            }
        }

        public async Task<TResponse> GetAsync<TResponse>(string action)
        {
            Uri baseUri = new Uri(ApiUrl.ApiBaseUrl);
            Uri uri = new Uri(baseUri, action);
            try
            {
                var response = await client.GetAsync(uri);
                var responsedata = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(responsedata);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in GetApi:-", ex.Message);
                return JsonConvert.DeserializeObject<TResponse>(null);
            }
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string action, TRequest request)
        {
            Uri baseUri = new Uri(ApiUrl.ApiBaseUrl);
            Uri uri = new Uri(baseUri, action);
            var json = JsonConvert.SerializeObject(request, _jsonSettings);
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(uri, content).ConfigureAwait(false);
                var responsedata = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(responsedata);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in PostApi:-", ex.Message);
                return JsonConvert.DeserializeObject<TResponse>(null);
            }
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string action, MultipartFormDataContent request)
        {
            Uri baseUri = new Uri(ApiUrl.ApiBaseUrl);
            Uri uri = new Uri(baseUri, action);

            try
            {
                var response = await client.PostAsync(uri, request);
                var responsedata = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(responsedata);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in PostFormApi:-", ex.Message);
                return JsonConvert.DeserializeObject<TResponse>(null);
            }
        }
    }
}
