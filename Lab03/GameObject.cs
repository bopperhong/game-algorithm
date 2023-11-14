using Microsoft.Xna.Framework;
using System;

namespace Lab03
{
    public abstract class GameObject
    {
        protected static Game1 _game;

        // Identification and Grouping Data
        public string Name
        {
            get; private set;
        }

        private string _tag = string.Empty;
        public string Tag
        {
            get { return _tag; }
            set
            {
                string valueLowercase = value.ToLower();
                if (GameObjectCollection.ChangeTag(this, valueLowercase))
                {
                    _tag = valueLowercase;
                }
            }
        }

        // Transformation Data
        public Vector2 Origin    = Vector2.Zero;
        public Vector2 Position  = Vector2.Zero;
        public float Orientation = 0f;
        public Vector2 Scale     = Vector2.One;

        public static void SetGame(Game1 game)
        {
            if (_game == null)
            {
                _game = game;
            }
        }

        private GameObject()
        {
            // Provide a default name based on object's type name and count
            string name = $"{this.GetType().Name}_{GameObjectCollection.Count}";
            AddObjectToCollection(name);
        }

        protected GameObject(string name)
        {
            AddObjectToCollection(name);
        }

        private void AddObjectToCollection(string name)
        {
            Name = name;
            if (!GameObjectCollection.Add(this))
            {
                throw new ArgumentException($"Attempt to create duplicate object named '{name}'");
            }
            else
            {
                Name = name;
            }
        }

        public virtual void Initialize() { }

        protected virtual void LoadContent() { }

        public virtual void Update() { }

        public virtual void Draw() { }
    }
}
