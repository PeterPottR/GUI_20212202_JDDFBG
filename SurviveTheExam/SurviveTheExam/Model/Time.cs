using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SurviveTheExam.Model
{
    public class Time : Element
    {
        public Time(double x, double y)
        {
            this.area = new Rect(x, y, Config.TimeTextSize, Config.TimeTextSize);
        }
    }
}
