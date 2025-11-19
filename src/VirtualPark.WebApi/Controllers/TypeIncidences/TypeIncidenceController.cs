using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.TypeIncidences.Service;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.WebApi.Controllers.TypeIncidences.ModelsIn;
using VirtualPark.WebApi.Controllers.TypeIncidences.ModelsOut;
using VirtualPark.WebApi.Filters.Authenticator;
using VirtualPark.WebApi.Filters.Authorization;

namespace VirtualPark.WebApi.Controllers.TypeIncidences;

[ApiController]
[AuthenticationFilter]
[Route("typeIncidences")]
public sealed class TypeIncidenceController(ITypeIncidenceService service) : ControllerBase
{
    private readonly ITypeIncidenceService _service = service;

    [HttpPost("incidence-types")]
    [AuthorizationFilter]
    public CreateTypeIncidenceResponse CreateTypeIncidence(CreateTypeIncidenceRequest request)
    {
        var args = request.ToArgs();

        Guid id = _service.Create(args);

        return new CreateTypeIncidenceResponse(id.ToString());
    }

    [HttpGet("incidence-types/{id}")]
    [AuthorizationFilter]
    public GetTypeIncidenceResponse GetTypeIncidenceById(string id)
    {
        var guid = ValidationServices.ValidateAndParseGuid(id);

        var typeIncidence = _service.Get(guid)!;

        return new GetTypeIncidenceResponse(
            id: typeIncidence.Id.ToString(),
            type: typeIncidence.Type);
    }

    [HttpGet("incidence-types")]
    [AuthorizationFilter]
    public List<GetTypeIncidenceResponse> GetAllTypeIncidences()
    {
        return _service.GetAll()
            .Select(t => new GetTypeIncidenceResponse(
                id: t.Id.ToString(),
                type: t.Type))
            .ToList();
    }

    [HttpDelete("incidence-types/{id}")]
    [AuthorizationFilter]
    public void DeleteTypeIncidence(string id)
    {
        var guid = ValidationServices.ValidateAndParseGuid(id);
        _service.Delete(guid);
    }

    [HttpPut("incidence-types/{id}")]
    [AuthorizationFilter]
    public void UpdateTypeIncidence(string id, CreateTypeIncidenceRequest request)
    {
        var guid = ValidationServices.ValidateAndParseGuid(id);

        var args = request.ToArgs();

        _service.Update(guid, args);
    }
}
