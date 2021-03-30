using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;

namespace MOPS
{
    class Program
    {
        Stream fs = new FileStream("symulacja.txt", FileMode.Create, FileAccess.Write);
        public static void obliczSredniCzasOpoznieniaTestowych()
        {
            int liczba_testowych = wezly[wezly.Count - 1].opoznieniaPakietowTestowych.Count;
            double suma = 0;
            foreach (Wezel wezel in wezly)
            {
                for (int i = 0; i < liczba_testowych; i++)
                {
                    suma += wezel.opoznieniaPakietowTestowych[i];
                }
            }
            double srednia = Math.Round(suma / liczba_testowych, 5);
            Console.WriteLine();
            Console.WriteLine("Sredni czas oczekiwania pakietow testowych we wszystkich kolejkach wynosi: " + srednia);
            zapiszDoPliku("Sredni czas oczekiwania pakietow testowych we wszystkich kolejkach wynosi: " + srednia, "symulacja.txt");
        }
        public static void zapiszDoPliku(string linijka,string path)
        {
            StreamWriter sw;
            sw = File.AppendText(path);
            sw.WriteLine(linijka);
            sw.Close();
        }
        public static void wczytajKonfiguracje()
        {
            XmlReader reader = new XmlTextReader("konfiguracja.xml");

            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    string tmp;
                    string[] tmp2;
                    switch (reader.Name.ToString())
                    {
                        case "CZAS":
                            tmp = reader.ReadString();
                            czasSymulacji = int.Parse(tmp);
                            break;
                        case "WEZEL":
                            tmp = reader.ReadString();
                            tmp2 = tmp.Split(' ');
                            dodajWezel(new Wezel(int.Parse(tmp2[0]), double.Parse(tmp2[1], System.Globalization.NumberStyles.AllowDecimalPoint)));
                            break;
                        case "ZRODLO":
                            tmp = reader.ReadString();
                            tmp2 = tmp.Split(' ');
                            if(int.Parse(tmp2[2])==0)
                            wezly[int.Parse(tmp2[0])].dodajZrodlo(new ZrodloRuchu(double.Parse(tmp2[1], System.Globalization.NumberStyles.AllowDecimalPoint), TypRuchu.podkladowy));
                            else
                            wezly[int.Parse(tmp2[0])].dodajZrodlo(new ZrodloRuchu(double.Parse(tmp2[1], System.Globalization.NumberStyles.AllowDecimalPoint), TypRuchu.testowy));
                            break;
                    }
                }
            }



        }
        public static void wyczyscPlik(string path)
        {
            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                if(path.Equals("symulacja.txt"))
                { 
                    using (StreamWriter sw = new StreamWriter(stream))
                    {
                        sw.WriteLine("IdWezla | TypZdarzenia | CzasZdarzenia | DlugoscKolejki | CzyZajety | TypRuchu | Opoznienie");
                    }
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(stream))
                    {
                        sw.Write(string.Empty);
                    }
                }
            }
        }
        public static void wyczyscWszystkiePliki()
        {
            wyczyscPlik("symulacja.txt");
            foreach (Wezel w in wezly)
            {
                wyczyscPlik("logs//zajetosc_" + w.getId() + ".txt");
                wyczyscPlik("logs//kolejki_" + w.getId() + ".txt");
                wyczyscPlik("logs//opoznienia_" + w.getId() + ".txt");
            }

        }
        private static double globalZegar;
        public static double czasSymulacji;
        private static List<Wezel> wezly = new List<Wezel>();
        public static void dodajWezel(Wezel wezel)
        {
            wezly.Add(wezel);
        }
        public static void przekazPakiet(Pakiet pakiet, double zegar)
        {
            Console.WriteLine("Przeslanie dalej zegar: " + zegar);
            if (pakiet.getId() + 1 < wezly.Count)
                wezly[pakiet.getId() + 1].dodajPakiet(pakiet, zegar);
            else
                Console.WriteLine("Pakiet testowy dotarl do konca");

        }
        public static void uruchom()
        {
            for(int i=0;i<wezly.Count;i++)
            {
                wezly[i].uruchom();
            }
        }
        public static void symulacja()
        {
            while(true)
            {

                int idWezla = 0;
                double minCzas = czasSymulacji * 2;
                
                for (int i = 0; i < wezly.Count; i++)
                {
                   if(minCzas>wezly[i].sprawdzCzas())
                    {
                        idWezla = i;
                        minCzas = wezly[i].sprawdzCzas();
                    }
                }

                if (minCzas > czasSymulacji) break;
                globalZegar = minCzas;
                wezly[idWezla].wykonajZdarzenie();
                
            }
            
        }




        static void Main(string[] args)
        {
            wczytajKonfiguracje();
            for (int i=0;i<wezly.Count;i++)
            {
                wezly[i].uruchom();
            }
            wyczyscWszystkiePliki();
            symulacja();

            obliczSredniCzasOpoznieniaTestowych();

            foreach (Wezel wezel in wezly)
            {
                Console.WriteLine("----------------------------------------------------------------------");
                wezel.wypiszOpoznienia();
                Console.WriteLine();
                wezel.obliczZajetoscSerwera();
                Console.WriteLine();
                wezel.obliczSredniaIloscPakietowWKolejce();
            }
            foreach(Wezel wezel in wezly)
            {
                wezel.zapiszStanSerwera();
                wezel.zapiszStanKolejek();
            }
            Console.ReadLine();

        }
    }
}
