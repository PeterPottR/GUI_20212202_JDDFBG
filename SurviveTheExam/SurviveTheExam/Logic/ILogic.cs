using SurviveTheExam.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SurviveTheExam.Logic.GLogic;

namespace SurviveTheExam.Logic
{
    public interface ILogic
    {
        void MovePlayer(int sp);
        Items[,] GameMatrix { get; set; }
        void MoveZh(WallList w);
        void NewScore(string name, TimeSpan time, int score);
        List<PointsToXML> ListScores();
        event EventHandler Change;
    }
}
