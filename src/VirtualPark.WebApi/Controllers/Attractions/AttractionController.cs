using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Attractions.Models;
using VirtualPark.BusinessLogic.Attractions.Services;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.WebApi.Controllers.Attractions.ModelsIn;
using VirtualPark.WebApi.Controllers.Attractions.ModelsOut;
using VirtualPark.WebApi.Filters.Authenticator;
using VirtualPark.WebApi.Filters.Authorization;

namespace VirtualPark.WebApi.Controllers.Attractions;

[ApiController]
[Route("attractions")]
[AuthenticationFilter]
public sealed class AttractionController(IAttractionService attractionService) : ControllerBase
{
    private readonly IAttractionService _attractionService = attractionService;

    [HttpPost]
    [AuthorizationFilter]
    public CreateAttractionResponse CreateAttraction(CreateAttractionRequest newAtraction)
    {
        AttractionArgs attractionArgs = newAtraction.ToArgs();

        Guid responseId = _attractionService.Create(attractionArgs);

        return new CreateAttractionResponse(responseId.ToString());
    }

    [HttpGet("attractions/{id}")]
    [AuthorizationFilter]
    public GetAttractionResponse GetAttractionById(string id)
    {
        var attractionId = ValidationServices.ValidateAndParseGuid(id);

        var attraction = _attractionService.Get(attractionId);

        return new GetAttractionResponse(
            id: attraction.Id.ToString(),
            name: attraction.Name,
            type: attraction.Type.ToString(),
            miniumAge: attraction.MiniumAge.ToString(),
            capacity: attraction.Capacity.ToString(),
            description: attraction.Description,
            eventsId: attraction.Events.Select(e => e.Id.ToString()).ToList(),
            available: attraction.Available.ToString());
    }

    [HttpGet]
    [AuthorizationFilter]
    public List<GetAttractionResponse> GetAllAttractions()
    {
        return _attractionService.GetAll().Select(a => new GetAttractionResponse(
                id: a.Id.ToString(),
                name: a.Name,
                type: a.Type.ToString(),
                miniumAge: a.MiniumAge.ToString(),
                capacity: a.Capacity.ToString(),
                description: a.Description,
                eventsId: a.Events.Select(e => e.Id.ToString()).ToList(),
                available: a.Available.ToString()))
            .ToList();
    }

    [HttpDelete("attractions/{id}")]
    [AuthorizationFilter]
    public void DeleteAttraction(string id)
    {
        var attractionId = ValidationServices.ValidateAndParseGuid(id);
        _attractionService.Remove(attractionId);
    }

    [HttpPut("attractions/{id}")]
    [AuthorizationFilter]
    public void UpdateAttraction(string id, CreateAttractionRequest newAttraction)
    {
        var idAttraction = ValidationServices.ValidateAndParseGuid(id);
        AttractionArgs attractionArgs = newAttraction.ToArgs();
        _attractionService.Update(attractionArgs, idAttraction);
    }

    [HttpGet("report")]
    public List<ReportAttractionsResponse> GetAttractionsReport(string from, string to)
    {
        var fromDate = ValidationServices.ValidateDateTime(from);
        var toDate = ValidationServices.ValidateDateTime(to);

        var lines = _attractionService.AttractionsReport(fromDate, toDate);

        return lines.Select(line =>
        {
            var parts = line.Split('\t');
            var name = parts.Length > 0 ? parts[0] : string.Empty;
            var visits = parts.Length > 1 ? parts[1] : "0";
            return new ReportAttractionsResponse(name, visits);
        }).ToList();
    }
}
