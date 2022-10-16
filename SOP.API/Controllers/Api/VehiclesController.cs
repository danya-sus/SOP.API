using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SOP.Data.Repositories;
using SOP.ModelsDto.Dto;
using System.Dynamic;
using System.Threading.Tasks;

namespace SOP.API.Controllers.Api
{
    [ApiController]
    [Route("api/{controller}")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleRepository _repository;
        private readonly ILogger<VehiclesController> _logger;

        public VehiclesController(IVehicleRepository repository, ILogger<VehiclesController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        private dynamic Paginate(string url, int index, int count, int total)
        {
            dynamic links = new ExpandoObject();
            links.self = new { href = url };
            links.final = new { href = $"{url}?index={total - (total % count)}&count={count}" };
            links.first = new { href = $"{url}?index=0&count={count}" };
            if (index > 0) links.previous = new { href = $"{url}?index={index - count}&count={count}" };
            if (index + count < total) links.next = new { href = $"{url}?index={index + count}&count={count}" };
            return links;
        }

        [HttpGet]
        [Produces("application/hal+json")]
        public IActionResult GetVehicles(int index = 0, int count = 10)
        {
            var items = _repository.ListVehicles(index, count);
            var total =  _repository.CountVehicles();

            var _links = Paginate("/api/vehicles", index, count, total);
            var _actions = new
            {
                create = new
                {
                    method = "POST",
                    type = "application/json",
                    name = "Create a new vehicle",
                    href = "/api/vehicles"
                },
                delete = new
                {
                    method = "DELETE",
                    name = "Delete a vehicle",
                    href = "/api/vehicles/{id}"
                }
            };

            var result = new
            {
                _links,
                _actions,
                index,
                count,
                total,
                items
            };

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetVehicle(string id)
        {
            if (id == null) return BadRequest();
            var item = _repository.FindVehicle(id);
            if (item == default) return NotFound();

            var json = item.ToDynamic();
            json._links = new
            {
                self = new { href = $"/api/vehicles/{id}" },
                vehicleModel = new { href = $"/api/models/{item.ModelCode}" }
            };
            json._actions = new
            {
                update = new
                {
                    method = "PUT",
                    href = $"/api/vehicles/{id}",
                    accept = "application/json"
                },
                delete = new
                {
                    method = "DELETE",
                    href = $"/api/vehicles/{id}"
                }
            };
            return Ok(json);
        }

        [HttpPost]
        public IActionResult Post([FromBody] VehicleDto dto)
        {
            _repository.CreateVehicle(dto);
            return GetVehicle(dto.Registration);
        }

        [HttpPut]
        public IActionResult Put([FromBody] VehicleDto dto)
        {
            _repository.UpdateVehicle(dto);
            return GetVehicle(dto.Registration);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (id == null) return BadRequest();
            _repository.DeleteVehicle(id);
            return NoContent();
        }
    }
}
