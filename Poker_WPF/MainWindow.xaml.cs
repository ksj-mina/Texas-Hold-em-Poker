using Poker_WPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Poker_WPF
{
    // Karten Klasse erstellen                                                  - gemacht (in eigene Klasse)
        // Karten erstellt                                                      - gemacht (in Karten Klasse) 
    // Stapel Klasse erstellen                                                  - gemacht (in eigene Klasse)
        // Karten mischen                                                       - gemacht (in Stapel Klasse)
        // Karten austeilen                                                     - gemacht (in Stapel Klasse)
    // Tisch Klasse erstellen                                                   - gmeacht (in eigene Klasse)
        // Karten auswerten - Werte (1Paar,2Paar,Drilling,FullHouse,Vierling)   - gemacht (in Tisch Klasse) 
        // Karten auswerten - Symbole (Flush,StraightFlush,RoyalFlush)          - gemacht (in Tisch Klasse)
        // Karten auswerten - Street (Straight,StraightFlush,RoyalFlush)        - gemacht (in Tisch Klasse)
        // Karten auswerten - Höher                                             - gemacht (in Tisch Klasse)
        // Karten regelmäßig verbrennen                                         - 
        // kombi in extra liste speichern                                       - 
        // symbol speichern                                                     - 
    // Spieler Klasse erstellen                                                 - gemacht (in eigene Klasse) 
        // Spieler Hand machen                                                  - gemacht (in Spieler Klasse) 
        // Rollen bestimmen mit high card draw                                  - 
        // Rollen machen & ihre Rechte bestimmen                                - 
        // Spieler optionen aufbauen                                            - 
        // Jeder spieler 20.000 chips                                           - 
    // Test werte machen zur Prüfung                                            - gemacht (in XAML.cs)
    // Gewinner bestimmen                                                       - 
    // UI erstellen                                                             - gemacht (in XAML)

    public partial class MainWindow : Window
    {

        private Tisch Tisch { get; set; }

        private Spieler Spieler { get; set; }

        public ObservableCollection<Karten> Hand => Spieler.Hand; // Expressions Properities
        public ObservableCollection<Karten> CommunityHand => Tisch.CommunityHand;

        private int assWert = 14; // ich muss die 2 weg machen, weil ich sie ja hier nicht brauche, ich muss es irgendwie in der klasse tisch machen 
        private int[] wertStaerkeFinden = new int[3]; // das nervt mich, ich werden das später anschauen & es wahrscheinlich weg machen, weil mich es stört 

        public MainWindow()
        {
            InitializeComponent();

            Tisch = new Tisch();
            Spieler = new Spieler();
            
            DataContext = this; 
            
            NeueRunde();
        }

        private void NeueRunde() // hier werde ich in Klasse eine neue Methode erstellen, mit dem ich dann einfach es hier aufrufen werde, oder warte 
        {

            Tisch.NeuerStapel();
            Spieler.NeuerStapel();
            Hand.Clear();
            CommunityHand.Clear();

            Spieler.EigeneHand();

            BesteHandText.Text = "Drücke 'Flop' für die ersten 3 Community Karten. ";
            BesteHandText.Foreground = System.Windows.Media.Brushes.DarkGray;
        }

        private void WerteTesten_Click(object sender, RoutedEventArgs e) // das ist nur um meine werte zu testen, nachdem ich fertig bin, kann ich das löschen oder auskommentieren
        {
            var karten = EingabeZuKarte(TestWerte.Text);

            foreach (var karte in karten)
            {
                Tisch.CommunityHand.Add(karte);
            }

            BesteHandText.Text = "5 Karten auf dem Tisch. Drücken Sie auf Beste Hand finden. ";
            BesteHandText.Foreground = System.Windows.Media.Brushes.Purple;
        }

        private ObservableCollection<Karten> EingabeZuKarte(string eingabe)
        {
            ObservableCollection<Karten> eingabeList = new ObservableCollection<Karten>();
            var karten = eingabe.Split(','); // hier trenne ich die 5 karten 

            foreach (string karte in karten)
            {
                var symbUndWert = karte.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (symbUndWert.Length != 2)
                {
                    throw new Exception($"symb und wert ist nicht 2: {symbUndWert.Length}");
                }
                int sym = int.Parse(symbUndWert[0]);
                int wer = int.Parse(symbUndWert[1]);
                eingabeList.Add(new Karten { Symbol = sym, Wert = wer }); 
            }
            return eingabeList;
        }

        private void Flop_Click(object sender, RoutedEventArgs e)
        {
            if (Tisch.CommunityHand.Count >= 3)
            {
                MessageBox.Show("Flop wurde schon aufgedeckt! ");
                return;
            }

            for (int i = 0; i < 3; i++)
                Tisch.ZiehenDerCommunityHand();

            BesteHandText.Text = "Flop: 3 Karten auf dem Tisch. Drücke 'Turn' für die 4. Karte. ";
            BesteHandText.Foreground = System.Windows.Media.Brushes.Blue;
        }

        private void Turn_Click(object sender, RoutedEventArgs e)
        {
            if (Tisch.CommunityHand.Count >= 4)
            {
                MessageBox.Show("Turn wurde schon aufgedeckt! ");
                return;
            }
            if (Tisch.CommunityHand.Count < 3)
            {
                MessageBox.Show("Decken Sie erster die Flop Karten auf. ");
                return;
            }

            Tisch.ZiehenDerCommunityHand();
            BesteHandText.Text = "Turn: 4 Karten auf dem Tisch. Drücken 'River' für die 5. Karte. ";
            BesteHandText.Foreground = System.Windows.Media.Brushes.Purple;
        }

        private void River_Click(object sender, RoutedEventArgs e)
        {
            if (Tisch.CommunityHand.Count >= 5)
            {
                MessageBox.Show("River wurde schon aufgedeckt! ");
                return;
            }
            if (Tisch.CommunityHand.Count < 4)
            {
                MessageBox.Show("Decken Sie erster die Turn Karte auf. ");
                return;
            }

            Tisch.ZiehenDerCommunityHand();
            BesteHandText.Text = "Turn: 5 Karten auf dem Tisch. Drücke auf 'Beste Hand finden'. ";
            BesteHandText.Foreground = System.Windows.Media.Brushes.DeepPink;
        }

        private void BesteHand_Click(object sender, RoutedEventArgs e)
        {
            if (Tisch.CommunityHand.Count < 5)
            {
                MessageBox.Show("Warten Sie bis alle Community Karten aufgedeckt sind. ");
                return;
            }

            List<Karten> alleKarten = new List<Karten>();
            alleKarten.AddRange(Spieler.Hand);
            alleKarten.AddRange(Tisch.CommunityHand);

            var besteHand = Tisch.HandAuswertung(alleKarten, assWert, wertStaerkeFinden);
            var kombiStaerke = Tisch.GetKombiStaerke(besteHand);
            var wertStaerke = Tisch.GetWertStaerke(wertStaerkeFinden, assWert);

            BesteHandText.Text = $"Beste Hand: {besteHand} \nKombination-Stärke: {kombiStaerke} \nWert-Stärke: {wertStaerke}";
            BesteHandText.Foreground = System.Windows.Media.Brushes.Gold;
        }

        private void NeueRunde_Click(object sender, RoutedEventArgs e)
        {
            NeueRunde();
        }
    }
}
