namespace Service_layer.DTOS.Auth
{
    /// <summary>
    /// Admin / owner self-registration (no business yet). For agents use <see cref="RegisterDTO"/> with a business id.
    /// </summary>
    public class RegisterBootstrapDTO
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
