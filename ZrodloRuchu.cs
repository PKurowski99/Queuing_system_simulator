using System;
using System.Threading;

namespace MOPS
{
    public enum TypRuchu { podkladowy, testowy };
    class ZrodloRuchu
    {
        public ZrodloRuchu()
        {
            lambda = 2;
        }

        public ZrodloRuchu(double lam, TypRuchu typRuchu)
        {
            lambda = lam;
            typ = typRuchu;
        }

        
        private TypRuchu typ;
        private double lambda;
        private double czasDoWyslania;
        public TypRuchu getTypRuchu() { return typ; }

        public double getCzasDoWyslania()
        {
            return czasDoWyslania;
        }

        public void zmniejszCzas(double czas)
        {
            czasDoWyslania -= czas;
        }

        public void losujCzas() // metoda odwracania dystrybuanty
        {
            var rand = new Random();
            double y = rand.NextDouble();

            double czas = Math.Log(1 - y) / (-lambda);
            czasDoWyslania = czas;
            Thread.Sleep(1);
        }

        public Pakiet wyslijPakiet()
        {
            losujCzas();
            return new Pakiet(typ);
        }
    }
}
