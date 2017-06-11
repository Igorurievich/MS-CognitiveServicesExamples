using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryDemo;

namespace LibraryDemo
{
    internal class JuniorState : State
    {
        public override decimal GetFine()
        {
            return 15;
        }
    }
}
