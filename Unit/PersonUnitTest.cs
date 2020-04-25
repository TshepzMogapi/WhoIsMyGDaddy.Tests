using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WhoIsMyGDaddy.API.Domain.Models;
using WhoIsMyGDaddy.API.Domain.Persistence.Contexts;
using WhoIsMyGDaddy.API.Domain.Repositories;
using WhoIsMyGDaddy.API.Domain.Services;
using WhoIsMyGDaddy.API.Persistence.Repositories;
using WhoIsMyGDaddy.API.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Tests.Unit
{
    public class UnitTest1
    {
        [Fact]
        public async Task ListPersonsAsync() {

            IPersonRepository _personTestRepository = GetInMemoryPersonRepository(); 

            IPersonService _personTestService  = new PersonService(_personTestRepository);

            var personList = Persons.Select(p => new Person {
                Id = Convert.ToInt32(p[0]),
                Name = p[1].ToString(),
                Surname = p[2].ToString(),
                FatherId = Convert.ToInt32(p[3]),
                MotherId = Convert.ToInt32(p[4]),
                BirthDate = (DateTime)p[5],
                IdentityNumber = p[6].ToString()
            }).ToList();

            await _personTestRepository.AddPersonsAsync(personList);

            var persons = await _personTestService.ListAsync();

            var expected = JsonConvert.SerializeObject(persons);

            var actual = JsonConvert.SerializeObject(personList);

            Assert.Equal(expected, actual);

        }
       

        [Theory]
        [InlineData("5001185555081", 9)]
        [InlineData("9608185555081", 3)]
        [InlineData("8404185555081", 5)]
        public async Task GetDescendantsAsync(string identityNumber, int actualDescendants){

            IPersonRepository _personTestRepository = GetInMemoryPersonRepository(); 

            IPersonService _personTestService  = new PersonService(_personTestRepository);

            var descendants = await _personTestService.GetDescendantsAsync(identityNumber);


            Assert.Equal(descendants.ToList().Count,actualDescendants);

        }


        [Theory]
        [InlineData("8404185555081", 2, "Tshepiso")]
        [InlineData("9608185555081", 3, "Tshepiso")]
        public async Task GetAncestorsAsync(string identityNumber, int actualAncestors,string parentAncestorName){

            IPersonRepository _personTestRepository = GetInMemoryPersonRepository(); 

            IPersonService _personTestService  = new PersonService(_personTestRepository);

            var ancestors = await _personTestService.GetAncestorByIdAsync(identityNumber);

            Assert.Equal(ancestors.ToList().Count,actualAncestors);

            var earlyAncestor = ancestors.OrderBy(p => p.BirthDate).First();

            Assert.Equal(earlyAncestor.Name , parentAncestorName);

        }

        private IPersonRepository GetInMemoryPersonRepository() {
            DbContextOptions<AppDbContext> options;

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            // builder.UseInMemoryDatabase("TestingDataBase");
            builder.UseSqlServer("Server=localhost; Database=WhoIsMyGDaddyTestsDb;User ID=sa;Password=<Password1234>;");
            options = builder.Options;

            AppDbContext personContext = new AppDbContext(options);

            personContext.Database.EnsureDeleted();
            personContext.Database.EnsureCreated();
            return new PersonRepository(personContext);
        }

   
        public static IEnumerable<object[]> Persons =>
            new List<object[]>
            {
                new object[]{1001, "Tshepiso", "Mogapi", 0, 0, new DateTime(1950, 1, 18),"5001185555081"},
                new object[]{1002, "Samy", "Mitchell", 0, 0, new DateTime(1960, 2, 18),"6002185555081"},
                new object[]{1003, "Angel", "Thebe",1002,1001,new DateTime(1980, 3, 18),"8003185555081"},
                new object[]{1004, "Mariana", "Murilo",0,1001,new DateTime(1984, 4, 18),"8404185555081"},
                new object[]{1005, "Correia", "Melo",1003,0,new DateTime(1985, 8, 18),"8508185555081"},
                new object[]{1006, "Rivera", "Gutierrez",0,1004,new DateTime(1996, 8, 18),"9608185555081"},
                new object[]{1007, "Rodas", "Quintero",1005,0,new DateTime(1999, 8, 18),"9908185555081"},
                new object[]{1008, "Rhys", "Lilly",1006,0,new DateTime(2014, 8, 18),"1408185555081"},
                new object[]{1009, "Freddie", "Thomson",1006,0,new DateTime(2000, 8, 18),"0008185555081"},
                new object[]{1010, "Celina", "Liam",0,1004,new DateTime(2001, 8, 18),"0108185555081"},
                
            };
    }
}
