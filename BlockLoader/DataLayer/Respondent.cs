using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockLoader.DataLayer
{
    public class Respondent
    {
        public Respondent(HashSet<string> reachedBlocks)
        {
            ReachedBlocks = reachedBlocks;
        }
        public HashSet<string> ReachedBlocks { get; }
    }
}
