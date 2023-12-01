using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainRingAppV2.Models
{
    public enum PhaseEnum
    {
        Idle,
        RegisterFalseStarts,
        RegisterEarly,
        RegisterOther,
        Stopped,
        Unknown
    }
}
