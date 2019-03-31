namespace ToDoList.Cache.Services
{
    public interface ICacheAccessor
    {
        object Get(string key);
    }
}
