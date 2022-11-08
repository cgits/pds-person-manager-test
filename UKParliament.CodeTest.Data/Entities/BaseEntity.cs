namespace UKParliament.CodeTest.Data.Entities;

public abstract record BaseEntity(int Id)
{
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}