namespace VirtualPark.BusinessLogic.VisitsScore.Models;

public class RecordVisitScoreArgs
{
    public Guid VisitorProfileId { get; set; }
    public string Origin { get; set; } = null!;
    public int? Points { get; set; }
}
