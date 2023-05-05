using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using project_actaware.Commands;
using project_actaware.Execptions;
using project_actaware.Models;

namespace project_actaware.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly string _apiKey = "TWÓJ_API_KEY";
        private readonly IMediator _mediator;
        public ProductsController (IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("getProductByBarcode/{barcode}")]
        public async Task<IActionResult> GetProductByBarcode(string barcode)
        {
            try
            {
                Product product = await _mediator.Send(new GetProductByBarcodeCommand(barcode));

                return Ok(product);
            }
            catch(BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getProductsByName/{productName}")]
        public async Task<IActionResult> GetProductByName(string productName)
        {
            try
            {
                IEnumerable<Product> products = await _mediator.Send(new GetProductsByNameCommand(productName));

                return Ok(products);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
