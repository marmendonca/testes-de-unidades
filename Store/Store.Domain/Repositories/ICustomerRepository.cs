using Store.Domain.Entities;

namespace Store.Domain.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Customer GetCustomer(string document);
    }
}