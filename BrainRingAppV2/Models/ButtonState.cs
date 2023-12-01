using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BrainRingAppV2.Models
{
    public class ButtonState
    {
        public int ButtonId { get; set; }
        public ButtonStateEnum State { get; set; }
        public int PressOrder { get; set; }
        public int PressTime { get; set; }
        public Brush StateColor { get; set; }
    }

}
