using System;
using System.Collections.Generic;
using System.Text;

namespace MOPS
{
    class Wezel
    {
        public Wezel(int id, double czasObslugi)
        {
            this.id = id;this.czasObslugi = czasObslugi;
        }
        private Pakiet obslugiwanyTMP;
        private double zegarTMP;
        private bool przeslijTMP = false;

        public List<double> opoznieniaPakietowTestowych = new List<double>();
        private double calkowiteOpoznienie = 0;
        private Pakiet obslugiwany;
        private Pakiet aktywny;
        private int id;
        private int licznikOpoznien = 0;
        private List<Pakiet> kolejka = new List<Pakiet>();
        private bool zajety=false;
        private double czasObslugi;
        private List<Zdarzenie> zdarzenia = new List<Zdarzenie>();
        private double zegar=0; // czas aktualny
        private List<ZrodloRuchu> zrodla = new List<ZrodloRuchu>();
        public List<double> czasyKolejek = new List<double>();
        public List<int> liczbaPakietowWKolejce = new List<int>();
        public List<double> czasyStanowSerwera = new List<double>();
        public List<bool> zajetoscSerwera = new List<bool>();
        public string zapisz(typZdarzenia typ)
        {
            string gap = "       ";
            if (typ == typZdarzenia.obsluzenie)
            {
                return id + gap + typ + gap + zegar + gap + liczbaPakietow() + gap + zajety + gap + aktywny.getTypRuchu() + gap + aktywny.getOpoznienie();
                
            }
            else
            {
                return id + gap + typ + gap + zegar + gap + liczbaPakietow() + gap + zajety + gap + aktywny.getTypRuchu();
            }
                

        }

        public void uruchom()
        {
            czasyKolejek.Add(0);
            liczbaPakietowWKolejce.Add(0);
            czasyStanowSerwera.Add(0);
            zajetoscSerwera.Add(false);
            for (int i = 0; i < zrodla.Count; i++)
                {
                    zrodla[i].losujCzas();
                }
                zrodla.Sort(
                   delegate (ZrodloRuchu a, ZrodloRuchu b)
                   {
                       return a.getCzasDoWyslania().CompareTo(b.getCzasDoWyslania());
                   });
                double czasWystapienia = zrodla[0].getCzasDoWyslania();
                for (int i = 0; i < zrodla.Count; i++)
                {
                    zrodla[i].zmniejszCzas(czasWystapienia);
                }
                Pakiet pakiet = zrodla[0].wyslijPakiet();
                pakiet.ustawCzasPrzyjscia(zegar);
                pakiet.zmienId(id);
                aktywny = pakiet;
                zrodla.Sort(
                    delegate (ZrodloRuchu a, ZrodloRuchu b)
                    {
                        return a.getCzasDoWyslania().CompareTo(b.getCzasDoWyslania());
                    });
                if (zdarzenia.Count != 0)
                {
                    zdarzenia.Remove(zdarzenia[0]);
                }

                zdarzenia.Add(new Zdarzenie(typZdarzenia.przyjscie, czasWystapienia + zegar));

           

            

        }

        public void dodajZrodlo(ZrodloRuchu zrodlo) { zrodla.Add(zrodlo); }

        public double sprawdzCzas() 
        {
            zdarzenia.Sort(
                   delegate (Zdarzenie a, Zdarzenie b)
                   {
                       return a.getCzas().CompareTo(b.getCzas());
                   });
            return zdarzenia[0].getCzas(); 
        }

