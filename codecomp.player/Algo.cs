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
using System.Collections.Generic;
using System.Linq;

namespace codecomp.player
{
    public class Algo : IAlgo
    {
        private readonly ILogger<Bot> _logger;
        private readonly AppSettings _appSettings;


        public Algo(IOptions<AppSettings> config, ILogger<Bot> logger)
        {
            if (config == null) throw new ArgumentNullException("config");
            _logger = logger ?? throw new ArgumentNullException("logger");

            _appSettings = config.Value;
        }

        public MyAction GetMyAction(GameStatus currentGameStatus)
        {
            // I only guess for teams who are alive and for those whose 
			// secret I haven't cracked yet. Am I already not smart? Everyone says I am a dumb bot. :(
			// Help me to prove them wrong. Make me smarter.	

            var myGuess = new List<MyGuess>();
            var legalParticipants = new List<Participant>();

            foreach (var participant in currentGameStatus.Participants)
            {
                if (participant.IsAlive && participant.TeamId != _appSettings.Team && (participant.KilledBy == null || !participant.KilledBy.Contains(_appSettings.Team)))
                {
                    legalParticipants.Add(new Participant { TeamId = participant.TeamId, IsAlive = participant.IsAlive, IsRobot = participant.IsRobot });
                }

            }

            if (legalParticipants.Count > 0)
            {
                var totalParticipants = legalParticipants.Count;
                for (int i = 0; i < 5; i++)
                {
                    var random = new Random();
                    var currentParticipant = legalParticipants[random.Next(totalParticipants)];
                    var secretRange = Math.Pow(10, currentGameStatus.SecretLength - 1);
                    var secret = Convert.ToString(random.Next(Convert.ToInt32(secretRange) * 10));
                    myGuess.Add(new MyGuess { Guess = secret, TeamName = currentParticipant.TeamId });
                }
            }
            else
            {
                _logger.LogError("No Other Participants to Play With!!!");
            }


            return new MyAction { Guesses = myGuess };
        }

    }
}