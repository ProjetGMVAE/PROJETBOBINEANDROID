using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetAndroid
{
	public class TodoItemManager
	{
        IRestService restService;
        public Task<List<Produit>> GetTasksAsync ()
		{
			return restService.RefreshDataAsync ();	
		}

		public Task SaveTaskAsync (Produit item, bool isNewItem = false)
		{
			return restService.SaveTodoItemAsync (item, isNewItem);
		}


        public Task EditTaskAsync(string item)
        {
            return restService.RefreshDataAsync(item);
        }

        public Task DeleteTaskAsync (Produit item)
		{
			return restService.DeleteTodoItemAsync (item.D_NUM_PRODUIT);
		}
	}
}
