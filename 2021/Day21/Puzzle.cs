using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day21
{
    internal class Puzzle : PuzzleBase
    {
        protected override string SampleResult => "444356092776315"; //"739785";

        protected override Task<string> GetSampleInputAsync()
        {
            return Task.FromResult(@"Player 1 starting position: 4
Player 2 starting position: 8");
        }

        private class Status
        {
            public int Position { get; set; }

            public long Score { get; set; } = 0;

            public void AddScore(int value)
            {
                Position = (Position - 1 + value) % 10 + 1;
                Score += Position;
            }

            public bool IsWinner => Score >= 1000;

            public override string ToString()
            {
                return $"Pos: {Position}, score: {Score}";
            }
        }

        protected override string DoCalculation(string input)
        {
            return DoCalculation3(input);
        }

        private string DoCalculation1(string input)
        {
            var game = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Split(new[] { "Player", "starting position:" }, StringSplitOptions.RemoveEmptyEntries))
                .ToDictionary(i => int.Parse(i[0]), i => new Status { Position = int.Parse(i[1]) });

            var players = game.Keys.OrderBy(p => p).ToArray();

            var dice = 0L;

            bool finished = false;
            do
            {
                foreach (var player in players)
                {
                    var play = Enumerable.Range(0, 3).Select(t => { return (int)(dice++ % 100) + 1; }).Sum();

                    game[player].AddScore(play);

                    if (game[player].IsWinner)
                    {
                        finished = true;
                        break;
                    }
                }
            } while (!finished);

            var loser = game.Values.Single(l => !l.IsWinner);

            return (loser.Score * dice).ToString();
        }

        private class Pawn {
            public long Score { get; set; }
            public long Count { get; set; }
            public int Position { get; set; }

            public override string ToString()
            {
                return $"Pos: {Position}, score: {Score}, count: {Count}";
            }
        }
        private List<(int Score, int Count)> Shuffles = new List<(int Score, int Count)>
        {
            (3, 1), (4, 3), (5, 6), (6, 7), (7, 6), (8, 3), (9, 1)
        };

        private string DoCalculation2(string input)
        {
            var start = input
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Split(new[] { "Player", "starting position:" }, StringSplitOptions.RemoveEmptyEntries))
                .ToDictionary(i => int.Parse(i[0]), i => int.Parse(i[1]));

            //var playground = Enumerable.Range(1, 10).ToDictionary(i => i, i => new List<Pawn>());

            var game = new HashSet<Pawn>();

            game.Add(new Pawn { Position = start[1], Count = 1, Score = 0 });

            var total = 0L;

            while (game.Any())
            {
                var q = game
                    .Select(g => Shuffles
                        .Select(s => new Pawn { Position = (g.Position + s.Score - 1) % 10 + 1, Count = g.Count * s.Count, Score = g.Score + (g.Position + s.Score - 1) % 10 + 1 } ).ToArray())
                    .SelectMany(s => s)
                    .ToHashSet();

                game = q.GroupBy(s => new { s.Position, s.Score }).Select(s => new Pawn { Score = s.Key.Score, Position = s.Key.Position, Count = s.Sum(t => t.Count) }).ToHashSet();

                total += game.Where(g => g.Score >= 21).Sum(t => t.Count);

                game  = game.Where(g => g.Score < 21).ToHashSet();
            }

            return "";
        }

        public string DoCalculation3(string input)
        {
            var players = input.Split(Environment.NewLine , StringSplitOptions.RemoveEmptyEntries).Select(t => t[(t.IndexOf(": ") + 2)..]).Select(long.Parse).ToArray();
            var memory = new Dictionary<State, (long Wins1, long Wins2)>();
            var (wins1, wins2) = Play(new(new(players[0]), new(players[1])));
            var result = Math.Max(wins1, wins2);
            return result.ToString();

            (long, long) Play(State state)
            {
                if (memory.TryGetValue(state, out var wins))
                    return wins;

                var (wins1, wins2) = (0L, 0L);
                foreach (var rollsSum in RollSums())
                {
                    var player = state.CurrentPlayer;
                    var space = player.Space + rollsSum;
                    space = (space - 1) % 10 + 1;
                    var score = player.Score + space;

                    if (score >= 21)
                    {
                        if (state.Player1Plays)
                            wins1++;
                        else
                            wins2++;
                    }
                    else
                    {
                        var newPlayer = player with { Score = score, Space = space };
                        var newState = state.Player1Plays ?
                            state with { Player1 = newPlayer, Player1Plays = !state.Player1Plays } :
                            state with { Player2 = newPlayer, Player1Plays = !state.Player1Plays };
                        var subWins = Play(newState);
                        wins1 += subWins.Item1;
                        wins2 += subWins.Item2;
                    }
                }

                return memory[state] = (wins1, wins2);
            }

            IEnumerable<int> RollSums()
            {
                for (int roll1 = 1; roll1 <= 3; roll1++)
                    for (int roll2 = 1; roll2 <= 3; roll2++)
                        for (int roll3 = 1; roll3 <= 3; roll3++)
                            yield return roll1 + roll2 + roll3;
            }
        }

        record State(PlayerState Player1, PlayerState Player2, bool Player1Plays = true)
        {
            public PlayerState CurrentPlayer => Player1Plays ? Player1 : Player2;
        }
        record PlayerState(long Space, long Score = 0);
    }
}
