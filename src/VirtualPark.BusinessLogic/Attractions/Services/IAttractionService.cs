using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Attractions.Models;

namespace VirtualPark.BusinessLogic.Attractions.Services;

public interface IAttractionService
{
    public Guid Create(AttractionArgs args);
    public Attraction? Get(Guid id);
    public List<Attraction> GetAll();
    public void Remove(Guid id);
    public void Update(AttractionArgs args, Guid userId);
}
