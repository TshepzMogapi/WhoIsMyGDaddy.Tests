using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using WhoIsMyGDaddy.API;
using Newtonsoft.Json;
// WhoIsMyGDaddy.API/Domain/Persistence/Contexts 
using WhoIsMyGDaddy.API.Domain.Persistence.Contexts;
using WhoIsMyGDaddy.API.Domain.Models;

public class IntegrationTestsPersons {
    private readonly HttpClient _client;
    private readonly AppDbContext _context;

    public IntegrationTestsPersons() {

        var configuration = new ConfigurationBuilder()
                                // .SetBasePath(Path.GetFullPath(@"../../WhoIsMyGDaddy.API"))
                                .AddJsonFile("appsettings.test.json")
                                .Build();
        
        var builder = new WebHostBuilder()
                // Set test environment
                .UseEnvironment("Testing")
                .UseStartup<Startup>()
                .UseConfiguration(configuration);

        var server = new TestServer(builder);

        this._context = server.Host.Services
                            .GetService(typeof(AppDbContext)) as AppDbContext;

        this._client = server.CreateClient();

    }


    [Fact]
    public async Task InsertAndGetPersons(){

        // var person = new Person { 
        //     Id = id, Name = name, Surname = surname, 
        //     FatherId = fatherId, MotherId = motherid, BirthDate = birthDate, IdentityNumber = identityNumber 
        // };

        // _context.Persons.Add(person);

        // {1001, "Tshepiso", "Mogapi", 0, 0, new DateTime(1950, 1, 18),"5001185555081"}
        var personList = Persons.Select(p => new Person {
            Id = Convert.ToInt32(p[0]),
            Name = p[1].ToString(),
            Surname = p[2].ToString(),
            FatherId = Convert.ToInt32(p[3]),
            MotherId = Convert.ToInt32(p[4]),
            BirthDate = (DateTime)p[5],
            IdentityNumber = p[6].ToString()
        });

        _context.Persons.AddRange(personList);

        _context.SaveChanges();

        var response = await _client.GetAsync($"/api/persons");

        Console.WriteLine(response.Content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var jsonResponse = await response.Content.ReadAsStringAsync();

        // var personResponse = JsonConvert.DeserializeObject<List<Person>>(jsonResponse);

        // Assert.Equal(person, personResponse);

        // Assert.Equal(person.Name, person.Name);

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