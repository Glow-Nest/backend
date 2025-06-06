﻿using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.Repositories;
using OperationResult;

namespace Domain.Aggregates.ServiceCategory;

public interface ICategoryRepository : IGenericRepository<Category, CategoryId>
{
    Task<Result<List<Category>>> GetAllAsync();
    Task<Result<List<Category>>> GetCategoriesWithServicesAsync();
    Task<Result<bool>> FindServiceWithIdAsync(CategoryId categoryId, ServiceId serviceId);
    Task<Result> DeleteAsync(CategoryId category);
    Task<Result> DeleteServiceAsync(CategoryId categoryId, ServiceId serviceId);
}