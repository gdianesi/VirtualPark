using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.TypeIncidences.Models;
using VirtualPark.BusinessLogic.TypeIncidences.Service;
using VirtualPark.WebApi.TypeIncidences.ModelsIn;
using VirtualPark.WebApi.TypeIncidences.ModelsOut;

namespace VirtualPark.WebApi.TypeIncidences;

[ApiController]
public sealed class TypeIncidenceController(ITypeIncidenceService service) : ControllerBase
{
    private readonly ITypeIncidenceService _service = service;

    [HttpPost("typeIncidences")]
    public CreateTypeIncidenceResponse CreateTypeIncidence(CreateTypeIncidenceRequest request)
    {
        var args = request.ToArgs();

        Guid id = _service.Create(args);

        return new CreateTypeIncidenceResponse(id.ToString());
    }
}
