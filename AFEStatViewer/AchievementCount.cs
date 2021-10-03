using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFEStatViewer
{
    public class AchievementCount
    {
        public AchievementCount(string name, string key, int target)
        {
            Name = name;
            Key = key;
            Value = 0;
            Target = target;
        }
        public string Name { get; set; }
        public string Key { get; set; }
        public int Value { get; set; }
        public int Target { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1} / {2}", Name, Value, Target);
        }
    }
}