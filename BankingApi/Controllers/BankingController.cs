using BankingApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankingController : ControllerBase
    {
        private readonly BankingDbContext _context;
        private readonly TransactionService _transactionService;
        public BankingController(BankingDbContext context, TransactionService transactionService)
        {
            _context = context;
            _transactionService = transactionService;
        }
        
        [HttpGet("balance")]
        [Authorize]
        public async Task<ActionResult> GetBalance()
        {
            var userId = User?.Identity?.Name;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            var account = await (from user in _context.Users
                                where user.Name == userId
                                select user.Account).FirstOrDefaultAsync();

            if (account == null)
            {
                return NotFound();
            }
            return Ok(new {balance = account.Balance});
        }

        [HttpPost("deposit")]
        [Authorize]
        public async Task<ActionResult> Deposit([FromBody] TransactionRequest transaction)
        {
            var userId = User?.Identity?.Name;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            var account = await (from user in _context.Users
                           where user.Name == userId
                           select user.Account).FirstOrDefaultAsync();

            if (account == null)
            {
                return NotFound();
            }

            Transaction transObject = new Transaction
            {
                AccountId = account.AccountId,
                Type = "Deposit",
                Amount = transaction.Amount,
                Date = DateTime.UtcNow
            };

            account.Balance += transaction.Amount;

            try
            {
                await _transactionService.HandleTransactionAsync(transObject, account);
                return RedirectToAction("GetBalance");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Transaction failed: {ex.Message}" });
            }
        }

        [HttpPost("withdraw")]
        [Authorize]
        public async Task<ActionResult> Withdraw([FromBody] TransactionRequest transaction)
        {
            var userId = User?.Identity?.Name;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            var account = await (from user in _context.Users
                           where user.Name == userId
                           select user.Account).FirstOrDefaultAsync();

            if (account == null)
            {
                return NotFound();
            }

            if (account.Balance < transaction.Amount)
            {
                return BadRequest("Insufficient funds");
            }

            Transaction transObject = new Transaction
            {
                AccountId = account.AccountId,
                Type = "Withdraw",
                Amount = transaction.Amount,
                Date = DateTime.UtcNow
            };

            account.Balance -= transaction.Amount;

            try
            {
                await _transactionService.HandleTransactionAsync(transObject, account);
                return RedirectToAction("GetBalance");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Transaction failed: {ex.Message}" });
            }
        }
    }

}
