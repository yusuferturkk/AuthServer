using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public GenericService(IGenericRepository<TEntity> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto<TDto>> AddAsync(TDto dto)
        {
            var newEntity = ObjectMapper.Mapper.Map<TEntity>(dto);
            
            await _repository.AddAsync(newEntity);
            await _unitOfWork.CommitAsync();

            var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);

            return ResponseDto<TDto>.Success(204, newDto);
        }

        public async Task<ResponseDto<IEnumerable<TDto>>> GetAllAsync()
        {
            var entities = ObjectMapper.Mapper.Map<List<TDto>>(await _repository.GetAllAsync());

            return ResponseDto<IEnumerable<TDto>>.Success(200, entities);
        }

        public async Task<ResponseDto<TDto>> GetByIdAsync(int id)
        {
            var entity = ObjectMapper.Mapper.Map<TDto>(await _repository.GetByIdAsync(id));

            if (entity == null)
            {
                return ResponseDto<TDto>.Fail(404, "Id not found!", true);
            }

            return ResponseDto<TDto>.Success(200, entity);
        }

        public async Task<ResponseDto<NoContentDto>> Remove(int id)
        {
            var isExistEntity = await _repository.GetByIdAsync(id);
            
            if (isExistEntity == null)
            {
                return ResponseDto<NoContentDto>.Fail(404, "Id not found!", true);
            }

            _repository.Remove(isExistEntity);
            await _unitOfWork.CommitAsync();

            return ResponseDto<NoContentDto>.Success(200);
        }

        public async Task<ResponseDto<NoContentDto>> Update(TDto dto, int id)
        {
            var isExistEntity = await _repository.GetByIdAsync(id);

            if (isExistEntity == null)
            {
                return ResponseDto<NoContentDto>.Fail(404, "Id not found!", true);
            };

            var updateEntity = ObjectMapper.Mapper.Map<TEntity>(dto);
            _repository.Update(updateEntity);
            await _unitOfWork.CommitAsync();

            return ResponseDto<NoContentDto>.Success(200);
        }

        public async Task<ResponseDto<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            var list = _repository.Where(predicate);
            return ResponseDto<IEnumerable<TDto>>.Success(200, ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await list.ToListAsync()));
        }
    }
}
