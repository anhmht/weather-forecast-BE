using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.CreateScenarioAction;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.DeleteScenario;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.DeleteScenarioAction;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.UpdateActionOrder;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.UpdateScenarioAction;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenarioActionDetail;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenarioDetail;
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
        private readonly IMapper _mapper;

        public ScenarioService(IUnitOfWork unitOfWork, ICommonService commonService, IImageService imageService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _commonService = commonService;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<ScenarioDetailVm> GetScenarioDetailAsync(GetScenarioDetailQuery request)
        {
            var response = new ScenarioDetailVm();

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
                            ScenarioActionOrder = sa.Order,
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

            var scenarioDetail = await query.OrderBy(x => x.ScenarioActionOrder).ToListAsync();

            var actionAreaType = await _commonService.GetCommonLookupByNameSpaceAsync(LookupNameSpace.ActionAreaType);
            var actionMethod = await _commonService.GetCommonLookupByNameSpaceAsync(LookupNameSpace.ActionMethod);
            var actionType = await _commonService.GetCommonLookupByNameSpaceAsync(LookupNameSpace.ActionType);
            var position = await _commonService.GetCommonLookupByNameSpaceAsync(LookupNameSpace.Position);
            var scenarioActionType = await _commonService.GetCommonLookupByNameSpaceAsync(LookupNameSpace.ScenarioActionType);

            response.ScenarioId = scenarioDetail.First().ScenarioId;
            response.ScenarioName = scenarioDetail.First().ScenarioName;
            response.ScenarioContent = scenarioDetail.First().ScenarioContent;

            var scenarioActionDetails = scenarioDetail.Where(x => !x.ScenarioActionDetailId.Equals(Guid.Empty)).Select(x => new ScenarioActionDetailDto()
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
                Position = position.FirstOrDefault(t => t.ValueId == x.ScenarioActionDetailPositionId)?.ValueText,
                ScenarioActionTypeName = scenarioActionType.FirstOrDefault(t => t.ValueId == x.ScenarioActionDetailScenarioActionTypeId)?.ValueText,
                PlaceId = x.ScenarioActionDetailPlaceId,
                IsProvince = x.ScenarioActionDetailIsProvince
            }).Distinct().ToList();

            foreach (var detail in scenarioDetail)
            {
                if (detail.ScenarioActionId.Equals(Guid.Empty))
                    continue;
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
                        Order = detail.ScenarioActionOrder,
                        Action = actionType.FirstOrDefault(t => t.ValueId == detail.ScenarioActionActionTypeId)?.ValueText,
                        Method = actionMethod.FirstOrDefault(t => t.ValueId == detail.ScenarioActionMethodId)?.ValueText,
                        AreaTypeName = actionAreaType.FirstOrDefault(t => t.ValueId == detail.ScenarioActionAreaTypeId)?.ValueText,
                        ScenarioActionDetails = scenarioActionDetails?.FindAll(d => d.ActionId == detail.ScenarioActionId)
                    });
                }
            }

            return response;
        }

        public async Task<ScenarioActionDetailVm> GetScenarioActionDetailAsync(GetScenarioActionDetailQuery request)
        {
            var scenarioAction = await _unitOfWork.ScenarioActionRepository.GetByIdAsync(request.Id);

            if (scenarioAction == null)
            {
                throw new NotFoundException(nameof(ScenarioAction), request.Id);
            }

            var actionAreaType = await _commonService.GetCommonLookupByNameSpaceAsync(LookupNameSpace.ActionAreaType);
            var actionMethod = await _commonService.GetCommonLookupByNameSpaceAsync(LookupNameSpace.ActionMethod);
            var actionType = await _commonService.GetCommonLookupByNameSpaceAsync(LookupNameSpace.ActionType);
            var position = await _commonService.GetCommonLookupByNameSpaceAsync(LookupNameSpace.Position);
            var scenarioActionType = await _commonService.GetCommonLookupByNameSpaceAsync(LookupNameSpace.ScenarioActionType);


            var response = _mapper.Map<ScenarioActionDetailVm>(scenarioAction);

            response.Action = actionType.Find(x => x.ValueId == scenarioAction.ActionTypeId)?.ValueText;
            response.AreaTypeName = actionAreaType.Find(x => x.ValueId == scenarioAction.AreaTypeId)?.ValueText;
            response.Method = actionMethod.Find(x => x.ValueId == scenarioAction.MethodId)?.ValueText;

            var scenarioActionDetails =
                (await _unitOfWork.ScenarioActionDetailRepository.GetWhereAsync(x => x.ActionId == scenarioAction.Id))
                .ToList();

            if (scenarioActionDetails.Any())
            {
                response.ScenarioActionDetails = _mapper.Map<List<ScenarioActionDetailDto>>(scenarioActionDetails);
                foreach (var scenarioActionDetailDto in response.ScenarioActionDetails)
                {
                    var scenarioActionDetail = scenarioActionDetails.Find(x => x.Id == scenarioActionDetailDto.Id);
                    if (scenarioActionDetail != null)
                    {
                        if (!string.IsNullOrEmpty(scenarioActionDetail.IconUrls))
                        {
                            scenarioActionDetailDto.IconsList = scenarioActionDetail.IconUrls
                                .Split(Constants.SemiColonStringSeparator, StringSplitOptions.RemoveEmptyEntries)
                                .ToList();
                        }
                    }
                    scenarioActionDetailDto.Action = actionType.Find(x => x.ValueId == scenarioActionDetailDto.ActionTypeId)?.ValueText;
                    scenarioActionDetailDto.ScenarioActionTypeName = scenarioActionType
                        .Find(x => x.ValueId == scenarioActionDetailDto.ScenarioActionTypeId)?.ValueText;
                    scenarioActionDetailDto.Position = position.Find(x => x.ValueId == scenarioActionDetailDto.PositionId)?.ValueText;
                    scenarioActionDetailDto.Method = actionMethod.Find(x => x.ValueId == scenarioActionDetailDto.MethodId)?.ValueText;
                }
            }

            return response;
        }

        public async Task<Guid> CreateScenarioActionAsync(CreateScenarioActionCommand request,
            CancellationToken cancellationToken)
        {
            var scenario = await _unitOfWork.ScenarioRepository.GetByIdAsync(request.ScenarioId);
            if (scenario == null)
            {
                throw new NotFoundException(nameof(Scenario), request.ScenarioId);
            }

            //Add ScenarioAction
            var scenarioAction = _mapper.Map<ScenarioAction>(request);
            scenarioAction.Id = Guid.NewGuid();
            _unitOfWork.ScenarioActionRepository.Add(scenarioAction);

            //Add ScenarioActionDetails
            if (request.ScenarioActionDetails?.Any() == true)
            {
                //copy icons to storage
                foreach (var createScenarioActionDetailDto in request.ScenarioActionDetails)
                {
                    if (createScenarioActionDetailDto.IconsList?.Any() == true)
                    {
                        createScenarioActionDetailDto.IconUrls =
                            await PopulateScenarioActionDetailIconUrls(createScenarioActionDetailDto.IconsList,
                                scenarioAction);
                    }
                }

                //Populate for ScenarioActionDetail
                var scenarioActionDetails = _mapper.Map<List<ScenarioActionDetail>>(request.ScenarioActionDetails);
                PopulateScenarioActionDetailIds(scenarioActionDetails, scenarioAction);

                _unitOfWork.ScenarioActionDetailRepository.AddRange(scenarioActionDetails);
            }

            await _unitOfWork.CommitAsync();

            return scenarioAction.Id;
        }

        public async Task<Guid> UpdateScenarioActionAsync(UpdateScenarioActionCommand request, CancellationToken cancellationToken)
        {
            //Get existing scenario
            var scenarioAction = await _unitOfWork.ScenarioActionRepository.GetByIdAsync(request.Id);
            if (scenarioAction == null)
            {
                throw new NotFoundException(nameof(Scenario), request.Id);
            }

            #region Update ScenarioAction
            var isUpdate = false;
            if (request.Duration != scenarioAction.Duration)
            {
                scenarioAction.Duration = request.Duration;
                isUpdate = true;
            }
            if (request.ActionTypeId != scenarioAction.ActionTypeId)
            {
                scenarioAction.ActionTypeId = request.ActionTypeId;
                isUpdate = true;
            }
            if (request.MethodId != scenarioAction.MethodId)
            {
                scenarioAction.MethodId = request.MethodId;
                isUpdate = true;
            }
            if (request.AreaTypeId != scenarioAction.AreaTypeId)
            {
                scenarioAction.AreaTypeId = request.AreaTypeId;
                isUpdate = true;
            }
            if (request.Data != scenarioAction.Data)
            {
                scenarioAction.Data = request.Data;
                isUpdate = true;
            }

            if (isUpdate)
            {
                _unitOfWork.ScenarioActionRepository.Update(scenarioAction);
            }
            #endregion

            #region Update ScenarioActionDetail
            //Get ScenarioActionDetail original
            var scenarioActionDetailsOriginal =
                (await _unitOfWork.ScenarioActionDetailRepository.GetWhereAsync(x => x.ActionId == scenarioAction.Id,
                    cancellationToken)).ToList();

            if (request.ScenarioActionDetails?.Any() == true)
            {
                //Populate IconUrls
                foreach (var scenarioActionDetailDto in request.ScenarioActionDetails)
                {
                    if (scenarioActionDetailDto.IconsList?.Any() == true)
                    {
                        scenarioActionDetailDto.IconUrls =
                            await PopulateScenarioActionDetailIconUrls(scenarioActionDetailDto.IconsList,
                                scenarioAction);
                    }
                }

                //Populate ScenarioActionDetail
                var scenarioActionDetails = _mapper.Map<List<ScenarioActionDetail>>(request.ScenarioActionDetails);
                PopulateScenarioActionDetailIds(scenarioActionDetails, scenarioAction);

                _unitOfWork.ScenarioActionDetailRepository.AddRange(scenarioActionDetails);
            }

            //Delete scenarioActionDetailsOriginal
            await DeleteScenarioActionDetail(scenarioActionDetailsOriginal);

            #endregion

            await _unitOfWork.CommitAsync();

            return scenarioAction.Id;
        }

        public async Task<bool> DeleteScenarioAsync(DeleteScenarioCommand request, CancellationToken cancellationToken)
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

            await DeleteScenarioActionDetail(scenarioActionDetails);

            //Delete scenarioActions
            if (scenarioActions.Any())
            {
                _unitOfWork.ScenarioActionRepository.DeleteRange(scenarioActions);
            }

            //Delete scenario
            _unitOfWork.ScenarioRepository.Delete(scenario);

            return await _unitOfWork.CommitAsync() > 0;
        }

        public async Task<bool> DeleteScenarioActionAsync(DeleteScenarioActionCommand request, CancellationToken cancellationToken)
        {
            var scenarioAction = await _unitOfWork.ScenarioActionRepository.GetByIdAsync(request.Id);
            if (scenarioAction == null)
            {
                throw new NotFoundException(nameof(ScenarioAction), request.Id);
            }

            //Delete ScenarioActionDetail
            var scenarioActionDetails =
                (await _unitOfWork.ScenarioActionDetailRepository.GetWhereAsync(x => x.ActionId == scenarioAction.Id,
                    cancellationToken)).ToList();
            await DeleteScenarioActionDetail(scenarioActionDetails);

            //Delete ScenarioAction
            _unitOfWork.ScenarioActionRepository.Delete(scenarioAction);

            return await _unitOfWork.CommitAsync() > 0;
        }

        public async Task<bool> UpdateActionOrderAsync(UpdateActionOrderCommand request, CancellationToken cancellationToken)
        {
            var actionIds = request.ActionOrders.Select(x => x.ActionId).ToList();
            var scenarioActions =
                (await _unitOfWork.ScenarioActionRepository.GetWhereAsync(x => actionIds.Contains(x.Id),
                    cancellationToken)).ToList();
            foreach (var action in scenarioActions)
            {
                var updateOrder = request.ActionOrders.Find(x => x.ActionId == action.Id);
                if (updateOrder != null && updateOrder.Order != action.Order)
                {
                    action.Order = updateOrder.Order;
                    _unitOfWork.ScenarioActionRepository.Update(action);
                }
            }

            return await _unitOfWork.CommitAsync() > 0;
        }

        #region Private functions
        private static void PopulateScenarioActionDetailIds(List<ScenarioActionDetail> scenarioActionDetails, ScenarioAction scenarioAction)
        {
            foreach (var scenarioActionDetail in scenarioActionDetails)
            {
                scenarioActionDetail.Id = Guid.NewGuid();
                scenarioActionDetail.ActionId = scenarioAction.Id;
            }
        }

        private async Task<string> PopulateScenarioActionDetailIconUrls(List<string> iconsList,
            ScenarioAction scenarioAction)
        {
            var iconsResult =
                await _imageService.CopyFileToStorageContainerAsync(
                    iconsList
                    , scenarioAction.Id.ToString(), Forder.IconImage,
                    StorageContainer.Scenarios);
            return string.Join(Constants.SemiColonStringSeparator,
                iconsResult);
        }

        private async Task DeleteScenarioActionDetail(List<ScenarioActionDetail> scenarioActionDetails)
        {
            if (scenarioActionDetails.Any())
            {
                //Delete icons
                foreach (var scenarioActionDetail in scenarioActionDetails.Where(x => !string.IsNullOrEmpty(x.IconUrls)))
                {
                    try
                    {
                        var images = scenarioActionDetail.IconUrls
                            .Split(Constants.SemiColonStringSeparator, StringSplitOptions.RemoveEmptyEntries).ToList();
                        await _imageService.DeleteFileInStorageContainerByNameAsync(
                            scenarioActionDetail.ActionId.ToString(), images, StorageContainer.Scenarios);
                    }
                    catch
                    {
                        // ignored
                    }
                }

                //Delete scenarioActionDetails
                _unitOfWork.ScenarioActionDetailRepository.DeleteRange(scenarioActionDetails);
            }
        }
        #endregion
    }
}
