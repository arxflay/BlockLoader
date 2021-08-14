using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockLoader.DataLayer;

namespace BlockLoader.Business
{
    public class ReachedBlockCounter
    {
        public int Count(IEnumerable<Respondent> respondents, string code)
        {
            bool containsNull;
            bool isEmpty;

            if (respondents == null)
            {
                throw new ArgumentNullException("respondents collection is null");
            }

            containsNull = respondents.Count(respondent => respondent == null || respondent.ReachedBlocks == null) > 0;

            if (containsNull)
            {
                throw new ArgumentNullException("Collection contains null elements");
            }

            isEmpty = respondents.Count() == 0;

            return isEmpty ? 0 : respondents.Count(respondent => respondent.ReachedBlocks.Contains(code));
        }
    }
}
