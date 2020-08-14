using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VepPrototype.Models
{
    public class Betalare
    {
        public Guid Id { get; set; }
        public string Namn { get; set; }
        public string Kod { get; set; }
        public BetalarTyp BetalarTyp { get; set; }
        public string EkonomiFöretag { get; set; }
        public string EkonomiMotpart { get; set; }
    }

    public enum BetalarTyp
    {
        VGR = 1,
        AnnanRegion = 2,
        Asylenheten = 3,
        Försäkringskassan = 4
    }
}
