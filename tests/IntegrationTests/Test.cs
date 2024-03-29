﻿using Client;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public abstract class Test
    {
        protected readonly ApiClient apiClient;
        protected readonly AppDbContext db;

        public Test(ITestOutputHelper testOutputHelper, WebApplicationFactoryFixture factory)
        {
            factory.TestOutputHelper = testOutputHelper;
            apiClient = new(factory.CreateClient());
            //db = factory.Services.GetRequiredService<AppDbContext>();
        }
    }
}
