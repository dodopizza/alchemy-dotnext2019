using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Models;
using UnityEngine;

namespace RatingScreen
{
	public class RatingManager : MonoBehaviour
	{
		public GameObject canvas;
		public GameObject ratingPrefab;
		public GameObject threeDotsPrefab;
		public GameObject underUpperLayer;

		private GameObject _somethingWrongWindowPrefab;

		private void Start()
		{
			_somethingWrongWindowPrefab = (GameObject) Resources.Load("Prefabs/SomethingWrongWindow", typeof(GameObject));

			var ratingFetcher = new NetworkRatingFetcher();

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
					Instantiate(_somethingWrongWindowPrefab, underUpperLayer.transform);
				}

				var topRatingResult = topRatingTask.Result;
				var myRatingResult = myRatingTask.Result;

				if (!topRatingResult.IsSuccess || !myRatingResult.IsSuccess)
				{
					Instantiate(_somethingWrongWindowPrefab, underUpperLayer.transform);
				}

				RenderRatings(topRatingResult.Data, myRatingResult.Data);
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		private void RenderRatings(IEnumerable<RatingEntry> top, RatingEntry own)
		{
			// re-sort just in case
			var orderedRatings = top.OrderBy(x => x.Position).ToArray();

			var lastPositionOfTop = orderedRatings.Last().Position;

			if (own.Position <= lastPositionOfTop)
			{
				// player is in top: just render top
				RenderPlayerInTop(own, orderedRatings);
			}
			else if (own.Position == lastPositionOfTop + 1)
			{
				// player is right after the top: render player after top
				RenderPlayerRightAfterTop(own, orderedRatings);
			}
			else
			{
				// player is way outside of top: render three dots and player after
				RenderPlayerWayOutOfTop(own, orderedRatings);
			}
		}

		private void RenderPlayerWayOutOfTop(RatingEntry own, IEnumerable<RatingEntry> orderedRatings)
		{
			foreach (var entry in orderedRatings)
			{
				Instantiate(ratingPrefab, canvas.transform).GetComponent<RatingEntryElement>().Setup(entry, false);
			}

			Instantiate(threeDotsPrefab, canvas.transform);

			Instantiate(ratingPrefab, canvas.transform).GetComponent<RatingEntryElement>().Setup(own, true);
		}

		private void RenderPlayerRightAfterTop(RatingEntry own, IEnumerable<RatingEntry> orderedRatings)
		{
			foreach (var entry in orderedRatings)
			{
				Instantiate(ratingPrefab, canvas.transform).GetComponent<RatingEntryElement>().Setup(entry, false);
			}

			Instantiate(ratingPrefab, canvas.transform).GetComponent<RatingEntryElement>().Setup(own, true);
		}

		private void RenderPlayerInTop(RatingEntry own, IEnumerable<RatingEntry> orderedRatings)
		{
			foreach (var entry in orderedRatings)
			{
				Instantiate(ratingPrefab, canvas.transform).GetComponent<RatingEntryElement>()
					.Setup(entry, entry.Position == own.Position);
			}
		}
	}
}