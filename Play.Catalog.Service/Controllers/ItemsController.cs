namespace Play.Catalog.Service.Controllers;
{
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

        public IEnumerable<ItemDto> Get()
        {
            return Items;
        }
    }
}