namespace Service_layer.DTOS.MenuCategory
{
    public class MenuCategoryResponseDTO
    {
        public string MenuCategoryId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string BusinessId { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public int MenuItemsCount { get; set; } // عدد العناصر في هذه الفئة
    }
}

