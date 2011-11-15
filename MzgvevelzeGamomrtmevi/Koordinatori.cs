using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MzgvevelzeGamomrtmevi.Models;
using NUnit.Framework;

namespace MzgvevelzeGamomrtmevi
{
    public class Programa
    {
        public void DackebisCertili()
        {
            var daaregistrireShemsrulebebli = new Koordinatori()
                .DaaregistrireShemsrulebebli(new ChabardaPaketisShemsrulebeli())
                .DaaregistrireShemsrulebebli(new GaformdaKontraktisShemsrulebeli())
                .DaaregistrireShemsrulebebli(new GauqmdaPolisiShemsrulebeli());

            var movlenebi = new List<Movlena>
                                       {new Chabarda()
                                        {
                                            ChabarebuliPaketi = new Paketi(),
                                            MigebisTarigi = DateTime.Now,
                                            SheasrulesStatusi = "გადაცემულია შესასრულებლად "
                                        },
                                    new GaformdaKontrakti(){},
                                    new GauqmdaPolisi(){}
                                };

            daaregistrireShemsrulebebli.Sacdeli(movlenebi);
        }

    }

    public class Koordinatori
    {
        private readonly Dictionary<Type, Action<Movlena>> _movlenebisCnobari;

        public Koordinatori()
        {
            _movlenebisCnobari = new Dictionary<Type, Action<Movlena>>();
        }
        public Koordinatori DaaregistrireShemsrulebebli<T>(IShemsrulebeli<T> shemsrulebeli) where T : Movlena
        {
            _movlenebisCnobari.Add(typeof(T), movlena => shemsrulebeli.Sheasrule((T)movlena));
            return this;
        }
        public void Sacdeli(List<Movlena> movlenebi)
        {
            foreach (var movlena in movlenebi)
            {
                var sh = _movlenebisCnobari[movlena.GetType()];
                sh(movlena);
            }
        }

    }

    public interface IShemsrulebeli<T>
    {
        void Sheasrule(T movlena);
    }

    public class ChabardaPaketisShemsrulebeli : IShemsrulebeli<Chabarda>
    {
        public void Sheasrule(Chabarda movlena)
        {
            Console.WriteLine("ჩაბარდააა - " + movlena.GetType().FullName + " - " + movlena.SheasrulesStatusi);
            //todo პირველი ვერსია, არ მომწონს, თუ მეორე არ დამტკიცდა კონტრაქტი, 
            //todo პირველს, 0 თანხიანს ვკარგავ და ახლიდან გადაანგარიშება მჭირდება
            foreach (var mzgveveli in movlena.ChabarebuliPaketi.MzgveveliKompaniebi)
            {
                var //movlena.ChabarebuliPaketi.
                mamentGamosartmeviTanxa =
                    new PaketisShemfasebeli().PaketisGirebulebisCalkulatori(movlena.ChabarebuliPaketi, movlena.Tarigi, mzgveveli);
                if (mamentGamosartmeviTanxa == - 10) return;

                //todo ესე იგი უკვე არსებობდა რომელიმე პაკეტში
                if (mamentGamosartmeviTanxa == 0) return;

                var axaliTranzaqcia = new TranzaqciebisSacavi.Tranzaqcia()
                                          {
                                              TranzaqciisID = Guid.NewGuid(),
                                              MzgveveliKompania = mzgveveli,
                                              Statusi = TranzaqciebisSacavi.TranzaqciisStatusi.GiaTranzaqcia,
                                              PaketisNomeri = movlena.ChabarebuliPaketi.PaketisNomeri,
                                              TranzaqciisTarigi = DateTime.Now,
                                              Tanxa = mamentGamosartmeviTanxa,
                                              Polisebi = movlena.ChabarebuliPaketi.Polisebi
                                                            .Where(x => x.Mzgveveli == mzgveveli)
                                                            .Select(x => x.Nomeri)
                                                            .ToList(),
                                              JgufisKodi = movlena.ChabarebuliPaketi.Damajgufebeli,
                                          };
                new TranzaqciebisSacavi().GaxseniTranzaqcia(axaliTranzaqcia);
            }

            //todo მეორე ვერსია - ტრანზაქციას ვავსებ ინფორმაციით: პაკეტის ნომერს ვამატებ და თარიღს ვაწერ ნაკლებს,
            //todo თითქოს რამდენი მოვლენაც არ უნდა მქონდეს და რა თანმიმდევრობითაც არ უნდა მომივიდეს, თითქოს არ უნდა მქონდეს პრობლემა
            foreach (var mzgveveli in movlena.ChabarebuliPaketi.MzgveveliKompaniebi)
            {
                new PaketisShemfasebeli().GadaxedeTranzaqciebs(movlena,  mzgveveli);
            }

        }
    }

    public class GaformdaKontraktisShemsrulebeli : IShemsrulebeli<GaformdaKontrakti>
    {
        public void Sheasrule(GaformdaKontrakti movlena)
        {
            var ss = movlena.GaformebuliPoli == null ? "არ აქვს პოლისის ნომერი" : movlena.GaformebuliPoli.Nomeri;
            Console.WriteLine("გაფორმდაააა - " + movlena.GetType().FullName + " - " + ss);
        }
    }

    public class GauqmdaPolisiShemsrulebeli : IShemsrulebeli<GauqmdaPolisi>
    {
        public void Sheasrule(GauqmdaPolisi movlena)
        {
            Console.WriteLine("გაუქმდა პოლისიიიი - " + movlena.GetType().FullName + " - " + movlena.Tarigi);
        }
    }

    public class Paketi
    {
        public JgufisKodi Damajgufebeli { get; set; }
        public string PaketisNomeri { get; set; }
        public List<string> MzgveveliKompaniebi { get; set; }
        public List<Polisi> Polisebi { get; set; }
        //        public Decimal MamentGamosartmeviTanxa { get; set; }
        public DateTime PaketisTarigi { get; set; }
        public DateTime CarmatebuliVizitisTarigi { get; set; }
        public Paketi()
        {
            PaketisNomeri = "P1";
            Damajgufebeli = new JgufisKodi() { Damajgufebeli = "j1" };
            MzgveveliKompaniebi = new List<string>() { "AGG", "ICG" };
            Polisebi = new List<Polisi>(){
                               new Polisi(){ Nomeri = "1"},
                               new Polisi(){ Nomeri = "2"},
                               new Polisi(){ Nomeri = "3"}
                           };
            //MamentGamosartmeviTanxa = -1;
            PaketisTarigi = new DateTime(2011, 09, 10);
            CarmatebuliVizitisTarigi = PaketisTarigi.AddDays(5);
        }
    }

    public class JgufisKodi
    {
        public string Damajgufebeli { get; set; }
    }

    public abstract class Movlena
    {
        public DateTime Tarigi { get; set; }
        public DateTime MigebisTarigi { get; set; }
    }

    public class Chabarda : Movlena
    {
        public Paketi ChabarebuliPaketi { get; set; }
        public string SheasrulesStatusi { get; set; } //todo ???? რამეში მჭირდება ვითომ?
    }

    public class GaformdaKontrakti : Movlena
    {
        public Polisi GaformebuliPoli { get; set; }
    }

    public class GauqmdaPolisi : Movlena
    {
        public Polisi GauqmebuiliPolisi { get; set; }
    }

    public class Polisi
    {
        public string Mzgveveli { get; set; }
        public string Nomeri { get; set; }
    }
}
