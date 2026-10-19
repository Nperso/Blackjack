


using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Blackjack
{
    public class CardImageConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            Card card = value as Card;

            if (card == null)
                return null;

            int suitNumber = -1;

            if (card.Suit == "Clubs")
                suitNumber = 0;
            else if (card.Suit == "Diamonds")
                suitNumber = 1;
            else if (card.Suit == "Hearts")
                suitNumber = 2;
            else if (card.Suit == "Spades")
                suitNumber = 3;

            int rankNumber = 0;

            if (card.Rank == "Ace")
                rankNumber = 1;
            else if (card.Rank == "Jack")
                rankNumber = 11;
            else if (card.Rank == "Queen")
                rankNumber = 12;
            else if (card.Rank == "King")
                rankNumber = 13;
            else
                int.TryParse(card.Rank, out rankNumber);

            if (suitNumber < 0 || rankNumber < 1 ||
                rankNumber > 13)
                return null;

            int imageNumber = suitNumber * 13 + rankNumber;

            // Your files only go up to 51.
            if (imageNumber > 51)
                return null;

            string imageFilename = imageNumber.ToString("00") + ".png";

            string imagePath =
                "pack://application:,,,/Blackjack;component/images/"
                + imageFilename;

            try
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(imagePath);
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();

                return image;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}

