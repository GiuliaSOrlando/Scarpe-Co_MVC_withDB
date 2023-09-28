using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scarpe_Co_MVC
{
    public class Prodotto
    {
        public int IdProdotto { get; set; }
        public string Nome { get; set; }
        public decimal Prezzo { get; set; }
        public string Descrizione { get; set; }
        public string CoverImg { get; set; }
        public string Img1 { get; set; }
        public string Img2 { get; set; }
        [Display(Name = "Disponibile")]
        public bool Disponibile { get; set; }
        public Prodotto() { }
        public Prodotto (int idProdotto, string nome, decimal prezzo, string descrizione, string coverimg, string img1, string img2, bool disponibile)
        {
            IdProdotto = idProdotto;
            Nome = nome;
            Prezzo = prezzo;
            Descrizione = descrizione;
            CoverImg = coverimg;
            Img1 = img1;
            Img2 = img2;
            Disponibile = disponibile;
        }

    }
}