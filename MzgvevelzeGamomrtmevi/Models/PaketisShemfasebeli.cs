using System;
using System.Collections.Generic;
using System.Linq;

namespace MzgvevelzeGamomrtmevi.Models
{
    public class PaketisShemfasebeli
    {
        public List<FasiTarigit> Preiskuranti()
        {
            return new List<FasiTarigit>()
                       {
                           new FasiTarigit() {MoqmediaDan = new DateTime(2011, 08, 01), Fasi = 1},
                           new FasiTarigit() {MoqmediaDan = new DateTime(2011, 09, 01), Fasi = (Decimal) 1.5},
                           new FasiTarigit() {MoqmediaDan = new DateTime(2011, 10, 01), Fasi = 2}
                       };
        }

        public Decimal PaketisGirebulebisCalkulatori(Paketi chabarebuliPaketi, DateTime movlenistarigi, string mzgveveli)
        {
            var shefaseba = -10;
            //foreach (var mzgveveli in chabarebuliPaketi.MzgveveliKompaniebi)
            //{
                // todo ვეძებ ყველა არაგაუქმებულ ტრანზაქციაში (თუ მეორედ მომივიდა შეცდომით)
                if (new TranzaqciebisSacavi().MozebneAraGauqmebuliTranzaqcia(chabarebuliPaketi, mzgveveli) != null)
                    return shefaseba;

                // todo ახალი პაკეტია და უკვე ვეძებ ამ პაკეტში შემავალი რომელიმე პოლისი თუ შედის სხვა ტრანზაქციის პაკეტში
                var arsebuliTranzaqcia = new TranzaqciebisSacavi().MomizebneTranzaqciaPolisit(chabarebuliPaketi,
                                                                                              mzgveveli);
                // todo აქ შეიძლება arsebuliTranzaqcia.Tanxa = 0 შემოწმებაც, თუ ვინმემ რატომღაც 0 შეაფასა
                var axaliMosuliPolisebi = chabarebuliPaketi.Polisebi.Select(x => x.Nomeri).ToList();
                var arsebuliPolisebi = arsebuliTranzaqcia.Polisebi;

                //todo თუ პირველი პაკეტის ჩაბარების ცნობამ დაასწრო მეორე ჩაბარების ცნობას ("პო პლანუ" რომ ვთქვათ)
                if (movlenistarigi > arsebuliTranzaqcia.TranzaqciisTarigi)
                {
                    var axaliSia = arsebuliPolisebi.Except(axaliMosuliPolisebi, StringComparer.OrdinalIgnoreCase).ToList();
                    shefaseba = axaliSia.Count() == 0 ? 0 : 3;
                }
                    //todo თუ მეორე პაკეტის ჩაბარების ცნობამ დაასწრო პირველის ჩაბარების ცნობამ
                else
                {
                    var axaliSia = axaliMosuliPolisebi.Except(arsebuliPolisebi, StringComparer.OrdinalIgnoreCase).ToList();
                    shefaseba = axaliSia.Count() == 0 ? 0 : 3;
                }

                //todo ამ შემთხვევაში ერთ-ერთ პაკეტში ვიღებ 3 ლარს, რაც ჩემი აზრით სადავო არასოდეს არ იქნება
               
           // }
            return shefaseba;
        }

        public class FasiTarigit
        {
            public DateTime MoqmediaDan { get; set; }
            public Decimal Fasi { get; set; }
        }

        public void GadaxedeTranzaqciebs(Chabarda movlena, /* Paketi chabarebuliPaketi, DateTime movlenistarigi, */ string mzgveveli)
        {
            
            // todo ვეძებ ყველა არაგაუქმებულ ტრანზაქციაში (თუ მეორედ მომივიდა შეცდომით)
            if (new TranzaqciebisSacavi().MozebneAraGauqmebuliTranzaqciaV1(movlena.ChabarebuliPaketi, mzgveveli) != null) return;

            // todo ახალი პაკეტია და უკვე ვეძებ ამ პაკეტში შემავალი რომელიმე პოლისი თუ შედის სხვა ტრანზაქციის პაკეტში
            var arsebuliTranzaqcia = new TranzaqciebisSacavi().MomizebneTranzaqciaPolisitV1(movlena.ChabarebuliPaketi,            
                                                                                            mzgveveli);
            if (arsebuliTranzaqcia != null)
            {
                var axaliMosuliPolisebi = movlena.ChabarebuliPaketi.Polisebi.Select(x => x.Nomeri).ToList();
                var arsebuliPolisebi = arsebuliTranzaqcia.Polisebi;
                var axaliSia = new List<string>();

                //todo თუ პირველი პაკეტის ჩაბარების ცნობამ დაასწრო მეორე ჩაბარების ცნობას ("პო პლანუ" რომ ვთქვათ)
                if (movlena.Tarigi > arsebuliTranzaqcia.TranzaqciisTarigi)
                {
                    axaliSia = arsebuliPolisebi.Except(axaliMosuliPolisebi, StringComparer.OrdinalIgnoreCase).ToList();
                    //todo ვავსებ ტრანზაქციას ახალი მონაცემებით, თარიღს ვტოვებ
                    new TranzaqciebisSacavi().SheavseTranzaqcia(arsebuliTranzaqcia, arsebuliTranzaqcia.TranzaqciisTarigi, axaliSia);
                }
                //todo თუ მეორე პაკეტის ჩაბარების ცნობამ დაასწრო პირველის ჩაბარების ცნობამ
                else
                {
                    axaliSia = axaliMosuliPolisebi.Except(arsebuliPolisebi, StringComparer.OrdinalIgnoreCase).ToList();
                    //todo ვავსებ ტრანზაქციას ახალი მონაცემებით, უკანა, მოვლენის თარიღით
                    new TranzaqciebisSacavi().SheavseTranzaqcia(arsebuliTranzaqcia, movlena.Tarigi, axaliSia);
                }

            }
            else
            {
                var axaliTranzaqcia = new TranzaqciebisSacavi.Tranzaqcia()
                                          {
                                              TranzaqciisID = Guid.NewGuid(),
                                              MzgveveliKompania = mzgveveli,
                                              Statusi = TranzaqciebisSacavi.TranzaqciisStatusi.GiaTranzaqcia,
                                              PaketisNomeri = movlena.ChabarebuliPaketi.PaketisNomeri,
                                              TranzaqciisTarigi = DateTime.Now,
                                              Tanxa = 3,
                                              Polisebi = movlena.ChabarebuliPaketi.Polisebi
                                                  .Where(x => x.Mzgveveli == mzgveveli)
                                                  .Select(x => x.Nomeri)
                                                  .ToList(),
                                              JgufisKodi = movlena.ChabarebuliPaketi.Damajgufebeli,
                                          };
                new TranzaqciebisSacavi().GaxseniTranzaqcia(axaliTranzaqcia);
            }


            //todo aq unda aresebuli tranzaqciis redaqtireba ???? tu zvelis gauqmeba da axlis sheqmna ????
        }
    }
}