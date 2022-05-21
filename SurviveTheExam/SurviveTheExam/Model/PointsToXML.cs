using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurviveTheExam.Model
{
    public class PointsToXML
    {
        public PointsToXML(string name, TimeSpan time, int score)
        {
            this.Time = time;
            this.Score = score;
            this.Name = name;
        }

        public int Score { get; set; }
        public TimeSpan Time { get; set; }
        public string Name{ get; set; }

        public override string ToString()
        {
            return $"Player name: {this.Name}\nTime: {this.Time}\nScore: {this.Score}\n------------------------------------------";
        }
    }
}
