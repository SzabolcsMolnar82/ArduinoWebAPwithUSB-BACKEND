using ArduinoWebAPwithUSB.Context;
using ArduinoWebAPwithUSB.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArduinoWebAPwithUSB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController(ApplicationDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Card>>> GetCards()
        {
            // Az összes Card lekérése az adatbázisból
            var cards = await context.Cards.ToListAsync();
            return Ok(cards); // A kártyák visszaadása HTTP 200 OK válaszként
        }
    }
}
