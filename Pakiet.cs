using System;
using System.Collections.Generic;
using System.Text;

namespace MOPS
{
    class Pakiet
    {
        public Pakiet(TypRuchu typ) { this.typ = typ; }

        private int idWezla;
        private double czasPrzyjscia;
        private double opoznienie = 1;
        private TypRuchu typ;

        public TypRuchu getTypRuchu() { return typ; }
        public double getOpoznienie() { return opoznienie; }
        public void zmienId(int id) { this.idWezla = id; }
        public void ustawCzasPrzyjscia(double czas) { this.czasPrzyjscia = czas; }
        public int getId() { return idWezla; }
        public void obliczOpoznienie(double czasRozpoczeciaObslugi)
        {
            opoznienie = czasRozpoczeciaObslugi - czasPrzyjscia;
        }
        public void setOpoznienie(double czas)
        {
            opoznienie = czas;
        }
    }
}
