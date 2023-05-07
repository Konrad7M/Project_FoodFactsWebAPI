using MediatR;
using project_actaware.Commands;
using project_actaware.Models;
using project_actaware.Services;


namespace project_actaware.CommandHandlers;

public class GetProductByBarcodeCommandHandler: IRequestHandler<GetProductByBarcodeCommand,ProductDTO>
{
    private readonly IProductService _productService;

    public GetProductByBarcodeCommandHandler(IProductService productService)
    {
        _productService = productService;
    }
    public async Task<ProductDTO> Handle(GetProductByBarcodeCommand command,CancellationToken cancelationToken)
    {
        return await _productService.GetByBarcode(command.Barcode, cancelationToken);
    }
}


