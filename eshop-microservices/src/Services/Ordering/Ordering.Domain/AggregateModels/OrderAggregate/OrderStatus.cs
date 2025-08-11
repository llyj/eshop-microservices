namespace Ordering.Domain.AggregateModels.OrderAggregate;

/// <summary>
/// 订单状态
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// 已提交
    /// </summary>
    Submitted = 1,

    /// <summary>
    /// 等待审核
    /// </summary>
    AwaitingValidation = 2,

    /// <summary>
    /// 库存确认
    /// </summary>
    StockConfirmed = 3,

    /// <summary>
    /// 已支付
    /// </summary>
    Paid = 4,

    /// <summary>
    /// 已发货
    /// </summary>
    Shipped = 5,

    /// <summary>
    /// 已取消
    /// </summary>
    Cancelled = 6
}