using project_actaware.Commands;
using project_actaware.Models;

namespace project_actaware.Services
{
    public interface IProductService
    {
        Task<ProductDTO> GetByBarcode(string barcode, CancellationToken cancelationToken);

        Task<IEnumerable<ProductDTO>> GetByName(string productName, CancellationToken cancelationToken);
    }
}
