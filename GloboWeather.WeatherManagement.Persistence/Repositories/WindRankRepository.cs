using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class WindRankRepository : BaseRepository<WindRank>, IWindRankRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public WindRankRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> DownloadDataAsync(List<WindRank> windRanks)
        {

            var existingWindRanks = await _unitOfWork.WindRankRepository.GetAllAsync();

            //Delete
            var listIds = windRanks.Select(x => x.Id);
            var deleteItems = existingWindRanks.Where(x => !listIds.Contains(x.Id));
            if (deleteItems?.Count() > 0)
            {
                _unitOfWork.WindRankRepository.DeleteRange(deleteItems);
            }

            foreach (var windRank in windRanks)
            {
                var existingWindRank = existingWindRanks.FirstOrDefault(x => x.Id == windRank.Id);

                if (existingWindRank != null) //Update
                {
                    if (!existingWindRank.Color.Equals(windRank.Color))
                        existingWindRank.Color = windRank.Color;
                    if (!existingWindRank.Description.Equals(windRank.Description))
                        existingWindRank.Description = windRank.Description;
                    if (!existingWindRank.Name.Equals(windRank.Name))
                        existingWindRank.Name = windRank.Name;
                    if (!existingWindRank.Wave.Equals(windRank.Wave))
                        existingWindRank.Wave = windRank.Wave;
                    if (!existingWindRank.WindSpeed.Equals(windRank.WindSpeed))
                        existingWindRank.WindSpeed = windRank.WindSpeed;
                    _unitOfWork.WindRankRepository.Update(existingWindRank);
                }
                else //Insert
                {
                    _unitOfWork.WindRankRepository.Add(windRank);
                }
            }

            return await _unitOfWork.CommitAsync();
        }
    }
}