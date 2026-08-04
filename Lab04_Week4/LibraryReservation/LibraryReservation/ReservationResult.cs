using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryReservation
{
    public class ReservationResult
    {
        public bool Success { get; }
        public string Message { get; }

        public ReservationResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}