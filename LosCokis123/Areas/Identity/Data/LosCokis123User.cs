using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LosCokis123.Areas.Identity.Data;

// Add profile data for application users by adding properties to the LosCokis123User class
public class LosCokis123User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

