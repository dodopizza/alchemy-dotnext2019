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
		async Task Start()
		{
			var ratingFetcher = new DummyRatingFetcher();
			
			try
			{
				var result = await ratingFetcher.GetRating();

				if (!result.IsSuccess)
				{
					// TODO error
				}

				RenderRatings(result.Data.Top);
			}
			catch (Exception e)
			{
				// TODO error
			}
		}

		void RenderRatings(RatingEntry[] ratings)
		{
			foreach (var entry in ratings)
			{
				Instantiate(ratingPrefab, canvas.transform).GetComponent<Text>().text =
					$"{entry.Nickname}: {entry.Rating}";
			}
		}

		// Update is called once per frame
		void Update()
		{
		}
	}
}