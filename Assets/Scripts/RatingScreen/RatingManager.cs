using Domain;
using Domain.Models;
using UnityEngine;

namespace RatingScreen
{
    public class RatingManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var ratingFetcher = new DummyRatingFetcher();

            ratingFetcher.GetRating()
                .ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        // TODO error
                    }

                    if (task.IsFaulted)
                    {
                        // TODO error
                    }
                    
                    var result = task.Result;

                    if (!result.IsSuccess)
                    {
                        // TODO error
                    }
                    
                    RenderRatings(task.Result.Data.Top);
                });
        }

        void RenderRatings(RatingEntry[] ratings)
        {
            foreach (var entry in ratings)
            {
                Debug.Log($"{entry.Nickname}: {entry.Rating}");
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
