using System;

namespace AquaG.TasksMVC.Models
{
    public class ErrorViewModel

    {
        public string ErrorMessage { get; set; }
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public bool ShowErrorMessage => !string.IsNullOrEmpty(ErrorMessage);
    }
}
