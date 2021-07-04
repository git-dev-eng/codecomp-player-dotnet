using codecomp.player.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace codecomp.player
{
    public interface IAlgo
    {
        MyAction GetMyAction(GameStatus currentGameStatus);

    }
}