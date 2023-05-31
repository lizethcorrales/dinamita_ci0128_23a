namespace JunquillalUserSystem.Models
{
    public class ErrorViewModel
    {
        public string? requestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(requestId);
    }
}