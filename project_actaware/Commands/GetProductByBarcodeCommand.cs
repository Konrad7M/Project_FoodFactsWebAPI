using MediatR;
using project_actaware.Models;

namespace project_actaware.Commands
{
    public class GetProductByBarcodeCommand : IRequest<Product>
    {
        public string Barcode { get; }
        public GetProductByBarcodeCommand(string barcode)
        {
            Barcode = barcode;
        }
    }
    
}
