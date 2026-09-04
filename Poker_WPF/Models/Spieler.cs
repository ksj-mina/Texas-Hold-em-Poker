using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker_WPF.Models
{
    internal class Spieler
    {
        // Spieler Klasse erstellen                                             - gemacht (in eigene Klasse)
        // Spieler Hand machen                                                  - gemacht (in Spieler Klasse) 
        // Rollen bestimmen mit high card draw                                  - 
        // Rollen machen & ihre Rechte bestimmen                                - 
        // Spieler optionen aufbauen                                            - 
        // Jeder spieler 20.000 chips                                           - 
        // Danach, nach high card draw, wieder alle karten ins spiel, und der dealer mischt alles gut, der spieler zu seinem rechten muss eigentlich dann noch einmal abheben/mischen...
        // Aber hier im spiel ka. 

        public ObservableCollection<Karten> Hand { get; set; } // ich habe das auf public umgeändert = schauen ob man dann die karten sieht 

        private Stapel stapel { get; set; } 

        public Spieler()
        {
            Hand = new ObservableCollection<Karten>();
            stapel = new Stapel();
        }

        public void EigeneHand() // das ist die Methode für die eiene Hand
        {
            for (int i = 0; i < 2; i++) 
                Hand.Add(stapel.Ziehen());
        }
    }
}
