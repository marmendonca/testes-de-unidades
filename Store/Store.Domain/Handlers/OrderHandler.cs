using Store.Domain.Commands;
using Store.Domain.Commands.Interfaces;
using Store.Domain.Entities;
using Store.Domain.Repositories.Interfaces;
using Flunt.Notifications;
using System.Linq;
using Store.Domain.Utils;

namespace Store.Domain.Handlers.Interfaces
{
    public class OrderHandler : Notifiable, IHandler<CreateOrderCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDeliveryFeeRepository _deliveryFeeRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderHandler(
            ICustomerRepository customerRepository,
            IDeliveryFeeRepository deliveryFeeRepository,
            IDiscountRepository discountRepository,
            IProductRepository productRepository,
            IOrderRepository orderRepository)
        {
            _customerRepository = customerRepository;
            _deliveryFeeRepository = deliveryFeeRepository;
            _discountRepository = discountRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public ICommandResult Handle(CreateOrderCommand command)
        {
            command.Validate();
            if (command.Invalid)
                return new GenericCommandResult(false, "Pedido invalido", command.Notifications);

            var customer = _customerRepository.GetCustomer(command.Customer);

            var deliveryFee = _deliveryFeeRepository.Get(command.ZipCode);

            var discount = _discountRepository.GetDiscount(command.PromoCode);

            var products = _productRepository.Get(ExtractGuids.Extract(command.Items)).ToList();
            var order = new Order(customer, deliveryFee, discount);
            foreach (var item in command.Items)
            {
                var product = products.Where(x => x.Id == item.Product).FirstOrDefault();
                order.AddItem(product, item.Quantity);
            }

            AddNotifications(order.Notifications); 

            if (Invalid)
                return new GenericCommandResult(false, "Falha ao gerar o pedido", Notifications);

            _orderRepository.Save(order);
            return new GenericCommandResult(true, $"Pedido {order.Number} gerado com sucesso", order);    
        }
    }
}