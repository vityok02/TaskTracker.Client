namespace Services.Interfaces;

public interface ICookieManager
{
    public void Set(string key, string value);
    public string Get(string key);
    public void Remove(string key);
}
