namespace Drawer.Web.Frontend
{
    public interface ILocalStorage
    {
        ValueTask<bool> ContainKeyAsync(string key, CancellationToken? cancellationToken = null);

        ValueTask<string?> GetItemAsync(string key, CancellationToken? cancellationToken = null);

        ValueTask SetItemAsync(string key, string item, CancellationToken? cancellationToken = null);

        ValueTask RemoveItemAsync(string key, CancellationToken? cancellationToken = null);
    }
}
