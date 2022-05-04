using SurviveTheExam.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurviveTheExam.Logic
{
    public class GameLogic : ILogic
    {
        public IGameModel.MapItem[,] Map
        { get; set; }

        private Queue<string> levelNames =
            new Queue<string>();




        //private readonly IModel model;
        private readonly IRepository repo;

        //ctor ahol this.model=model és this.repo=repo
        public GameLogic()
        {
            var lvls = Directory.GetFiles(Path
        .Combine(Directory
        .GetCurrentDirectory(),
        "Levels"));

            foreach (string lvl in lvls)
            {
                levelNames.Enqueue(lvl);
            }

            LoadLevel(levelNames.Dequeue());
        }


        public event EventHandler GameOver;

        public void MovePlayer()
        {
            throw new NotImplementedException();
        }

        public void MoveZh()
        {
            throw new NotImplementedException();
        }

        public void NewScore(string name, TimeSpan time, int score)
        {
            throw new NotImplementedException();
        }

        //??refreshscreen?

        private void LoadLevel(string fileName)
        {

            string[] lines =
                File.ReadAllLines(fileName);

            Map = new IGameModel.MapItem[
                int.Parse(lines[0]),
                int.Parse(lines[1])];

            for (int x = 0; x < Map.GetLength(0); x++)
            {
                for (int y = 0; y < Map.GetLength(1); y++)
                {
                    Map[x, y] = CharToMapItem(lines[y + 2][x]);
                    if (Map[x, y] == IGameModel.MapItem.Player)
                    {
                        playerX = x;
                        playerY = y;
                    }
                }
            }

            // TODO: jatekos kezdeti
            // poziciojanak beallitasa
        }

        private IGameModel.MapItem
            CharToMapItem(char c)
        {

            switch (c)
            {
                case '0': return IGameModel.MapItem.Wall;
                case '1': return IGameModel.MapItem.Wall;
                case '2': return IGameModel.MapItem.Wall;
                case '3': return IGameModel.MapItem.Wall;
                case '4': return IGameModel.MapItem.Wall;
                case '5': return IGameModel.MapItem.Wall;
                case '6': return IGameModel.MapItem.Wall;
                case '7': return IGameModel.MapItem.Wall;
                case '8': return IGameModel.MapItem.Wall;
                case 's': return IGameModel.MapItem.Player;
                case 'f': return IGameModel.MapItem.Exit;
                case 'o': return IGameModel.MapItem.Otos;
                case 'z': return IGameModel.MapItem.Zh;
                default: return IGameModel.MapItem.Floor;
            }
        }
    }


}
}
