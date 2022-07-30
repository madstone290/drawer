namespace Drawer.Web.Services
{
    /// <summary>
    /// Async/Await Func, Action에 락기능을 제공한다.
    /// DI사용시 Transient 적용할 것.
    /// </summary>
    public interface ILockService
    {
        Task<T> DoAsync<T>(Func<Task<T>> func);
    }

    public class LockService : ILockService
    {
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);
        private object? _cache;

        public LockService()
        {
        }

        public async Task<T> DoAsync<T>(Func<Task<T>> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            if (_cache != null)
                return (T)_cache;

            await _semaphoreSlim.WaitAsync();

            if (_cache != null)
            {
                _semaphoreSlim.Release();
                return (T)_cache;
            }

            var t = await func.Invoke();
            _cache = t;

            _semaphoreSlim.Release();
            return t;
        }

    }
}
