﻿namespace WorkerNode.Services
{
    public interface IUserRepository
    {
        IEnumerable<string> GetUserRoles(string email);
    }

    public class UserRepository : IUserRepository
    {
        public IEnumerable<string> GetUserRoles(string email)
        {
            return ["user", "admin"];
        }
    }
}
