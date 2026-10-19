
using System.Collections.ObjectModel;
using System.Windows;

namespace Blackjack
{
    public partial class DeckWindow : Window
    {
        private ObservableCollection<Card> deck;

        public DeckWindow(ObservableCollection<Card> gameDeck)
        {
            InitializeComponent();

            deck = gameDeck;
            DataContext = deck;

            if (deck.Count > 0)
                deckList.SelectedIndex = 0;
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            int index = deckList.SelectedIndex;

            if (index <= 0)
                return;

            deck.Move(index, index - 1);
            deckList.SelectedIndex = index - 1;
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            int index = deckList.SelectedIndex;

            if (index < 0 || index >= deck.Count - 1)
                return;

            deck.Move(index, index + 1);
            deckList.SelectedIndex = index + 1;
        }
    }
}
