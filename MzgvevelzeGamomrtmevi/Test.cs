using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MzgvevelzeGamomrtmevi.Models;
using NUnit.Framework;

namespace MzgvevelzeGamomrtmevi
{
    [TestFixture]
    internal class Test
    {
        [Test]
        public void SacdeliDictionary()
        {
            Koordinatori zura;
            zura = Dackeba();
            var movlenebi = new List<Movlena>()
                                {
                                    new Chabarda()
                                        {
                                            ChabarebuliPaketi = new Paketi(),
                                            MigebisTarigi = DateTime.Now,
                                            SheasrulesStatusi = "გადაცემულია შესასრულებლად "
                                        }
                                    ,
                                    new GaformdaKontrakti()
                                    ,
                                    new GauqmdaPolisi()
                                };
            zura.Sacdeli(movlenebi);
        }

        [Test]
        public void TranzaqciisDzebna()
        {
            var tran = new TranzaqciebisSacavi().MomeciTranzaqciebi();
            var dzebna = new TranzaqciebisSacavi().MozebneAraGauqmebuliTranzaqcia(
                new Paketi() {PaketisNomeri = "01"},
                "AGG"
                );
            Console.WriteLine(tran.Count + " - "+ (dzebna == null));
        }

        [Test]
        //todo mushaobs preiskuranti
        public void FasiPreiskurantidan()
        {
            var fasebi = new PaketisShemfasebeli().Preiskuranti();

            var paketisTarigi = new DateTime(2011, 8, 11);
            var lastOrDefault = fasebi.Where(x => x.MoqmediaDan < paketisTarigi).OrderBy(x => x.MoqmediaDan).LastOrDefault();
            if (lastOrDefault != null)
            {
                var tanxa = lastOrDefault.Fasi;
                Console.WriteLine(paketisTarigi + "- " +  tanxa.ToString());
            }
            Console.WriteLine(" *************************** ");
            foreach(var item in fasebi)
            {
                Console.WriteLine(string.Format("ფასი - {0}; თარიღი - {1}", item.Fasi, item.MoqmediaDan));
            }
        }

        [Test]
        public void StringebisShedareba()
        {
            var l1 = new List<string>(){"a", "b", "c", "d", "f"};
            var l2 = new List<string>(){"d", "c","b"};
            List<string> lNew = l1.Except(l2, StringComparer.OrdinalIgnoreCase).ToList();

            Console.WriteLine(lNew.Count);
            //Intersect saertoebi, Except - gansxvaveba
            foreach (var variable in lNew)
            {
                Console.WriteLine(variable);
            }

            Console.WriteLine(" **************************** ");
            lNew.Remove("f");
            foreach (var variable in lNew)
            {
                Console.WriteLine(variable);
            }
        }

        public Koordinatori Dackeba()
        {
            var daaregistrireShemsrulebebli = new Koordinatori()
                .DaaregistrireShemsrulebebli(new ChabardaPaketisShemsrulebeli())
                .DaaregistrireShemsrulebebli(new GaformdaKontraktisShemsrulebeli())
                .DaaregistrireShemsrulebebli(new GauqmdaPolisiShemsrulebeli());
            
            return daaregistrireShemsrulebebli;
        }
    }

}
