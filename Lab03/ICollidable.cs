using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Lab03
{
    public interface ICollidable
    {
        // Methods
        public virtual void OnCollision(CollisionInfo collisionInfo) { }

        public string GetName();

        public Rectangle GetBound();
    }
}
