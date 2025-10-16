namespace PROG3340_MidtermProject_V2.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> CompleteAsync();
    }
}
