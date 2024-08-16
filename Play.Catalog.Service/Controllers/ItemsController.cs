using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Play.Catalog.Service.Controllers;

[ApiController]
[Route("items")]
public class ItemsController : ControllerBase
{
    private static readonly List<ItemDto> Items = new()
        {
            new ItemDto(Guid.NewGuid(), "Potion", "Restores 10 HP", 9, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Antidote", "Cures poison", 5, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Sword", "Deals 5 damage", 20, DateTimeOffset.UtcNow),
        };

    [HttpGet]
    public IEnumerable<ItemDto> Get()
    {
        return Items;
    }

    [HttpGet("{id}")]
    public ActionResult<ItemDto> Get(Guid id)
    {
        var item = Items.Find(item => item.Id == id);
        if (item is null)
        {
            return NotFound();
        }
        return item;
    }

    [HttpPost]
    public ActionResult<ItemDto> Post(CreateItemDto itemDto)
    {
        var item = new ItemDto(Guid.NewGuid(), itemDto.Name, itemDto.Description, itemDto.Price, DateTimeOffset.UtcNow);
        Items.Add(item);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public ActionResult Put(Guid id, UpdateItemDto itemDto)
    {
        var item = Items.Find(item => item.Id == id);
        if (item is null)
        {
            return NotFound();
        }
        var updatedItem = item with
        {
            Name = itemDto.Name,
            Description = itemDto.Description,
            Price = itemDto.Price
        };

        var index = Items.FindIndex(item => item.Id == id);
        Items[index] = updatedItem;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(Guid id)
    {
        var item = Items.Find(item => item.Id == id);
        if (item is null)
        {
            return NotFound();
        }
        Items.Remove(item);
        return NoContent();
    }


}
