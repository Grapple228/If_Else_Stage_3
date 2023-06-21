using Database.Entities;

namespace Database.Repositories;

public interface IAccountsRepository : IRepository<Account>
{
    Account? Authenticate(string username, string password);
    Account? GetByUsername(string username);

}

public class AccountsRepository : RepositoryBase<Account>, IAccountsRepository
{
    public AccountsRepository(AppDbContext context) : base(context)
    {
    }

    public Account? Authenticate(string username, string password)
    {
        var account = GetByUsername(username);
        if (account == null) return null;
        return account.Password != password ? null : account;
    }

    public Account? GetByUsername(string username)
    {
        return Get(x => x.Username.ToLower() == username.ToLower());
    }
}