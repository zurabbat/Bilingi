using System;
using System.Collections.Generic;
using System.Linq;

namespace MzgvevelzeGamomrtmevi.Models
{
    public class PaketisShemfasebeli
    {

        public void GadaxedeTranzaqciebs(Chabarda movlena, string mzgveveli)
        {
            // todo ახალი პაკეტია და უკვე ვეძებ ამ პაკეტში შემავალი რომელიმე პოლისი თუ შედის სხვა ტრანზაქციის პაკეტში
            var arsebuliTranzaqcia = new TranzaqciebisSacavi().MomizebneTranzaqciaPolisebit(movlena.ChabarebuliPaketi,
                                                                                            mzgveveli);
            if (arsebuliTranzaqcia == null)
            {
                
                var axaliTranzaqcia = new TranzaqciebisSacavi.Tranzaqcia()
                                          {
                                              TranzaqciisID = Guid.NewGuid(),
                                              MzgveveliKompania = mzgveveli,
                                              Statusi = TranzaqciebisSacavi.TranzaqciisStatusi.GiaTranzaqcia,
                                              PaketisNomeri = new List<string>() { movlena.ChabarebuliPaketi.PaketisNomeri },
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
            else
            {
                //todo ახალი მოვლენის ერთი მაინც პილისი შედის სხვა ტრანზაქციის პოლისებში
                // ძაან გასააზრებელიაა ***********************************
                var axaliMosuliPolisebi = movlena.ChabarebuliPaketi.Polisebi.Select(x => x.Nomeri).ToList();
                var arsebuliPolisebi = arsebuliTranzaqcia.Polisebi;
                List<string> axaliSia;

                //todo თუ 1 პოლისი მაინცაა სხვა პაკეტში //Intersect - საერთოები, Except - განსხვავებები

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

            //todo aq unda aresebuli tranzaqciis redaqtireba ???? tu zvelis gauqmeba da axlis sheqmna ????
        }
    }
}