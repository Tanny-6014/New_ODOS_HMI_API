namespace OrderService.Interfaces
{
    public interface ITokenCacheService
    {
        void SaveToken(string key, string token, TimeSpan expirationTime);

        string GetToken(string key);
    }
}
