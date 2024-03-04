using Microsoft.EntityFrameworkCore;

namespace Tumanji.Models
{
    [Keyless]
    public class PaninoEntity
    {
        public Guid PaninoID { get; set; }
        public required string Nome { get; set;}
        public required string Descrizione { get; set;}
        public double Prezzo { get; set;}
        public DateTime DataCreazione { get; set;}
        public required Byte[] Immagine { get; set;}
        public required string PathImage { get; set;}
        public bool InMenu { get; set;}
        public bool PaninoMese { get; set;}
    }
}
