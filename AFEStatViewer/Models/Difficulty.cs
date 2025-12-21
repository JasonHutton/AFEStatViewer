using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFEStatViewer.Models
{
    public enum Difficulty
    {
        Casual,
        Standard,
        Intense,
        Extreme,
        Insane
    }

    public static class Difficulties
    {
        public static readonly IReadOnlyDictionary<Difficulty, string> ToSaveKey =
            new Dictionary<Difficulty, string>
            {
                [Difficulty.Casual] = "Easy|Campaign",
                [Difficulty.Standard] = "Normal|Campaign",
                [Difficulty.Intense] = "Hard|Campaign",
                [Difficulty.Extreme] = "Extreme|Campaign",
                [Difficulty.Insane] = "Insane|Campaign",
            };

        public static readonly IReadOnlyList<Difficulty> All = new[] { Difficulty.Casual, Difficulty.Standard, Difficulty.Intense, Difficulty.Extreme, Difficulty.Insane };
    }
}
