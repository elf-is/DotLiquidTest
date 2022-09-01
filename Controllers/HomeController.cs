using System.Diagnostics;
using DotLiquid;
using Microsoft.AspNetCore.Mvc;
using DotLiquidTest.Models;
using Newtonsoft.Json;

namespace DotLiquidTest.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // we need to read the contents of the template file
        string liquidTemplateContent = System.IO.File.ReadAllText("wwwroot/template/example.liquid");
        // then we parse the contents of the template file into a liquid template
        Template template = Template.Parse(liquidTemplateContent);
        // then we create a new instance of the model class
        var addresses = @"
		{
			""Addresses"": [{
					""Street"": ""123 Main Street"",
					""City"": ""Montreal"",
					""State"": ""QC"",
					""Zip"": ""H1S1M5"",
					""Country"": ""Canada""
				},
				{
					""Street"": ""456 Main Street"",
					""City"": ""Montreal"",
					""State"": ""QC"",
					""Zip"": ""H1S1M5"",
					""Country"": ""Canada""
				}
			]
		}";
        Employee employee = new Employee
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Phone = "555-555-5555",
            Address = addresses
        };

        Hash hash = Hash.FromAnonymousObject(new
        {
            employee = new EmployeeDrop(employee)
        });
        // then we render the template
        string result = template.Render(hash);
        // and then we return the result to the view in a view bag object
        ViewBag.template = result;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}