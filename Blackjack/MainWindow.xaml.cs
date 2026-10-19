
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Blackjack
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly Random random = new Random();

        public ObservableCollection<Card> Deck { get; private set; }
        public ObservableCollection<Card> PlayerHand { get; private set; }
        public ObservableCollection<Card> DealerHand { get; private set; }

        private int playerScore;

        public int PlayerScore
        {
            get { return playerScore; }
            set
            {
                playerScore = value;
                OnPropertyChanged();
            }
        }

        private int dealerScore;

        public int DealerScore
        {
            get { return dealerScore; }
            set
            {
                dealerScore = value;
                OnPropertyChanged();
            }
        }

        private string gameStatus = "Click New Game to begin.";

        public string GameStatus
        {
            get { return gameStatus; }
            set
            {
                gameStatus = value;
                OnPropertyChanged();
            }
        }

        private DeckWindow deckWindow;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();

            Deck = new ObservableCollection<Card>();
            PlayerHand = new ObservableCollection<Card>();
            DealerHand = new ObservableCollection<Card>();

            DataContext = this;
        }

        private void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CreateDeck()
        {
            Deck.Clear();

            string[] suits =
            {
                "Clubs", "Diamonds", "Hearts", "Spades"
            };

            string[] ranks =
            {
                "Ace", "2", "3", "4", "5", "6", "7",
                "8", "9", "10", "Jack", "Queen", "King"
            };

            foreach (string suit in suits)
            {
                foreach (string rank in ranks)
                {
                    int value;

                    if (rank == "Ace")
                        value = 11;
                    else if (rank == "Jack" ||
                             rank == "Queen" ||
                             rank == "King")
                        value = 10;
                    else
                        value = int.Parse(rank);

                    Deck.Add(new Card(suit, rank, value));
                }
            }
        }

        private void Shuffle()
        {
            for (int i = Deck.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);

                Card temp = Deck[i];
                Deck[i] = Deck[j];
                Deck[j] = temp;
            }
        }

        private Card DrawCard()
        {
            if (Deck.Count == 0)
                return null;

            Card card = Deck[0];
            Deck.RemoveAt(0);

            return card;
        }

        private void DealPlayer()
        {
            Card card = DrawCard();

            if (card != null)
                PlayerHand.Add(card);
        }

        private void DealDealer()
        {
            Card card = DrawCard();

            if (card != null)
                DealerHand.Add(card);
        }

        private int CalculateScore(ObservableCollection<Card> hand)
        {
            int score = hand.Sum(card => card.Value);
            int aces = hand.Count(card => card.Rank == "Ace");

            while (score > 21 && aces > 0)
            {
                score -= 10;
                aces--;
            }

            return score;
        }

        private void UpdateScores()
        {
            PlayerScore = CalculateScore(PlayerHand);
            DealerScore = CalculateScore(DealerHand);
        }

        private void StartNewGame()
        {
            if (deckWindow != null)
                deckWindow.Close();

            CreateDeck();
            Shuffle();

            PlayerHand.Clear();
            DealerHand.Clear();

            DealPlayer();
            DealDealer();
            DealPlayer();
            DealDealer();

            UpdateScores();

            GameStatus = "Your turn! Click Hit to draw a card.";
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }

        private void Hit_Click(object sender, RoutedEventArgs e)
        {
            if (Deck.Count == 0)
            {
                GameStatus = "The deck is empty!";
                return;
            }

            DealPlayer();
            UpdateScores();

            if (PlayerScore > 21)
                GameStatus = "Bust! You can still hit again.";
            else if (PlayerScore == 21)
                GameStatus = "21! You can still hit again.";
            else
                GameStatus = "Keep going or start a New Game.";

            if (Deck.Count == 0)
                GameStatus += " The deck is now empty.";
        }

        private void Deck_Click(object sender, RoutedEventArgs e)
        {
            if (deckWindow == null)
            {
                deckWindow = new DeckWindow(Deck);

                deckWindow.Closed += delegate
                {
                    deckWindow = null;
                };

                deckWindow.Show();
            }
            else
            {
                deckWindow.Activate();
            }
        }
    }
}
