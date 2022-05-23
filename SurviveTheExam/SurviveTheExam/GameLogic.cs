using SurviveTheExam.Control;
using SurviveTheExam.Logic;
using SurviveTheExam.Model;
using SurviveTheExam.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SurviveTheExam
{
    public class GameLogic :FrameworkElement
    {
        private IRepository rep;
        private GLogic log;
        private GLogic Clog;
        private GLogic zlog;
        private GLogic z2log;
        private string currentPlayerName;
        private string currentFileName;
        private Player p;
        private Heart h = new Heart(660, 709);
        private Time t = new Time(20, 709);
        private int FiveActive = 0;
        private WallList wall = new WallList();
        //private int[] zhCoord;
        //private int[] PCoord = new int[2];
        private Zh zh;
        private Zh zh2;


        public ILogic model;
        public Size size;

        public void Resize(Size size)
        {
            this.size = size;
        }

        public void SetupModel(ILogic model)
        {
            this.model = model;
            this.model.Change += (sender, eventargs) => this.InvalidateVisual();
        }

        public GameLogic()
        {
            this.Loaded += Window_Loaded;
            
        }
        public GameLogic(string playerName)
        {
            this.Loaded += this.Window_Loaded;
            this.currentPlayerName = playerName;
        }

        private void Win_KDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            p.KeyPressed(e.Key);
            this.InvalidateVisual();
        }
        private void Win_KUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            p.KeyRelease(e.Key);
            this.InvalidateVisual();
        }

        private FormattedText TimeTextSetup()
        {
            int ElapsedTime = (int)log.gameTime.ElapsedMilliseconds /1000;
            int seconds = ElapsedTime % 60;
            int minutes = ElapsedTime / 60;
            string testString = $"Time: {minutes}:{seconds}";

            FormattedText formattedText = new FormattedText(testString, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Comic Sans MS"), 30, Brushes.White);

            formattedText.MaxTextWidth = 200;
            formattedText.MaxTextHeight = 50;

            //formattedText.SetFontStyle(FontStyles.Italic, 36, 5);
            //formattedText.SetForegroundBrush(new LinearGradientBrush(Colors.Pink, Colors.Crimson, 90.0), 36, 5);
            //formattedText.SetFontSize(36, 36, 5);
            //formattedText.SetFontWeight(FontWeights.Bold, 42, 48);

            return formattedText;
        }

        private FormattedText ScoreTextSetUp()
        {
            int score = log.sc.ScoreNum;
            string scoreString = $"Score: {score}";

            FormattedText formattedText = new FormattedText(scoreString, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Comic Sans MS"), 30, Brushes.White);

            formattedText.MaxTextWidth = 200;
            formattedText.MaxTextHeight = 50;

            return formattedText;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (model != null && size.Width > 100 && size.Height > 100)
            {
                double rectWidth = size.Width / model.GameMatrix.GetLength(1);
                double rectHeight = size.Height / model.GameMatrix.GetLength(0);

                //zhCoord = new int[4];

                drawingContext.DrawRectangle(Brushes.DarkBlue, new Pen(Brushes.Black, 0), new Rect(0, 0, 750, 55));
                drawingContext.DrawRectangle(Brushes.Black, new Pen(Brushes.Black, 0), new Rect(0, 55, 750, 700));
                drawingContext.DrawRectangle(Brushes.DarkBlue, new Pen(Brushes.Black, 0), new Rect(0, 700, 750, 800));

                drawingContext.DrawRectangle(Brushes.OrangeRed, new Pen(Brushes.Black, 0), new Rect(0, 0, 200, 50));
                drawingContext.DrawText(TimeTextSetup(), new Point(10, 10));
                drawingContext.DrawText(ScoreTextSetUp(), new Point(250, 10));

                log.wall = new List<Rect>();
                wall.wall = new List<Rect>();

                for (int i = 0; i < model.GameMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < model.GameMatrix.GetLength(1); j++)
                    {
                        ImageBrush brush = new ImageBrush();
                        switch (model.GameMatrix[i, j])
                        {
                            case GLogic.Items.zwall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "0.png"), UriKind.RelativeOrAbsolute)));
                                var y = new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight);
                                drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), y);
                                log.wall.Add(y);
                                wall.wall.Add(y);
                                break;
                            case GLogic.Items.owall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "1.png"), UriKind.RelativeOrAbsolute)));
                                var x = new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight);
                                drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), x);
                                log.wall.Add(x);
                                wall.wall.Add(x);
                                break;
                            case GLogic.Items.twall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "2.png"), UriKind.RelativeOrAbsolute)));
                                Rect c = new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight);
                                drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), c);
                                log.wall.Add(c);
                                wall.wall.Add(c);
                                break;
                            case GLogic.Items.thwall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "3.png"), UriKind.RelativeOrAbsolute)));
                                Rect v = new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight);
                                drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), v);
                                log.wall.Add(v);
                                wall.wall.Add(v);
                                break;
                            case GLogic.Items.fowall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "4.png"), UriKind.RelativeOrAbsolute)));
                                Rect b = new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight);
                                drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), b);
                                log.wall.Add(b);
                                wall.wall.Add(b);
                                break;
                            case GLogic.Items.fvwall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "5.png"), UriKind.RelativeOrAbsolute)));
                                drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                if (log.where.Exists(x => x.Item1 == (j * rectWidth + 24) && x.Item2 == (i * rectHeight) + 72 && x.Item3 == 1))
                                {
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("images", "score.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                }
                                else if (log.where.Exists(x => x.Item1 == (j * rectWidth + 24) && x.Item2 == (i * rectHeight) + 72 && x.Item3 == 0))
                                {
                                }
                                else
                                {
                                    var q = new Score(j * rectWidth, (i * rectHeight) + 50, log.where);
                                    drawingContext.DrawRectangle(new ImageBrush(q.pic), new Pen(Brushes.DarkGray, 0), q.Area);
                                }
                                break;
                            case GLogic.Items.swall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "6.png"), UriKind.RelativeOrAbsolute)));
                                drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                if (log.where.Exists(x => x.Item1 == (j * rectWidth + 24) && x.Item2 == (i * rectHeight) + 72 && x.Item3 == 1))
                                {
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("images", "score.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                }
                                else if (log.where.Exists(x => x.Item1 == (j * rectWidth + 24) && x.Item2 == (i * rectHeight) + 72 && x.Item3 == 0))
                                {
                                }
                                else
                                {
                                    var q = new Score(j * rectWidth, (i * rectHeight) + 50, log.where);
                                    drawingContext.DrawRectangle(new ImageBrush(q.pic), new Pen(Brushes.DarkGray, 0), q.Area);
                                }
                                break;
                            case GLogic.Items.svwall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "7.png"), UriKind.RelativeOrAbsolute)));
                                drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                if (log.where.Exists(x => x.Item1 == (j * rectWidth + 24) && x.Item2 == (i * rectHeight) + 72 && x.Item3 == 1))
                                {
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("images", "score.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                }
                                else if (log.where.Exists(x => x.Item1 == (j * rectWidth + 24) && x.Item2 == (i * rectHeight) + 72 && x.Item3 == 0))
                                {
                                }
                                else
                                {
                                    var q = new Score(j * rectWidth, (i * rectHeight) + 50, log.where);
                                    drawingContext.DrawRectangle(new ImageBrush(q.pic), new Pen(Brushes.DarkGray, 0), q.Area);
                                }
                                break;
                            case GLogic.Items.ewall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "8.png"), UriKind.RelativeOrAbsolute)));
                                drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                if (log.where.Exists(x => x.Item1 == (j * rectWidth + 24) && x.Item2 == (i * rectHeight) + 72 && x.Item3 == 1))
                                {
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("images", "score.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                }
                                else if (log.where.Exists(x => x.Item1 == (j * rectWidth + 24) && x.Item2 == (i * rectHeight) + 72 && x.Item3 == 0))
                                {
                                }
                                else
                                {
                                    var q = new Score(j * rectWidth, (i * rectHeight) + 50, log.where);
                                    drawingContext.DrawRectangle(new ImageBrush(q.pic), new Pen(Brushes.DarkGray, 0), q.Area);
                                }
                                break;
                            case GLogic.Items.floor:
                                drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                if (log.where.Exists(x => x.Item1 == (j * rectWidth + 24) && x.Item2 == (i * rectHeight) + 72 && x.Item3 == 1))
                                {
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("images", "score.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                }
                                else if (log.where.Exists(x => x.Item1 == (j * rectWidth + 24) && x.Item2 == (i * rectHeight) + 72 && x.Item3 == 0))
                                {
                                }
                                else
                                {
                                    var q = new Score(j * rectWidth, (i * rectHeight) + 50, log.where);
                                    drawingContext.DrawRectangle(new ImageBrush(q.pic), new Pen(Brushes.DarkGray, 0), q.Area);
                                }
                                break;
                            //ötös implementálás
                            case GLogic.Items.five:
                                //where.Add(Tuple.Create(int.Parse((this.area.X + 24).ToString()), int.Parse((this.area.Y + 22).ToString()), 1));
                                if (FiveActive == log.FiveCount)
                                { 
                                    FiveActive++;
                                    if (log.where.Exists(x => x.Item1 == (j * rectWidth + 24) && x.Item2 == (i * rectHeight) + 72 && x.Item3 == 5))
                                    {
                                        brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "five.png"), UriKind.RelativeOrAbsolute)));
                                        drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                    }
                                    else if (log.where.Exists(x => x.Item1 == (j * rectWidth + 24) && x.Item2 == (i * rectHeight) + 72 && x.Item3 == 0))
                                    {
                                    }
                                    else
                                    {
                                        var q = new Five(j * rectWidth, (i * rectHeight) + 50, log.where);
                                        drawingContext.DrawRectangle(new ImageBrush(q.pic), new Pen(Brushes.DarkGray, 0), q.Area);
                                    }
                                }
                                else
                                {
                                    FiveActive++;
                                    drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                }
                                break;
                            //kávé implementálás
                            case GLogic.Items.coffee:
                                if (log.hearts.Count<3)
                                {
                                    if (log.where.Exists(x => x.Item1 == (j * rectWidth + 24) && x.Item2 == (i * rectHeight) + 72 && x.Item3 == 3))
                                    {
                                        brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "coffee.png"), UriKind.RelativeOrAbsolute)));
                                        drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                    }
                                    else if (log.where.Exists(x => x.Item1 == (j * rectWidth + 24) && x.Item2 == (i * rectHeight) + 72 && x.Item3 == 0))
                                    {
                                    }
                                    else
                                    {
                                        var q = new Coffee(j * rectWidth, (i * rectHeight) + 50, log.where);
                                        drawingContext.DrawRectangle(new ImageBrush(q.pic), new Pen(Brushes.DarkGray, 0), q.Area);
                                    }
                                }
                                else
                                {
                                    drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                }
                                break;
                            //kijárat implementálás
                            case GLogic.Items.door:
                                if (log.AllFivesCollected)
                                {
                                    if (log.where.Exists(x => x.Item1 == (j * rectWidth + 24) && x.Item2 == (i * rectHeight) + 72 && x.Item3 == 6))
                                    {
                                        brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "door.png"), UriKind.RelativeOrAbsolute)));
                                        drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                    }
                                    else if (log.where.Exists(x => x.Item1 == (j * rectWidth + 24) && x.Item2 == (i * rectHeight) + 72 && x.Item3 == 0))
                                    {
                                    }
                                    else
                                    {
                                        var q = new Finish(j * rectWidth, (i * rectHeight) + 50, log.where);
                                        drawingContext.DrawRectangle(new ImageBrush(q.pic), new Pen(Brushes.DarkGray, 0), q.Area);
                                    }
                                }
                                else
                                {
                                    drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                }
                                break;
                            default:
                                break;
                        }
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("images", "zh2_bal.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.DarkGray, 0), zh2.Area);
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("images", "zh1_bal.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.DarkGray, 0), zh.Area);
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("images", "fiu_e_1.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.DarkGray, 0), p.Area);
                        foreach (var item in log.hearts)
                        {
                            drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("images", "life.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.DarkGray, 0), item.Area);
                        }
                    }
                }
                FiveActive = 0;
            }
        }

        private void zh_Tick(object sender, EventArgs e)
        {
            zlog.MoveZh(wall,log.where, 2);
        }
        private void zh1_Tick(object sender, EventArgs e)
        {
            z2log.MoveZh(wall, log.where, 3);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.rep = new GRepository();
            this.Clog = new GLogic();
            int[] plc = Clog.PCoord();
            p = new Player((plc[0] * 49) + 1, (plc[1] * 44) + 50);
            this.log = new GLogic(rep, p, plc);

            int[] c = Clog.zhCoord();

            zh = new Zh(c[0] * 49, (c[1] * 44)+50);
            zh2 = new Zh(c[0] * 49, (c[1] * 44) + 50);

            this.zlog = new GLogic(zh);
            this.z2log = new GLogic(zh2);

            Resize(new Size(735, 660));
            SetupModel(log);

            Window win = Window.GetWindow(this);
            if (win != null)
            {
                win.KeyDown += Win_KDown;
                win.KeyUp += Win_KUp;
            }

            zh2.Zh_timer.Start();
            zh.Zh_timer.Start();
            zh.Zh_timer.Tick += zh_Tick;
            zh2.Zh_timer.Tick += zh1_Tick;

            this.InvalidateVisual();
        }
    }
}
