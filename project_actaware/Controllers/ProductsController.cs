using MediatR;
using Microsoft.AspNetCore.Mvc;
using project_actaware.Commands;
using project_actaware.Execptions;
using project_actaware.Models;

namespace project_actaware.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController (IMediator mediator)
        {
            _mediator = mediator;
        }
       
        [HttpGet("productByBarcode/{barcode}")]
        public async Task<IActionResult> GetProductByBarcode(string barcode)
        {
            try
            {
                ProductDTO product = await _mediator.Send(new GetProductByBarcodeCommand(barcode));

                return Ok(product);
            }
            catch(BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("productsByName/{productName}")]
        public async Task<IActionResult> GetProductByName(string productName)
        {
            try
            {
                IEnumerable<ProductDTO> products = await _mediator.Send(new GetProductsByNameCommand(productName));

                return Ok(products);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
