using Store.Domain.Entities;
using Store.Domain.Repositories.Interfaces;

namespace Store.Tests.Repositories
{
    public class FakeCustomerRepository : ICustomerRepository
    {
        public Customer GetCustomer(string document)
        {
            if (document == "12345678911")
                return new Customer ("Mulher Maravilha", "maravilha@gmail.com");

            return null;
        }
    }
}