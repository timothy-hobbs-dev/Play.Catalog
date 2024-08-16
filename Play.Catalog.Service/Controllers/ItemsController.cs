using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;

namespace Play.Catalog.Service.Controllers;

[ApiController]
[Route("items")]
public class ItemsController : ControllerBase
{
    private readonly ItemsRepository itemsRepository = new();

    [HttpGet]
    public async Task<IEnumerable<ItemDto>> GetAllAsync()
    {
        var items = (await itemsRepository.GetAllAsync())
        .Select(item => item.AsDto());

        return items;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetAsync(Guid id)
    {
        var item = await itemsRepository.GetAsync(id);

        if (item is null)
        {
            return NotFound();
        }
        return item.AsDto();
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto itemDto)
    {
        var item = new Item
        {
            Id = Guid.NewGuid(),
            Name = itemDto.Name,
            Description = itemDto.Description,
            Price = itemDto.Price,
            CreatedDate = DateTimeOffset.UtcNow
        };

        await itemsRepository.CreateAsync(item);
        return CreatedAtAction(nameof(PostAsync), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(Guid id, UpdateItemDto itemDto)
    {
        var existingItem = await itemsRepository.GetAsync(id);
        if (existingItem is null)
        {
            return NotFound();
        }

        existingItem.Name = itemDto.Name;
        existingItem.Description = itemDto.Description;
        existingItem.Price = itemDto.Price;

        await itemsRepository.UpdateAsync(existingItem);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var item = await itemsRepository.GetAsync(id);
        if (item is null)
        {
            return NotFound();
        }
        await itemsRepository.RemoveAsync(id);
        return NoContent();
    }


}
