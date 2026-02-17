namespace Service_layer.DTOS.MenuCategory
{
    public class ReorderCategoriesDTO
    {
        public List<CategoryOrderItem> Categories { get; set; } = new List<CategoryOrderItem>();
    }

    public class CategoryOrderItem
    {
        public string MenuCategoryId { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}

