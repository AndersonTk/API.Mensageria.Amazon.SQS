using System.ComponentModel.DataAnnotations;
using RS = Resources.Common;

namespace Application.DTOs;
public class CategoryDto
{
    [Display(ResourceType = typeof(RS), Name = "ENTITY_LBL_IDENTIFIER")]
    public Guid Id { get; set; }
    [Display(ResourceType = typeof(RS), Name = "CATEGORY_LBL_NAME")]
    public string Name { get; set; }
}
