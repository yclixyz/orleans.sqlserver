using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orleans.SqlServerDemo
{
    public class CustomerGrain : Grain<Customer>, ICustomerGrain
    {
        public async Task Create(string name, string cellPhone)
        {
            State.Id = this.GetPrimaryKeyString();
            State.CellPhone = cellPhone;
            State.Name = name;

            await WriteStateAsync();
        }

        public async Task<Customer?> Get()
        {
            var stateId = this.GetPrimaryKeyString();

            if (stateId == State.Id)
            {
                return await Task.FromResult(State);
            }

            return default;
        }
    }
}
