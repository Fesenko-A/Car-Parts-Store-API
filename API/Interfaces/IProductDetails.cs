using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces {
    internal interface IProductDetails<T> {
        Task<ActionResult<ApiResponse>> GetAll();

        Task<ActionResult<ApiResponse>> Create(T dto);
    }
}
