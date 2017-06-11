using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDemo
{
    internal class StandartState : State
    {
        public override decimal GetFine()
        {
            return 10;
        }
    }
}
