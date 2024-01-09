using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces {
    internal interface IProductDetails<T> {
        Task<ActionResult> GetAll();

        Task<ActionResult> Create(T dto);
    }
}
