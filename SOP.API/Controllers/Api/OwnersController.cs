using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SOP.Data.Repositories;
using SOP.Messages.Messages;
using SOP.Models.Entities;
using SOP.ModelsDto.Dto;
using System;
using System.Dynamic;

namespace SOP.API.Controllers.Api
{
    [ApiController]
    [Route("api/{controller}")]
    public class OwnersController : ControllerBase
    {
        private readonly IOwnerRepository _repository;
        private readonly ILogger<OwnersController> _logger;
        private readonly IBus _bus;

        public OwnersController(IOwnerRepository repository, ILogger<OwnersController> logger, IBus bus)
        {
            _repository = repository;
            _bus = bus;
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
        public IActionResult GetOwners(int index = 0, int count = 10)
        {
            var items = _repository.ListOwners(index, count);
            var total = _repository.CountOwners();

            var _links = Paginate("/api/owners", index, count, total);
            var _actions = new
            {
                create = new
                {
                    method = "POST",
                    type = "application/json",
                    name = "Create a new owner",
                    href = "/api/owners"
                },
                delete = new
                {
                    methos = "DELETE",
                    name = "Delete the owner",
                    href = "/api/owners/{id}"
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
        public IActionResult GetOwner(string id)
        {
            if (id == default) return BadRequest();
            var item = _repository.FindOwner(id);
            if (item == default) return NotFound();

            var json = item.ToDynamic();
            json._links = new
            {
                self = new { href = $"/api/owners/{id}" },
            };
            json._actions = new
            {
                update = new
                {
                    method = "PUT",
                    href = $"/api/owners",
                    accept = "application/json"
                },
                delete = new
                {
                    method = "DELETE",
                    href = $"/api/owners/{id}"
                }
            };
            return Ok(json);
        }

        [HttpPost]
        public IActionResult Post([FromBody] OwnerDto dto)
        {
            var result = _repository.CreateOwner(dto);
            PublishNewOwnerMessage(result);
            return GetOwner(dto.Email);
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] dynamic dto)
        {
            var owner = new OwnerDto
            {
                Email = id,
                Name = dto.Name,
                Surname = dto.Surname,
                Birthday = dto.Birthday
            };

            _repository.UpdateOwner(owner);
            return GetOwner(id);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (id == default) return BadRequest();
            _repository.DeleteOwner(id);
            return NoContent();
        }

        private void PublishNewOwnerMessage(Owner owner)
        {
            var message = new NewOwnerMessage
            {
                Email = owner.Email,
                Name = owner.Name,
                Surname = owner.Surname,
                Birthday = owner.Birthday,
                VehicleRegistration = owner.VehicleRegistration,
                ListedAtUtc = DateTime.Now
            };
            _bus.PubSub.Publish(message);
        }
    }
}
