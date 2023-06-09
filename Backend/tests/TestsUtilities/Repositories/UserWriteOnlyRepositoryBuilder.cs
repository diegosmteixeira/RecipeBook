﻿using Moq;
using RecipeBook.Domain.Repositories.User;

namespace TestsUtilities.Repositories;

public class UserWriteOnlyRepositoryBuilder
{
    private static UserWriteOnlyRepositoryBuilder _instance;
    private readonly Mock<IUserWriteOnlyRepository> _repository;

    private UserWriteOnlyRepositoryBuilder()
    {
        if (_repository is null)
        {
            _repository = new Mock<IUserWriteOnlyRepository>();
        }
    }

    public static UserWriteOnlyRepositoryBuilder Instance()
    {
        _instance = new UserWriteOnlyRepositoryBuilder();
        return _instance;
    }

    public IUserWriteOnlyRepository Build()
    {
        return _repository.Object;
    }
}
