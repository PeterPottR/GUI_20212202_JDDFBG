using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SurviveTheExam.Model
{
    public class Wall : Element
    {
        public Wall(double x, double y)
        {
            this.area = new Rect(x, y, Config.WallPlaceX, Config.WallPlaceY);
        }
    }
}
