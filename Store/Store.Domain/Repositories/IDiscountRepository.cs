using Store.Domain.Entities;

namespace Store.Domain.Repositories.Interfaces
{
    public interface IDiscountRepository
    {
        Discount GetDiscount(string code);
    }
}