using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {
        Task<ResponseDto<TDto>> GetByIdAsync(int id);

        Task<ResponseDto<IEnumerable<TDto>>> GetAllAsync();

        Task<ResponseDto<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate);

        Task<ResponseDto<NoContentDto>> AddAsync(TDto dto);

        Task<ResponseDto<NoContentDto>> Remove(int id);

        Task<ResponseDto<NoContentDto>> Update(TDto dto, int id);
    }
}
