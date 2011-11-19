using System;
using System.Collections;
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

            //var movlenebi = new List<Movlena>
            //                           {new Chabarda()
            //                            {
            //                                ChabarebuliPaketi = new Paketi(),
            //                                MigebisTarigi = DateTime.Now,
            //                                SheasrulesStatusi = MzgvevelzeGamomrtmevi.ChabardasStatusi.PaketiMovidaMovidaGanmeorebit
            //                            },
            //    new GaformdaKontrakti(){},
            //    new GauqmdaPolisi(){}
            //};

            // daaregistrireShemsrulebebli.Sacdeli(movlenebi);
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
            var chabardaStatusi = DaamushaveChabardaMovlena(movlena);
            //todo შევინახო მოვლენა სტატუსიანად
        }

        private static ChabardasStatusi DaamushaveChabardaMovlena(Chabarda movlena)
        {
            var tranzaqciebi = new TranzaqciebisSacavi();
            if (tranzaqciebi.MozebneShecdomitXomArMovidaChabarda(movlena.Paketi.PaketisNomeri))
                return ChabardasStatusi.PaketiMovidaGanmeorebit;

            var axaliPilisebi = movlena.Paketi.Polisebi.Select(x => x.PolisiNomeri);

            //todo მოვძებნო რომელიმე პოლისი სხვა ტრანზაქციაშიც ხომ არ მონაწილეობს
            foreach (var polisi in movlena.Paketi.Polisebi)
            {
                var arsebuliTranzaqciebi = tranzaqciebi.MozebnePolisiSxvaTranzaqciashiXomAraa(polisi);
                if (arsebuliTranzaqciebi == null)
                {
                    tranzaqciebi.GaxseniAxaliTranzaqcia(movlena);
                    return ChabardasStatusi.GaixsnaAxaliTranzaqcia;
                }
                else
                {
                    //todo შევავსო არსებული ტრანზაქცია: 
                    //todo პოლისს მივამატო პაკეტი; OK
                    //todo პეკეტს მივამატო პოლისი; OK
                    //todo შევაფასო პაკეტი და გავხსნა ახალი ტრანზაქცია ან შევავსო ინფორმაციით

                    //todo ჯერ უნდა დავადგინო მოვლენის და ტრანზაქციის რიგითობა
                    foreach (var tranzaqcia in arsebuliTranzaqciebi)
                    { 
                        var zveliPolisebi = tranzaqcia.Paketebi.Polisebi.Select(x => x.PolisisNomeri);
                        if (movlena.Tarigi > tranzaqcia.Tarigi)
                        {
                            //todo რიგითობა სწორია
                           
                            var sia = zveliPolisebi.Except(axaliPilisebi).ToList(); 
                            if (sia.Count() == 0)
                            {
                                tranzaqciebi.SheavseMonacemebi(tranzaqcia, polisi.PolisiNomeri,
                                                               movlena.Paketi.PaketisNomeri, tranzaqcia.Tarigi);
                            }
                            else
                            {
                                //todo თუ რამეა განსხვავება
                                tranzaqciebi.GaxseniAxaliTranzaqcia(movlena);
                                foreach (var variable in sia)
                                {
                                    if (tranzaqcia.Paketebi.Polisebi.Select(x=>x.PolisisNomeri).Contains(variable))
                                    {
                                        tranzaqciebi.SheavseMonacemebi(tranzaqcia, polisi.PolisiNomeri, movlena.Paketi.PaketisNomeri, tranzaqcia.Tarigi);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //todo რიგითობა არასწორია
                            var sia = axaliPilisebi.Except(zveliPolisebi).ToList();
                            if (sia.Count() == 0)
                            {
                                tranzaqciebi.SheavseMonacemebi(tranzaqcia, polisi.PolisiNomeri,
                                                               movlena.Paketi.PaketisNomeri, movlena.Tarigi);
                            }
                            else
                            {
                                //todo თუ რამეა განსხვავება
                                tranzaqciebi.GaxseniAxaliTranzaqcia(movlena);
                                foreach (var variable in sia)
                                {
                                    if (tranzaqcia.Paketebi.Polisebi.Select(x => x.PolisisNomeri).Contains(variable))
                                    {
                                        tranzaqciebi.SheavseMonacemebi(tranzaqcia, polisi.PolisiNomeri, movlena.Paketi.PaketisNomeri, tranzaqcia.Tarigi);
                                    }
                                }
                            }
                            //todo me mgoni ar unda 
                            //tranzaqciebi.SheavseMonacemebi(tranzaqcia, polisi.PolisiNomeri, movlena.Paketi.PaketisNomeri, movlena.Tarigi);
                        }
                        return ChabardasStatusi.GanaxldaTranzaqciisMonacemebi;
                    }
                }
            }

            return ChabardasStatusi.GaixsnaAxaliTranzaqcia;
        }
    }

    public class GaformdaKontraktisShemsrulebeli : IShemsrulebeli<GaformdaKontrakti>
    {
        public void Sheasrule(GaformdaKontrakti movlena)
        {
            var tranzaqciebi = new TranzaqciebisSacavi();
            string ss = "";
            Console.WriteLine("გაფორმდაააა - " + movlena.GetType().FullName + " - " + ss);

            var chabardaStatusi = DaamushaveDamtkicdaMovlena(movlena); 
        }

        private static DamtkicdaStatusi DaamushaveDamtkicdaMovlena(GaformdaKontrakti movlena)
        {
            //todo შეცდომით ხომ არ მოვიდა მეორედ
            var tranzaqciebi = new TranzaqciebisSacavi();
            if (tranzaqciebi.MozebneShecdomitXomArMovidaDamtkicda(movlena.PolisiNomeri))
                return DamtkicdaStatusi.DamtkicdaMovidaShecdomit;

            //todo მოვძებნო რომელ ჩავაბარეშია ეს პოლისი და დავხურო სავარაუდო გამოსართმევი ტრანზაქციები

            var arsebuliTranzaqciebi = tranzaqciebi.MozebneChabarebulPaketebshiPolis(movlena.PolisiNomeri);
            if (arsebuliTranzaqciebi == null)
            {
                //todo არ ყოფილა ჩავაბარე
                return DamtkicdaStatusi.ArMoizebnaChabareba;
            }
            else
            {
                //todo დავიარო ტრანზაქციები და დავხურო ???
                foreach (var tranzaqcia in arsebuliTranzaqciebi)
                {
                    
                }

            }
            
            //tranzaqciebi.DaaregistrireMzgvevelzeGamosartmeviTanxa(movlena);
            return DamtkicdaStatusi.DamtkicdaMovidaShecdomit;
        }
    }

    internal enum DamtkicdaStatusi
    {
        DamtkicdaMovidaShecdomit,
        ArMoizebnaChabareba
    }

    public class GauqmdaPolisiShemsrulebeli : IShemsrulebeli<GauqmdaPolisi>
    {
        public void Sheasrule(GauqmdaPolisi movlena)
        {
            Console.WriteLine("გაუქმდა პოლისიიიი - " + movlena.GetType().FullName + " - " + movlena.Tarigi);
        }
    }


    public abstract class Movlena
    {
        public DateTime Tarigi { get; set; }
        public DateTime MigebisTarigi { get; set; }
    }

    public class Chabarda : Movlena
    {
        public ChabardasPaketi Paketi;
    }

    public class ChabardasPaketi
    {
        public List<Polisi> Polisebi;
        public string PaketisNomeri { get; set; }

    }

    public class Polisi
    {
        public string MzgveveliKompania { get; set; }
        public string PolisiNomeri { get; set; }
    }

    public enum ChabardasStatusi
    {
        PaketiMovidaGanmeorebit,
        GaixsnaAxaliTranzaqcia,
        GauqmdaTranzaqcia,
        GanaxldaTranzaqciisMonacemebi
    }

    public class GaformdaKontrakti : Movlena
    {
        public string PolisiNomeri { get; set; }
    }

    public class GauqmdaPolisi : Movlena
    {
    }

    public class JgufisKodi
    {
        public string Damajgufebeli { get; set; }
    }

}
