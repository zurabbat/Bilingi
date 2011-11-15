using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MzgvevelzeGamomrtmevi.Models
{
    class TranzaqciebisSacavi
    {
        public class Tranzaqcia
        {
            public JgufisKodi JgufisKodi { get; set; }
            public Guid TranzaqciisID { get; set; }
            public Decimal Tanxa { get; set; }
            public DateTime TranzaqciisTarigi { get; set; }
            public List<string> PaketisNomeri { get; set; }
            //public DateTime PaketisTarigi { get; set; }
            public List<string> Polisebi { get; set; } 
            public TranzaqciisStatusi Statusi { get; set; }
            public string MzgveveliKompania { get; set; }
        }

        public enum TranzaqciisStatusi
        {
            GiaTranzaqcia,
            DaxuruliTranzaqcia, //ანუ დამტკიცებული
            GauqmebuliTranzaqcia
        }

        public Tranzaqcia MozebneAraGauqmebuliTranzaqcia(Paketi chabarebuliPaketi)
        {
            var rezultati = MomeciTranzaqciebi()
                .Where(
                x => x.PaketisNomeri.Contains(chabarebuliPaketi.PaketisNomeri) &
                    x.Statusi != TranzaqciisStatusi.GauqmebuliTranzaqcia 
                )
                .FirstOrDefault();
            return rezultati;
        }

        public Tranzaqcia MomizebneTranzaqciaPolisebit(Paketi chabarebuliPaketi, string mzgveveli)
        {
            var paketisPolisebi = chabarebuliPaketi.Polisebi.Select(x => x.Nomeri);

            var resultati = MomeciTranzaqciebi()
                .Where(x =>
                       x.MzgveveliKompania == mzgveveli & 
                       x.Statusi != TranzaqciisStatusi.GauqmebuliTranzaqcia &
                       x.Polisebi.Intersect(paketisPolisebi).Count() != 0
                )  //todo თუ 1 პოლისი მაინცაა სხვა პაკეტში //Intersect საერთოები, Except - განსხვავებები
                .FirstOrDefault();
            return resultati;
        }

        public void SheavseTranzaqcia(Tranzaqcia arsebuliTranzaqcia, DateTime tranzaqciisTarigi, List<string> axaliMosuliPolisebi)
        {
            //todo dasamtavrebelia
            Tranzaqcia mozebniliTranzaqcia;
            mozebniliTranzaqcia = MomeciTranzaqciebi()
                .Where(x => x.TranzaqciisID == arsebuliTranzaqcia.TranzaqciisID)
                .FirstOrDefault();

            Debug.Assert(mozebniliTranzaqcia != null, "mozebniliTranzaqcia != null");
            mozebniliTranzaqcia.TranzaqciisTarigi = tranzaqciisTarigi;
            

        }

        public void GaxseniTranzaqcia(TranzaqciebisSacavi.Tranzaqcia axaliTranzaqcia)
        {
            throw new NotImplementedException();
        }

        public List<Tranzaqcia> MomeciTranzaqciebi()
        {
            return new List<Tranzaqcia>()
                       {
                           new Tranzaqcia() {PaketisNomeri = new List<string>(){"01"}, MzgveveliKompania = "AGG", Statusi = TranzaqciisStatusi.GiaTranzaqcia},
                           new Tranzaqcia() {PaketisNomeri = new List<string>(){"02"}, MzgveveliKompania = "ICG", Statusi = TranzaqciisStatusi.DaxuruliTranzaqcia},
                           new Tranzaqcia() {PaketisNomeri = new List<string>(){"03"}, MzgveveliKompania = "ALP", Statusi = TranzaqciisStatusi.GiaTranzaqcia},
                           new Tranzaqcia() {PaketisNomeri = new List<string>(){"04"}, MzgveveliKompania = "AGG", Statusi = TranzaqciisStatusi.GauqmebuliTranzaqcia}
                       };
        }
    }
}
