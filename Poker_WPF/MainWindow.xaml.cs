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

        public ObservableCollection<Karten> Hand { get; set; }
        public ObservableCollection<Karten> CommunityHand { get; set; }

        private int assWert = 14; // ich muss die 2 weg machen, weil ich sie ja hier nicht brauche, ich muss es irgendwie in der klasse tisch machen 
        private int[] wertStaerkeFinden = new int[3]; // das nervt mich, ich werden das später anschauen & es wahrscheinlich weg machen, weil mich es stört 

        public MainWindow()
        {
            InitializeComponent();

            Tisch = new Tisch();
            Spieler = new Spieler();

            Hand = Spieler.Hand;
            CommunityHand = Tisch.CommunityHand;
            
            DataContext = this; 
            
            NeueRunde();
        }

        private void NeueRunde() // hier werde ich in Klasse eine neue Methode erstellen, mit dem ich dann einfach es hier aufrufen werde, oder warte 
        {

            Hand.Clear();
            CommunityHand.Clear();

            Hand = Spieler.Hand;
            CommunityHand = Tisch.CommunityHand;

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

        /*private string FindeBesteHand (List<Karten> alleKarten) //ist doch wichtig wenn ich mehrere spieler habe !!!----------------------------------------------------------------------------------------------------------
        {
            var kombinationen = GetKombinationen(alleKarten, 5);

            string besteBewertung = "Keine Hand";
            int besteStaerke = -1;

            foreach (var hand in kombinationen)
            {
                string bewertung = HandAuswertung(hand);
                int staerke = GetKombiStaerke(bewertung);

                if(staerke > besteStaerke)
                {
                    besteStaerke = staerke;
                    besteBewertung = bewertung;
                }
            }

            return besteBewertung;
        }

        private List<List<Karten>> GetKombinationen(List<Karten> list, int k) //finde unnötig-----------------------------------------------------------------------------------------
        {
            var result = new List<List<Karten>>();
            Kombiniere(list, k, 0, new List<Karten>(), result);
            return result;
        }


        private void Kombiniere(List<Karten> list, int k, int start, List<Karten> aktuell, List<List<Karten>> result) //finde unnötig--------------------------------------------------------
        {
            if(aktuell.Count == k)
            {
                result.Add(new List<Karten>(aktuell));
                return;
            }

            for (int i = start; i < list.Count; i++)
            {
                aktuell.Add(list[i]);
                Kombiniere(list, k, +1, aktuell, result);
                aktuell.RemoveAt(aktuell.Count - 1);
            }
        }

        /*private string HandAuswertung (List<Karten> alleKarten, int assWert, int[] wertStaerke) 
        {
            int[] wertHaeufigkeit = new int[13];
            int[] symbolHaeufigkeit = new int[4];
            bool hatRoyal = false;
            int[] royal = new int[5];
            int[] royalSymbol = new int[4];
            int royalsAnzahl = 0;
            int assVorhanden = 0;
            Array.Clear(wertStaerke, 0, 3);

            foreach (var karte in alleKarten) // hier wird die häufigkeit von wert und symbol bestimmt
            {
                wertHaeufigkeit[karte.Wert]++;
                symbolHaeufigkeit[karte.Symbol]++;

                if (karte.Wert == 8 || karte.Wert == 9 || karte.Wert == 10 || karte.Wert == 11 || karte.Wert == 12) // hier stelle ich fest ob ich die höchsten werte habe
                {
                    royalSymbol[karte.Symbol]++;
                    royal[karte.Wert - 8]++;
                }
            }
            
            // Prüfen 
            for (int i = 0; i < 4; i++)
            {
                if (royalSymbol[i] >= 5)
                {
                    for(int j = 0; j < 5; j++) // hier erfahre ich wie viele von den royals ich habe
                        if (royal[j] > 0)
                            royalsAnzahl++;
                }
            }

            for (int i = 0; i < alleKarten.Count; i++) // bubble sort der karten in der hand
            {
                for (int j = 0; j < alleKarten.Count; j++)
                {
                    if (alleKarten[i].Wert > alleKarten[j].Wert)
                        (alleKarten[i], alleKarten[j]) = (alleKarten[j], alleKarten[i]);
                }
            }

            bool hatPaar = false;
            int anzahlPaare = 0;
            bool hatDrilling = false;
            bool hatStreet = false;
            int anzahlFuerStreet = 0;
            bool hatFlush = false; 
            bool hatVierling = false;

            for (int i = 0; i < wertHaeufigkeit.Length; i++) // hier erfahre ich wie viele ich von einem Wert habe == die bestimmte häufigkeit
            {
                if (wertHaeufigkeit[i] == 2)
                {
                    if (anzahlPaare == 0)
                        wertStaerke[2] = i + 2;
                    else
                    {
                        if (wertHaeufigkeit[2] != 0)
                            wertStaerke[2] = wertStaerke[1];
                        wertStaerke[1] = i + 2;
                    }
                    hatPaar = true;
                    anzahlPaare++;
                }
                else if (wertHaeufigkeit[i] == 3)
                {
                    hatDrilling = true;
                    wertStaerke[0] = i + 2;
                }
                else if (wertHaeufigkeit[i] == 4)
                {
                    Array.Clear(wertStaerke, 0, 3);
                    hatVierling = true;
                    wertStaerke[0] = i + 2;
                }
            }
            
            int gabsAss = 0;
            int symbol = 0;
            int nichtSymbol = 0;

            for (int i = 0; i < symbolHaeufigkeit.Length; i++) // hier wird geschaut ob ein symbol 5 mal vorhanden ist = flush
            {
                if (symbolHaeufigkeit[i] >= 5)
                {
                    symbol = i;
                    hatFlush = true;
                    wertStaerke[0] = alleKarten[0].Wert + 2;
                }
            }

            if (hatFlush)
            {
                for (int i = 0; i < alleKarten.Count - nichtSymbol; i++)
                {
                    if (alleKarten[i].Symbol != symbol)
                    {
                        nichtSymbol++;
                        var hilfe = alleKarten[i];
                        for (int j = i; j < alleKarten.Count - 1; j++)
                            alleKarten[j] = alleKarten[j + 1];
                        alleKarten[alleKarten.Count - 1] = hilfe; 
                    }
                }
            }

            int hoechsterWertInStreet = 0; 
            for (int i = 1; i < alleKarten.Count; i++) // hier wird geschaut ob ich eine street habe
            {
                int activity = 0; 
                if (hatFlush && i == alleKarten.Count - nichtSymbol)
                    break;
                int hilfeFuerStreet = alleKarten[i - 1].Wert;
                if (hilfeFuerStreet == 12 && i == 1)
                    gabsAss = 1;

                if (hilfeFuerStreet == alleKarten[i].Wert && assVorhanden == 0 || hilfeFuerStreet == alleKarten[i].Wert && hilfeFuerStreet != 0)
                    continue;

                if (hilfeFuerStreet - 1 == alleKarten[i].Wert)
                {
                    if (anzahlFuerStreet == 0)
                        hoechsterWertInStreet = hilfeFuerStreet;
                    anzahlFuerStreet++;
                    activity++;
                }
                if (assVorhanden == 1 && anzahlFuerStreet < 4 && hilfeFuerStreet == 0 || assVorhanden == 1 && anzahlFuerStreet < 4 && alleKarten[i].Wert == 0)
                {
                    assWert = hilfeFuerStreet - 1;
                    anzahlFuerStreet++;
                }
                else if (anzahlFuerStreet < 4 && activity == 0) 
                {   
                    if (gabsAss ==1)
                            assVorhanden = 1;
                    gabsAss = 0;
                    anzahlFuerStreet = 0;
                }
            }

            if (anzahlFuerStreet >= 4)
            {
                Array.Clear(wertStaerke, 0, 3);
                wertStaerke[0] = hoechsterWertInStreet + 2;
                hatStreet = true;
            }

            if (royalsAnzahl == 5)
            {
                Array.Clear(wertStaerke, 0, 3);
                wertStaerke[0] = 14;
                hatRoyal = true;
            }

            if (hatRoyal && hatFlush) return "ROYAL FLUSH!";
            if (hatStreet && hatFlush && anzahlFuerStreet >= 4) 
            {
                Array.Clear(wertStaerke, 0, 3);
                wertStaerke[0] = hoechsterWertInStreet + 2;
                return "STRAIGHT FLUSH!";
            }
            if (hatVierling) return "VIERLING!";
            if (hatPaar && hatDrilling) return "FULL HOUSE!";
            if (hatFlush) return "FLUSH!";
            if (hatStreet) return "STREET!";    
            if (hatDrilling) return "DRILLING!";
            if (anzahlPaare >= 2) return "ZWEI PAARE!";
            if (hatPaar) return "EIN PAAR!";

            Array.Clear(wertStaerke, 0, 3);
            wertStaerke[0] = alleKarten[0].Wert + 2;
            return "HÖCHSTE KARTE!"; 
        } 

        private int GetKombiStaerke(string bewertung) // für die Kombination Stärke
        {
            switch (bewertung)
            {
                case "ROYAL FLUSH!": return 10; 
                case "STRAIGHT FLUSH!": return 9;
                case "VIERLING!": return 8;
                case "FULL HOUSE!": return 7;
                case "FLUSH!": return 6;
                case "STREET!": return 5; 
                case "DRILLING!": return 4;
                case "ZWEI PAARE!": return 3;
                case "EIN PAAR!": return 2;
                case "HÖCHSTE KARTE!": return 1;
                default: return 0;
            };
        }

        int wertStaerkeIndex = 0;
        private int GetWertStaerke(int[] wert, int assWert) // das ist irgendwie voll ünnötig, ich glaube ich werde das weg machen 
        {
            bool staerkeGegeben = false;
            int staerke = 0; 
            for (int i = wertStaerkeIndex; i < wert.Length; i++)
            {
                wertStaerkeIndex = i;
                
                if (staerkeGegeben)
                    break;
                if (wertStaerkeIndex == wert.Length - 1)
                    wertStaerkeIndex = 0;

                if (wert[i] == 14)
                    staerke = assWert;
                else
                    staerke = wert[i];

                if (wert[i] > 0)
                    staerkeGegeben = true; 
                
                /*switch (wert[i]) // ich muss überlegen, wie ich das noch machen, falls 2 spieler den selben wert haben. weil hier gebe ich nur den ersten wert aus, anstatt alle..., ich habe auch schon bisschen eine grobe idee.
                {
                    case 14: 
                        staerke = assWert;
                        breackGemacht = true;
                        break;
                    case 13: 
                        staerke = 13;
                        breackGemacht = true;
                        break;
                    case 12:
                        staerke = 12;
                        breackGemacht = true;
                        break;
                    case 11:
                        staerke = 11;
                        breackGemacht = true;
                        break;
                    case 10:
                        staerke = 10;
                        breackGemacht = true;
                        break;
                    case 9:
                        staerke = 9;
                        breackGemacht = true;
                        break;
                    case 8:
                        staerke = 8;
                        breackGemacht = true;
                        break;
                    case 7:
                        staerke = 7;
                        breackGemacht = true;
                        break;
                    case 6:
                        staerke = 6;
                        breackGemacht = true;
                        break;
                    case 5:
                        staerke = 5;
                        breackGemacht = true;
                        break;
                    case 4:
                        staerke = 4;
                        breackGemacht = true;
                        break;
                    case 3:
                        staerke = 3;
                        breackGemacht = true;
                        break;
                    case 2:
                        staerke = 2;
                        breackGemacht = true;
                        break;
                    default:
                        continue;
                }
            }
            return staerke;
        }*/
    }
}
