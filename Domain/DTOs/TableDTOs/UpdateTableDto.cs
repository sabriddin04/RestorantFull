using Domain.Enums;

namespace Domain.DTOs.TableDTOs;

public class UpdateTableDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int SizePlace { get; set; } = 0!;
    public TableStatus Status { get; set; }
}
