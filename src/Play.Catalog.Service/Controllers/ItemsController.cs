using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDto> items = new()
        {
            new ItemDto(Guid.NewGuid(), "Potion", "Restore a small amount of HP",5,DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Antidote", "Cures poison",7,DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Bronze sword", "Deals a small amount of damage",20,DateTimeOffset.UtcNow)
        };

        [HttpGet]
        public IEnumerable<ItemDto> Get()
        {
            return items;
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetById(Guid id)
        {
            var item = items.SingleOrDefault(item => item.Id == id);
            if (item == null) return NotFound();
            return item;
        }

        [HttpPost]
        public ActionResult<ItemDto> Post(CreateItemDto createItemDto)
        {
            var item = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.UtcNow);
            items.Add(item);

            // return 201 and the link reference to new item by item id
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public ActionResult Put(Guid id, UpdateItemDto updateItemDto)
        {
            var item = items.SingleOrDefault(item => item.Id == id);
            if (item == null) return BadRequest("Can't find this item");
            var itemUpdated = item with
            {
                Name = updateItemDto.Name,
                Description = updateItemDto.Description,
                Price = updateItemDto.Price
            };

            var index = items.FindIndex(item => item.Id == id);
            items[index] = itemUpdated;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var index = items.FindIndex(item => item.Id == id);
            if (index == -1) return BadRequest();
            items.RemoveAt(index);

            return NoContent();
        }
    }
}