using System;
using UnityEngine;

namespace Domain.Models
{
    public class Element
    {
        public Guid Id { get; }
        
        public Sprite Sprite { get; }
        
        public string Name { get; }
        
        public int Score { get; }
        
        public string Description { get; }
        
        public Element(Guid id, string spriteName, string name, int score, string description)
        {
            Id = id;
            Sprite = (Sprite) Resources.Load($"Sprites/Elements/{spriteName}", typeof(Sprite));
            // костыль, пока нет всех картинок
            if (Sprite == null)
                Sprite = (Sprite) Resources.Load($"Sprites/Elements/no_picture", typeof(Sprite));
            Name = name;
            Score = score;
            Description = description;
        }

        public static Element StartElement(Guid id, string spriteName, string name)
        {
            return new Element(id, spriteName, name, 0, null);
        }
    }
}