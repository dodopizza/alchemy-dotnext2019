using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using UnityEngine;
using Utils;

namespace Domain
{
    public class NetworkRatingFetcher : IRatingFetcher
    {
        private readonly string _userId;

        public NetworkRatingFetcher()
        {
            _userId = PlayerPrefs.GetString(Constants.UserIdKey);
        }
        
        public async Task<OperationResult<RatingEntry[]>> GetTopRating()
        {
            Players intermediateResult = null;
            try
            {
                var url = Constants.ApiUrl + "/api/Stats/ladder/20";
                intermediateResult = await HttpClient.GetWithRetries<Players>(url);
            }
            catch
            {
                return OperationResult<RatingEntry[]>.Failure();
            }

            var ratingEntries = intermediateResult.players.Select(r =>
                new RatingEntry(r.name, r.score, r.elementCount, r.place)).ToArray();
            return OperationResult<RatingEntry[]>.Success(ratingEntries);
        }

        public async Task<OperationResult<RatingEntry>> GetMyRating()
        {
            Player result = null;
            try
            {
                var url = Constants.ApiUrl + $"/api/UserProfile/get/{_userId}";
                result = await HttpClient.GetWithRetries<Player>(url);
            }
            catch
            {
                return OperationResult<RatingEntry>.Failure();
            }

            return OperationResult<RatingEntry>.Success(new RatingEntry(
                result.name,
                result.score,
                result.elementCount,
                result.place));
        }
        
        [Serializable]
        private class Players
        {
            public Player[] players;
        }
        
        [Serializable]
        private class Player
        {
            public string name;
            public int score;
            public int place;
            public int elementCount;
        }
    }
}