using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Attractions.Models;
using VirtualPark.BusinessLogic.Attractions.Services;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.WebApi.Controllers.Attractions.ModelsIn;
using VirtualPark.WebApi.Controllers.Attractions.ModelsOut;

namespace VirtualPark.WebApi.Controllers.Attractions;

[ApiController]
public sealed class AttractionController(IAttractionService attractionService) : ControllerBase
{
    private readonly IAttractionService _attractionService = attractionService;

    [HttpPost("attraction")]
    public CreateAttractionResponse Create(CreateAttractionRequest newAtraction)
    {
        AttractionArgs attractionArgs = newAtraction.ToArgs();

        Guid responseId = _attractionService.Create(attractionArgs);

        return new CreateAttractionResponse(responseId.ToString());
    }
    
}
