namespace Recrutment.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
    }
}
