using System;
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
		public GameObject ratingPanel;
		public GameObject underUpperLayer;

		private GameObject _somethingWrongWindowPrefab;
		private GameObject _ratingElementPrefab;


		private void Start()
		{
			_somethingWrongWindowPrefab = (GameObject) Resources.Load("Prefabs/SomethingWrongWindow", 
				typeof(GameObject));
			_ratingElementPrefab = (GameObject) Resources.Load("Prefabs/RatingElement", typeof(GameObject));

			var ratingFetcher = new NetworkRatingFetcher();

			var topRatingTask = ratingFetcher.GetTopRating();
			var playerInfoTask = ratingFetcher.GetPlayerInfo();

			Task.WhenAll(
				topRatingTask,
				playerInfoTask
			).ContinueWith(_ =>
			{
				if (topRatingTask.IsCanceled || topRatingTask.IsFaulted || !topRatingTask.Result.IsSuccess)
				{
					Instantiate(_somethingWrongWindowPrefab, underUpperLayer.transform);
					return;
				}

				var topRatingResult = topRatingTask.Result;

				PlayerInfo playerInfo = null;
				if (! playerInfoTask.IsCanceled && !playerInfoTask.IsFaulted && playerInfoTask.Result.IsSuccess)
				{
					playerInfo = playerInfoTask.Result.Data;
				}

				RenderRatings(topRatingResult.Data, playerInfo);
			}, 
				TaskScheduler.FromCurrentSynchronizationContext());
		}

		private void RenderRatings(IEnumerable<RatingEntry> top, PlayerInfo playerInfo)
		{
			var orderedRatings = top.OrderBy(x => x.Position).ToArray();

			foreach (var ratingEntry in orderedRatings)
			{
				var isSelf = ratingEntry.Nickname == playerInfo?.Name
				            && ratingEntry.Score == playerInfo?.Score;
				try
				{
					Instantiate(_ratingElementPrefab, ratingPanel.transform)
						.GetComponent<RatingEntryElement>().Setup(ratingEntry, isSelf);
				}
				catch (Exception ex)
				{
					Debug.Log(ex);
				}
			}
		}
	}
}