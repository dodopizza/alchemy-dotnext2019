using System;
using System.Collections.Generic;
using System.Linq;
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
		public GameObject threeDotsPrefab;

		// Start is called before the first frame update
		void Start()
		{
			var ratingFetcher = new DummyRatingFetcher();

			var topRatingTask = ratingFetcher.GetTopRating();
			var myRatingTask = ratingFetcher.GetMyRating();

			Task.WhenAll(
				topRatingTask,
				myRatingTask
			).ContinueWith(_ =>
			{
				if (topRatingTask.IsCanceled
				    || topRatingTask.IsFaulted
				    || myRatingTask.IsCanceled
				    || myRatingTask.IsFaulted)
				{
					// TODO error
				}

				var topRatingResult = topRatingTask.Result;
				var myRatingResult = myRatingTask.Result;

				if (!topRatingResult.IsSuccess || !myRatingResult.IsSuccess)
				{
					// TODO error
				}

				RenderRatings(topRatingResult.Data, myRatingResult.Data);
			}, TaskScheduler.FromCurrentSynchronizationContext());
			
			ratingFetcher.GetTopRating()
				.ContinueWith(task =>
				{
					
				}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		void RenderRatings(Domain.Models.RatingEntry[] top, Domain.Models.RatingEntry own)
		{
			// re-sort just in case
			var orderedRatings = top.OrderBy(x => x.Position).ToArray();

			var lastPositionOfTop = orderedRatings.Last().Position;

			if (own.Position <= lastPositionOfTop)
			{
				// player is in top: just render top
				foreach (var entry in orderedRatings)
				{
					Instantiate(ratingPrefab, canvas.transform).GetComponent<RatingEntry>().Setup(entry, entry.Position == own.Position);
				}

				return;
			}
			
			if (own.Position == lastPositionOfTop + 1)
			{
				// player is right after the top: render player after top
				foreach (var entry in orderedRatings)
				{
					Instantiate(ratingPrefab, canvas.transform).GetComponent<RatingEntry>().Setup(entry, false);
				}
				
				Instantiate(ratingPrefab, canvas.transform).GetComponent<RatingEntry>().Setup(own, true);
				
				return;
			}
				
			// player is way outside of top: render three dots and player after
			
			foreach (var entry in orderedRatings)
			{
				Instantiate(ratingPrefab, canvas.transform).GetComponent<RatingEntry>().Setup(entry, false);
			}

			Instantiate(threeDotsPrefab, canvas.transform);
				
			Instantiate(ratingPrefab, canvas.transform).GetComponent<RatingEntry>().Setup(own, true);
		}


		// Update is called once per frame
		void Update()
		{
		}
	}
}