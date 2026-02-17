namespace Service_layer.DTOS.MenuCategory
{
    public class MenuCategoryCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public string BusinessId { get; set; } = string.Empty;
    }
}

