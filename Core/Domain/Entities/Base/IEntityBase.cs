namespace Domain.Entities.Base;

public interface IEntityBase
{
    Guid Id { get; set; }
    DateTime CreateDate { get; init; }
    string CreateUser { get; set; }
    string UpdateUser { get; set; }
    DateTime? UpdateDate { get; set; }
}
