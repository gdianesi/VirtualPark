using VirtualPark.BusinessLogic.TypeIncidences.Entity;
using VirtualPark.BusinessLogic.TypeIncidences.Models;

namespace VirtualPark.BusinessLogic.TypeIncidences.Service;

public interface ITypeIncidenceService
{
    public Guid Create(TypeIncidenceArgs args);
    public List<TypeIncidence> GetAll();
    public TypeIncidence Get(Guid id);
    public void Update(Guid id, TypeIncidenceArgs args);
    public void Delete(Guid id);
}
