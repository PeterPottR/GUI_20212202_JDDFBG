using SurviveTheExam.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SurviveTheExam.Control
{
    internal class Controller
    {
        ILogic log;

        public Controller(ILogic lg)
        {
            this.log = lg;
        }

        public void KeyPressed(Key key)
        {
            switch (key)
            {
                case Key.Up:
                    log.MovePlayer(GLogic.Direction.up);
                    break;
                case Key.Down:
                    log.MovePlayer(GLogic.Direction.down);
                    break;
                case Key.Left:
                    log.MovePlayer(GLogic.Direction.left);
                    break;
                case Key.Right:
                    log.MovePlayer(GLogic.Direction.right);
                    break;
            }
        }
    }
}
