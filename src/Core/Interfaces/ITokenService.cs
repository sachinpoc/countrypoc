using System.Threading.Tasks;

namespace MyCleanArchitectureApp.Core.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(string username);
    }
}
