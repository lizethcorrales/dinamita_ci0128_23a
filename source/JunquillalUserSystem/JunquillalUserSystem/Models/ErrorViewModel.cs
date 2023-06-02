namespace JunquillalUserSystem.Models
{
    public class ErrorViewModel
    {
        private string? requestId;
        public string RequestId
        {
            get { return requestId; }
            set { requestId = value; }
        }

        public bool ShowRequestId => !string.IsNullOrEmpty(requestId);
    }
}