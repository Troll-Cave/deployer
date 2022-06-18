// See https://aka.ms/new-console-template for more information
// dotnet run example.json | pbcopy

using System.Text;
using System.Text.Json;
using api.Models;
using data.Models;

var fileName = "";
if (args.Length > 0)
{
    fileName = args[0];
}
else
{
    Console.WriteLine("give me a file lol");
    return 1;
}

if (!File.Exists(fileName))
{
    Console.WriteLine("file doesn't exist lol");
    return 1;
}

var pipelineText = await File.ReadAllTextAsync(fileName);
var data = JsonSerializer.Deserialize<Pipeline>(pipelineText);

var version = new PipelineVersion()
{
    Pipeline = Guid.Parse("0a3f9729-53d7-4bab-8b6b-4408b1e73a3a"),
    Name = "1",
    Code = data!
};

string bytes = JsonSerializer.Serialize(version);

var client = new HttpClient();
var request = new HttpRequestMessage()
{
    RequestUri = new Uri("http://localhost:5251/Pipeline/version"),
    Method = HttpMethod.Post,
    Content = new StringContent(bytes, Encoding.UTF8, "application/json")
};

//await client.SendAsync(request);

Console.WriteLine(bytes);

return 0;