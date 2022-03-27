using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimManager.Model
{
    public class SimCard
    {
        public int id { get; set; }
        public long MSISDN { get; set; }
        public int PinCode { get; set; }
        public string Subscriber { get; set; }
        public DateTime Created { get; set; }
        public SimCardType Type { get; set; }
        public SimCardStatus Status = SimCardStatus.Inactive;
        private int Balance = 0;
        private int TriesCount = 0;

        public SimCard()
        {

        }

        public string GetInfo()
        {
            return string.Format("MSISDN: {0}; Subscriber: {1}; PinCode: {2}", MSISDN, Subscriber, PinCode);
        }

        public int GetBalance()
        {
            Balance += new Random().Next(0,1000);
            return Balance;
        }

        public bool Activate(int givenPinCode)
        {
            if (givenPinCode == PinCode)
            {
                TriesCount = 0;
                Status = SimCardStatus.Active;
                return true;
            }
            TriesCount++;

            if (TriesCount >= 3)
            {
                this.Status = SimCardStatus.Disabled;
            }

            return false;
        }
    }
}
