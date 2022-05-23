using SurviveTheExam.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SurviveTheExam.Repository
{
    public class GRepository : IRepository
    {
        private XDocument scores;

        public GRepository()
        {
            if (File.Exists("scores.xml"))
            {
                this.scores = XDocument.Load("scores.xml");
            }
        }

        public List<PointsToXML> GetScores()
        {
            List<PointsToXML> scores = new List<PointsToXML>();

            foreach (XElement item in this.scores.Root.Descendants("onescore"))
            {
                scores.Add(new PointsToXML(item.Element("name").Value, item.Element("time").Value, int.Parse(item.Element("points").Value)));
            }

            List<PointsToXML> orderedScores = scores.OrderByDescending(x => x.Time).ToList();

            return orderedScores;
        }

        public void NewScore(string name, TimeSpan time, int score)
        {
            string tm = time.ToString(@"mm\:ss");
            if (scores == null)
            {
                scores = new XDocument();
                scores.Add(new XElement("scores"));
                XElement newScore = new XElement("onescore", new XElement("name", name), new XElement("time", tm), new XElement("points", score));
                scores.Element("scores").Add(newScore);
                scores.Save("scores.xml");
            }
            else
            {
                XElement newScore = new XElement("onescore", new XElement("name", name), new XElement("time", tm), new XElement("points", score));
                scores.Element("scores").Add(newScore);
                scores.Save("scores.xml");
            }
        }
    }
}
