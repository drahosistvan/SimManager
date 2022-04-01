using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimManager.Model
{
    public class SimCard: ISimCard
    {
        public int id { get; set; }
        public long MSISDN { get; set; }
        public int PinCode { get; set; }
        public string Subscriber { get; set; }
        public DateTime Created { get; set; }
        public SimCardType Type { get; set; }
        public SimCardStatus Status = SimCardStatus.Inactive;
        public string StatusText { 
            get {
                switch (Status)
                {
                    case SimCardStatus.Inactive:
                        return "A kártya inaktív";
                    case SimCardStatus.Active:
                        return "A kártya aktív";
                    default:
                        return "A kártya letiltott";

                }
            } 
        }
        public bool CanBeReactivated { get => Type == SimCardType.Enhanced; }

        private int Balance = 0;
        private int TriesCount = 0;

        public string GetInfo()
        {
            return string.Format("MSISDN: {0}; Subscriber: {1}; PinCode: {2}", MSISDN, Subscriber, PinCode);
        }

        public int GetBalance()
        {
            Balance += new Random().Next(0,1000);
            return Balance;
        }

        public virtual bool Activate(int givenPinCode)
        {
            if (Status == SimCardStatus.Inactive)
            {
                if (givenPinCode == PinCode)
                {
                    TriesCount = 0;
                    Status = SimCardStatus.Active;
                    return true;
                }
                TriesCount++;
            }

            if (TriesCount > 2)
            {
                System.Diagnostics.Debug.WriteLine("Disabling");
                Status = SimCardStatus.Disabled;
            }

            System.Diagnostics.Debug.WriteLine("Tries counter: " + TriesCount);


            return false;
        }

        public virtual bool ReActivate(int reactivationCode)
        {
            System.Diagnostics.Debug.WriteLine("Base readtivation");
            return false;
        }
    }
}
