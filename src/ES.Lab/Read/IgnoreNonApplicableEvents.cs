using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Lab.Read
{
    public abstract class IgnoreNonApplicableEvents
    {
        protected void Handle(dynamic eventToIgnore)
        {
            //Do nothing
        }

    }
}
