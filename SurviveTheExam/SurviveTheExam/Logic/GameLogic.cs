using SurviveTheExam.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurviveTheExam.Logic
{
    public class GameLogic : ILogic
    {
        //private readonly IModel model;
        private readonly IRepository repo;

        //ctor ahol this.model=model és this.repo=repo

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


    }
}
