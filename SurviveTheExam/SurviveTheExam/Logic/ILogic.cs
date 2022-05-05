using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurviveTheExam.Logic
{
    public interface ILogic
    {
        void MovePlayer();
        void MoveZh();
        void NewScore(string name, TimeSpan time, int score);
        //List<pointsToXML> ListScores();
    }
}
