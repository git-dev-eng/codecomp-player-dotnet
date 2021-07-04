using codecomp.player.Model;
using System.Threading;
using System.Threading.Tasks;

namespace codecomp.player
{
    public interface IApiHandler
    {
        Task<GameStatus> GetGameStatus();
        Task<JoinResponse> Join();
        Task<MyActionResponse> Action(MyAction action);
    }
}