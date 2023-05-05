using MediatR;
using project_actaware.Models;

namespace project_actaware.Commands
{
    public class GetProductsByNameCommand : IRequest<IEnumerable<Product>>
    {
        public string ProductName { get; }
        public GetProductsByNameCommand(string productName)
        {
            ProductName = productName;
        }
    }

}
