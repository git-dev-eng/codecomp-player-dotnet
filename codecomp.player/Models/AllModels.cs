using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace codecomp.player.Model
{
    public class RoundParameter
    {
        public string SecretLength { get; set; }
        public int LifeLines { get; set; }
        public double Penalty { get; set; }
    }

    public class GameStatus
    {
        public string GameId { get; set; }
        public string RoundId { get; set; }
        public int RoundNumber { get; set; }
        public string Status { get; set; }

        public int SecretLength { get; set; }

        public List<Participant> Participants { get; set; }
        public RoundParameter RoundParameters { get; set; }
    }

    public class Participant
    {
        public string TeamId { get; set; }
        public bool IsRobot { get; set; }
        public List<string> KilledBy { get; set; }
        public bool IsAlive { get; set; }
    }

    public class JoinResponse
    {
        public bool Joined { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class MyGuess
    {
        [JsonProperty("guess")]
        public string Guess { get; set; }
        [JsonProperty("team")]
        public string TeamName { get; set; }
    }


    public class MyAction
    {
        [JsonProperty("guesses")]
        public List<MyGuess> Guesses { get; set; }


    }

    public class MyScore
    {
        public string TargetTeam { get; set; }
        public int Score { get; set; }
        public string Guess { get; set; }
        public int NoOfDigitsMatchedByPositionAndValue { get; set; }
        public int NoOfDigitsMatchedByValue { get; set; }
        public bool IsValid { get; set; }
    }
    public class MyActionResponse
    {
        public string RequestId { get; set; }
        public List<MyScore> Guesses { get; set; }
        public Error Error { get; set; }
    }



    public class Error
    {
        public string Description { get; set; }
        public string Message { get; set; }
    }
}
