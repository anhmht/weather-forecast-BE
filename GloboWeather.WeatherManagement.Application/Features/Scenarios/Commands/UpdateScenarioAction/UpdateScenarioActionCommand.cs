using System;
using System.Collections.Generic;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.UpdateScenarioAction
{
    public class UpdateScenarioActionCommand : IRequest<Guid>
    {
        public Guid ScenarioId { get; set; }
        public string ScenarioName { get; set; }
        public List<UpdateScenarioActionDto> ScenarioActions { get; set; }
    }

    public class UpdateScenarioActionDto
    {
        public Guid Id { get; set; }
        public int? ActionTypeId { get; set; }
        public int? MethodId { get; set; }
        public int? AreaTypeId { get; set; }
        public string Data { get; set; }
        public int? Duration { get; set; }
        public List<UpdateScenarioActionDetailDto> ScenarioActionDetails { get; set; }

        public UpdateScenarioActionDto()
        {
            ScenarioActionDetails = new List<UpdateScenarioActionDetailDto>();
        }
    }

    public class UpdateScenarioActionDetailDto
    {
        public Guid Id { get; set; }
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

        public UpdateScenarioActionDetailDto()
        {
            IconsList = new List<string>();
        }
    }
}