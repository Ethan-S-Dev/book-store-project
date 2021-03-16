namespace BookStore.DAL.Interfaces
{
    /// <summary>
    /// Interface for DAL models, for saving in a repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIdable<T>
    {
        int Id { get; set; }
        bool ComperPrimeryKey(T other);
    }
}
