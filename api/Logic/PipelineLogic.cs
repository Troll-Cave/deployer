using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml;
using api.Models;
using data;
using data.DataModels;
using data.Models;
using Microsoft.EntityFrameworkCore;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace api.Logic;

public class PipelineLogic
{
    private readonly DeployerContext _deployerContext;

    public PipelineLogic(DeployerContext deployerContext)
    {
        _deployerContext = deployerContext;
    }

    public Pipeline MapDTO(PipelineDTO dto)
    {
        return new Pipeline()
        {
            ID = dto.ID,
            Name = dto.Name,
            Organization = dto.Organization
        };
    }

    public void SetDTO(PipelineDTO dto, Pipeline model)
    {
        dto.Name = model.Name;
        dto.Organization = model.Organization;
    }

    public async Task Start(string appId, string reference)
    {
        var app = await _deployerContext.Applications.FindAsync(Guid.Parse(appId));

        if (app == null)
        {
            throw new Exception("app doesn't exist");
        }

        var version = await _deployerContext.PipelineVersions.FindAsync(app.PipelineVersionId);

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

        await _deployerContext.AddAsync(job);
        await _deployerContext.SaveChangesAsync();
    }
    
    public async Task UpsertVersion(PipelineVersion version)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)  // see height_in_inches in sample yml 
            .Build();

        var code = deserializer.Deserialize<PipelineCode>(version.YAML);
        
        var currentVersion = _deployerContext.PipelineVersions.FirstOrDefault(x => 
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
            await _deployerContext.SaveChangesAsync();
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

            await _deployerContext.AddAsync(newVersion);
            await _deployerContext.SaveChangesAsync();
        }
    }

    public async Task<List<Pipeline>> GetAll()
    {
        return (await _deployerContext.Pipelines.ToListAsync())
            .Select(MapDTO)
            .ToList();
    }

    public async Task<List<PipelineVersion>> GetAllVersions(Guid pipelineId)
    {
        throw new NotImplementedException();
    }

    public async Task Create(Pipeline pipeline)
    {
        var dto = new PipelineDTO()
        {
            ID = Guid.NewGuid()
        };
        
        SetDTO(dto, pipeline);

        await _deployerContext.AddAsync(dto);
        await _deployerContext.SaveChangesAsync();
    }

    public async Task Update(Pipeline pipeline, Guid id)
    {
        var dto = await _deployerContext.Pipelines.FindAsync(id);

        if (dto == null)
        {
            throw new Exception($"Pipeline {id} doesn't exist lol");
        }
        
        SetDTO(dto, pipeline);
        await _deployerContext.SaveChangesAsync();
    }
}