using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SurviveTheExam.Model
{
    public class Player : Element
    {
        public bool up, down, right, left;
        public int Life;
        public Player(int x, int y)
        {
            this.area = new Rect(x, y, Config.PlayerWidth, Config.PlayerHeight);
            Life = 3;
        }
        public void KeyPressed(Key key)
        {
            switch (key)
            {
                case Key.Up:
                    up = true;
                    break;
                case Key.Down:
                    down = true;
                    break;
                case Key.Left:
                    left = true;
                    break;
                case Key.Right:
                    right = true;
                    break;
            }
        }
        public void KeyRelease(Key key)
        {
            switch (key)
            {
                case Key.Up:
                    up = false;
                    break;
                case Key.Down:
                    down = false;
                    break;
                case Key.Left:
                    left = false;
                    break;
                case Key.Right:
                    right = false;
                    break;
            }
        }
    }
}
