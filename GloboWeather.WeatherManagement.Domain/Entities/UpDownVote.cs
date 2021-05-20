using System;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class UpDownVote: AuditableEntity
    {
        public Guid UpDownSVoteId { get; set; }
        [Required]
        public Guid PostId { get; set; }
        
        [Required]
        [MaxLength(256)]
        public string UniqueIdentifier { get;set; }
        
        [Required]
        [MaxLength(256)]
        public string PosterId { get; set; }
        
        [Required]
        [MaxLength(256)]
        public string VoterId { get; set; }
        
        public DateTime DateVoted { get; set; }
        
        [Required]
        public int VoteIncrement { get; set; }
    }
}