        public void zdarzeniePrzyjscia()
        {
            
            double czasWystapienia = zrodla[0].getCzasDoWyslania();

            for (int i = 0; i < zrodla.Count; i++)
            {
                zrodla[i].zmniejszCzas(czasWystapienia);
            }
            Pakiet pakiet = zrodla[0].wyslijPakiet();
            pakiet.ustawCzasPrzyjscia(zegar);
            pakiet.zmienId(id);
            aktywny = pakiet;
            zrodla.Sort(
                delegate (ZrodloRuchu a, ZrodloRuchu b)
                {
                    return a.getCzasDoWyslania().CompareTo(b.getCzasDoWyslania());
                });
           
            if (zdarzenia.Count!=0)
            {
                zdarzenia.Remove(zdarzenia[0]);
                
            }
            
            zdarzenia.Add(new Zdarzenie(typZdarzenia.przyjscie, czasWystapienia + zegar));
            if (zajety == true)
            {
                kolejka.Add(pakiet);
                aktywny = pakiet;
                czasyKolejek.Add(zegar);
                liczbaPakietowWKolejce.Add(kolejka.Count);
            }
            else if (zajety == false)
            {
                zajety = true;
                zajetoscSerwera.Add(zajety);
                czasyStanowSerwera.Add(zegar);
                zdarzenia.Add(new Zdarzenie(typZdarzenia.obsluzenie, czasObslugi + zegar));
                obslugiwany = pakiet;
                obslugiwany.setOpoznienie(0);
            }
        }
        public void zdarzenieObslugi()
        {
            zdarzenia.Remove(zdarzenia[0]);
            aktywny = obslugiwany;


            if (aktywny.getOpoznienie() != 0)
            {
                aktywny.obliczOpoznienie(zegar - czasObslugi);
            }

            if (obslugiwany.getTypRuchu() == TypRuchu.testowy)
            {
                obslugiwanyTMP = obslugiwany;
                zegarTMP = zegar;
                przeslijTMP = true;
                
            }


            calkowiteOpoznienie += aktywny.getOpoznienie();
            licznikOpoznien += 1;

            if (aktywny.getTypRuchu() == TypRuchu.testowy)
            {
                opoznieniaPakietowTestowych.Add(aktywny.getOpoznienie());
            }

            Program.zapiszDoPliku(zegar - czasObslugi + "      " + aktywny.getOpoznienie(), "logs/opoznienia_" + id + ".txt");
            if(zegar<Program.czasSymulacji)
            {
                Program.zapiszDoPliku(zegar+ "      " + aktywny.getOpoznienie(), "logs/opoznienia_" + id + ".txt");
            }
            else
            {
                Program.zapiszDoPliku(Program.czasSymulacji + "      " + aktywny.getOpoznienie(), "logs/opoznienia_" + id + ".txt");
            }
            if (liczbaPakietow() != 0)
            {
                obslugiwany = kolejka[0];
                kolejka.Remove(kolejka[0]);
                zdarzenia.Add(new Zdarzenie(typZdarzenia.obsluzenie, czasObslugi + zegar));
                czasyKolejek.Add(zegar);
                liczbaPakietowWKolejce.Add(kolejka.Count);
            }
            else
            {
                zajety = false;
                zajetoscSerwera.Add(zajety);
                czasyStanowSerwera.Add(zegar);
                Program.zapiszDoPliku(zegar + "      " + "0", "logs/opoznienia_" + id + ".txt");
            }
            

        }
   
        public int liczbaPakietow() { return kolejka.Count; }

        public void dodajPakiet(Pakiet pakiet, double staryZegar) 
        {
            zegar = staryZegar;
            pakiet.zmienId(id);
            pakiet.ustawCzasPrzyjscia(zegar);
            pakiet.setOpoznienie(1);
            aktywny = pakiet;
            Console.WriteLine("PRZYJSCIE DO WEZLA " + id + " zegar: " +zegar);
            if (zajety == false)
            {
                zdarzenia.Add(new Zdarzenie(typZdarzenia.obsluzenie, czasObslugi + zegar));
                aktywny.setOpoznienie(0);
                obslugiwany = pakiet;
                zajety = true;
                zajetoscSerwera.Add(zajety);
                czasyStanowSerwera.Add(zegar);
            }
            else if (zajety == true)
            {

                kolejka.Add(pakiet);
                czasyKolejek.Add(zegar);
                liczbaPakietowWKolejce.Add(kolejka.Count);
            }
            zdarzenia.Sort(
                    delegate (Zdarzenie a, Zdarzenie b)
                    {
                        return a.getCzas().CompareTo(b.getCzas());
                    });
            Program.zapiszDoPliku(zapisz(typZdarzenia.przyjscie),"symulacja.txt");
        }

