1. Create Model 

  public class Product
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [Range(10,1000,ErrorMessage ="Price shoul between 10 to 1000")]
        public decimal Price { get; set; }
    }

2. Add namespaces 

Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Design
Microsoft.AspNetCore.Cors
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools

3. Add DataContext

 public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Product> products { get; set; }
    }

4. Add ConnectionString

"ConnectionStrings": {
    "DBCS": "Data Source=DESKTOP-2EN9JEA;Initial Catalog=SampleWebApi; Integrated Security=true"
  }

5. Add Service in program.cs

builder.Services.AddDbContext<DataContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("DBCS")));

6. Add Cors 

app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

7. Run Migration 

8. Update-database 

9. Add Controller ProductsController

  public class ProductsController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public ProductsController(DataContext dataContext)
        {
            _dataContext = dataContext;
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

10. Run and test all endpoints 

11. Add Client Application

12. Add bootstrap package

13. Update Index.cshtml

<div>
     <div class="row">
        <div class="col-md-2">
            Name
        </div>
        <div class="col-md-4">
            <input type="text" style="margin: 5px;" class="form-control" id="txtName" placeholder="Name">
        </div>
        <div class="col-md-2">
            Price
        </div>
        <div class="col-md-4">
            <input type="number" style="margin: 5px;" class="form-control" placeholder="Price" id="txtPrice">
        </div>
    </div>
    <div class="row" style="margin: 5px;">
            <div class="col">
                <button type="button" class="btn btn-primary" onclick="return Create()">Add</button>
                &nbsp;&nbsp;&nbsp;
                <button type="button" class="btn btn-primary" onclick="return Update()">Update</button>
                &nbsp;&nbsp;&nbsp;
                <button type="button" class="btn btn-secondary">Cancel</button>
            </div>
        </div>
</div>

<table class="table table-hover" id="tblPatients">
    <thead>
        <tr>
            <th scope="col">#</th>
            <th scope="col">Name</th>
            <th scope="col">Price</th>
            <th scope="col">Actions</th>
        </tr>
    </thead>
    <tbody id="tblBody">
    </tbody>
</table>

 <script type="text/javascript" src="https://code.jquery.com/jquery-2.1.1.min.js"></script>

        <script>

    $(document).ready(function () {
        GetDetails();
    });

    let ProductId = '';

    function onEdit(id) {
        ProductId = id;
          var options = {};
            options.url = "http://localhost:5140/api/Products/" + id;
            options.type = "GET";
            options.success = function (msg) {
                console.log(msg);               
                 if (msg!== undefined && msg !== null) {                    
                    $("#txtName").val(msg.name);
                    $("#txtPrice").val(msg.price);                   
                }
            },
                options.error = function () {
                    console.log("Error while calling the Web API!");
                };
            $.ajax(options);
    }

    function onDelete(id) {
        if (window.confirm("Are you sure to delete this record?")) {
            var options = {};
            options.url = "http://localhost:5140/api/Products/" + id;
            options.type = "DELETE";         
            options.success = function (msg) {
                console.log(msg);
                if (msg!== undefined && msg !== null) {
                     clear()
                    GetDetails();
                    alert('Record deleted');
                }                
            },
                options.error = function () {
                    console.log("Error while calling the Web API!");
                };
            $.ajax(options);
        }

    }

    function GetDetails() {
        var options = {};
        options.url = "http://localhost:5140/api/Products";
        options.type = "GET";       
        options.contentType = "application/json";
        options.dataType = "html";

        options.success = function (msg) {
            $('#tblBody').html('');
              if (msg!== undefined && msg !== null) {
                const dt = JSON.parse(msg);
                for (let i = 0; i < dt.length; i++) {
                    var row = "<tr>";
                    row += "<td> " + parseInt(i + 1) + "</td>";                   
                    row += "<td> " + dt[i].name + "</td>"; row += "<td> " + dt[i].price + "</td>"; 
                    row += "<td> <button class='btn btn-primary'  onclick='return onEdit(" + dt[i].id + ")'>Edit</button>&nbsp;|&nbsp;<button class='btn btn - danger' onclick='return onDelete(" + dt[i].id + ")'>Delete</button> </td>";
                    $('#tblBody').append(row);               
                    }
              }
        },
            options.error = function () {
                console.log("Error while calling the Web API!");
            };
        $.ajax(options);
    }

    function Create() {

            var options = {};
            options.url = "http://localhost:5140/api/Products";
            options.type = "POST";
            var obj = {};
            obj.Name = $("#txtName").val();
            obj.Price = $("#txtPrice").val();
            options.data = JSON.stringify(obj);
            options.contentType = "application/json";
            options.dataType = "html";

            options.success = function (msg) {
                if (msg!== undefined && msg !== null) {
                    clear()
                    GetDetails();
                    alert('Record added')
                }
                
            },
                options.error = function () {
                    console.log("Error while calling the Web API!");
                };
            $.ajax(options);

    }

     function Update() {
            var options = {};
            options.url = "http://localhost:5140/api/Products/" + ProductId;
            options.type = "PUT";
            var obj = {};
            obj.Id = ProductId;
            obj.Name = $("#txtName").val();
            obj.Price = $("#txtPrice").val();
            options.data = JSON.stringify(obj);
            options.contentType = "application/json";
            options.dataType = "html";

            options.success = function (msg) {
                console.log(msg);

                 if (msg!== undefined && msg !== null) {
                     clear()
                    GetDetails();
                    alert('Record updated')
                }
            },
                options.error = function () {
                    console.log("Error while calling the Web API!");
                };
            $.ajax(options);
        }


        function clear(){
            $("#txtName").val('');
            $("#txtPrice").val('');
        }
    </script>