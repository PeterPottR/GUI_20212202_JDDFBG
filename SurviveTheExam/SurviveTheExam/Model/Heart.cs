using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SurviveTheExam.Model
{
    public class Heart : Element
    {
        public int HeartCount { get; set; }

        public Heart(double x, double y)
        {
            this.area = new Rect(x, y, Config.HeartWidth, Config.HeartHeight);
            HeartCount = 3;
        }
        
    }
}
