using System;
using System.Collections.Generic;
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
            public string PaketisNomeri { get; set; }
            //public DateTime PaketisTarigi { get; set; }
            public List<string> Polisebi { get; set; } 
            public TranzaqciisStatusi Statusi { get; set; }
            public string MzgveveliKompania { get; set; }
        }

        public List<Tranzaqcia> MomeciTranzaqciebi()
        {
            return new List<Tranzaqcia>()
                       {
                           new Tranzaqcia() {PaketisNomeri = "01", MzgveveliKompania = "AGG", Statusi = TranzaqciisStatusi.GiaTranzaqcia},
                           new Tranzaqcia() {PaketisNomeri = "02", MzgveveliKompania = "ICG", Statusi = TranzaqciisStatusi.DaxuruliTranzaqcia},
                           new Tranzaqcia() {PaketisNomeri = "03", MzgveveliKompania = "ALP", Statusi = TranzaqciisStatusi.GiaTranzaqcia},
                           new Tranzaqcia() {PaketisNomeri = "04", MzgveveliKompania = "AGG", Statusi = TranzaqciisStatusi.GauqmebuliTranzaqcia}
                       };
        }

        public Tranzaqcia MozebneAraGauqmebuliTranzaqcia(Paketi chabarebuliPaketi, string mzgveveliKompania)
        {
            var rezultati = MomeciTranzaqciebi()
                .Where(
                x => x.PaketisNomeri == chabarebuliPaketi.PaketisNomeri &
                    x.Statusi != TranzaqciisStatusi.GauqmebuliTranzaqcia &
                    x.MzgveveliKompania == mzgveveliKompania
                )
                //.Any()
                //.Select(x => x)
                .FirstOrDefault();
            return rezultati;
        }

        public Tranzaqcia MomizebneTranzaqciaPolisit(Paketi chabarebuliPaketi, string mzgveveli)
        {
            var paketisPolisebi = chabarebuliPaketi.Polisebi.Select(x => x.Nomeri);//.OrderBy(x => x);
            //todo polisebic misabmelia 
            var resultati = MomeciTranzaqciebi()
                .Where(x => x.PaketisNomeri == chabarebuliPaketi.PaketisNomeri &
                    x.MzgveveliKompania == mzgveveli &
                    x.Polisebi.Intersect(paketisPolisebi).Count() != 0 //todo es unda ????
                    )
                .OrderBy(x => x.TranzaqciisTarigi)
                .FirstOrDefault();

            return resultati;
        }

        public enum TranzaqciisStatusi
        {
            GiaTranzaqcia,
            DaxuruliTranzaqcia, //ანუ დამტკიცებული
            GauqmebuliTranzaqcia
        }

        public Tranzaqcia MozebneAraGauqmebuliTranzaqciaV1(Paketi chabarebuliPaketi, string mzgveveliKompania)
        {
            var rezultati = MomeciTranzaqciebi()
                .Where(
                x => x.PaketisNomeri.Contains(chabarebuliPaketi.PaketisNomeri) &
                    x.Statusi != TranzaqciisStatusi.GauqmebuliTranzaqcia &
                    x.MzgveveliKompania == mzgveveliKompania
                )
                //.Any()
                //.Select(x => x)
                .FirstOrDefault();
            return rezultati;
        }

        public Tranzaqcia MomizebneTranzaqciaPolisitV1(Paketi chabarebuliPaketi, string mzgveveli)
        {
            var paketisPolisebi = chabarebuliPaketi.Polisebi.Select(x => x.Nomeri);//.OrderBy(x => x);

            Tranzaqcia resultati;
            resultati = MomeciTranzaqciebi()
                .Where(x =>
                       x.MzgveveliKompania == mzgveveli & 
                       x.Statusi != TranzaqciisStatusi.GauqmebuliTranzaqcia &
                       x.Polisebi.Intersect(paketisPolisebi).Count() != 0 //todo თუ 1 პოლისი მაინცაა სხვა პაკეტში
                       )
                .FirstOrDefault();
            return resultati;
        }

        public void SheavseTranzaqcia(Tranzaqcia arsebuliTranzaqcia, DateTime mosuliPolisebi, List<string> axaliMosuliPolisebi)
        {
            var mozebniliTranzaqcia = MomeciTranzaqciebi()
                .Where(x => x.TranzaqciisID == arsebuliTranzaqcia.TranzaqciisID)
                .FirstOrDefault();

            //mozebniliTranzaqcia.TranzaqciisTarigi = 
        }

        public void GaxseniTranzaqcia(TranzaqciebisSacavi.Tranzaqcia axaliTranzaqcia)
        {
            throw new NotImplementedException();
        }
    }
}
