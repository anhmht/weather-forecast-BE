using System;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class UpDownVote: AuditableEntity
    {
        public Guid UpDownSVoteId { get; set; }
        public Guid PostId { get; set; }
        public string UniqueIdentifier { get;set; }
        public string PosterId { get; set; }
        public string VoterId { get; set; }
        public string DateVoted { get; set; }
        public int VoteIncrement { get; set; }
    }
}