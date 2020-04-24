



using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using WhoIsMyGDaddy.API;
using WhoIsMyGDaddy.API.Domain.Models;
using WhoIsMyGDaddy.API.Domain.Persistence.Contexts;
using WhoIsMyGDaddy.API.Domain.Services;

namespace WhoIsMyGDaddy.Tests.Integration {

    public class PersonServiceFake : IPersonService
    {

        private readonly List<Person> _personList;

        private readonly AppDbContext _context;

        public PersonServiceFake() {

            _personList = new List<Person>() {
                    new Person { Id = 1001, Name = "Tshepiso", Surname = "Mogapi", BirthDate = new DateTime(1950, 1, 18), IdentityNumber = "5001185555081" },
                new Person { Id = 1002, Name = "Samy", Surname = "Mitchell", BirthDate = new DateTime(1960, 2, 18), IdentityNumber = "6002185555081" },
                new Person { Id = 1003, Name = "Angel", Surname = "Thebe", FatherId = 1002, MotherId = 1001, BirthDate = new DateTime(1980, 3, 18), IdentityNumber = "8003185555081" },
                new Person { Id = 1004, Name = "Mariana", Surname = "Murilo", MotherId = 1001, BirthDate = new DateTime(1984, 4, 18), IdentityNumber = "8404185555081" },
                new Person { Id = 1005, Name = "Correia", Surname = "Melo", FatherId = 1003, BirthDate = new DateTime(1985, 8, 18), IdentityNumber = "8508185555081" },
                new Person { Id = 1006, Name = "Rivera", Surname = "Gutierrez", MotherId = 1004, BirthDate = new DateTime(1996, 8, 18), IdentityNumber = "9608185555081" },
                new Person { Id = 1007, Name = "Rodas", Surname = "Quintero", FatherId = 1005, BirthDate = new DateTime(1999, 8, 18), IdentityNumber = "9908185555081" },
                new Person { Id = 1008, Name = "Rhys", Surname = "Lilly", FatherId = 1006, BirthDate = new DateTime(2014, 8, 18), IdentityNumber = "1408185555081" },
                new Person { Id = 1009, Name = "Freddie", Surname = "Thomson", FatherId = 1006, BirthDate = new DateTime(2000, 8, 18), IdentityNumber = "0008185555081" },
                new Person { Id = 1010, Name = "Celina", Surname = "Liam", MotherId = 1004, BirthDate = new DateTime(2001, 8, 18), IdentityNumber = "0108185555081" }

            };

        }
        public Task<IEnumerable<Person>> Get(string id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Person>> GetAllListAsync(string id)
        {
            await Task.Delay(1000);

            var person = _personList.Find(p => p.IdentityNumber == id);

            return _personList.FindAll(p => p.MotherId == person.Id || p.FatherId == person.Id);
        }

        public async Task<IEnumerable<Person>> ListAsync()
        {

            await Task.Delay(1000);

            return _personList;

        }



       
    }
}