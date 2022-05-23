using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurviveTheExam.Model
{
    public class PointsToXML
    {
        public PointsToXML(string name, string time, int score)
        {
            int m = int.Parse(time.Split(':')[0]) * 60;
            int s = int.Parse(time.Split(':')[1]);
            this.Time = TimeSpan.FromSeconds(m+s);
            this.Tm = time;
            this.Score = score;
            this.Name = name;
        }

        public int Score { get; set; }
        public TimeSpan Time { get; set; }
        public string Tm { get; set; }
        public string Name{ get; set; }

        public override string ToString()
        {
            return $"Player name: {this.Name}\nTime: {this.Tm}\nScore: {this.Score}\n------------------------------------------";
        }
    }
}
