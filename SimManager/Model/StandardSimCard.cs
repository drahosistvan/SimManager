using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimManager.Model
{
    public class StandardSimCard : SimCard
    {
        private int TriesCount = 0;

        public StandardSimCard()
        {
            this.Type = SimCardType.Standard;
        }
    }
}
