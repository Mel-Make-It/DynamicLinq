using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VepPrototype.Models
{
    public class Vårdkontakt
    {
        public Guid Id { get; set; }
        public string VårdgivareId { get; set; }
        public VårdFormTyp VårdForm { get; set; }
        public VårdNivåTyp VårdNivå { get; set; }
        public EkonomiskOmrådesKodTyp EkonomiskOmrådesKod { get; set; }
        public string BetalarKod { get; set; }
        public string PatientPersonnummer { get; set; }
        public string PatientLmaNummer { get; set; }
        public string PatientLän { get; set; }
        public string PatientKommun { get; set; }
        public string BesöksDatumStart { get; set; }
        public DateTime BesöksDatumSlut { get; set; }
        public decimal Särkostnader { get; set; }
        public decimal Grundkostnader { get; set; }
        public string Priskod { get; set; }
        public string DrgKod { get; set; }
        public BetalarTyp BetalarTyp { get; set; }
        public VårdKontaktStatusTyp VårdKontaktStatus { get; set; }
    }
}
