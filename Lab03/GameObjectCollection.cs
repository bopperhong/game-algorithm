using System;
using System.Collections.Generic;

namespace Lab03
{
    public class GameObjectCollection
    {
        public static Dictionary<string, GameObject> Objects;
        private static Dictionary<string, HashSet<GameObject>> Tags;

        static GameObjectCollection()
        {
            Objects = new Dictionary<string, GameObject>();
            Tags = new Dictionary<string, HashSet<GameObject>>();
        }

        public static int Count()
        {
            return Objects.Count;
        }

        public static bool Add(GameObject gameObject)
        {
            // Preconditions:
            // 1. Objects that already exists cannot be added again.

            if (!Objects.ContainsKey(gameObject.Name))
            {
                // Add by name
                Objects.Add(gameObject.Name, gameObject);

                // Add by tag
                string tag = gameObject.Tag.ToLower();
                if (!Tags.ContainsKey(tag))
                {
                    Tags[tag] = new HashSet<GameObject>();                   
                }
                Tags[tag].Add(gameObject);

                return true;
            }

            return false;
        }

        public static void Clear()
        {
            Tags.Clear();
            Objects.Clear();
        }

        public static bool ChangeTag(GameObject gameObject, string tag)
        {
            if (!Objects.ContainsKey(gameObject.Name))
            {
                string oldTagLowercase = gameObject.Tag.ToLower();
                string newTagLowercase = tag.ToLower();

                if (!Tags.ContainsKey(newTagLowercase))
                {
                    Tags[newTagLowercase] = new HashSet<GameObject>();
                }

                Tags[oldTagLowercase].Remove(gameObject);
                Tags[newTagLowercase].Add(gameObject);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the game object by name, or null if not found.
        /// </summary>
        /// <param name="name">The name of the object to find.</param>
        /// <returns>
        /// GameObject - if a game object with the name exists.
        /// null - if there is no game object with such a name.
        /// </returns>
        public static GameObject FindByName(string name)
        {
            if (Objects.ContainsKey(name))
            {
                return Objects[name];
            }
            return null;
        }

        public static GameObject[] FindObjectsByTag(string tag)
        {
            string tagLowercase = tag.ToLower();

            if (Tags.ContainsKey(tagLowercase))
            {
                GameObject[] objects = new GameObject[Tags[tagLowercase].Count];
                Tags[tagLowercase].CopyTo(objects);
                return objects;
            }
            else
            {
                return null;
            }
        }

        public static GameObject[] FindObjectsByType(Type type)
        {
            List<GameObject> objectList = new List<GameObject>();

            foreach (var (_, obj) in Objects)
            {
                if (obj.GetType() == type)
                {
                    objectList.Add(obj);
                }
            }

            GameObject[] objects = null;
            if (objectList.Count > 0)
            {
                objects = new GameObject[objectList.Count];
                objectList.CopyTo(objects);
            }
            
            return objects;
        }
    }
}
