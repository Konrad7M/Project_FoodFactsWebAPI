using MediatR;
using project_actaware.Commands;
using project_actaware.Models;
using project_actaware.Services;

namespace project_actaware.CommandHandlers
{
    public class GetProductsByNameCommandHandler : IRequestHandler<GetProductsByNameCommand, IEnumerable<ProductDTO>>
    {
        readonly IProductService _productService;
        public GetProductsByNameCommandHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IEnumerable<ProductDTO>> Handle(GetProductsByNameCommand command, CancellationToken cancelationToken)
        {
            return await _productService.GetByName(command.ProductName, cancelationToken);
        }
    }
}
