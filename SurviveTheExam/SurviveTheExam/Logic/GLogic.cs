using SurviveTheExam.Model;
using SurviveTheExam.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public GLogic(IRepository r)
        {
            this.repo = r;

            level = new Queue<string>();
            var lvls = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "levels"), "*.lvl");
            foreach (var item in lvls)
            {
                level.Enqueue(item);
            }
            LoadNext(level.Dequeue());
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

        public void MovePlayer(Direction dc)
        {
            var coords = WhereP();
            int i = coords[0];
            int j = coords[1];
            int old_i = i;
            int old_j = j;
            switch (dc)
            {
                case Direction.up:
                    if (i - 1 >= 0) { i--; }
                    break;
                case Direction.down:
                    if (i + 1 < GameMatrix.GetLength(0)) { i++; }
                    break;
                case Direction.left:
                    if (j - 1 >= 0) { j--; }
                    break;
                case Direction.right:
                    if (j + 1 < GameMatrix.GetLength(1)) { j++; }
                    break;
                default:
                    break;
            }
            if (GameMatrix[i, j] == Items.floor)
            {
                GameMatrix[i, j] = Items.player;
                GameMatrix[old_i, old_j] = prev;
                prev = Items.floor;
            }
            else if (GameMatrix[i, j] == Items.fvwall)
            {
                GameMatrix[i, j] = Items.player;
                GameMatrix[old_i, old_j] = prev;
                prev = Items.fvwall;
            }
            else if (GameMatrix[i, j] == Items.swall)
            {
                GameMatrix[i, j] = Items.player;
                GameMatrix[old_i, old_j] = prev;
                prev = Items.swall;
            }
            else if (GameMatrix[i, j] == Items.svwall)
            {
                GameMatrix[i, j] = Items.player;
                GameMatrix[old_i, old_j] = prev;
                prev = Items.svwall;
            }
            else if (GameMatrix[i, j] == Items.ewall)
            {
                GameMatrix[i, j] = Items.player;
                GameMatrix[old_i, old_j] = prev;
                prev = Items.ewall;
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
