using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Attractions.Models;
using VirtualPark.BusinessLogic.Attractions.Services;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.WebApi.Controllers.Attractions.ModelsIn;
using VirtualPark.WebApi.Controllers.Attractions.ModelsOut;

namespace VirtualPark.WebApi.Controllers.Attractions;

[ApiController]
[Route("attractions")]
public sealed class AttractionController(IAttractionService attractionService) : ControllerBase
{
    private readonly IAttractionService _attractionService = attractionService;

    [HttpPost]
    public CreateAttractionResponse CreateAttraction(CreateAttractionRequest newAtraction)
    {
        AttractionArgs attractionArgs = newAtraction.ToArgs();

        Guid responseId = _attractionService.Create(attractionArgs);

        return new CreateAttractionResponse(responseId.ToString());
    }

    [HttpGet("attractions/{id}")]
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
    public void DeleteAttraction(string id)
    {
        var attractionId = ValidationServices.ValidateAndParseGuid(id);
        _attractionService.Remove(attractionId);
    }
}
