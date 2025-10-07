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
}
