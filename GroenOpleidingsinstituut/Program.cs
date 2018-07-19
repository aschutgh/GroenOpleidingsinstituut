using System;
using System.Collections.Generic;

// Install nuget package System.ValueTuple

namespace GroenOpleidingsinstituut
{
    class Program
    {

        static int bepaalTotaalVakken(Dictionary<String, int> av)
        {
            int totaalvakken = 0;
            foreach(String s in av.Keys)
            {
                totaalvakken += av[s];
            }

            return totaalvakken;
        }

        static Dictionary<String, int> VraagWelkeVakken(Dictionary<String, int> vak)
        {
            Console.WriteLine("Geef van elk soort vak (schriftelijk, mondeling, practisch) het aantal op:");
            Console.WriteLine("schriftelijk");
            vak["schriftvak"] += int.Parse(Console.ReadLine());
            Console.WriteLine("mondeling");
            vak["mondvak"] += int.Parse(Console.ReadLine());
            Console.WriteLine("practisch");
            vak["pracvak"] += int.Parse(Console.ReadLine());

            return vak;
        }

        static bool VraagLeeftijd()
        {
            DateTime negentienJaarLater;

            Console.Write("Geef de geboortedatum van de kandidaat in het formaat jjjj/mm/dd: ");
            DateTime gebdat;
            gebdat = DateTime.Parse(Console.ReadLine());

            Console.Write("Geef de peildatum in het formaat jjjj/mm/dd: ");
            DateTime peildat;
            peildat = DateTime.Parse(Console.ReadLine());

            negentienJaarLater = gebdat.AddYears(19);
            if (DateTime.Compare(negentienJaarLater, peildat) < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        static bool VraagArbeidsBureau()
        {
            String antw = "";

            Console.WriteLine("Cursist volgt cursus na bemiddeling door het arbeidsbureau? (ja/nee)");
            antw = Console.ReadLine();
            if (antw == "ja")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static Double berekenCursusGeld(Dictionary<String, (int, int)> tarieven, Dictionary<string, int> vakken, bool jongernegentien, bool arbeidsbureau)
        {
            //verschuldigd cursusgeld opslaan in dictionary, uitgesplitst naar lesgeld en studiematerialen
            Dictionary<String, Double> cursusgeld = new Dictionary<String, Double>();
            cursusgeld.Add("lesgeld", 0.0);
            cursusgeld.Add("studmat", 0.0);

            // bereken basis cursusgeld
            foreach (String s in vakken.Keys)
            {
                // tarieven["soortvak"].Item1 is lesgeld voor soort vak
                // tarieven["soortvak"].Item2 is studmat (studie materialen) voor soort vak
                cursusgeld["lesgeld"] = vakken[s] * tarieven[s].Item1;
                cursusgeld["studmat"] = vakken[s] * tarieven[s].Item2;
            }

            // indien bemiddeling arbeidsbureau dan hoeft de cursist geen studmat te betalen
            if (arbeidsbureau)
                cursusgeld["studmat"] = 0.0;

            // indien de cursist jonger is dan negentien dan krijgt de cursist 2% korting op het totale cursusgeld
            Double jnkorting = 0.0;
            if (jongernegentien)
                jnkorting = 0.02 * (cursusgeld["lesgeld"] + cursusgeld["studmat"]);

            // indien aantal vakken >= 5 dan krijgt de cursist 5% korting op het lesgeld
            if (bepaalTotaalVakken(vakken) >= 5)
                cursusgeld["lesgeld"] = 0.95 * cursusgeld["lesgeld"];

            Double totaalcursusgeld = cursusgeld["lesgeld"] + cursusgeld["studmat"];
            totaalcursusgeld = totaalcursusgeld - jnkorting;

            return totaalcursusgeld;
        }
        
        static void Main(string[] args)
        {
            // tarieven cursusgeld bestaande uit lesgeld en studiemateriaal
            // tuples
            (int lesgeld, int studmat) schriftvak = (lesgeld: 50, studmat: 50);
            (int lesgeld, int studmat) mondvak = (lesgeld: 150, studmat: 50);
            (int lesgeld, int studmat) pracvak = (lesgeld: 150, studmat: 125);

            //tarief tuples in tarief dictionary opslaan
            Dictionary<String, (int, int)> tarieven = new Dictionary<String, (int, int)>();
            tarieven.Add("schriftvak", schriftvak);
            tarieven.Add("mondvak", mondvak);
            tarieven.Add("pracvak", pracvak);

            //aantal vakken voor een cursist opslaan in een dictionary
            Dictionary<String, int> vakken = new Dictionary<String, int>();
            vakken.Add("schriftvak", 0);
            vakken.Add("mondvak", 0);
            vakken.Add("pracvak", 0);

            //verschuldigd cursusgeld opslaan in dictionary, uitgesplitst naar lesgeld en studiematerialen
            //Dictionary<String, Double> cursusgeld = new Dictionary<String, Double>();
            //cursusgeld.Add("lesgeld", 0.0);
            //cursusgeld.Add("studmat", 0.0);

            // gegevens van cursist
            // leeftijd cursist in aantal dagen
            bool jongernegentien = false;
            // bemiddeling door arbeidsbureau
            bool arbeidsbureau = false;
            // totaal cursusgeld
            //Double totaalcursusgeld = 0.0;

            // Vraag welke vakken de cursist volgt
            vakken = VraagWelkeVakken(vakken);
            // Bepaal of de cursist jonger dan negentien is
            jongernegentien = VraagLeeftijd();
            // Vraag bemiddeling door arbeidsbureau
            arbeidsbureau = VraagArbeidsBureau();

            Console.WriteLine("Het cursusgeld bedraagt: {0}", berekenCursusGeld(tarieven, vakken, jongernegentien, arbeidsbureau));
            
        }
    }
}
