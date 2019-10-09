using System;
using System.Threading.Tasks;
using Domain;
using Domain.Models;
using UnityEngine;
using UnityEngine.UI;

namespace RatingScreen
{
    public class RatingManager : MonoBehaviour
    {
        public GameObject canvas;
        public GameObject ratingPrefab;
        
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
                    
                    RenderRatings(result.Data.Top);
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        
        void RenderRatings(RatingEntry[] ratings)
        {
            foreach (var entry in ratings)
            {
                
                Debug.Log($"{entry.Nickname}: {entry.Rating}");

                try
                {
                    Instantiate(ratingPrefab, canvas.transform).GetComponent<Text>().text = $"{entry.Nickname}: {entry.Rating}";
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    throw;
                }
                
                
                
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
