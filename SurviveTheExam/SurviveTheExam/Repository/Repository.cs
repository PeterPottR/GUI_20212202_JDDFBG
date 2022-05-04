using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SurviveTheExam.Repository
{
    public class Repository : IRepository
    {
        private XDocument scores;

        public Repository()
        {
            this.scores = XDocument.Load("scores.xml");
        }

        //public List<pointsToXML> GetScores()
        //{
        //    List<pointsToXML> scores = new List<pointsToXML>();

        //    foreach (XElement item in this.scores.Root.Descendants("onescore"))
        //    {
        //        scores.Add(new ScoreToXML(item.Element("name").Value, Timespan.parse(item.Element("time").Value), int.Parse(item.Element("points").Value)));
        //    }

        //    List<ScoreToXML> orderedScores = scores.OrderByAscending(x => x.time).ToList();

        //    return orderedScores;
        //}

        public void NewScore(string name, TimeSpan time, int score)
        {
            if (scores == null)
            {
                scores = new XDocument();
                scores.Add(new XElement("scores"));
                XElement newScore = new XElement("onescore", new XElement("name", name), new XElement("time", time), new XElement("points", score));
                scores.Element("scores").Add(newScore);
                scores.Save("scores.xml");
            }
            else
            {
                XElement newScore = new XElement("onescore", new XElement("name", name), new XElement("time", time), new XElement("points", score));
                scores.Element("scores").Add(newScore);
                scores.Save("scores.xml");
            }
        }
    }
}
