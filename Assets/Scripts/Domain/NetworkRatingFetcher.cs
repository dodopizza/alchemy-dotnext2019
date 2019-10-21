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
            using (var request = HttpClient.CreateApiGetRequest(Constants.ApiUrl + "/api/Stats/ladder/20"))
            {
                request.timeout = Constants.RpcTimeoutSeconds;
                
                await request.SendWebRequest();

                if (!request.isHttpError && !request.isNetworkError)
                {
                    var intermediateResult = JsonUtility.FromJson<Players>(request.downloadHandler.text);
                    var ratingEntries = intermediateResult.players.Select(r =>
                        new RatingEntry(r.name, r.score, r.elementCount, r.place)).ToArray();
                    return OperationResult<RatingEntry[]>.Success(ratingEntries);
                }

                return OperationResult<RatingEntry[]>.Failure();
            }
        }

        public async Task<OperationResult<RatingEntry>> GetMyRating()
        {
            using (var request = HttpClient.CreateApiGetRequest(Constants.ApiUrl + $"/api/UserProfile/get/{_userId}"))
            {
                request.timeout = Constants.RpcTimeoutSeconds;
                
                await request.SendWebRequest();

                if (!request.isHttpError && !request.isNetworkError)
                {
                    var result = JsonUtility.FromJson<Player>(request.downloadHandler.text);

                    return OperationResult<RatingEntry>.Success(new RatingEntry(
                        result.name,
                        result.score,
                        result.elementCount,
                        result.place));
                }

                return OperationResult<RatingEntry>.Failure();
            }
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