using System;
using UnityEngine;

namespace Domain.Models
{
    public class Element
    {
        public Guid Id { get; }
        
        public Sprite Sprite { get; }
        
        public string Name { get; }
        
        public Element(Guid id, Sprite sprite, string name)
        {
            Id = id;
            Sprite = sprite;
            Name = name;
        }
    }
}