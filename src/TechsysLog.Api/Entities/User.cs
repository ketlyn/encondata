using TechsysLog.Core.DomainObjects;

namespace TechsysLog.Api.Entities
{
    public class User : Entity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public int Password { get; private set; }

        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = Password;
        }
        protected User() { }
    }
}
