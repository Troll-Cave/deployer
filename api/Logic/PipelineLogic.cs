using api.Models;
using data;
using data.DataModels;

namespace api.Logic;

public class PipelineLogic
{
    private readonly DeployerContext _context;

    public PipelineLogic(DeployerContext context)
    {
        _context = context;
    }
    
    public async Task UpsertVersion(PipelineVersion version)
    {
        var currentVersion = _context.PipelineVersions.FirstOrDefault(x => x.ID == version.ID);

        if (currentVersion != null)
        {
            // update
            currentVersion.Name = version.name;
            await _context.SaveChangesAsync();
        }
        else
        {
            // insert
            var newVersion = new PipelineVersionDTO()
            {
                ID = Guid.NewGuid(),
                Name = version.name
            };

            await _context.AddAsync(newVersion);
        }
    }
}