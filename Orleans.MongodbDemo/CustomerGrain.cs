using Orleans;
using Orleans.Providers;

namespace Orleans.MongodbDemo
{
    [StorageProvider(ProviderName = "MongoDBStore")]
    public class CustomerGrain : Grain<Customer>, ICustomerGrain
    {
        public async Task Create(string name, string cellPhone)
        {
            State.Id = this.GetPrimaryKeyLong();
            State.CellPhone = cellPhone;
            State.Name = name;

            await WriteStateAsync();
        }

        public async Task<Customer?> Get()
        {
            var stateId = this.GetPrimaryKeyLong();

            if (stateId == State.Id)
            {
                return await Task.FromResult(State);
            }

            return default;
        }
    }
}
