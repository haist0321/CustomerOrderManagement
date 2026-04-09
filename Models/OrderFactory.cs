namespace CustomerOrderManagement.Factories;

using CustomerOrderManagement.Models;

public abstract class OrderFactory
{
    public abstract Order CreateOrder(decimal total, decimal discount, int customerId);
}

public class OnlineOrderFactory : OrderFactory
{
    public override Order CreateOrder(decimal total, decimal discount, int customerId)
    {
        return new Order
        {
            CustomerId = customerId,
            OrderType = "Online",
            OrderDate = DateTime.Now,
            TotalAmount = total,
            Discount = discount,
            FinalAmount = total - discount
        };
    }
}

public class InStoreOrderFactory : OrderFactory
{
    public override Order CreateOrder(decimal total, decimal discount, int customerId)
    {
        return new Order
        {
            CustomerId = customerId,
            OrderType = "In-Store",
            OrderDate = DateTime.Now,
            TotalAmount = total,
            Discount = discount,
            FinalAmount = total - discount
        };
    }
}

public class WholesaleOrderFactory : OrderFactory
{
    public override Order CreateOrder(decimal total, decimal discount, int customerId)
    {
        return new Order
        {
            CustomerId = customerId,
            OrderType = "Wholesale",
            OrderDate = DateTime.Now,
            TotalAmount = total,
            Discount = discount,
            FinalAmount = total - discount
        };
    }
}
