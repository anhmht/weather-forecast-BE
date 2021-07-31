using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.UpdateScenarioAction
{
    public class UpdateScenarioActionCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public int? ActionTypeId { get; set; }
        public int? MethodId { get; set; }
        public int? AreaTypeId { get; set; }
        public string Data { get; set; }
        public int? Duration { get; set; }
        public int Order { get; set; }
        public float? Top { get; set; }
        public float? Left { get; set; }
        public float? Bottom { get; set; }
        public float? Right { get; set; }
        public bool? IsEnableIcon { get; set; }
        public bool? IsEnableLayer { get; set; }
        public bool? IsDisplayHydrological { get; set; }
        
        public List<UpdateScenarioActionDetailDto> ScenarioActionDetails { get; set; }

        public UpdateScenarioActionCommand()
        {
            ScenarioActionDetails = new List<UpdateScenarioActionDetailDto>();
        }
    }

    public class UpdateScenarioActionDetailDto
    {
        public Guid? Id { get; set; }
        public int ScenarioActionTypeId { get; set; }
        public int? ActionTypeId { get; set; }
        public int? MethodId { get; set; }
        public string Content { get; set; }
        public int? Duration { get; set; }
        public int? Time { get; set; }
        public int? PositionId { get; set; }
        public bool? CustomPosition { get; set; }
        public int? Left { get; set; }
        public int? Top { get; set; }
        public bool? IsDisplay { get; set; }
        public int? StartTime { get; set; }
        public int? Width { get; set; }
        public string PlaceId { get; set; }
        public bool? IsProvince { get; set; }
        public List<string> IconsList { get; set; }
        [JsonIgnore]
        public string IconUrls { get; set; }
        public bool? IsEnableIcon { get; set; }

        public UpdateScenarioActionDetailDto()
        {
            IconsList = new List<string>();
        }
    }
}