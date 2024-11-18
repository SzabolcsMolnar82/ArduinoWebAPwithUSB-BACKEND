using ArduinoWebAPwithUSB.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArduinoWebAPwithUSB.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Card> Cards { get; set; }  // Például egy Products tábla
    }
}
