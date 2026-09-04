using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker_WPF.Models
{
    internal class Stapel
    {
        // Stapel Klasse erstellen                                              - gemacht (in eigene Klasse)
        // Karten mischen                                                       - gemacht (in Stapel Klasse)
        // Karten austeilen                                                     - gemacht (in Stapel Klasse)

        private List<Karten> stapel;

        public Stapel()
        {   
            stapel = new List<Karten>();
            for (int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 13; j++) 
                {
                    stapel.Add(new Karten(i, j));
                }
            }
            FischerYatesAlgorithm();
        }
        
        public void FischerYatesAlgorithm() // die karten werden gemischt
        {
            Random randomFY = new Random();
            for (int i = stapel.Count - 1; i > 0; i--)
            {
                int j = randomFY.Next(i + 1);
                (stapel[i], stapel[j]) = (stapel[j], stapel[i]);
            }
        }

        public Karten Ziehen()
        {
            Karten obersteKarte = stapel[0];
            stapel.RemoveAt(0);
            return obersteKarte;
        }
    }
}
