using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using UnityEngine;
using Utils;

namespace Domain
{
    public static class BaseElementsLoader
    {
        public static async Task<OperationResult<IEnumerable<Element>>> GetBaseElements()
        {
            BaseElements intermediateResult = null;
            try
            {
                var url = Constants.ApiUrl + "/api/Elements/base";
                intermediateResult = await HttpClient.GetWithRetries<BaseElements>(url, 5);
            }
            catch
            {
                return OperationResult<IEnumerable<Element>>.Failure();
            }
            
            var elements = intermediateResult.elements.Select(r =>
                new Element(new Guid(r.id), r.imageName, r.name, r.score, r.description));
            return OperationResult<IEnumerable<Element>>.Success(elements);
        }

        [Serializable]
        private class BaseElements
        {
            public BaseElement[] elements;
        }
        
        [Serializable]
        private class BaseElement
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