using System;
using UnityEngine;

namespace Domain.Models
{
    public class Element
    {
        public Guid Id { get; }
        
        public Sprite Sprite { get; }
        
        public string Name { get; }
        
        public int Scores { get; }
        
        public string Description { get; }
        
        public Element(Guid id, string spriteName, string name, int scores, string description)
        {
            Id = id;
            Sprite = (Sprite) Resources.Load($"Sprites/Elements/{spriteName}", typeof(Sprite));
            Name = name;
            Scores = scores;
            Description = description;
        }
    }
}