using MediatR;
using project_actaware.Models;

namespace project_actaware.Commands
{
    public class GetProductByBarcodeCommand : IRequest<Product>
    {
        public int Barcode { get; }
        public GetProductByBarcodeCommand(int barcode)
        {
            this.Barcode = barcode;
        }
    }
    
}
