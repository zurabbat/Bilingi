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
            //Koordinatori zura;
            //zura = Dackeba();
            //var movlenebi = new List<Movlena>()
            //                    {
            //                        new Chabarda()
            //                            {
            //                                ChabarebuliPaketi = new Paketi(),
            //                                MigebisTarigi = DateTime.Now,
            //                                SheasrulesStatusi = MzgvevelzeGamomrtmevi.ChabardasStatusi.PaketiMovidaMovidaGanmeorebit
            //                            }
            //                        ,
            //                        new GaformdaKontrakti()
            //                        ,
            //                        new GauqmdaPolisi()
            //                    };
            //zura.Sacdeli(movlenebi);
        }

        [Test]
        //todo mushaobs preiskuranti
        public void FasiPreiskurantidan()
        {
            var paketisTarigi = new DateTime(2011, 9, 11);
            var tanxa = new FasiTarigitBase().MomeciFasiTarigistvis(paketisTarigi);

                Console.WriteLine(string.Format("ფასი - {0}; თარიღი - {1}", tanxa.Fasi, tanxa.MoqmediaDan));
           
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

        [Test]
        public void StringebisListContains()
        {
            var l1 = new List<string>() { "a", "b", "c", "d", "f" };
            Console.WriteLine(l1.Contains("k"));
        }


        [Test]
        public void StringebisListEqual()
        {
            var l1 = new List<string>() { "a", "b", "c", "d", "f" };
            var l2 = new List<string>() { "a", "f", "c", "d"};
            l1.Sort();
            l2.Sort();
            
            foreach (var item in l2)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine(l1.SequenceEqual(l2));
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
