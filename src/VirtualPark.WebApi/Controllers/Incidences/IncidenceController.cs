using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Incidences.Models;
using VirtualPark.BusinessLogic.Incidences.Service;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.WebApi.Controllers.Incidences.ModelsIn;
using VirtualPark.WebApi.Controllers.Incidences.ModelsOut;

namespace VirtualPark.WebApi.Controllers.Incidences;

[ApiController]
[Route("api/incidences")]
public sealed class IncidenceController(IIncidenceService incidenceService) : ControllerBase
{
    private readonly IIncidenceService _incidenceService = incidenceService;

    [HttpPost]
    public CreateIncidenceResponse CreateIncidence(CreateIncidenceRequest newIncidece)
    {
        IncidenceArgs incidenceArgs = newIncidece.ToArgs();

        Guid responseId = _incidenceService.Create(incidenceArgs);

        return new CreateIncidenceResponse(responseId.ToString());
    }

    [HttpGet("incidences/{id}")]
    public GetIncidenceResponse GetIncidence(string id)
    {
        var incidenceId = ValidationServices.ValidateAndParseGuid(id);

        var incidence = _incidenceService.Get(incidenceId);
        var incidenceResponse = new GetIncidenceResponse(
            id: incidenceId.ToString(),
            typeId: incidence.Type.Id.ToString(),
            description: incidence.Description,
            start: incidence.Start.ToString(),
            end: incidence.End.ToString(),
            attractionId: incidence.AttractionId.ToString(),
            active: incidence.Active.ToString());

        return incidenceResponse;
    }

    [HttpGet]
    public List<GetIncidenceResponse> GetAllIncidences()
    {
        var incidences = _incidenceService.GetAll();

        return incidences.Select(i => new GetIncidenceResponse(
            id: i.Id.ToString(),
            typeId: i.Type?.Id.ToString() ?? string.Empty,
            description: i.Description,
            start: i.Start.ToString(),
            end: i.End.ToString(),
            attractionId: i.AttractionId.ToString(),
            active: i.Active.ToString()
        )).ToList();
    }

    [HttpDelete("incidences/{id}")]
    public void DeleteIncidence(string id)
    {
        var incidenceId = ValidationServices.ValidateAndParseGuid(id);
        _incidenceService.Remove(incidenceId);
    }

    [HttpPut("incidences/{id}")]
    public void UpdateIncidence(string id,  CreateIncidenceRequest request)
    {
        var incidenceId = ValidationServices.ValidateAndParseGuid(id);
        var args = request.ToArgs();
        _incidenceService.Update(args, incidenceId);
    }
}
