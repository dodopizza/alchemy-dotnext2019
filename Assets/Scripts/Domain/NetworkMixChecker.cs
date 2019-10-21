using System;
using System.Threading.Tasks;
using Domain.Models;
using UnityEngine;
using Utils;

namespace Domain
{
	public class NetworkMixChecker : Interfaces.IMixChecker
	{
		private readonly string _userId;

		public NetworkMixChecker()
		{
			_userId = PlayerPrefs.GetString(Constants.UserIdKey);
		}

		public async Task<OperationResult<CheckResult>> Check(Guid firstElementId, Guid secondElementId)
		{
			var checkRequest = new CheckRequest
			{
				FirstElement = firstElementId.ToString(),
				SecondElement = secondElementId.ToString(),
				UserId = _userId
			};


			InternalCheckResult intermediateResult;
			try
			{
				var url = Constants.ApiUrl + "/api/Elements/merge";
				intermediateResult = await HttpClient.PostWithRetry<InternalCheckResult>(url, checkRequest);
			}
			catch
			{
				return OperationResult<CheckResult>.Failure();
			}

			if (intermediateResult.isSuccess)
			{
				return OperationResult<CheckResult>.Success(
					CheckResult.Success(
						intermediateResult.mergeResultElement.id,
						intermediateResult.mergeResultElement.imageName,
						intermediateResult.mergeResultElement.name,
						intermediateResult.mergeResultElement.score,
						intermediateResult.mergeResultElement.description));
			}

			return OperationResult<CheckResult>.Success(CheckResult.Failure());
		}

		private class CheckRequest
		{
			public string FirstElement;

			public string SecondElement;

			public string UserId;
		}

		[Serializable]
		private class InternalCheckResult
		{
			public bool isSuccess;

			public MergeResultElement mergeResultElement;
		}

		[Serializable]
		private class MergeResultElement
		{
			public string id;
			public string imageName;
			public int score;
			public string name;
			public string description;
			public bool isBaseElement;
		}
	}
}