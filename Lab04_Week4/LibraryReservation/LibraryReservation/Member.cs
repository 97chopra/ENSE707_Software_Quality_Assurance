using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryReservation
{
    public class Member
    {
        public string Id { get; }
        public string FullName { get; }

        public Member(string id, string fullName)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Member ID is required.");
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Member name is required.");

            Id = id;
            FullName = fullName;
        }
    }
}