using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SurviveTheExam.Model
{
    public class Player : Element
    {
        public Player(int x, int y)
        {
            this.area = new Rect(x, y, Config.PlayerWidth, Config.PlayerHeight);
        }
    }
}
