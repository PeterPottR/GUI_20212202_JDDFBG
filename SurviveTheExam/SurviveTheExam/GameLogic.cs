using SurviveTheExam.Control;
using SurviveTheExam.Logic;
using SurviveTheExam.Repository;
using System;
using System.Collections.Generic;
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
        private Controller cont;
        private GLogic log;
        private string currentPlayerName;
        private string currentFileName;

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
            cont.KeyPressed(e.Key);
            this.InvalidateVisual();
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

                for (int i = 0; i < model.GameMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < model.GameMatrix.GetLength(1); j++)
                    {
                        ImageBrush brush = new ImageBrush();
                        switch (model.GameMatrix[i, j])
                        {
                            case GLogic.Items.player:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "fiu_e_1.png"), UriKind.RelativeOrAbsolute)));
                                break;
                            case GLogic.Items.zwall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "0.png"), UriKind.RelativeOrAbsolute)));
                                break;
                            case GLogic.Items.owall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "1.png"), UriKind.RelativeOrAbsolute)));
                                break;
                            case GLogic.Items.twall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "2.png"), UriKind.RelativeOrAbsolute)));
                                break;
                            case GLogic.Items.thwall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "3.png"), UriKind.RelativeOrAbsolute)));
                                break;
                            case GLogic.Items.fowall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "4.png"), UriKind.RelativeOrAbsolute)));
                                break;
                            case GLogic.Items.fvwall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "5.png"), UriKind.RelativeOrAbsolute)));
                                break;
                            case GLogic.Items.swall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "6.png"), UriKind.RelativeOrAbsolute)));
                                break;
                            case GLogic.Items.svwall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "7.png"), UriKind.RelativeOrAbsolute)));
                                break;
                            case GLogic.Items.ewall:
                                brush = new ImageBrush
                                    (new BitmapImage(new Uri(Path.Combine("images", "8.png"), UriKind.RelativeOrAbsolute)));
                                break;
                            case GLogic.Items.floor:
                                break;
                            default:
                                break;
                        }

                        drawingContext.DrawRectangle(brush, new Pen(Brushes.DarkGray, 0), new Rect(j * rectWidth, (i * rectHeight) + 50, rectWidth, rectHeight));
                    }
                }
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.rep = new GRepository();
            this.log = new GLogic(rep);
            Resize(new Size(735, 660));
            SetupModel(log);
            this.cont = new Controller(log);

            Window win = Window.GetWindow(this);
            if (win != null)
            {
                win.KeyDown += Win_KDown;
            }
            this.InvalidateVisual();
        }
    }
}
