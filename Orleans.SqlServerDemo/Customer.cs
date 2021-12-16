using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orleans.SqlServerDemo
{
    public class Customer
    {
        public string Id { get; set; } = null!;

        public string CellPhone { get; set; } = null!;

        public string Name { get; set; } = null!;
    }
}
