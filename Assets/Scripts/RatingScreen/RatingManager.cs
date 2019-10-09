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

		void RenderRatings(RatingEntry[] top, RatingEntry own)
		{
			// re-sort just in case
			var orderedRatings = top.OrderBy(x => x.Position).ToArray();

			var lastPositionOfTop = orderedRatings.Last().Position;

			if (own.Position <= lastPositionOfTop)
			{
				// player is in top: just render top
				foreach (var entry in orderedRatings)
				{
					Instantiate(ratingPrefab, canvas.transform).GetComponent<Text>().text =
						$"{entry.Position}. {(entry.Position == own.Position ? "You" : entry.Nickname)}: {entry.Rating}";
				}

				return;
			}
			
			if (own.Position == lastPositionOfTop + 1)
			{
				// player is right after the top: render player after top
				foreach (var entry in orderedRatings)
				{
					Instantiate(ratingPrefab, canvas.transform).GetComponent<Text>().text =
						$"{entry.Position}. {entry.Nickname}: {entry.Rating}";
				}
				
				Instantiate(ratingPrefab, canvas.transform).GetComponent<Text>().text = $"{own.Position}. You: {own.Rating}";
				
				return;
			}
				
			// player is way outside of top: render three dots and player after
			
			foreach (var entry in orderedRatings)
			{
				Instantiate(ratingPrefab, canvas.transform).GetComponent<Text>().text =
					$"{entry.Position}. {entry.Nickname}: {entry.Rating}";
			}
			Instantiate(ratingPrefab, canvas.transform).GetComponent<Text>().text = $"...";
			Instantiate(ratingPrefab, canvas.transform).GetComponent<Text>().text = $"{own.Position}. You: {own.Rating}";

			
		}


		// Update is called once per frame
		void Update()
		{
		}
	}
}