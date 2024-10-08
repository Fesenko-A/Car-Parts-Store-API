﻿namespace BL.Interfaces {
    internal interface IProductDetailsBL<T, D> {
        Task<ErrorOr<T>> Create(D dto);

        Task<List<T>> GetAll();
    }
}
