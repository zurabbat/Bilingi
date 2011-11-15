using System;
using System.Collections.Generic;
using System.Linq;

namespace MzgvevelzeGamomrtmevi.Models
{
    public class FasiTarigitBase
    {
        public class FasiTarigit
        {
            public DateTime MoqmediaDan { get; set; }
            public Decimal Fasi { get; set; }
        }

        public List<FasiTarigit> Preiskuranti()
        {
            return new List<FasiTarigit>()
                       {
                           new FasiTarigit() {MoqmediaDan = new DateTime(2011, 08, 01), Fasi = 1},
                           new FasiTarigit() {MoqmediaDan = new DateTime(2011, 09, 01), Fasi = (Decimal) 1.5},
                           new FasiTarigit() {MoqmediaDan = new DateTime(2011, 10, 01), Fasi = 2}
                       };
        }

        public FasiTarigit MomeciFasiTarigistvis(DateTime tarigi)
        {
            var tarifi = Preiskuranti().Where(x => x.MoqmediaDan < tarigi).OrderBy(x => x.MoqmediaDan).LastOrDefault();
            return tarifi ?? new FasiTarigit() { Fasi = -1, MoqmediaDan = new DateTime() };
        }
    }
}