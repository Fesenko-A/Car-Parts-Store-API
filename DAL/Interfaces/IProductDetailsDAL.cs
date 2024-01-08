namespace DAL.Interfaces {
    internal interface IProductDetailsDAL<T> {
        Task<T?> FindByName(string name);

        Task<List<T>> GetAll();

        Task<T?> GetById(int id);

        Task Create(T toAdd);
    }
}
