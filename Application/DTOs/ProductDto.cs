using System.ComponentModel.DataAnnotations;
using RS = Resources.Common;

namespace Application.DTOs;

public class ProductDto
{
    [Display(ResourceType = typeof(RS), Name = "ENTITY_LBL_IDENTIFIER")]
    public Guid Id { get; set; }
    [Display(ResourceType = typeof(RS), Name = "PRODUCT_LBL_NAME")]
    public string Name { get; set; }
}
