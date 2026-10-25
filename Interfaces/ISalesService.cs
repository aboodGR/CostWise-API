using WalMart_ERP_System.Models;
using WalMart_ERP_System.Results;

namespace WalMart_ERP_System.Interfaces;

public interface ISalesService
{
    Task<Result<Sale>> CompleteSale(Sale sale);
    Task<Sale?> GetSaleById(int id);
    Task<List<Sale>> GetSalesHistory(DateTime? fromUtc, DateTime? toUtc);
}
