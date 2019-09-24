using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Workbench
{
    public class NetworkElementMixer : IElementMixer
    {
        public Task<MixResult> MixElements(IEnumerable<ElementData> elementsData)
        {
            var taskCompletionSource = new TaskCompletionSource<MixResult>();

            var mixRequest = new MixRequest
            {
                elementIds = elementsData.Select(ed => ed.id).ToArray()
            };
            var requestBody = JsonUtility.ToJson(mixRequest);
            
            var request = UnityWebRequest.Put(Constants.ApiUrl + "api/workbench", requestBody);
            request.SetRequestHeader("Content-Type", "application/json");
            request.timeout = Constants.RpcTimeoutSeconds;
            var operation = request.SendWebRequest();
            operation.completed += asyncOperation =>
            {
                try
                {
                    // TODO Fail fast on 4xx/5xx responses
                    var response = JsonUtility.FromJson<MixResult>(request.downloadHandler.text);
                    taskCompletionSource.TrySetResult(response);
                }
                catch (Exception e)
                {
                    taskCompletionSource.TrySetException(e);
                }
            };

            return taskCompletionSource.Task;
        }
    }

    public class MixRequest
    {
        public int[] elementIds;
    }
}