using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerUI.Models
{
    public class CredentialViewModel
    {
        public Guid Id { get; set; }
        public string AspNetUserId { get; set; }
        public string BuilderInstanceName { get; set; }
        public string BuilderUn { get; set; }
        public string BuilderPw { get; set; }
        public bool IsActive { get; set; }
    }
}