﻿using Application.AppEntry;
using Application.AppEntry.Commands.Product.UpdateProduct;
using Domain.Aggregates.Product;
using OperationResult;

namespace Application.Handlers.ProductHandlers.UpdateProductHandler;

public class UpdateProductNameHandler : ICommandHandler<UpdateProductNameCommand, None>
{
    private readonly IProductRepository _productRepository;
    
    public UpdateProductNameHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<Result<None>> HandleAsync(UpdateProductNameCommand command)
    {
        var findProductResult = await _productRepository.GetAsync(command.Id);
        
        if (!findProductResult.IsSuccess)
        {
            return findProductResult.ToNonGeneric().ToNone();
        }

        var product = findProductResult.Data;
        var updateResult = product.UpdateProductName(command.Name);
        
        if (!updateResult.IsSuccess)
        {
            return updateResult.ToNone();
        }
        
        return Result<None>.Success(None.Value);
    }
}