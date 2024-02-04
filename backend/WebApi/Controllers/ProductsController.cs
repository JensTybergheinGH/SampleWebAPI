using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Model;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _dataContext;
		private readonly WebSocketManager _webSocketManager;

		public ProductsController(DataContext dataContext, WebSocketManager webSocketManager)
        {
            _dataContext = dataContext;
			_webSocketManager = webSocketManager;
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
            if (id == 0)
                return BadRequest();

            _dataContext.Entry(product).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();

			await _webSocketManager.BroadcastMessageAsync("NewProduct", product);

			return Ok();
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
