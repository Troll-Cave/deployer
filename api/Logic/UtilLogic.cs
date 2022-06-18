using data;
using data.DataModels;
using data.Models;

namespace api.Logic;

public class UtilLogic
{
    private readonly DeployerContext _context;

    public UtilLogic(DeployerContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// This exists because the EF generator sucks with jsonb
    /// </summary>
    public async Task Seed()
    {
        var pipelineId = Guid.Parse("89b0041b-b8cb-4b4e-81b7-c505c72dffa6");
        var orgId = Guid.Parse("3972a75e-6f02-4752-9914-416d17f78654");
        var versionId = Guid.Parse("86053667-c9ef-4097-8964-bf1e26215d63");
        var appId = Guid.Parse("858b05e9-ec30-4edd-ba07-9536b11b8d1f");

        if (await _context.Organizations.FindAsync(orgId) == null)
        {
            await _context.AddAsync(new OrganizationDTO()
            {
                ID = orgId,
                Name = "my org"
            });
        }
        
        if (await _context.Pipelines.FindAsync(pipelineId) == null)
        {
            await _context.AddAsync(new PipelineDTO()
            {
                ID = pipelineId,
                Name = "my pipeline",
                Organization = orgId
            });
        }

        if (await _context.PipelineVersions.FindAsync(versionId) == null)
        {
            await _context.AddAsync(new PipelineVersionDTO()
            {
                ID = versionId,
                Name = "1",
                PipelineId = pipelineId,
                Code = new Pipeline(),
                Files = new Dictionary<string, string>()
            });
        }

        if (await _context.Applications.FindAsync(appId) == null)
        {
            await _context.Applications.AddAsync(new ApplicationDTO()
            {
                ID = appId,
                Name = "lol",
                PipelineVersionId = versionId,
                Organization = orgId,
                Variables = new Dictionary<string, string>(),
                Source = "https://github.com/Troll-Cave/story"
            });
        }

        await _context.SaveChangesAsync();
    }
}