
using FlaxEngine;
using System.Collections.Generic;

namespace Game
{
    /// <summary>
    /// Useful or missing extensions for the Flax Engine
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Recursively searches for a script of type T in the parent hierarchy of the provided actor.
        /// </summary>
        public static T FindScriptInParent<T>(this Actor actor) where T : Script
        {
            if (actor == null)
                return null;

            // Check the current actor for the script component
            T script = actor.GetScript<T>();
            if (script != null)
                return script;

            // If the script was not found, recursively check the parent actor
            if (actor.Parent != null)
                return actor.Parent.FindScriptInParent<T>();

            // If no matching script is found in the parent hierarchy, return null
            return null;
        }


        /// <summary>
        /// Finds a script of type T in the parent hierarchy of the provided actor.
        /// </summary>
        public static T FindScriptInParent<T>(this Actor actor, int maxIterations) where T : Script
        {
            if (actor == null || maxIterations <= 0)
                return null;

            // Check the current actor for the script component
            T script = actor.GetScript<T>();
            if (script != null)
                return script;

            // If the script was not found and maxIterations allows, recursively check the parent actor
            if (actor.Parent != null)
                return actor.Parent.FindScriptInParent<T>(maxIterations - 1);

            // If no matching script is found in the parent hierarchy, return null
            return null;
        }

        /// <summary>
        /// Finds all script components of type T in the children hierarchy of the provided actor.
        /// </summary>
        public static T[] FindScriptsInChildren<T>(this Actor actor) where T : Script
        {
            List<T> foundScripts = new List<T>();

            if (actor == null)
                return foundScripts.ToArray();

            // Check the current actor for the script component
            T script = actor.GetScript<T>();
            if (script != null)
                foundScripts.Add(script);

            // Recursively check children actors
            foreach (Actor child in actor.Children)
            {
                T[] childScripts = child.FindScriptsInChildren<T>();
                if (childScripts.Length > 0)
                    foundScripts.AddRange(childScripts);
            }

            return foundScripts.ToArray();
        }



        /// <summary>
        /// Checks if an actor has any of the specified tags from an array of Tag objects.
        /// </summary>
        public static bool HasAnyTag(this Actor actor, Tag[] tags)
        {
            if (actor == null || tags == null || tags.Length == 0)
                return false;

            if (actor.HasTag() == false)
                return false;

            foreach (Tag tag in tags)
            {
                if (actor.HasTag(tag))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if an actor has any of the specified tags from an array of string Tags.
        /// </summary>
        public static bool HasAnyTag(this Actor actor, string[] tags)
        {
            if (actor == null || tags == null || tags.Length == 0)
                return false;

            if (actor.HasTag() == false)
                return false;

            foreach (string tag in tags)
            {
                if (actor.HasTag(tag))
                    return true;
            }

            return false;
        }
    }
}
