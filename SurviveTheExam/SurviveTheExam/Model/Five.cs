using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SurviveTheExam.Model
{
    public class Five : Element
    {
        public Five(double x, double y)
        {
            this.area = new Rect(x, y, Config.CoffeeSize, Config.CoffeeSize);
        }
    }
}
