using System.Linq.Expressions;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Attractions.Models;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Attractions.Services;

public sealed class AttractionService(IRepository<Attraction> attractionRepository, IRepository<VisitorProfile> visitorProfileRepository, IRepository<Ticket> ticketRepository)
{
    private readonly IRepository<Attraction> _attractionRepository = attractionRepository;
    private readonly IRepository<VisitorProfile> _visitorProfileRepository = visitorProfileRepository;
    private readonly IRepository<Ticket> _ticketRepository = ticketRepository;

    public Attraction Create(AttractionArgs args)
    {
        var attraction = MapToEntity(args);

        _attractionRepository.Add(attraction);

        return attraction;
    }

    public List<Attraction> GetAll(Expression<Func<Attraction, bool>>? predicate = null)
    {
        return _attractionRepository.GetAll(predicate);
    }

    public Attraction? Get(Expression<Func<Attraction, bool>> predicate)
    {
        return _attractionRepository.Get(predicate);
    }

    public bool Exist(Expression<Func<Attraction, bool>> predicate)
    {
        return _attractionRepository.Exist(predicate);
    }

    public void Update(AttractionArgs args, Guid id)
    {
        Attraction attraction = Get(a => a.Id == id) ?? throw new InvalidOperationException($"Attraction with id {id} not found.");
        ApplyArgsToEntity(attraction, args);

        _attractionRepository.Update(attraction);
    }

    public void Remove(Guid id)
    {
        Attraction attraction = Get(a => a.Id == id) ?? throw new InvalidOperationException($"Attraction with id {id} not found.");
        _attractionRepository.Remove(attraction);
    }

    public static void ApplyArgsToEntity(Attraction entity, AttractionArgs args)
    {
        entity.Type = args.Type;
        entity.Name = args.Name;
        entity.MiniumAge = args.MiniumAge;
        entity.Capacity = args.Capacity;
        entity.Description = args.Description;
        entity.CurrentVisitors = args.CurrentVisitor;
        entity.Available = args.Available;
    }

    public void ValidateAttractionName(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Attraction name cannot be empty.", nameof(name));
        }

        if(_attractionRepository.Exist(a => a.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)))
        {
            throw new Exception("Attraction name already exists.");
        }
    }

    public Attraction MapToEntity(AttractionArgs args)
    {
        ValidateAttractionName(args.Name);
        ValidationServices.ValidateAge(args.MiniumAge);

        var attraction = new Attraction
        {
            Name = args.Name,
            Type = args.Type,
            Description = args.Description,
            MiniumAge = args.MiniumAge,
            Capacity = args.Capacity,
            CurrentVisitors = args.CurrentVisitor,
            Available = args.Available
        };

        return attraction;
    }

    public bool ValidateEntryByNfc(Guid attractionId, Guid visitorId)
    {
        var attraction = _attractionRepository.Get(a => a.Id == attractionId);
        var visitor = _visitorProfileRepository.Get(v => v.Id == visitorId);

        if(attraction is null || visitor is null)
        {
            return false;
        }

        if(!attraction.Available)
        {
            return false;
        }

        if(IsAtCapacity(attraction))
        {
            return false;
        }

        if(!IsOldEnough(visitor, attraction.MiniumAge))
        {
            return false;
        }

        attraction.CurrentVisitors++;
        _attractionRepository.Update(attraction);

        return true;
    }

    private static bool IsAtCapacity(Attraction attraction) =>
        attraction.CurrentVisitors >= attraction.Capacity;

    private static bool IsOldEnough(VisitorProfile visitor, int minAge)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - visitor.DateOfBirth.Year;

        if(visitor.DateOfBirth > today.AddYears(-age))
        {
            age--;
        }

        return age >= minAge;
    }

    public bool ValidateEntryByQr(Guid attractionId, Guid qrId)
    {
        Ticket? ticket = _ticketRepository.Get(t => t.QrId == qrId);
        return ticket != null;
    }
}
