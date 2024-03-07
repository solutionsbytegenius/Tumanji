using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tumanji.Models
{
    public class ModalDetail
    {
        public required string Nome { get; set; }
        public required string Descrizione { get; set; }
        public double Prezzo { get; set; }
        public required string PathImage { get; set; }

    }
}
