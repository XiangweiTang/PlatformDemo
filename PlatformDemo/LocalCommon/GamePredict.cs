using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformDemo
{
    abstract class GamePredict
    {
        protected Dictionary<string, int> InitDict = null;
        protected (string team1, string team2)[] TeamSequence = null;
        public GamePredict(Dictionary<string,int> initDict, (string,string)[] teamSequence)
        {
            InitDict = initDict;
            TeamSequence = teamSequence;
        }
        public void RunSequence()
        {
            List<GameStatus> list = new List<GameStatus> { new GameStatus { Dict = InitDict, GameSequence = "" } };
            foreach (var teams in TeamSequence)
            {
                list = list.SelectMany(x => Game(x, teams.team1, teams.team2)).ToList();
            }
            OutputResult(list);
        }
        abstract protected IEnumerable<GameStatus> Game(GameStatus status, string team1, string team2);
        abstract protected void OutputResult(List<GameStatus> list);
    }

    abstract class GameEstimationBO2 : GamePredict
    {
        protected GameEstimationBO2(Dictionary<string, int> initDict, (string, string)[] teamSequence) : base(initDict, teamSequence)
        {
        }

        protected override IEnumerable<GameStatus> Game(GameStatus status, string team1, string team2)
        {
            var currentDict = status.Dict;
            string currentSequence = status.GameSequence;

            var dict1 = new Dictionary<string, int>(currentDict);
            dict1[team1] += 2;
            yield return new GameStatus { Dict = dict1, GameSequence = $"{currentSequence} {team1}>{team2}" };

            var dict2 = new Dictionary<string, int>(currentDict);
            dict2[team1] += 1;
            dict2[team2] += 1;
            yield return new GameStatus { Dict = dict2, GameSequence = $"{currentSequence} {team1}={team2}" };

            var dict3 = new Dictionary<string, int>(currentDict);
            dict3[team2] += 2;
            yield return new GameStatus { Dict = dict3, GameSequence = $"{currentSequence} {team1}<{team2}" };
        }
    }

    abstract class GameEstimationWL : GamePredict
    {
        protected GameEstimationWL(Dictionary<string, int> initDict, (string, string)[] teamSequence) : base(initDict, teamSequence)
        {
        }
        protected override IEnumerable<GameStatus> Game(GameStatus status, string team1, string team2)
        {
            var currentDict = status.Dict;
            var currentSequence = status.GameSequence;

            var dict1 = new Dictionary<string, int>(currentDict);
            dict1[team1]++;
            yield return new GameStatus { Dict = dict1, GameSequence = $"{currentSequence} {team1}>{team2}" };

            var dict2 = new Dictionary<string, int>(currentDict);
            dict2[team2]++;
            yield return new GameStatus { Dict = dict2, GameSequence = $"{currentSequence} {team1}<{team2}" };
        }
    }

    struct GameStatus
    {
        public Dictionary<string,int> Dict { get; set; }
        public string GameSequence { get; set; }
    }
}
