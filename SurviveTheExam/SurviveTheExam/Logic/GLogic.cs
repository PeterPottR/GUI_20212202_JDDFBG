using SurviveTheExam.Model;
using SurviveTheExam.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SurviveTheExam.Logic
{
    public class GLogic : ILogic
    {
        public enum Items
        {
            player, zwall, owall, twall, thwall, fowall, fvwall, swall, svwall, ewall, floor, zh
        }
        public Items[,] GameMatrix { get; set; }

        public enum Direction
        {
            up, down, left, right
        }
        //private readonly IModel model;
        private readonly IRepository repo;

        public event EventHandler GameOver;

        private Queue<string> level;

        public Items prev = Items.floor;

        Player boy;

        public List<Rect> wall;

        //Változó, amely jelzi hogy elkezdődött-e az idő már
        public bool GameStarted = false;

        //Timer a játéklépésekhez
        public DispatcherTimer timer = new DispatcherTimer();

        public GLogic(IRepository r, Player p)
        {
            timer.Interval = TimeSpan.FromMilliseconds(5);
            timer.IsEnabled = true;
            timer.Tick += timer_Tick;
            this.boy = p;
            this.repo = r;

            level = new Queue<string>();
            var lvls = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "levels"), "*.lvl");
            foreach (var item in lvls)
            {
                level.Enqueue(item);
            }
            LoadNext(level.Dequeue());
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            MovePlayer(7);
        }

        private void LoadNext(string path)
        {
            string[] lines = File.ReadAllLines(path);
            GameMatrix = new Items[int.Parse(lines[1]), int.Parse(lines[0])];
            for (int i = 0; i < GameMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < GameMatrix.GetLength(1); j++)
                {
                    GameMatrix[i, j] = ConvertToEnum(lines[i + 2][j]);
                }
            }

        }

        public void MovePlayer(int sp)
        {
            //Első nyomáskor elindul a stopper
            if (!GameStarted) { timer.Start(); GameStarted = true; }

            //mozgások
            if (boy.up)
            {
                var i = false;
                foreach (var item in wall)
                {
                    if (boy.Area.Left < item.Right - 3 && boy.Area.Right > item.Left + 3 && ((boy.Area.Top) - 7) < item.Bottom && boy.Area.Bottom > item.Top + 3)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            if (boy.Area.Top - 1 >= item.Bottom)
                            {
                                boy.ChangeY(-1);
                            }
                        }

                        i = true;
                    }
                }
                if (i == false)
                {
                    boy.ChangeY(-7);
                }

            }
            else if (boy.down)
            {
                var i = false;
                foreach (var item in wall)
                {
                    if (boy.Area.Left < item.Right - 3 && boy.Area.Right > item.Left + 3 && boy.Area.Top < item.Bottom && ((boy.Area.Bottom) + 7) > item.Top + 3)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            if (boy.Area.Bottom + 1 <= item.Top)
                            {
                                boy.ChangeY(+1);
                            }
                        }
                        i = true;
                    }
                }
                if (i == false)
                {
                    boy.ChangeY(sp);
                }
            }
            else if (boy.left)
            {
                var i = false;
                foreach (var item in wall)
                {
                    if (((boy.Area.Left) - 4) < item.Right - 3 && boy.Area.Right > item.Left + 3 && boy.Area.Top < item.Bottom && boy.Area.Bottom > item.Top + 3)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (boy.Area.Left - 1 >= item.Right)
                            {
                                boy.ChangeX(-1);
                            }
                        }
                        i = true;
                    }
                }
                if (i == false)
                {
                    boy.ChangeX(-4);
                }
            }
            else if (boy.right)
            {
                var i = false;
                foreach (var item in wall)
                {
                    if (boy.Area.Left < item.Right - 3 && ((boy.Area.Right) + 4) > item.Left + 3 && boy.Area.Top < item.Bottom && boy.Area.Bottom > item.Top + 3)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (boy.Area.Right + 1 <= item.Left)
                            {
                                boy.ChangeX(1);
                            }
                        }
                        i = true;
                    }
                }
                if (i == false)
                {
                    boy.ChangeX(4);
                }
            }
        }

        private int[] WhereP()
        {
            for (int i = 0; i < GameMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < GameMatrix.GetLength(1); j++)
                {
                    if (GameMatrix[i, j] == Items.player)
                    {
                        return new int[] { i, j };
                    }
                }
            }
            return new int[] { -1, -1 };
        }

        public void MoveZh()
        {
            throw new NotImplementedException();
        }

        public void NewScore(string name, TimeSpan time, int score)
        {
            this.repo.NewScore(name, time, score);
        }

        public List<PointsToXML> ListScores()
        {
            return this.repo.GetScores();
        }

        private Items ConvertToEnum(char v)
        {
            switch (v)
            {
                case '0': return Items.zwall;
                case '1': return Items.owall;
                case '2': return Items.twall;
                case '3': return Items.thwall;
                case '4': return Items.fowall;
                case '5': return Items.fvwall;
                case '6': return Items.swall;
                case '7': return Items.svwall;
                case '8': return Items.ewall;
                case 's': return Items.player;
                case 'z': return Items.zh;
                default:
                    return Items.floor;
            }
        }
    }
}
