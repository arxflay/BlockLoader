using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockLoader.DataLayer
{
    public interface IRespondentRepository
    {
        IEnumerable<Respondent> LoadRespondents();
    }
}
