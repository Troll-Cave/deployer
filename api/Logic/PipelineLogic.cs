using System.Text.Json;
using System.Text.Json.Nodes;
using api.Models;
using data;
using data.DataModels;
using data.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace api.Logic;

public class PipelineLogic
{
    private readonly DeployerContext _context;

    public PipelineLogic(DeployerContext context)
    {
        _context = context;
    }

    public async Task Start(string appId, string reference)
    {
        var app = await _context.Applications.FindAsync(Guid.Parse(appId));

        if (app == null)
        {
            throw new Exception("app doesn't exist");
        }

        var version = await _context.PipelineVersions.FindAsync(app.PipelineVersionId);

        if (version == null)
        {
            throw new Exception("version doesn't exist");
        }

        var job = new JobDTO()
        {
            ID = Guid.NewGuid(),
            ApplicationId = app.ID,
            Code = version.Code,
            State = new JobState()
            {
                State = "pending",
                StepState = new (),
                Metadata = new ()
            },
            PipelineVersionId = version.ID,
            SourceReference = reference
        };

        await _context.AddAsync(job);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpsertVersion(PipelineVersion version)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)  // see height_in_inches in sample yml 
            .Build();

        var code = deserializer.Deserialize<Pipeline>(version.YAML);
        
        var currentVersion = _context.PipelineVersions.FirstOrDefault(x => 
            x.PipelineId == version.Pipeline &&
            x.Name == version.Name);

        if (currentVersion != null)
        {
            // update
            currentVersion.Code = code;
            currentVersion.YAML = version.YAML;
            currentVersion.Files = new PipelineVersionFiles()
            {
                Files = version.Files
            };
            await _context.SaveChangesAsync();
        }
        else
        {
            // insert
            var newVersion = new PipelineVersionDTO()
            {
                ID = Guid.NewGuid(),
                PipelineId = version.Pipeline,
                Name = version.Name,
                YAML = version.YAML,
                Code = code,
                Files = new PipelineVersionFiles()
                {
                    Files = version.Files
                }
            };

            await _context.AddAsync(newVersion);
            await _context.SaveChangesAsync();
        }
    }
}