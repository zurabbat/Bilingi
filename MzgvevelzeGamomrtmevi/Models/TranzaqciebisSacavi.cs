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
            public Decimal Tanxa { get; set; }
            public string MzgveveliKompania { get; set; }
            public Guid TranzaqciisId { get; set; }
            public DateTime Tarigi { get; set; }
            public TranzaqciisPaketi Paketebi { get; set; }
            public TranzaqciisStatusi Statusi { get; set; }
            public TranzaqciisTipi Tipi { get; set; }

            public void Sheinaxe()
            {
                throw new NotImplementedException();
            }
        }

        public enum TranzaqciisStatusi
        {
            GiaTranzaqcia,
            DaxuruliTranzaqcia, //todo ანუ დამტკიცებული??
            GauqmebuliTranzaqcia,
            Ganaxlebuli
        }

        public List<Tranzaqcia> MomeciTranzaqciebi()
        {
            return new List<Tranzaqcia>()
                       {

                       };
        }

        public List<Tranzaqcia> MozebnePolisiSxvaTranzaqciashiXomAraa(Polisi polisi)
        {
            var resultati =
                MomeciTranzaqciebi().Where(
                    x => x.Paketebi.Polisebi.Select(y => y.PolisisNomeri).Contains(polisi.PolisiNomeri)).ToList();

            return resultati;
        }

        public void GaxseniAxaliTranzaqcia(Chabarda movlena)
        {
            var mzgveveliKompaniebi = movlena.Paketi.Polisebi.Select(x => x.MzgveveliKompania).Distinct().ToList();

            foreach (var compania in mzgveveliKompaniebi)
            {
                var axaliTranzaqcia = new Tranzaqcia()
                                          {
                                              TranzaqciisId = Guid.NewGuid(),
                                              Tipi = TranzaqciisTipi.SavaraudodGamosartmevi,
                                              MzgveveliKompania = compania,
                                              Tarigi = movlena.Tarigi,
                                              Tanxa = 3,
                                              Paketebi = new TranzaqciisPaketi()
                                                             {
                                                                 PaketisNomeri =
                                                                     new List<string>() { movlena.Paketi.PaketisNomeri },
                                                                 Polisebi = new List<TranzaqciisPolisi>()
                                                             }
                                          };

                foreach (var polisi in movlena.Paketi.Polisebi)
                {
                    axaliTranzaqcia.Paketebi.Polisebi.Add(new TranzaqciisPolisi()
                                                              {
                                                                  PolisisNomeri = polisi.PolisiNomeri, 
                                                                  Paketebi = new List<string>()
                                                                                 {
                                                                                     movlena.Paketi.PaketisNomeri
                                                                                 }
                                                              });
                }
            }

        }

        public void SheavseMonacemebi(Tranzaqcia tranzaqcia, string polisisNomeri, string paketisNomeri, DateTime tarigi)
        {
            //todo ავიღო ეს ტრანზაქცია, და დავამატო პაკეტი, პოლისი[პაკეტი], და თარიღი ახალი
            //todo ძველი მამენტ შეიძლება გავაუქმო და ამის საფუძველზე ახალი შევქმნა ???
            tranzaqcia.Statusi = TranzaqciisStatusi.Ganaxlebuli;
            tranzaqcia.Paketebi.PaketisNomeri.Add(paketisNomeri);

            tranzaqcia.Paketebi.Polisebi
                .Where(x => x.PolisisNomeri == polisisNomeri)
                .Select(x => x.Paketebi)
                .ToList()
                .Add(new List<string>() { paketisNomeri });
            tranzaqcia.Tarigi = tarigi;
            tranzaqcia.Sheinaxe();
        }

        public bool MozebneShecdomitXomArMovidaChabarda(string paketisNomeri)
        {
            var tranzaqciebi = new TranzaqciebisSacavi();
            var shecdoma = tranzaqciebi.MomeciTranzaqciebi().
                Where(x => x.Paketebi.PaketisNomeri.Contains(paketisNomeri) &
                           x.Statusi != TranzaqciisStatusi.GauqmebuliTranzaqcia &
                           x.Tipi == TranzaqciisTipi.SavaraudodGamosartmevi
                );
            return shecdoma.Count() == 0;
        }

        public void DaaregistrireMzgvevelzeGamosartmeviTanxa(GaformdaKontrakti movlena)
        {
            //todo შევამოწმო განმეორებით ხომ არ მოვიდა შეცდომით
            var tranzaqciebi = new TranzaqciebisSacavi();
            var zzz = tranzaqciebi.MomeciTranzaqciebi()
                .Where(x => x.Paketebi.Polisebi.Select(y => y.PolisisNomeri).Contains(movlena.PolisiNomeri) &
                    x.Tipi == TranzaqciisTipi.UechveliGamosartmevi
                );


            var polisTranzaqciebshi = MomeciTranzaqciebi()
                .Where(x => x.Paketebi.Polisebi.Select(y => y.PolisisNomeri).Contains(movlena.PolisiNomeri)).ToList();


        }

        public bool MozebneShecdomitXomArMovidaDamtkicda(string polisisNomeri)
        {
            var tranzaqciebi = new TranzaqciebisSacavi();
            var shecdoma = tranzaqciebi.MomeciTranzaqciebi()
                .Where(x => x.Paketebi.Polisebi.Select(y => y.PolisisNomeri).Contains(polisisNomeri) &
                    x.Tipi == TranzaqciisTipi.UechveliGamosartmevi
                );
            return shecdoma.Count() == 0;
        }

        public List<Tranzaqcia> MozebneChabarebulPaketebshiPolis(string polisiNomeri)
        {
            throw new NotImplementedException();
        }
    }

    internal enum TranzaqciisTipi
    {
        SavaraudodGamosartmevi,
        UechveliGamosartmevi
    }

    internal class TranzaqciisPaketi
    {
        public List<TranzaqciisPolisi> Polisebi { get; set; }
        public List<string> PaketisNomeri { get; set; }
    }

    internal class TranzaqciisPolisi
    {
        public List<string> Paketebi { get; set; }
        public string PolisisNomeri { get; set; }

    }
}
