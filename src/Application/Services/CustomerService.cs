using Application.Models.Requests;
using Domain.Models;
using Domain.ValueObjects;

namespace Application.Services
{
    public class CustomerService
    {
        public async Task<long> CreateCustomer(CreateCustomerRequest request)
        {
            var customer = new Customer()
            {
                FirstName = new Name(request.FirstName),
                LastName = new Name(request.LastName),
                Address = new Address(request.AddressLine, request.City, request.State, request.PostalCode, request.Country),
                Email = new Email(request.Email),
                Phone = new Phone(request.PhoneNumber)
            };

            return 1;

            //Db
            //Return id
        }
    }
}
