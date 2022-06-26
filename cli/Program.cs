// See https://aka.ms/new-console-template for more information
// dotnet run example.json | pbcopy

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using api.Models;

var pipelineText = await File.ReadAllTextAsync("./example.yml");

var version = new PipelineVersion()
{
    Pipeline = Guid.Parse("89b0041b-b8cb-4b4e-81b7-c505c72dffa6"),
    Name = "1",
    YAML = pipelineText,
    Files = new Dictionary<string, string>()
    {
        { "dockerfile", Convert.ToBase64String(File.ReadAllBytes("./dockerfile")) }
    }
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