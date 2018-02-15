using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetAndroid
{
    public interface IRestService
    {
        List<Produit> Items { get; }

        Task DeleteTodoItemAsync(string id);
        Task<List<Produit>> RefreshDataAsync();
        Task<List<Produit>> RefreshDataAsync(string item);
        Task SaveTodoItemAsync(Produit item, bool isNewItem = false);
    }
}