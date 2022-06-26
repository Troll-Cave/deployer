// See https://aka.ms/new-console-template for more information
// dotnet run example.json | pbcopy

using System.Net.Http.Headers;
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
    Pipeline = Guid.Parse("89b0041b-b8cb-4b4e-81b7-c505c72dffa6"),
    Name = "1",
    Code = data!
};

var bytes = JsonSerializer.Serialize(version);

var client = new HttpClient();
var request = new HttpRequestMessage()
{
    RequestUri = new Uri("http://localhost:5251/Pipeline/version"),
    Method = HttpMethod.Post,
    Content = new StringContent(bytes, Encoding.UTF8, "application/json")
};

try
{
    var res = await client.SendAsync(request);
    Console.WriteLine(await res.Content.ReadAsStringAsync());
    res.EnsureSuccessStatusCode();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}

await client.PostAsync("http://localhost:5251/Application/858b05e9-ec30-4edd-ba07-9536b11b8d1f/start/HEAD", null);

Console.WriteLine(bytes);

return 0;