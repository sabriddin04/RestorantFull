using Domain.Enums;

namespace Domain.DTOs.TableDTOs;

public class CreateTableDto
{
    public string Name { get; set; } = null!;
    public int SizePlace { get; set; } = 0!;
    public TableStatus Status { get; set; }
}
