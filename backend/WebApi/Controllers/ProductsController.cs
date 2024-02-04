using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApi.Model;
using WebApi.Services;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _dataContext;
		private readonly IHubContext<NotificationMessageModel> _hubContext;

		public ProductsController(DataContext dataContext, IHubContext<NotificationMessageModel> hubContext)
        {
            _dataContext = dataContext;
			_hubContext = hubContext;
		}

        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _dataContext.products.ToListAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get(int id)
        {
            var product = await _dataContext.products.FindAsync(id);
            if (product != null)
                return Ok(product);
            else
                return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Post(Product product)
        {
            await _dataContext.products.AddAsync(product);
            await _dataContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> Put(int id, Product product)
		{
			try
			{
				if (id != product.Id)
				{
					return BadRequest("ID mismatch between route parameter and product data.");
				}

				var existingProduct = await _dataContext.products.FindAsync(id);
				if (existingProduct == null)
				{
					return NotFound("Product not found.");
				}

				_dataContext.Entry(existingProduct).CurrentValues.SetValues(product);
				await _dataContext.SaveChangesAsync();

				await _hubContext.Clients.All.SendAsync("ProductChanged", product);

				return Ok();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error updating product: {ex.Message}");
				return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
			}
		}

		[HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await _dataContext.products.FindAsync(id);
            if (product != null)
            {
                _dataContext.products.Remove(product);
                await _dataContext.SaveChangesAsync();
                return Ok();
            }
            else
                return NotFound();
        }

    }
}
