using System.Runtime.CompilerServices;
using eduediligence.Config;
using eduediligence.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace eduediligence.Services
{
    public class ValidationService : IValidationService
    {

        private readonly string un;
        private readonly string pw;

        public ValidationService(IOptions<AuthSettings> settings)
        {
            un = settings.Value.User;
            pw = settings.Value.Password;
        }
        
        public bool Validate(string username, string password)
        {
            if (username == un && password == pw)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
