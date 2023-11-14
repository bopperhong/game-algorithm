using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Lab03
{
    public class CollisionEngine
    {
        // Delegates & Events
        public delegate bool CollisionDetector(ICollidable thisCollidable, ICollidable thatCollidable);
        public delegate void CollisionHandler(ICollidable thisCollidable, ICollidable thatCollidable);

        // Structs
        private struct CollisionKey : IEquatable<CollisionKey>
        {
            public string FirstName;
            public string SecondName;

            public CollisionKey(string first, string second)
            {
                FirstName = first;
                SecondName = second;
            }

            public bool Equals(CollisionKey that)
            {
                // Prevents empty strings from being inserted
                if (FirstName.Length * SecondName.Length == 0)
                    return true;
                else
                    return (FirstName == that.FirstName &&
                            SecondName == that.SecondName ||
                            FirstName == that.SecondName &&
                            SecondName == that.FirstName);
            }

            public override int GetHashCode()
            {
                return FirstName.GetHashCode() ^ SecondName.GetHashCode();
            }

            public override string ToString()
            {
                return $"Collision Key[{FirstName}, {SecondName}]";
            }
        }

        // Fields and Properties
        public bool Enabled { get; set; }

        private LinkedList<ICollidable> _collidables;
        private HashSet<CollisionKey> _collisionKeySet;
        private Dictionary<CollisionKey, CollisionDetector> _collisionDetectors;
        private Dictionary<CollisionKey, CollisionHandler> _collisionHandlers;
        private Dictionary<string, HashSet<ICollidable>> _collidableLists;

        // Constructors and Methods
        public CollisionEngine()
        {
            Enabled = true;
            _collidables = new LinkedList<ICollidable>();
            _collisionKeySet = new HashSet<CollisionKey>();
            _collisionDetectors = new Dictionary<CollisionKey, CollisionDetector>();
            _collisionHandlers = new Dictionary<CollisionKey, CollisionHandler>();
            _collidableLists = new Dictionary<string, HashSet<ICollidable>>();
        }

        private void AddCollidable(CollisionKey collisionKey)
        {
            foreach (var (name, gameObject) in GameObjectCollection.Objects)
            {
                if (name == collisionKey.FirstName || name == collisionKey.SecondName)
                {
                    if (!_collidableLists.ContainsKey(name))
                    {
                        _collidableLists[name] = new HashSet<ICollidable>();
                    }
                    
                    _collidableLists[name].Add((ICollidable)gameObject);
                }
            }
        }

        // Note: Listen should be called after all game objects has been initialized
        public void Listen(string thisName, string thatName
                          ,CollisionDetector collisionDetector)
        {
            var key = new CollisionKey(thisName, thatName);
            if (_collisionKeySet.Add(key))
            {
                // Add collidable objects for collision detection
                AddCollidable(key);

                // Add collision detector (this is the first one)
                _collisionDetectors.Add(key, collisionDetector);
            }
            else
            {
                // Prevent duplicated collision detector added
                _collisionDetectors[key] -= collisionDetector;
                _collisionDetectors[key] += collisionDetector;
            }

        }

        // Note: Listen should be called after all game objects has been initialized
        public void Listen(string thisName, string thatName
                          ,CollisionDetector detector, CollisionHandler handler)
        {
            var key = new CollisionKey(thisName, thatName);
            if (_collisionKeySet.Add(key))
            {
                // Add collidable objects for collision detection
                AddCollidable(key);

                // Add collision detector (this is the first one)
                _collisionDetectors.Add(key, detector);
                _collisionHandlers.Add(key, handler);
            }
            else
            {
                // Prevent duplicated collision detector and handler added
                _collisionDetectors[key] -= detector;
                _collisionDetectors[key] += detector;
                _collisionHandlers[key] -= handler;
                _collisionHandlers[key] += handler;
            }

        }

        // Note: Listen should be called after all game objects has been initialized
        public void Listen(ICollidable thisCollidable, ICollidable thatCollidable
                          ,CollisionDetector detector)
        {
            Listen(thisCollidable.GetName(), thatCollidable.GetName(), detector);
        }

        // Note: Listen should be called after all game objects has been initialized
        public void Listen(ICollidable thisCollidable, ICollidable thatCollidable
                          ,CollisionDetector detector, CollisionHandler handler)
        {
            Listen(thisCollidable.GetName(), thatCollidable.GetName(), detector, handler);
        }

        public void Update()
        {
            if (Enabled)
            {
                foreach (var collisionKey in _collisionKeySet)
                {
                    foreach (var thisCollidable in _collidableLists[collisionKey.FirstName])
                    {
                        foreach (var thatCollidable in _collidableLists[collisionKey.SecondName])
                        {
                            DetectCollision(collisionKey, thisCollidable, thatCollidable);
                        }
                    }
                }
            }
        }

        private void DetectCollision(CollisionKey collisionKey, ICollidable thisObject, ICollidable thatObject)
        {
            var collisionDetector = _collisionDetectors[collisionKey];

            if (collisionDetector(thisObject, thatObject))
            {
                CollisionInfo thisCollisionData = new CollisionInfo();
                CollisionInfo thatCollisionData = new CollisionInfo();

                thisCollisionData.Other = thatObject;
                thatCollisionData.Other = thisObject;

                // Execute external collision handlers
                if (_collisionHandlers.ContainsKey(collisionKey))
                {
                    _collisionHandlers[collisionKey](thisObject, thatObject);
                }

                // Execute colliders' internal handler
                thisObject.OnCollision(thisCollisionData);
                thatObject.OnCollision(thatCollisionData);
            }
        }

        public static bool AABB(ICollidable thisCollidable, ICollidable thatCollidable)
        {
            // return thisCollidable.GetBound().Intersects(thatCollidable.GetBound());
            return thisCollidable.GetBound().Left <= thatCollidable.GetBound().Right &&
                   thatCollidable.GetBound().Left <= thisCollidable.GetBound().Right &&
                   thisCollidable.GetBound().Top <= thatCollidable.GetBound().Bottom &&
                   thatCollidable.GetBound().Top <= thisCollidable.GetBound().Bottom;
        }

        public static bool IsUncontained(ICollidable thisCollidable, ICollidable thatCollidable)
        {
            bool thisHasThat = thisCollidable.GetBound().Contains(thatCollidable.GetBound());
            bool thatHasThis = thatCollidable.GetBound().Contains(thisCollidable.GetBound());
            return !(thisHasThat || thatHasThis);
        }
    }
}
