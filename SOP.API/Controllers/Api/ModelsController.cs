using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SOP.Data.Repositories;
using System.Threading.Tasks;

namespace SOP.API.Controllers.Api
{
    [ApiController]
    [Route("api/{controller}")]
    public class ModelsController : ControllerBase
    {
        private readonly IModelRepository _repository;
        private readonly ILogger<ModelsController> _logger;

        public ModelsController(IModelRepository repository, ILogger<ModelsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetModelsAsync()
        {
            return Ok(_repository.ListModels());
        }

        [HttpGet("{id}")]
        public IActionResult GetModelAsync(string id)
        {
            var model = _repository.FindModel(id);
            if (model == default) return NotFound();
            return Ok(model);
        }
    }
}
