using sydtrucking_payroll_front.business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sydtrucking_payroll_front
{
    public sealed class Ticket
    {
        public Authenticate Authenticate { get; set; }

        private readonly static Ticket _instance = new Ticket();

        private Ticket()
        {
        }

        public static Ticket Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
