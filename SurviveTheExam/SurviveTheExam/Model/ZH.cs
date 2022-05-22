using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SurviveTheExam.Model
{
    public class Zh : Element
    {
        static Random r = new Random();
        public DispatcherTimer Zh_timer = new DispatcherTimer();

        public Zh(int x, int y)
        {
            this.area = new Rect(x, y, 49, 44);
            Zh_timer.Interval = TimeSpan.FromMilliseconds(2);
            Zh_timer.IsEnabled = true;
        }

        public int Random(int min, int max)
        {
            int rnd = 0;
            rnd = r.Next(min, max + 1);
            return rnd;
        }

        public bool GoUp(List<Rect> lista)
        {
            foreach (var item in lista/*.Where(x => x.Left == area.Left)*/)
            {
                if (area.Left < item.Right && area.Right > item.Left && area.Top < item.Bottom + 1 && area.Bottom > item.Top)
                {
                    return false;
                }
            }
            return true;
        }
        public bool GoDown(List<Rect> lista)
        {
            foreach (var item in lista/*.Where(x => x.Left == area.Left)*/)
            {
                if (area.Left < item.Right && area.Right > item.Left && area.Top < item.Bottom && area.Bottom > item.Top - 1)
                {
                    return false;
                }
            }
            return true;
        }
        public bool GoLeft(List<Rect> lista)
        {
            foreach (var item in lista/*.Where(x => x.Top == area.Top)*/)
            {
                if (area.Left < item.Right + 1 && area.Right > item.Left && area.Top < item.Bottom && area.Bottom > item.Top)
                {
                    return false;
                }
            }
            return true;
        }
        public bool GoRight(List<Rect> lista)
        {
            foreach (var item in lista/*.Where(x => x.Top == area.Top)*/)
            {
                if (area.Left < item.Right && area.Right > item.Left - 1 && area.Top < item.Bottom && area.Bottom > item.Top)
                {
                    return false;
                }
            }
            return true;
        }

        public bool TurnLR(double y)
        {
            if ((y - 50) % 44 == 0)
            {
                return true;
            }
            return false;
        }
        public bool TurnUpDown(double x)
        {
            if (x % 49 == 0)
            {
                return true;
            }
            return false;
        }
    }
}
