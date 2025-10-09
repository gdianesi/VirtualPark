namespace VirtualPark.BusinessLogic.VisitRegistrations.Service;

public interface IVisitRegistrationService
{
    public void CloseVisitByVisitor(Guid visitorProfileId);
}
