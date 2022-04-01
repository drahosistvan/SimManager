using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimManager.Model
{
    public interface ISimCard
    {
        int id { get; set; }
        long MSISDN { get; set; }
        int PinCode { get; set; }
        string Subscriber { get; set; }
        DateTime Created { get; set; }
        SimCardType Type { get; set; }
    }
}
