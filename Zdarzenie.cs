using System;
using System.Collections.Generic;
using System.Text;

namespace MOPS
{
    enum typZdarzenia { przyjscie, obsluzenie};
    class Zdarzenie
    {
        public Zdarzenie(typZdarzenia typ, double czas)
        {
            this.typ = typ;
            this.czas = czas;
        }

        private double czas;
        private typZdarzenia typ;
        public double getCzas() { return czas; }
        public typZdarzenia getTyp() { return typ; }
    }
}
