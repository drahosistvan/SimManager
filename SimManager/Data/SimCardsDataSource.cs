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
        private static ObservableCollection<SimCard> _cards = new ObservableCollection<SimCard>()
        {
            new SimCard()
            {
                id = 0,
                MSISDN = 308285112,
                Subscriber = "Drahos István",
                PinCode = 1234,
                Created = DateTime.Now,
            },
            new SimCard()
            {
                id = 1,
                MSISDN = 308285113,
                Subscriber = "Drahos István",
                PinCode = 1234,
                Created = DateTime.Now,
            },
            new SimCard()
            {
                id = 2,
                MSISDN = 308285113,
                Subscriber = "Drahos István",
                PinCode = 1234,
                Created = DateTime.Now,
            },
            new SimCard()
            {
                id = 3,
                MSISDN = 308285113,
                Subscriber = "Drahos István",
                PinCode = 1234,
                Created = DateTime.Now,
            }
        };

        public static ObservableCollection<SimCard> GetAllItems()
        {
            return _cards;
        }

        public static SimCard GetItemById(int id)
        {
            return _cards[id];
        }
    }
}
