using SurviveTheExam.Model;
using SurviveTheExam.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        Player boy;
        public Score sc;
        public List<Tuple<int, int, int>> where = new List<Tuple<int, int, int>>();
        List<Five> fives;
        public DispatcherTimer timer = new DispatcherTimer();
        Zh zh;
        WallList wallL;
        int[] p;
        public List<Rect> wall;
        public List<Heart> hearts;
        public int FiveCount = 0;
        public Stopwatch gameTime = new Stopwatch();
        public event EventHandler Change;

        public enum Items
        {
            zwall, owall, twall, thwall, fowall, fvwall, swall, svwall, ewall, floor, coffee, five, door, zh, pl
        }
        public Items[,] GameMatrix { get; set; }
        public bool AllFivesCollected = false;

        public int[] zhCoord()
        {
            int k = 0;
            int[] cd = new int[4];
            for (int i = 0; i < GameMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < GameMatrix.GetLength(1); j++)
                {
                    if (GameMatrix[i, j] == Items.zh)
                    {
                        cd[k] = j;
                        k++;
                        cd[k] = i;
                        k++;
                        GameMatrix[i, j] = Items.floor;
                    }
                }
            }
            return cd;
        }

        public int[] PCoord()
        {
            int k = 0;
            int[] cd = new int[2];
            for (int i = 0; i < GameMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < GameMatrix.GetLength(1); j++)
                {
                    if (GameMatrix[i, j] == Items.pl)
                    {
                        cd[k] = j;
                        k++;
                        cd[k] = i;
                        GameMatrix[i, j] = Items.floor;
                    }
                }
            }
            return cd;
        }

        private readonly IRepository repo;

        public event EventHandler GameOver;

        private Queue<string> level;
        private int heartsNumb = 3;

        //public Items prev = Items.floor;

        public GLogic(Zh zh)
        {
            this.zh = zh;
        }

        public GLogic()
        {
            level = new Queue<string>();
            var lvls = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "levels"), "*.lvl");
            foreach (var item in lvls)
            {
                level.Enqueue(item);
            }
            LoadNext(level.Dequeue());
        }

        public GLogic(IRepository r, Player p, int[] pc)
        {
            this.p = pc;
            timer.Interval = TimeSpan.FromMilliseconds(15);
            timer.IsEnabled = true;
            timer.Tick += timer_Tick;
            timer.Start();
            this.boy = p;
            this.repo = r;
            sc = new Score();

            level = new Queue<string>();
            var lvls = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "levels"), "*.lvl");
            foreach (var item in lvls)
            {
                level.Enqueue(item);
            }
            LoadNext(level.Dequeue());
            hearts = new List<Heart>();

            for (int i = 0; i < heartsNumb; i++)
            {
                hearts.Add(new Heart(620 + i * 30, 709));
                //660, 709
            }
            gameTime.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            MovePlayer(7);
        }

        public void CoffeeCollected()
        {
            heartsNumb++;
            hearts.Add(new Heart(620 + (heartsNumb-1) * 30, 709));
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
            Change?.Invoke(this, null);
            //score coin keresés
            var q = where.Find(x => boy.Area.Contains(x.Item1, x.Item2) && x.Item3!=0);
            if (q != null)
            {
                //Ha coin-t veszünk fel
                if (q.Item3 == 1)
                {
                    var index = where.FindIndex(x => x.Item1 == q.Item1 && x.Item2 == q.Item2);
                    where[index] = Tuple.Create(q.Item1, q.Item2, 0);
                    sc.ScoreNum++;
                }
                //Ha ötöst veszünk fel
                else if (q.Item3 == 5)
                {
                    var index = where.FindIndex(x => x.Item1 == q.Item1 && x.Item2 == q.Item2);
                    where[index] = Tuple.Create(q.Item1, q.Item2, 0);
                    FiveCollected();
                }
                //Ha kimegyünk az ajtón
                else if (q.Item3 == 6)
                {
                    LevelFinished();
                }
                else if (q.Item3 == 2 || q.Item3 == 3)
                {
                    var h = hearts.FindIndex(x => x.Area.X == (620 + (heartsNumb - 1) * 30));
                    hearts.RemoveAt(h);
                    if (heartsNumb == 1)
                    {
                        //GameOver;
                    }

                    else 
                    { 
                        heartsNumb--;
                        boy.SetPosition((p[0] * 49) + 1, (p[1] * 44) + 50);
                        Change?.Invoke(this, null);
                    }
                }
                //Ha kávét veszünk fel
                else
                {
                    if (heartsNumb < 3)
                    {
                        var index = where.FindIndex(x => x.Item1 == q.Item1 && x.Item2 == q.Item2);
                        where[index] = Tuple.Create(q.Item1, q.Item2, 0);
                        CoffeeCollected();
                    }

                }
                
            }
        }

        private int merre = 1;

        public void MoveZh(WallList w, List<Tuple<int, int, int>> where, int numb)
        {
            this.wallL = w;
            switch (merre)
            {
                case 1:
                    if (zh.GoUp(wallL.wall))
                    {
                        zh.ChangeY(-4);
                        if (zh.GoLeft(wallL.wall) && !zh.GoRight(wallL.wall))
                        {
                            var q = zh.Random(1, 6);
                            if (q > 4 && zh.TurnLR(zh.Area.Y))
                            {
                                merre = 3;
                            }
                            else merre = 1;
                        }
                        else if (zh.GoRight(wallL.wall) && !zh.GoLeft(wallL.wall))
                        {
                            var q = zh.Random(1, 6);
                            if (q > 4 && zh.TurnLR(zh.Area.Y))
                            {
                                merre = 4;
                            }
                            else merre = 1;
                        }
                        else if (zh.GoRight(wallL.wall) && zh.GoLeft(wallL.wall))
                        {
                            //random 3as
                            var q = zh.Random(3, 7);
                            if (q < 5 && zh.TurnLR(zh.Area.Y))
                            {
                                merre = q;
                            }
                            else merre = 1;
                        }
                    }
                    else if (!zh.GoUp(wallL.wall))
                    {
                        if (zh.GoLeft(wallL.wall) && !zh.GoRight(wallL.wall) && zh.GoDown(wallL.wall))
                        {
                            var q = zh.Random(1, 50);
                            if (q > 44)
                            {
                                merre = 2;
                            }
                            else merre = 3;
                        }
                        else if (zh.GoLeft(wallL.wall) && !zh.GoRight(wallL.wall) && !zh.GoDown(wallL.wall))
                        {
                            merre = 3;
                        }
                        else if (zh.GoRight(wallL.wall) && !zh.GoLeft(wallL.wall) && zh.GoDown(wallL.wall))
                        {
                            //random 2es
                            var q = zh.Random(1, 50);
                            if (q > 44)
                            {
                                merre = 2;
                            }
                            else merre = 4;
                        }
                        else if (zh.GoRight(wallL.wall) && !zh.GoLeft(wallL.wall) && !zh.GoDown(wallL.wall))
                        {
                            merre = 4;
                        }
                        else if (zh.GoRight(wallL.wall) && zh.GoLeft(wallL.wall) && zh.GoDown(wallL.wall))
                        {
                            //random 3as
                            var q = zh.Random(1, 50);
                            if (q > 44)
                            {
                                merre = 2;
                            }
                            else if (q % 2 == 0)
                            {
                                merre = 4;
                            }
                            else merre = 3;
                        }
                        else if (zh.GoRight(wallL.wall) && zh.GoLeft(wallL.wall) && !zh.GoDown(wallL.wall))
                        {
                            //random 2es
                            var q = zh.Random(1, 50);
                            if (q % 2 == 0)
                            {
                                merre = 4;
                            }
                            else merre = 3;
                        }
                        else
                        {
                            //lefelé
                            merre = 2;
                        }
                    }
                    break;
                case 2:
                    if (zh.GoDown(wallL.wall))
                    {
                        zh.ChangeY(4);
                        if (zh.GoLeft(wallL.wall) && !zh.GoRight(wallL.wall))
                        {
                            var q = zh.Random(1, 6);
                            if (q > 4 && zh.TurnLR(zh.Area.Y))
                            {
                                merre = 3;
                            }
                            else merre = 2;
                        }
                        else if (zh.GoRight(wallL.wall) && !zh.GoLeft(wallL.wall))
                        {
                            var q = zh.Random(1, 6);
                            if (q > 4 && zh.TurnLR(zh.Area.Y))
                            {
                                merre = 4;
                            }
                            else merre = 2;
                        }
                        else if (zh.GoRight(wallL.wall) && zh.GoLeft(wallL.wall))
                        {
                            var q = zh.Random(3, 7);
                            if (q < 5 && zh.TurnLR(zh.Area.Y))
                            {
                                merre = q;
                            }
                            else merre = 2;
                        }

                    }
                    else if (!zh.GoDown(wallL.wall))
                    {
                        if (zh.GoLeft(wallL.wall) && !zh.GoRight(wallL.wall) && zh.GoUp(wallL.wall))
                        {
                            var q = zh.Random(1, 50);
                            if (q > 44)
                            {
                                merre = 1;
                            }
                            else merre = 3;
                            //random 2es
                        }
                        else if (zh.GoLeft(wallL.wall) && !zh.GoRight(wallL.wall) && !zh.GoUp(wallL.wall))
                        {
                            merre = 3;
                        }
                        else if (zh.GoRight(wallL.wall) && !zh.GoLeft(wallL.wall) && zh.GoUp(wallL.wall))
                        {
                            var q = zh.Random(1, 50);
                            if (q > 44)
                            {
                                merre = 1;
                            }
                            else merre = 4;
                        }
                        else if (zh.GoRight(wallL.wall) && !zh.GoLeft(wallL.wall) && !zh.GoUp(wallL.wall))
                        {
                            //jobb
                            merre = 4;
                        }
                        else if (zh.GoRight(wallL.wall) && zh.GoLeft(wallL.wall) && zh.GoUp(wallL.wall))
                        {
                            //random 3as
                            var q = zh.Random(1, 50);
                            if (q > 44)
                            {
                                merre = 1;
                            }
                            else if (q % 2 == 0)
                            {
                                merre = 4;
                            }
                            else merre = 3;
                        }
                        else if (zh.GoRight(wallL.wall) && zh.GoLeft(wallL.wall) && !zh.GoUp(wallL.wall))
                        {
                            //random 2es
                            var q = zh.Random(1, 50);
                            if (q % 2 == 0)
                            {
                                merre = 4;
                            }
                            else merre = 3;
                        }
                        else
                        {
                            //felfelé
                            merre = 1;
                        }
                    }
                    break;
                case 3:
                    if (zh.GoLeft(wallL.wall))
                    {
                        zh.ChangeX(-7);
                        if (zh.GoUp(wallL.wall) && !zh.GoDown(wallL.wall))
                        {
                            //random 2es
                            var q = zh.Random(1, 6);
                            if (q > 4 && zh.TurnUpDown(zh.Area.X))
                            {
                                merre = 1;
                            }
                            else merre = 3;
                        }
                        else if (zh.GoDown(wallL.wall) && !zh.GoUp(wallL.wall))
                        {
                            var q = zh.Random(1, 6);
                            if (q > 4 && zh.TurnUpDown(zh.Area.X))
                            {
                                merre = 2;
                            }
                            else merre = 3;
                        }
                        else if (zh.GoDown(wallL.wall) && zh.GoUp(wallL.wall))
                        {
                            //random 3as
                            var q = zh.Random(1, 6);
                            if (q < 3 && zh.TurnUpDown(zh.Area.X))
                            {
                                merre = q;
                            }
                            else merre = 3;
                        }
                    }
                    else if (!zh.GoLeft(wallL.wall))
                    {
                        if (zh.GoUp(wallL.wall) && !zh.GoRight(wallL.wall) && zh.GoDown(wallL.wall))
                        {
                            var q = zh.Random(1, 50);
                            if (q % 2 == 0)
                            {
                                merre = 1;
                            }
                            else merre = 2;
                        }
                        else if (zh.GoUp(wallL.wall) && !zh.GoRight(wallL.wall) && !zh.GoDown(wallL.wall))
                        {
                            merre = 1;
                        }
                        else if (zh.GoRight(wallL.wall) && !zh.GoUp(wallL.wall) && zh.GoDown(wallL.wall))
                        {
                            //random 2es
                            var q = zh.Random(1, 50);
                            if (q > 44)
                            {
                                merre = 4;
                            }
                            else merre = 2;
                        }
                        else if (!zh.GoRight(wallL.wall) && !zh.GoUp(wallL.wall) && zh.GoDown(wallL.wall))
                        {
                            merre = 2;
                        }
                        else if (zh.GoRight(wallL.wall) && zh.GoUp(wallL.wall) && zh.GoDown(wallL.wall))
                        {
                            var q = zh.Random(1, 50);
                            if (q > 44)
                            {
                                merre = 4;
                            }
                            else if (q % 2 == 0)
                            {
                                merre = 1;
                            }
                            else merre = 2;
                        }
                        else if (!zh.GoDown(wallL.wall) && zh.GoRight(wallL.wall) && zh.GoUp(wallL.wall))
                        {
                            var q = zh.Random(1, 50);
                            if (q > 44)
                            {
                                merre = 4;
                            }
                            else merre = 1;
                        }
                        else
                        {
                            //lefelé
                            merre = 4;
                        }
                    }
                    break;
                case 4:
                    if (zh.GoRight(wallL.wall))
                    {
                        zh.ChangeX(7);
                        if (zh.GoUp(wallL.wall) && !zh.GoDown(wallL.wall))
                        {
                            //random 2es
                            var q = zh.Random(1, 6);
                            if (q > 4 && zh.TurnUpDown(zh.Area.X))
                            {
                                merre = 1;
                            }
                            else merre = 4;

                        }
                        else if (zh.GoDown(wallL.wall) && !zh.GoUp(wallL.wall))
                        {
                            var q = zh.Random(1, 6);
                            if (q > 4 && zh.TurnUpDown(zh.Area.X))
                            {
                                merre = 2;
                            }
                            else merre = 4;
                        }
                        else if (zh.GoDown(wallL.wall) && zh.GoUp(wallL.wall))
                        {
                            var q = zh.Random(1, 6);
                            if (q < 3 && zh.TurnUpDown(zh.Area.X))
                            {
                                merre = q;
                            }
                            else merre = 4;
                        }
                    }
                    else if (!zh.GoRight(wallL.wall))
                    {
                        if (zh.GoUp(wallL.wall) && !zh.GoLeft(wallL.wall) && zh.GoDown(wallL.wall))
                        {
                            var q = zh.Random(1, 50);
                            if (q % 2 == 0)
                            {
                                merre = 1;
                            }
                            else merre = 2;
                        }
                        else if (zh.GoUp(wallL.wall) && !zh.GoLeft(wallL.wall) && !zh.GoDown(wallL.wall))
                        {
                            merre = 1;
                        }
                        else if (zh.GoLeft(wallL.wall) && !zh.GoUp(wallL.wall) && zh.GoDown(wallL.wall))
                        {
                            var q = zh.Random(1, 50);
                            if (q > 44)
                            {
                                merre = 3;
                            }
                            else merre = 2;
                        }
                        else if (!zh.GoLeft(wallL.wall) && !zh.GoUp(wallL.wall) && zh.GoDown(wallL.wall))
                        {
                            merre = 2;
                        }
                        else if (zh.GoLeft(wallL.wall) && zh.GoUp(wallL.wall) && zh.GoDown(wallL.wall))
                        {
                            var q = zh.Random(1, 50);
                            if (q > 44)
                            {
                                merre = 3;
                            }
                            else if (q % 2 == 0)
                            {
                                merre = 1;
                            }
                            else merre = 2;
                        }
                        else if (!zh.GoDown(wallL.wall) && zh.GoLeft(wallL.wall) && zh.GoUp(wallL.wall))
                        {
                            var q = zh.Random(1, 50);
                            if (q > 44)
                            {
                                merre = 3;
                            }
                            else merre = 1;
                        }
                        else
                        {
                            //lefelé
                            merre = 3;
                        }
                    }
                    break;
            }
            Change?.Invoke(this, null);
            var z = where.FindIndex(x => x.Item3 == numb);
            if (z != -1)
            {
                //var index = where.FindIndex(x => x.Item1 == z.Item1 && x.Item2 == z.Item2);
                where[z] = Tuple.Create(int.Parse((zh.Area.X + 24).ToString()), int.Parse((zh.Area.Y + 22).ToString()), numb);
            }
            else where.Add(Tuple.Create(int.Parse((zh.Area.X + 24).ToString()), int.Parse((zh.Area.Y + 22).ToString()), numb));
        }

        //szint befejezése, következő szint betöltése
        private void LevelFinished()
        {
            if (level.Count>0)
            {
                LoadNext(level.Dequeue());
            }
        }

        //ötösök kezelése
        public void FiveCollected()
        {
            FiveCount++;
            if(FiveCount==3)
            {
                AllFivesCollected = true;
            }
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
                case 'o': return Items.five;
                case 'k': return Items.coffee;
                case 'f': return Items.door;
                case 'z': return Items.zh;
                case 's': return Items.pl;
                default:
                    return Items.floor;
            }
        }
    }
}
