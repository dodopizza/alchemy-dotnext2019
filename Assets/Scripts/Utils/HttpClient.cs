using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Utils
{
	public static class HttpClient
	{
		public static async Task<TResult> GetWithRetries<TResult>(string url, int retryCount = 3)
		{
			return await RequestWithRetry<TResult>(UnityWebRequest.kHttpVerbGET, url, null, retryCount);
		}

		public static async Task<TResult> PostWithRetry<TResult>(string url, object body = null, int retryCount = 3)
		{
			return await RequestWithRetry<TResult>(UnityWebRequest.kHttpVerbPOST, url, body, retryCount);
		}

		public static async Task PostWithRetry(string url, object body = null, int retryCount = 3)
		{
			await RequestWithRetry(UnityWebRequest.kHttpVerbPOST, url, body, retryCount);
		}

		private static async Task RequestWithRetry(string verb, string url,
			object body = null, int retryCount = 3)
		{
			await ExecuteRequestWithRetries(verb, url, body, retryCount);
		}

		private static async Task<TResult> RequestWithRetry<TResult>(string verb, string url,
			object body = null, int retryCount = 3)
		{
			var responseText = await ExecuteRequestWithRetries(verb, url, body, retryCount);
			return JsonUtility.FromJson<TResult>(responseText);
		}

		private static async Task<string> ExecuteRequestWithRetries(string verb, string url, object body,
			int retryCount)
		{
			UnityWebRequest request = null;
			for (var i = 0; i < retryCount; i++)
			{
				using (request = CreateApiRequest(url, verb, body))
				{
					request.timeout = Constants.RpcTimeoutSeconds;

					await request.SendWebRequest();

					if (!request.isHttpError && !request.isNetworkError)
						return request.downloadHandler.text;
				}

				Debug.Log($"Error requesting data from url '{url}'. Attempt number: '{i}'.");
				await Task.Delay(1000);
			}

			throw new Exception(
				$"Failed to perform request to address {url}. Retries: {retryCount}. Last error:\n{request?.error}.");
		}

		private static UnityWebRequest CreateApiRequest(string url, string method, object body)
		{
			string bodyString = null;

			if (body is string s)
			{
				bodyString = s;
			}
			else if (body != null)
			{
				bodyString = JsonUtility.ToJson(body);
			}

			var request = new UnityWebRequest
			{
				url = url,
				method = method,
				downloadHandler = new DownloadHandlerBuffer(),
				uploadHandler =
					new UploadHandlerRaw(string.IsNullOrEmpty(bodyString) ? null : Encoding.UTF8.GetBytes(bodyString))
			};

			request.SetRequestHeader("Accept", "application/json");
			request.SetRequestHeader("Content-Type", "application/json");
			return request;
		}
	}
}