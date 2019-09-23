using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace ElementsBook
{
    public class NetworkElementProvider : IElementProvider
    {
        public Task<List<ElementData>> GetElements()
        {
            var taskCompletionSource = new TaskCompletionSource<List<ElementData>>();

            var request = UnityWebRequest.Get(Constants.ApiUrl + "api/values");
            request.timeout = Constants.RpcTimeoutSeconds;
            var operation = request.SendWebRequest();
            operation.completed += asyncOperation =>
            {
                try
                {
                    // TODO Fail fast on 4xx/5xx responses
                    var elementData = JsonUtility.FromJson<List<ElementData>>(request.downloadHandler.text);
                    taskCompletionSource.TrySetResult(elementData);
                }
                catch (Exception e)
                {
                    taskCompletionSource.TrySetException(e);
                }
            };

            return taskCompletionSource.Task;
        }
    }
}