using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhilscanExcellence.Models
{
    public class UserModel
    {
        public Guid ID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string MiddleInitial { get; set; }

        public string LastName { get; set; }

        public string Department { get; set; }

        public int? Type { get; set; }

        public string ShowType
        {
            get
            {
                if (Type == 0)
                    return "Admin";
                else if (Type == 1)
                    return "Exam Admin";
                else if (Type == 2)
                    return "User";
                else
                    return "";
            }
        }

        public int? Status { get; set; }
        
        public string ShowStatus
        {
            get
            {
                if (Status == 1)
                    return "Active";
                else if (Status == 0)
                    return "Inactive";
                else
                    return "";
            }
        }

        public Guid CreatedBy { get; set; }

        public string ShowCreatedBy { get; set; }
        
        public DateTime? CreatedDate { get; set; }

        public string ShowCreatedDate
        {
            get
            {
                if (CreatedDate != null)
                    return DateTime.Parse(CreatedDate.ToString()).ToShortDateString();
                else
                    return "";
            }
        }
    }
}