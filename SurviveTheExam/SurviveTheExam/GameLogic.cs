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
        private string currentPlayerName;
        private string currentFileName;
        private Player p = new Player((7 * 49) + 1, (8 * 44) + 50);
        private Heart h = new Heart(660, 709);
        private Time t = new Time(20, 709);
        private int FiveActive = 0;
        

        public ILogic model;
        public Size size;

        public void Resize(Size size)
        {
            this.size = size;
        }

        public void SetupModel(ILogic model)
        {
            this.model = model;
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

            FormattedText formattedText = new FormattedText(testString, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Comic sans m"), 30, Brushes.White);

            formattedText.MaxTextWidth = 200;
            formattedText.MaxTextHeight = 50;

            //formattedText.SetFontStyle(FontStyles.Italic, 36, 5);
            //formattedText.SetForegroundBrush(new LinearGradientBrush(Colors.Pink, Colors.Crimson, 90.0), 36, 5);
            //formattedText.SetFontSize(36, 36, 5);
            //formattedText.SetFontWeight(FontWeights.Bold, 42, 48);

            return formattedText;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (model != null && size.Width > 100 && size.Height > 100)
            {
                double rectWidth = size.Width / model.GameMatrix.GetLength(1);
                double rectHeight = size.Height / model.GameMatrix.GetLength(0);

                drawingContext.DrawRectangle(Brushes.DarkBlue, new Pen(Brushes.Black, 0), new Rect(0, 0, 750, 55));
                drawingContext.DrawRectangle(Brushes.Black, new Pen(Brushes.Black, 0), new Rect(0, 55, 750, 700));
                drawingContext.DrawRectangle(Brushes.DarkBlue, new Pen(Brushes.Black, 0), new Rect(0, 700, 750, 800));


                drawingContext.DrawRectangle(Brushes.OrangeRed, new Pen(Brushes.Black, 0), new Rect(0, 0, 200, 50));
                drawingContext.DrawText(TimeTextSetup(), new Point(10, 10));

                log.wall = new List<Rect>();

                for (int i = 0; i < model.GameMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < model.GameMatrix.GetLength(1); j++)
                    {
                        ImageBrush brush = new ImageBrush();
                        switch (model.GameMatrix[i, j])
                        {
                            //case GLogic.Items.player:
                            //    brush = new ImageBrush
                            //        (new BitmapImage(new Uri(Path.Combine("images", "fiu_e_1.png"), UriKind.RelativeOrAbsolute)));
                            //    break;
                            case GLogic.Items.zwall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "0.png"), UriKind.RelativeOrAbsolute)));
                                var y = new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight);
                                drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), y);
                                log.wall.Add(y);
                                break;
                            case GLogic.Items.owall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "1.png"), UriKind.RelativeOrAbsolute)));
                                var x = new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight);
                                drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), x);
                                log.wall.Add(x);
                                break;
                            case GLogic.Items.twall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "2.png"), UriKind.RelativeOrAbsolute)));
                                Rect c = new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight);
                                drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), c);
                                log.wall.Add(c);
                                break;
                            case GLogic.Items.thwall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "3.png"), UriKind.RelativeOrAbsolute)));
                                Rect v = new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight);
                                drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), v);
                                log.wall.Add(v);
                                break;
                            case GLogic.Items.fowall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "4.png"), UriKind.RelativeOrAbsolute)));
                                Rect b = new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight);
                                drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), b);
                                log.wall.Add(b);
                                break;
                            case GLogic.Items.fvwall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "5.png"), UriKind.RelativeOrAbsolute)));
                                drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                break;
                            case GLogic.Items.swall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "6.png"), UriKind.RelativeOrAbsolute)));
                                drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                break;
                            case GLogic.Items.svwall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "7.png"), UriKind.RelativeOrAbsolute)));
                                drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                break;
                            case GLogic.Items.ewall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "8.png"), UriKind.RelativeOrAbsolute)));
                                drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                break;
                            case GLogic.Items.floor:
                                drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                break;
                            case GLogic.Items.five:
                                if (FiveActive == log.FiveCount)
                                {
                                    FiveActive++;
                                    brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "book.png"), UriKind.RelativeOrAbsolute)));
                                    drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                }
                                else
                                {
                                    FiveActive++;
                                    drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                                }
                                break;
                            default:
                                break;
                        }
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
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.rep = new GRepository();
            this.log = new GLogic(rep, p);

            Resize(new Size(735, 660));
            SetupModel(log);

            Window win = Window.GetWindow(this);
            if (win != null)
            {
                win.KeyDown += Win_KDown;
                win.KeyUp += Win_KUp;
            }
            this.InvalidateVisual();
        }
    }
}
