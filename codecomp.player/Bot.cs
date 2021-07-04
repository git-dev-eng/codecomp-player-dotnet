using codecomp.player.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace codecomp.player
{
    public class Bot : IHostedService
    {
        #region local variables
        private readonly IApiHandler _api;
        private readonly AppSettings _appSettings;
        private readonly ILogger<Bot> _logger;
        private readonly IAlgo _myAlgo;
        private Timer _timer;

        #endregion

        public Bot(IOptions<AppSettings> config, ILogger<Bot> logger, IApiHandler api, IAlgo algo)
        {
            if (config == null) throw new ArgumentNullException("config");
            _logger = logger ?? throw new ArgumentNullException("logger");
            _api = api ?? throw new ArgumentNullException("api");
            _myAlgo = algo ?? throw new ArgumentNullException("algo");

            _appSettings = config.Value;

            //Initialize timer
            _timer = new Timer(Play, null, Timeout.Infinite, 0);
        }

        private async void Play(object state)
        {
            _logger.LogInformation($"{DateTime.Now} Bot running...");

            _timer?.Change(Timeout.Infinite, 0); //Pause the timer to ensure "Play" will not be called till it finishes

            try
            {
                var myTeamName = _appSettings.Team;
                var myPassword = _appSettings.Password;
                if (string.IsNullOrWhiteSpace(myTeamName) || string.IsNullOrWhiteSpace(myPassword))
                    throw new InvalidOperationException("To begin, put your team name and password into appsettings.json");

                var currentGameStatus = await _api.GetGameStatus(); //Fetch current game status
                _logger.LogInformation($"{DateTime.Now} Round {currentGameStatus.RoundNumber} status is {currentGameStatus.Status}");

                //I need to join the game once, within a specific "joining" phase (starting of each round)
                var haveIJoinedOnce = currentGameStatus.Participants.Any(member => member.TeamId == _appSettings.Team);
                var isAlive = currentGameStatus.Participants.Any(member => member.IsAlive);


                switch (currentGameStatus.Status.ToUpperInvariant())
                {
                    case "JOINING": //joining phase

                        if (!haveIJoinedOnce) //Join if only I haven't
                        {
                            var joinResponse = await _api.Join();
                            if (joinResponse.Joined)
                                _logger.LogInformation($"{DateTime.Now} Round {currentGameStatus.RoundNumber} joined");
                            else
                                _logger.LogError($"{DateTime.Now} Round {currentGameStatus.RoundNumber} joined failed : {joinResponse.ErrorMessage}");
                        }

                        break;
                    case "RUNNING": //running phase

                        if (haveIJoinedOnce) //Play if I have joined
                        {
                            if (isAlive)
                            {

                                //My strategy
                                var myAction = _myAlgo.GetMyAction(currentGameStatus);

                                if (myAction.Guesses.Count > 0)
                                {
                                    Console.WriteLine(myAction.Guesses.Count);
                                    //My action
                                    var myActionResponse = await _api.Action(myAction);

                                    if (myActionResponse.Error != null)
                                    {
                                        _logger.LogError($"{myActionResponse.RequestId}|{myActionResponse.Error.Message}|{myActionResponse.Error.Description}");
                                    }
                                    else
                                    {
                                        int totalGuessScore = 0;
                                        foreach (var score in myActionResponse.Guesses)
                                        {
                                            totalGuessScore += score.Score;

                                        }
                                        _logger.LogInformation($"{totalGuessScore}");
                                        _logger.LogInformation($"{myActionResponse.RequestId}");

                                    }

                                }
                            }
                            else
                            {
                                _logger.LogInformation($"I am dead, waiting to respawn in next round...:(");

                            }


                        }
                        else
                        {
                            _logger.LogError($"Oho, I have missed the joining phase, let me wait till the next round starts");

                        }
                        break;
                    default:
                        break;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            finally
            {
                _timer?.Change(_appSettings.Interval, Timeout.Infinite); //Ensure that "Play" will be called again after defined interval (default:5sec) 
            }
        }

        #region IHostedService 

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now} ---------------Player bot started---------------");

            _timer?.Change(0, Timeout.Infinite);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now} ---------------Player bot stopped---------------");

            _timer?.Change(Timeout.Infinite, 0);

            _timer?.Dispose();

            return Task.CompletedTask;
        }

        #endregion


    }
}