using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using UnityEngine;

namespace Domain.Local
{
    public class CsvWorker : Interfaces.IMixChecker, Interfaces.IBaseElementsLoader
    {
        private IEnumerable<Element> _elements;
        private readonly List<Element> _baseElements = new List<Element>();
        private Dictionary<(Guid, Guid), Element> _links;
        public CsvWorker()
        {
            var elementsText = (TextAsset) Resources.Load("elements");
            var linksText = (TextAsset) Resources.Load("links");

            _elements = elementsText.text.Split('\n')
                .Select(line =>
                {
                    var a = line.Split(';');
                    var element = new Element(
                        Guid.Parse(a[0]),
                        a[1],
                        a[2],
                        int.Parse(a[3]),
                        a[5]);

                    if (int.Parse(a[4]) == 1)
                        _baseElements.Add(element);
                    return element;
                }).ToList();


            _links = linksText.text.Split('\n')
                .Select(line =>
                {
                    var a = line.Split(';');
                    var id1 = _elements.First(e => e.SpriteName == a[0]).Id;
                    var id2 = _elements.First(e => e.SpriteName == a[1]).Id;
                    var result = _elements.First(e => e.SpriteName == a[2]);
                    return (key: (id1, id2), value: result);
                }).ToDictionary(pair => pair.key, key => key.value);
        }
        
        public Task<OperationResult<CheckResult>> Check(Guid firstElementId, Guid secondElementId)
        {
            var key1 = (firstElementId, secondElementId);
            var key2 = (secondElementId, firstElementId);

            if (_links.TryGetValue(key1, out var result1) | _links.TryGetValue(key2, out var result2))
            {
                var element = result1 ?? result2;
                return Task.FromResult(OperationResult<CheckResult>.Success(CheckResult.Success(
                    element.Id.ToString(),
                    element.SpriteName, element.Name, element.Score, element.Description)));
            }

            return Task.FromResult(OperationResult<CheckResult>.Success(CheckResult.Failure()));
        }

        public Task<OperationResult<IEnumerable<Element>>> GetBaseElements()
        {
            return Task.FromResult(OperationResult<IEnumerable<Element>>.Success(_baseElements));
        }
    }
}