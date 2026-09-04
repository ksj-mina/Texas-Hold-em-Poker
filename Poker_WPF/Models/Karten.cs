using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker_WPF.Models
{
    public class Karten
    {
        // Karten Klasse erstellen                                              - gemacht (in eigene Klasse)
        // Karten erstellt                                                      - gemacht (in Karten Klasse) 

        public int Symbol { get; set; }
        public int Wert { get; set;  }

        //public Karten() { }

        public Karten() { }

        public Karten(int symbol, int wert)
        {
            Symbol = symbol;
            Wert = wert;
        }
        
        public string SymbolString
        {
            get
            {
                switch (Symbol)
                {
                    case 0: return "❤️";  
                    case 1: return "♦";
                    case 2: return "♣️";
                    case 3: return "♠";
                    default: return "?";
                }
            }
        }

        public string WertString
        {
            get
            {
                int wert = Wert + 2;
                switch (wert)
                { // ich glaube ich muss alle cases nennen, sonst glaube ich werden sie als default gezählt werden = ich hatte wahrscheinlich recht, ich habe es mal verbessert
                    case 2: return "2";
                    case 3: return "3";
                    case 4: return "4";
                    case 5: return "5";
                    case 6: return "6";
                    case 7: return "7";
                    case 8: return "8";
                    case 9: return "9";
                    case 10: return "10";
                    case 11: return "J";
                    case 12: return "Q";
                    case 13: return "K";
                    case 14: return "A";
                    default: return "?";
                }
            }
        }
    }
}
