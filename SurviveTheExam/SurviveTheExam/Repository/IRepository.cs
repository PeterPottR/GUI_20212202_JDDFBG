using SurviveTheExam.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurviveTheExam.Repository
{
    public interface IRepository
    {
        void NewScore(string name, TimeSpan time, int score);
        List<PointsToXML> GetScores();
    }
}
