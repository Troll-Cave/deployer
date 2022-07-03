using data;
using data.DataModels;
using data.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Logic;

public class ApplicationLogic
{
    private readonly DeployerContext _deployerContext;

    public ApplicationLogic(DeployerContext deployerContext)
    {
        _deployerContext = deployerContext;
    }
    
    public Application MapDTO(ApplicationDTO dto)
    {
        return new Application()
        {
            ID = dto.ID,
            Name = dto.Name,
            Organization = dto.Organization,
            PipelineVersionId = dto.PipelineVersionId,
            Source = dto.Source,
            Variables = dto.Variables.Variables
        };
    }

    public void SetDTO(ApplicationDTO dto, Application model)
    {
        dto.Name = model.Name; // TODO: May remove this
        dto.Organization = model.Organization;
        dto.PipelineVersionId = model.PipelineVersionId;
        dto.Source = model.Source;
        dto.Variables = new ApplicationVariables()
        {
            Variables = model.Variables
        };
    }

    public async Task<List<Application>> GetAll()
    {
        return (await _deployerContext.Applications.ToListAsync())
            .Select(MapDTO)
            .ToList();
    }

    public async Task Create(Application application)
    {
        var app = new ApplicationDTO()
        {
            ID = Guid.NewGuid()
        };
        
        SetDTO(app, application);

        await _deployerContext.AddAsync(app);
        await _deployerContext.SaveChangesAsync();
    }
    
    public async Task Update(Application application, Guid id)
    {
        var app = await _deployerContext.Applications.FindAsync(id);

        if (app == null)
        {
            // TODO: Correct this abomination with a custom error
            throw new IndexOutOfRangeException($"App {application.ID} not found"); 
        }
        
        SetDTO(app, application);

        await _deployerContext.SaveChangesAsync();
    }
}