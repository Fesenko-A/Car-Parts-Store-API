namespace BL.Interfaces {
    internal interface IProductDetailsBL<T, D> {
        Task<T?> Create(D dto);

        Task<List<T>> GetAll();
    }
}
