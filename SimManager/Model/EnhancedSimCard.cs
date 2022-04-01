using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimManager.Model
{
    public class EnhancedSimCard : SimCard
    {
        private readonly int PukCode = 123456789;

        public EnhancedSimCard()
        {
            this.Type = SimCardType.Enhanced;
        }

        new public bool ReActivate(int reactivationCode)
        {
            System.Diagnostics.Debug.WriteLine("Enhanced readtivation");
            System.Diagnostics.Debug.WriteLine("Given PUK: {0}, PUK: {1}", reactivationCode, PukCode);
            if (reactivationCode == PukCode)
            {
                System.Diagnostics.Debug.WriteLine("IttE");
                Status = SimCardStatus.Active;
                return true;
            }
                
            return false;
        }
    }
}
