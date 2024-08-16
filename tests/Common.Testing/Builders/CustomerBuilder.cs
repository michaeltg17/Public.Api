using Common.Builders;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Testing.Builders
{
    internal class CustomerBuilder : BuilderWithValues<CustomerBuilder, Customer>
    {
        protected override Customer Item { get; set; }

        public CustomerBuilder()
        {
            Item
        }

        public CustomerBuilder WithDefaultValues()
        {
            Ite

            return this;
        }
    }
}
