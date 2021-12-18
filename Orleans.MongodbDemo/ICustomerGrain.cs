using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orleans.MongodbDemo
{
    public interface ICustomerGrain : IGrainWithIntegerKey
    {
        Task Create(string name, string cellPhone);

        Task<Customer?> Get();
    }
}
