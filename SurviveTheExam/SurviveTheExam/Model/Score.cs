using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SurviveTheExam.Model
{
    public class Score : Element
    {
        public Score(double x, double y, List<Tuple<int, int, int>> where)
        {
            this.area = new Rect(x, y, 49, 44);
            where.Add(Tuple.Create(int.Parse((this.area.X + 24).ToString()), int.Parse((this.area.Y + 22).ToString()), 1));
        }
        public Score()
        {

        }

        public int ScoreNum { get; set; }

        public BitmapImage pic = new BitmapImage(new Uri(Path.Combine("images", "score.png"), UriKind.RelativeOrAbsolute));

    }
}
