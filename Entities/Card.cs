namespace ArduinoWebAPwithUSB.Entities
{
    public class Card
    {
        public int Id { get; set; } // Azonosító, automatikusan generálódik
        public string CardId { get; set; } // Kártya azonosító
        public DateTime CreatedAt { get; set;  } // Időbélyeg, kártya létrehozásának időpontja
    }

}
