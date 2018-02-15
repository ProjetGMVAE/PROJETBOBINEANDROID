using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProjetAndroid
{
	public class RestService : IRestService
    {
		HttpClient client;

        public List<Produit> produit { get; private set; }

        List<Produit> IRestService.Items => throw new NotImplementedException();

        public RestService ()
		{
			var authData = string.Format("{0}:{1}", Constants.Username, Constants.Password);
			var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

			client = new HttpClient ();
			client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
		}

		public async Task<List<Produit>> RefreshDataAsync ()
		{
            produit = new List<Produit> ();

			// RestUrl = http://developer.xamarin.com:8081/api/todoitems
			var uri = new Uri (string.Format (Constants.RestUrl, string.Empty));

			try {
				var response = await client.GetAsync (uri);
				if (response.IsSuccessStatusCode) {
					var content = await response.Content.ReadAsStringAsync ();
                    produit = JsonConvert.DeserializeObject <List<Produit>> (content);
				}
			} catch (Exception ex) {
				Debug.WriteLine (@"				ERROR {0}", ex.Message);
			}

			return produit;
		}

        public async Task<List<Produit>> RefreshDataAsync(string item)
        {
            //Items = new List<TodoItem>();

            // RestUrl = http://developer.xamarin.com:8081/api/todoitems
            // http://localhost:5000/api/produit?id=113
            var uri = new Uri(string.Format(Constants.RestUrl, "id=" + item));

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    produit = JsonConvert.DeserializeObject<List<Produit>>(content);

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }

            return produit;
        }

        public async Task SaveTodoItemAsync (Produit item, bool isNewItem = false)
		{
			// RestUrl = http://developer.xamarin.com:8081/api/todoitems
			var uri = new Uri (string.Format (Constants.RestUrl, string.Empty));

			try {
				var json = JsonConvert.SerializeObject (item);
				var content = new StringContent (json, Encoding.UTF8, "application/json");

				HttpResponseMessage response = null;
				if (isNewItem) {
					response = await client.PostAsync (uri, content);
				} else {
					response = await client.PutAsync (uri, content);
				}
				
				if (response.IsSuccessStatusCode) {
					Debug.WriteLine (@"				TodoItem successfully saved.");
				}
				
			} catch (Exception ex) {
				Debug.WriteLine (@"				ERROR {0}", ex.Message);
			}
		}

		public async Task DeleteTodoItemAsync (string id)
		{
			// RestUrl = http://developer.xamarin.com:8081/api/todoitems/{0}
			var uri = new Uri (string.Format (Constants.RestUrl, id));

			try {
				var response = await client.DeleteAsync (uri);

				if (response.IsSuccessStatusCode) {
					Debug.WriteLine (@"				TodoItem successfully deleted.");	
				}
				
			} catch (Exception ex) {
				Debug.WriteLine (@"				ERROR {0}", ex.Message);
			}
		}

        Task IRestService.DeleteTodoItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        Task<List<Produit>> IRestService.RefreshDataAsync()
        {
            throw new NotImplementedException();
        }

        Task<List<Produit>> IRestService.RefreshDataAsync(string item)
        {
            throw new NotImplementedException();
        }

        Task IRestService.SaveTodoItemAsync(Produit item, bool isNewItem)
        {
            throw new NotImplementedException();
        }
    }
}
