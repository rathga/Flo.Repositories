using System;
using System.Collections.Generic;
using System.Text;

namespace Flo.Repositories
{
    public interface IAuthenticationService
    {
        string GetCurrentUserId();
    }
}
