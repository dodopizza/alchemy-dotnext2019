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

        public static Element StartElement(Guid id, string spriteName, string name)
        {
            return new Element(id, spriteName, name, 0, null);
        }

        public SerializableElement ToSerializable()
        {
            return new SerializableElement
            {
                Id = Id.ToString(),
                Description = Description,
                Name = Name,
                Scores = Scores,
                Sprite = Sprite.name
            };
        }

        public static Element FromSerializable(SerializableElement element)
        {
            return new Element(new Guid(element.Id), element.Sprite, element.Name, element.Scores, element.Description);
        }
    }
}