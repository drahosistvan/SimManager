using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimManager.Model;

namespace SimManager.Data
{
    class SimCardsDataSource
    {
        private static ObservableCollection<ISimCard> _cards = new ObservableCollection<ISimCard>();
        

        private static void InitSimCards()
        {
            if (_cards.Count == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    ISimCard card;
                    Random random = new Random();

                    if (random.Next() > (Int32.MaxValue / 2))
                    {
                        card = new StandardSimCard();
                        card.PinCode = 1111;
                    } else
                    {
                        card = new EnhancedSimCard();
                        card.PinCode = 2222;
                    }

                    card.id = i;
                    card.MSISDN = 308285112;
                    card.Subscriber = string.Format("Subscriber {0}", i + 1);
                    card.Created = DateTime.Now;
                    
                    _cards.Add(card);
                }
            }
        }

        public static ObservableCollection<ISimCard> GetAllItems()
        {
            InitSimCards();
            return _cards;
        }

        public static ISimCard GetItemById(int id)
        {
            return _cards[id];
        }
    }
}
