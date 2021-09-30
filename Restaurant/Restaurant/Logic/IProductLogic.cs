using Restaurant.Models;
using System.Threading.Tasks;

namespace Restaurant.Logic
{
    public interface IProductLogic
    {
        Task<Product> Buy(BuyRequest buyRequest);
    }
}