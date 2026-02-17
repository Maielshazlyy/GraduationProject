namespace Service_layer.DTOS.MenuCategory
{
    public class MenuCategoryUpdateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}