        public void wykonajZdarzenie() 
        {
            zdarzenia.Sort(
                    delegate (Zdarzenie a, Zdarzenie b)
                    {
                        return a.getCzas().CompareTo(b.getCzas());
                    });


            zegar = zdarzenia[0].getCzas();
            typZdarzenia tmp = zdarzenia[0].getTyp();
            if (zdarzenia[0].getTyp() == typZdarzenia.przyjscie) // przyjscie
            {

                Console.WriteLine("Przyjscie do wezla " + id + " zegar: " +zegar);
                    zdarzeniePrzyjscia();
            }
            else if(zdarzenia[0].getTyp() == typZdarzenia.obsluzenie) // obsluzenie
            {
                Console.WriteLine("Obsluga w wezle " + id+ " zegar: " + zegar);
                zdarzenieObslugi();          
            }

            Program.zapiszDoPliku(zapisz(tmp),"symulacja.txt");
            if (przeslijTMP==true)
            {
                Program.przekazPakiet(obslugiwanyTMP, zegarTMP);
                przeslijTMP = false;
            }

            
            
            
        }
        public void wypiszOpoznienia()
        {
            double srednieOpoznienie;
            if (licznikOpoznien != 0)
                srednieOpoznienie = Math.Round(calkowiteOpoznienie / licznikOpoznien, 5);
            else
                srednieOpoznienie = 0;
            Console.WriteLine("Calkowite opoznienie w wezle " + id + " wynosi: " + calkowiteOpoznienie);
            Console.WriteLine("Liczba opoznien w wezle " + id + " wynosi: " + licznikOpoznien);
            Console.WriteLine("Srednie opoznienie w wezle " + id + " wynosi: " + srednieOpoznienie);
            Program.zapiszDoPliku("Calkowite opoznienie w wezle " + id + " wynosi: " + calkowiteOpoznienie, "symulacja.txt");
            Program.zapiszDoPliku("Liczba opoznien w wezle " + id + " wynosi: " + licznikOpoznien, "symulacja.txt");
            Program.zapiszDoPliku("Srednie opoznienie w wezle " + id + " wynosi: " + srednieOpoznienie, "symulacja.txt");
        }
        public void obliczZajetoscSerwera()
        {
            double czasZajetosci = 0;
            for (int i = 0; i < zajetoscSerwera.Count; i++)
            {
                if (i != zajetoscSerwera.Count - 1)
                {
                    if (zajetoscSerwera[i])
                        czasZajetosci += czasyStanowSerwera[i + 1] - czasyStanowSerwera[i];
                }
                else
                {
                    if (zajetoscSerwera[i])
                        czasZajetosci += Program.czasSymulacji - czasyStanowSerwera[i];
                }
            }
            double procentowaZajetosc = Math.Round(czasZajetosci / Program.czasSymulacji * 100, 2);
            Console.WriteLine("Wezel " + id + " jest zajety przez " + procentowaZajetosc + "% czasu.");
            Program.zapiszDoPliku("Wezel " + id + " jest zajety przez " + procentowaZajetosc + " % czasu.", "symulacja.txt");
        }
        public void obliczSredniaIloscPakietowWKolejce()
        {
            double suma = 0;
            for (int i = 0; i < liczbaPakietowWKolejce.Count; i++)
            {
                if (i != liczbaPakietowWKolejce.Count - 1)
                {
                    suma += liczbaPakietowWKolejce[i] * (czasyKolejek[i + 1] - czasyKolejek[i]);
                }
                else
                {
                    suma += liczbaPakietowWKolejce[i] * (Program.czasSymulacji - czasyKolejek[i]);
                }
            }
            double sredniaPakietow = Math.Round(suma / Program.czasSymulacji, 2);
            Console.WriteLine("W wezle " + id + ". jest srednio " + sredniaPakietow + " pakietow w kolejce");
            Program.zapiszDoPliku("W wezle " + id + ". jest srednio " + sredniaPakietow + " pakietow w kolejce", "symulacja.txt");
        }
        public void zapiszStanSerwera()
        {
            for (int i = 0; i< zajetoscSerwera.Count; i++)
            {
                
                if (i != zajetoscSerwera.Count - 1)
                {
                    if (zajetoscSerwera[i])
                    {
                        Program.zapiszDoPliku(czasyStanowSerwera[i] + "      " + "1", "logs//zajetosc_" + id + ".txt");
                        Program.zapiszDoPliku(czasyStanowSerwera[i + 1] + "      " + "1", "logs//zajetosc_" + id + ".txt");

                    }
                    else
                    {
                        Program.zapiszDoPliku(czasyStanowSerwera[i] + "      " + "0", "logs//zajetosc_" + id + ".txt");
                        Program.zapiszDoPliku(czasyStanowSerwera[i + 1] + "      " + "0", "logs//zajetosc_" + id + ".txt");
                    }
                }
                else
                {
                    if (zajetoscSerwera[i])
                    {
                        Program.zapiszDoPliku(czasyStanowSerwera[i] + "      " + "1", "logs//zajetosc_" + id + ".txt");
                        Program.zapiszDoPliku(Program.czasSymulacji + "      " + "1", "logs//zajetosc_" + id + ".txt");

                    }
                    else
                    {
                        Program.zapiszDoPliku(czasyStanowSerwera[i] + "      " + "0", "logs//zajetosc_" + id + ".txt");
                        Program.zapiszDoPliku(Program.czasSymulacji + "      " + "0", "logs//zajetosc_" + id + ".txt");
                    }
                }
            }

        }
        public void zapiszStanKolejek()
        {
            for (int i = 0; i < liczbaPakietowWKolejce.Count; i++)
            {

                if (i != liczbaPakietowWKolejce.Count - 1)
                {
                    Program.zapiszDoPliku(czasyKolejek[i] + "      " + liczbaPakietowWKolejce[i], "logs//kolejki_" + id + ".txt");
                    Program.zapiszDoPliku(czasyKolejek[i + 1] + "      " + liczbaPakietowWKolejce[i], "logs//kolejki_" + id + ".txt");
                }
                else
                {
                    Program.zapiszDoPliku(czasyKolejek[i] + "      " + liczbaPakietowWKolejce[i], "logs//kolejki_" + id + ".txt");
                    Program.zapiszDoPliku(Program.czasSymulacji + "      " + liczbaPakietowWKolejce[i], "logs//kolejki_" + id + ".txt");
                }
            }
        }
        public int getId() { return id; }
    }
    
}
