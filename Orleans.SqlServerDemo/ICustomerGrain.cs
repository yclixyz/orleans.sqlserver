using Orleans.SqlServerDemo;

namespace Orleans.SqlServerDemo
{
    public interface ICustomerGrain : IGrainWithStringKey
    {
        Task Create(string name,string cellPhone);

        Task<Customer?> Get();
    }
}