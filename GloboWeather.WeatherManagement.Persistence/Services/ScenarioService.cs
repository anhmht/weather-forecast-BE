using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.CreateScenarioAction;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.DeleteScenarioAction;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.UpdateScenarioAction;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenarioActionDetail;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Media;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Persistence.Services
{
    public class ScenarioService : IScenarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommonService _commonService;
        private readonly IImageService _imageService;

        public ScenarioService(IUnitOfWork unitOfWork, ICommonService commonService, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _commonService = commonService;
            _imageService = imageService;
        }

        public async Task<ScenarioActionDetailVm> GetDetailAsync(GetScenarioActionDetailQuery request)
        {
            var response = new ScenarioActionDetailVm();

            var query = from s in _unitOfWork.ScenarioRepository.GetAllQuery()
                        join satemp in _unitOfWork.ScenarioActionRepository.GetAllQuery()
                            on s.ScenarioId equals satemp.ScenarioId into temp1
                        from sa in temp1.DefaultIfEmpty()
                        join sadtemp in _unitOfWork.ScenarioActionDetailRepository.GetAllQuery()
                            on sa.Id equals sadtemp.ActionId into temp2
                        from sad in temp2.DefaultIfEmpty()
                        where s.ScenarioId == request.ScenarioId
                        select new
                        {
                            s.ScenarioId,
                            s.ScenarioName,
                            s.ScenarioContent,
                            s.CreateBy,
                            s.CreateDate,
                            s.LastModifiedBy,
                            s.LastModifiedDate,
                            ScenarioActionId = sa.Id,
                            ScenarioActionData = sa.Data,
                            ScenarioActionActionTypeId = sa.ActionTypeId,
                            ScenarioActionAreaTypeId = sa.AreaTypeId,
                            ScenarioActionDuration = sa.Duration,
                            ScenarioActionMethodId = sa.MethodId,
                            ScenarioActionCreateBy = sa.CreateBy,
                            ScenarioActionCreateDate = sa.CreateDate,
                            ScenarioActionLastModifiedBy = sa.LastModifiedBy,
                            ScenarioActionLastModifiedDate = sa.LastModifiedDate,
                            ScenarioActionDetailId = sad.Id,
                            ScenarioActionDetailContent = sad.Content,
                            ScenarioActionDetailMethodId = sad.MethodId,
                            ScenarioActionDetailActionTypeId = sad.ActionTypeId,
                            ScenarioActionDetailCustomPosition = sad.CustomPosition,
                            ScenarioActionDetailDuration = sad.Duration,
                            ScenarioActionDetailIconUrls = sad.IconUrls,
                            ScenarioActionDetailIsDisplay = sad.IsDisplay,
                            ScenarioActionDetailLeft = sad.Left,
                            ScenarioActionDetailPositionId = sad.PositionId,
                            ScenarioActionDetailScenarioActionTypeId = sad.ScenarioActionTypeId,
                            ScenarioActionDetailStartTime = sad.StartTime,
                            ScenarioActionDetailTime = sad.Time,
                            ScenarioActionDetailTop = sad.Top,
                            ScenarioActionDetailWidth = sad.Width,
                            ScenarioActionDetailCreateBy = sad.CreateBy,
                            ScenarioActionDetailCreateDate = sad.CreateDate,
                            ScenarioActionDetailLastModifiedBy = sad.LastModifiedBy,
                            ScenarioActionDetailLastModifiedDate = sad.LastModifiedDate,
                            ScenarioActionDetailPlaceId = sad.PlaceId,
                            ScenarioActionDetailIsProvince = sad.IsProvince
                        };

            var scenarioDetail = await query.ToListAsync();

            var actionAreaType = await _commonService.GetCommonLookupByNameSpaceAsync(LookupNameSpace.ActionAreaType);
            var actionMethod = await _commonService.GetCommonLookupByNameSpaceAsync(LookupNameSpace.ActionMethod);
            var actionType = await _commonService.GetCommonLookupByNameSpaceAsync(LookupNameSpace.ActionType);
            var position = await _commonService.GetCommonLookupByNameSpaceAsync(LookupNameSpace.Position);
            var scenarioActionType = await _commonService.GetCommonLookupByNameSpaceAsync(LookupNameSpace.ScenarioActionType);

            response.ScenarioId = scenarioDetail.First().ScenarioId;
            response.ScenarioName = scenarioDetail.First().ScenarioName;
            response.ScenarioContent = scenarioDetail.First().ScenarioContent;

            var scenarioActionDetails = scenarioDetail.Select(x => new ScenarioActionDetailDto()
            {
                Action = actionType.FirstOrDefault(t => t.ValueId == x.ScenarioActionDetailActionTypeId)?.ValueText,
                Id = x.ScenarioActionDetailId,
                CreateBy = x.ScenarioActionDetailCreateBy,
                LastModifiedDate = x.ScenarioActionDetailLastModifiedDate,
                CreateDate = x.ScenarioActionDetailCreateDate,
                LastModifiedBy = x.ScenarioActionDetailLastModifiedBy,
                Content = x.ScenarioActionDetailContent,
                ScenarioActionTypeId = x.ScenarioActionDetailScenarioActionTypeId,
                ActionId = x.ScenarioActionId,
                IconsList = x.ScenarioActionDetailIconUrls?.Split(Constants.SemiColonStringSeparator, StringSplitOptions.RemoveEmptyEntries).ToList(),
                Duration = x.ScenarioActionDetailDuration,
                Time = x.ScenarioActionDetailTime,
                MethodId = x.ScenarioActionDetailMethodId,
                Width = x.ScenarioActionDetailWidth,
                Top = x.ScenarioActionDetailTop,
                IsDisplay = x.ScenarioActionDetailIsDisplay,
                CustomPosition = x.ScenarioActionDetailCustomPosition,
                StartTime = x.ScenarioActionDetailStartTime,
                Left = x.ScenarioActionDetailLeft,
                PositionId = x.ScenarioActionDetailPositionId,
                ActionTypeId = x.ScenarioActionDetailActionTypeId,
                IconUrls = x.ScenarioActionDetailIconUrls,
                Method = actionMethod.FirstOrDefault(t => t.ValueId == x.ScenarioActionDetailMethodId)?.ValueText,
                Posision = position.FirstOrDefault(t => t.ValueId == x.ScenarioActionDetailPositionId)?.ValueText,
                ScenarioActionTypeName = scenarioActionType.FirstOrDefault(t => t.ValueId == x.ScenarioActionDetailScenarioActionTypeId)?.ValueText,
                PlaceId = x.ScenarioActionDetailPlaceId,
                IsProvince = x.ScenarioActionDetailIsProvince
            }).Distinct().ToList();

            foreach (var detail in scenarioDetail)
            {
                if (response.ScenarioActions.All(x => x.Id != detail.ScenarioActionId))
                {
                    response.ScenarioActions.Add(new ScenarioActionDto()
                    {
                        Id = detail.ScenarioActionId,
                        CreateBy = detail.ScenarioActionCreateBy,
                        LastModifiedDate = detail.ScenarioActionLastModifiedDate,
                        Data = detail.ScenarioActionData,
                        ScenarioId = detail.ScenarioId,
                        CreateDate = detail.ScenarioActionCreateDate,
                        LastModifiedBy = detail.ScenarioActionLastModifiedBy,
                        AreaTypeId = detail.ScenarioActionAreaTypeId,
                        Duration = detail.ScenarioActionDuration,
                        MethodId = detail.ScenarioActionMethodId,
                        ActionTypeId = detail.ScenarioActionActionTypeId,
                        Action = actionType.FirstOrDefault(t => t.ValueId == detail.ScenarioActionActionTypeId)?.ValueText,
                        Method = actionMethod.FirstOrDefault(t => t.ValueId == detail.ScenarioActionMethodId)?.ValueText,
                        AreaTypeName = actionAreaType.FirstOrDefault(t => t.ValueId == detail.ScenarioActionAreaTypeId)?.ValueText,
                        ScenarioActionDetails = scenarioActionDetails?.FindAll(d => d.ActionId == detail.ScenarioActionId)
                    });
                }
            }

            return response;
        }

        public async Task<Guid> CreateAsync(CreateScenarioActionCommand request, CancellationToken cancellationToken)
        {
            var scenario = new Scenario()
            {
                ScenarioId = Guid.NewGuid(),
                ScenarioName = request.ScenarioName,
                ScenarioContent = string.Empty
            };
            _unitOfWork.ScenarioRepository.Add(scenario);

            if (request.ScenarioActions?.Any() == true)
            {
                foreach (var createScenarioActionDto in request.ScenarioActions)
                {
                    var scenarioAction = new ScenarioAction()
                    {
                        Id = Guid.NewGuid(),
                        ActionTypeId = createScenarioActionDto.ActionTypeId,
                        AreaTypeId = createScenarioActionDto.AreaTypeId,
                        Data = createScenarioActionDto.Data,
                        Duration = createScenarioActionDto.Duration,
                        MethodId = createScenarioActionDto.MethodId,
                        ScenarioId = scenario.ScenarioId
                    };
                    _unitOfWork.ScenarioActionRepository.Add(scenarioAction);

                    if (createScenarioActionDto.ScenarioActionDetails?.Any() == true)
                    {
                        foreach (var createScenarioActionDetailDto in createScenarioActionDto.ScenarioActionDetails)
                        {
                            var scenarioActionDetail = new ScenarioActionDetail()
                            {
                                MethodId = createScenarioActionDetailDto.MethodId,
                                Id = Guid.NewGuid(),
                                ActionId = scenarioAction.Id,
                                ActionTypeId = createScenarioActionDetailDto.ActionTypeId,
                                Content = createScenarioActionDetailDto.Content,
                                CustomPosition = createScenarioActionDetailDto.CustomPosition,
                                Duration = createScenarioActionDetailDto.Duration,
                                IsDisplay = createScenarioActionDetailDto.IsDisplay,
                                IsProvince = createScenarioActionDetailDto.IsProvince,
                                Left = createScenarioActionDetailDto.Left,
                                PlaceId = createScenarioActionDetailDto.PlaceId,
                                PositionId = createScenarioActionDetailDto.PositionId,
                                ScenarioActionTypeId = createScenarioActionDetailDto.ScenarioActionTypeId,
                                StartTime = createScenarioActionDetailDto.StartTime,
                                Time = createScenarioActionDetailDto.Time,
                                Top = createScenarioActionDetailDto.Top,
                                Width = createScenarioActionDetailDto.Width
                            };
                            if (createScenarioActionDetailDto.IconsList?.Any() == true)
                            {
                                var iconsResult =
                                    await _imageService.CopyFileToStorageContainerAsync(
                                        createScenarioActionDetailDto.IconsList
                                        , scenarioAction.Id.ToString(), Forder.IconImage,
                                        StorageContainer.Scenarios);
                                scenarioActionDetail.IconUrls = string.Join(Constants.SemiColonStringSeparator,
                                    iconsResult);
                            }
                            _unitOfWork.ScenarioActionDetailRepository.Add(scenarioActionDetail);
                        }
                    }
                }
            }

            await _unitOfWork.CommitAsync();

            return scenario.ScenarioId;
        }

        public async Task<Guid> UpdateAsync(UpdateScenarioActionCommand request, CancellationToken cancellationToken)
        {
            //Get existing scenario
            var scenario = await _unitOfWork.ScenarioRepository.GetByIdAsync(request.ScenarioId);
            if (scenario == null)
            {
                throw new NotFoundException(nameof(Scenario), request.ScenarioId);
            }

            //Update Scenario
            UpdateScenario(request, scenario);

            //UpdateScenarioAction & detail
            await UpdateScenarioAction(request, cancellationToken, scenario);

            await _unitOfWork.CommitAsync();

            return scenario.ScenarioId;
        }

        private async Task UpdateScenarioAction(UpdateScenarioActionCommand request, CancellationToken cancellationToken,
            Scenario scenario)
        {
            //Get ScenarioAction original
            var scenarioActionsOriginal =
                (await _unitOfWork.ScenarioActionRepository.GetWhereAsync(x => x.ScenarioId == scenario.ScenarioId,
                    cancellationToken)).ToList();

            //Get ScenarioActionDetail original
            var scenarioActionDetailsOriginal = await (
                from s in _unitOfWork.ScenarioRepository.GetAllQuery()
                join sa in _unitOfWork.ScenarioActionRepository.GetAllQuery()
                    on s.ScenarioId equals sa.ScenarioId
                join sad in _unitOfWork.ScenarioActionDetailRepository.GetAllQuery()
                    on sa.Id equals sad.ActionId
                where s.ScenarioId == request.ScenarioId
                select sad
            ).ToListAsync(cancellationToken: cancellationToken);

            //Insert new ScenarioAction & ScenarioActionDetail
            if (request.ScenarioActions?.Any() == true)
            {
                foreach (var scenarioActionDto in request.ScenarioActions)
                {
                    var scenarioAction = new ScenarioAction()
                    {
                        Id = Guid.NewGuid(),
                        ScenarioId = scenario.ScenarioId,
                        ActionTypeId = scenarioActionDto.ActionTypeId,
                        AreaTypeId = scenarioActionDto.AreaTypeId,
                        Data = scenarioActionDto.Data,
                        Duration = scenarioActionDto.Duration,
                        MethodId = scenarioActionDto.MethodId
                    };

                    if (scenarioActionDto.ScenarioActionDetails?.Any() == true)
                    {
                        foreach (var scenarioActionDetailDto in scenarioActionDto.ScenarioActionDetails)
                        {
                            var scenarioActionDetail = new ScenarioActionDetail()
                            {
                                MethodId = scenarioActionDetailDto.MethodId,
                                Id = Guid.NewGuid(),
                                ActionId = scenarioAction.Id,
                                ActionTypeId = scenarioActionDetailDto.ActionTypeId,
                                Content = scenarioActionDetailDto.Content,
                                CustomPosition = scenarioActionDetailDto.CustomPosition,
                                Duration = scenarioActionDetailDto.Duration,
                                IsDisplay = scenarioActionDetailDto.IsDisplay,
                                IsProvince = scenarioActionDetailDto.IsProvince,
                                Left = scenarioActionDetailDto.Left,
                                PlaceId = scenarioActionDetailDto.PlaceId,
                                PositionId = scenarioActionDetailDto.PositionId,
                                ScenarioActionTypeId = scenarioActionDetailDto.ScenarioActionTypeId,
                                StartTime = scenarioActionDetailDto.StartTime,
                                Time = scenarioActionDetailDto.Time,
                                Top = scenarioActionDetailDto.Top,
                                Width = scenarioActionDetailDto.Width
                            };

                            if (scenarioActionDetailDto.IconsList?.Any() == true)
                            {
                                var iconsResult =
                                    await _imageService.CopyFileToStorageContainerAsync(
                                        scenarioActionDetailDto.IconsList
                                        , scenarioAction.Id.ToString(), Forder.IconImage,
                                        StorageContainer.Scenarios);
                                scenarioActionDetail.IconUrls = string.Join(Constants.SemiColonStringSeparator,
                                    iconsResult);
                            }

                            _unitOfWork.ScenarioActionDetailRepository.Add(scenarioActionDetail);
                        }
                    }

                    _unitOfWork.ScenarioActionRepository.Add(scenarioAction);
                }
            }

            //Delete scenarioActionDetailsOriginal
            if (scenarioActionDetailsOriginal.Any())
            {
                //Delete icons
                foreach (var scenarioActionDetail in scenarioActionDetailsOriginal.Where(x => !string.IsNullOrEmpty(x.IconUrls))
                )
                {
                    var images = scenarioActionDetail.IconUrls
                        .Split(Constants.SemiColonStringSeparator, StringSplitOptions.RemoveEmptyEntries).ToList();
                    await _imageService.DeleteFileInStorageContainerByNameAsync(
                        scenarioActionDetail.ActionId.ToString(), images, StorageContainer.Scenarios);
                }

                _unitOfWork.ScenarioActionDetailRepository.DeleteRange(scenarioActionDetailsOriginal);
            }

            //Delete scenarioActionsOriginal
            if (scenarioActionsOriginal?.Any() == true)
            {
                _unitOfWork.ScenarioActionRepository.DeleteRange(scenarioActionsOriginal);
            }
        }

        private void UpdateScenario(UpdateScenarioActionCommand request, Scenario scenario)
        {
            scenario.ScenarioName = request.ScenarioName;
            _unitOfWork.ScenarioRepository.Update(scenario);
        }

        public async Task<bool> DeleteAsync(DeleteScenarioActionCommand request, CancellationToken cancellationToken)
        {
            var scenario = await _unitOfWork.ScenarioRepository.GetByIdAsync(request.ScenarioId);

            if (scenario == null)
            {
                throw new NotFoundException(nameof(Scenario), request.ScenarioId);
            }

            //Get ScenarioAction original
            var scenarioActions =
                (await _unitOfWork.ScenarioActionRepository.GetWhereAsync(x => x.ScenarioId == scenario.ScenarioId,
                    cancellationToken)).ToList();

            //Get ScenarioActionDetail original
            var scenarioActionDetails = await (
                from s in _unitOfWork.ScenarioRepository.GetAllQuery()
                join sa in _unitOfWork.ScenarioActionRepository.GetAllQuery()
                    on s.ScenarioId equals sa.ScenarioId
                join sad in _unitOfWork.ScenarioActionDetailRepository.GetAllQuery()
                    on sa.Id equals sad.ActionId
                where s.ScenarioId == request.ScenarioId
                select sad
            ).ToListAsync(cancellationToken: cancellationToken);

            if (scenarioActionDetails.Any())
            {
                //Delete icons
                foreach (var scenarioActionDetail in scenarioActionDetails.Where(x => !string.IsNullOrEmpty(x.IconUrls))
                )
                {
                    var images = scenarioActionDetail.IconUrls
                        .Split(Constants.SemiColonStringSeparator, StringSplitOptions.RemoveEmptyEntries).ToList();
                    await _imageService.DeleteFileInStorageContainerByNameAsync(
                        scenarioActionDetail.ActionId.ToString(), images, StorageContainer.Scenarios);
                }
                //Delete scenarioActionDetails
                _unitOfWork.ScenarioActionDetailRepository.DeleteRange(scenarioActionDetails);
            }

            //Delete scenarioActions
            if (scenarioActions.Any())
            {
                _unitOfWork.ScenarioActionRepository.DeleteRange(scenarioActions);
            }

            //Delete scenario
            _unitOfWork.ScenarioRepository.Delete(scenario);

            return await _unitOfWork.CommitAsync() > 0;
        }
    }
}
