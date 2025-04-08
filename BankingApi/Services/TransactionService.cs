using BankingApi.Models;

public class TransactionService
{
    private readonly BankingDbContext _context;

    public TransactionService(BankingDbContext context)
    {
        _context = context;
    }

    public async Task HandleTransactionAsync(Transaction transObject, Account account)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                await _context.Transactions.AddAsync(transObject);

                _context.Accounts.Update(account);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Transaction failed: {ex.Message}");
                throw;
            }
        }
    }
}
