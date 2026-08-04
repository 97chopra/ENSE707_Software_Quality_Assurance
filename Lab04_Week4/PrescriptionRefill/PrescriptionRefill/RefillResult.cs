using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrescriptionRefill
{
    public class RefillResult
    {
        public bool Success { get; }
        public string Message { get; }
        public bool IsUrgent { get; }

        public RefillResult(bool success, string message, bool isUrgent = false)
        {
            Success = success;
            Message = message;
            IsUrgent = isUrgent;
        }
    }
}