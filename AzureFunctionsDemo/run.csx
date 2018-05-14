#r "Newtonsoft.Json"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

public static IActionResult Run(HttpRequest req, TraceWriter log)
{
	log.Info("C# HTTP trigger function processed a request.");

	// Parsing query parameters
	string name = req.Query["name"];
	log.Info("name = " + name);

	string numberOfTerms = req.Query["numberOfTerms"];
	log.Info("numberOfTerms = " + numberOfTerms);

	// Validating the parameters received
	if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(numberOfTerms))
	{
		return new BadRequestObjectResult("Please pass a name and the number of digits on the query string."); 
	}

	int termsToShow;
	try
	{
		termsToShow = Int32.Parse(numberOfTerms);
	}
	catch (FormatException e)
	{
		return new BadRequestObjectResult("The numberOfTerms parameter must be an integer!"); 
	}

	if (termsToShow < 0 || termsToShow > 93) {
		return new BadRequestObjectResult("Please pass a numberOfTerms parameter between 0 and 93."); 
	}

	// Building the response
	string incompleteResponse = "Hello, " + name + ", you requested the first " + numberOfTerms + " terms of the Fibonacci sequence. Here they are: ";
	string completeResponse = GenerateFibonacciTerms(incompleteResponse, termsToShow);
	var response = new OkObjectResult(completeResponse); 

	// Returning the HTTP response with the string we created
	log.Info("response = " + response);
	return response;
}

public static string GenerateFibonacciTerms(string incompleteResponse, int termsToShow)
{    
    long a = 0;
    long b = 1;
    string temporalString = "";
    
    for (int i = 0; i < termsToShow; i++)
    {
        long temp = a;
        a = b;
        b = temp + b;
        temporalString = temporalString + temp.ToString() + " ";
    }

	string result = incompleteResponse + temporalString + "- That's it, have an awesome day!";
	return result;    
}
