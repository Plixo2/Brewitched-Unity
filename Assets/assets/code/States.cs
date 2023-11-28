#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace assets.code
{
    /// <summary>
    /// Class to register connection points, items, cauldrons and interactables
    /// Also used to keep track of the overall game progress, like waterlevel
    /// </summary>
    public class States : MonoBehaviour
    {
        private static States _instance;

        public int level = 0;
        private List<ConnectionPoint> _connectionPoints = new();
        private TheCauldron _cauldrons = new();
        private List<Item> _items = new();
        private List<Interactable> _interactables = new();

        private void Awake()
        {
            _instance = this;
        }

        
        public static void AddConnectionPoint(ConnectionPoint connectionPoint)
        {
            _instance._connectionPoints.Add(connectionPoint);
        }
        public static void AddItem(Item item)
        {
            _instance._items.Add(item);
        }
        public static void AddInteractable(Interactable interactable)
        {
            _instance._interactables.Add(interactable);
        }
        public static void AddCauldron(TheCauldron cauldron)
        {
            _instance._cauldrons = cauldron;
        }

        public static TheCauldron? CurrentCauldron()
        {
            return _instance._cauldrons;
        }
    
        public static Interactable? GetInteractable(Vector3 position, float range, Predicate<Interactable> predicate)
        {
            var rangeMin = range;
            Interactable? bestPoint = null;
            foreach (var point in _instance._interactables)
            {
                var pos = point.transform.position;
                var distSqr = Vector3.Distance(position, pos);
                if ((distSqr-point.interactionRange) < rangeMin && predicate.Invoke(point))
                {
                    rangeMin = distSqr;
                    bestPoint = point;
                }
            }

            return bestPoint;
        }
        public static ConnectionPoint? GetPoint(Vector3 position, float range, Predicate<ConnectionPoint> predicate)
        {
            var rangeMin = range;
            ConnectionPoint? bestPoint = null;
            foreach (var point in _instance._connectionPoints)
            {
                var pos = point.transform.position;
                var distSqr = Vector3.Distance(position, pos);
                if (distSqr < rangeMin && predicate.Invoke(point))
                {
                    rangeMin = distSqr;
                    bestPoint = point;
                }
            }

            return bestPoint;
        }
        public static Item? GetItem(Vector3 position, float range)
        {
            var rangeMin = range;
            Item? bestPoint = null;
            foreach (var point in _instance._items)
            {
                var pos = point.transform.position;
                var distSqr = Vector3.Distance(position, pos);
                if (distSqr < rangeMin)
                {
                    rangeMin = distSqr;
                    bestPoint = point;
                }
            }

            return bestPoint;
        }
        public static Item? GetItem(Vector3 position, float range, Predicate<Item> predicate)
        {
            var rangeMin = range;
            Item? bestPoint = null;
            foreach (var point in _instance._items)
            {
                var pos = point.transform.position;
                var distSqr = Vector3.Distance(position, pos);
                if (distSqr < rangeMin && predicate.Invoke(point))
                {
                    rangeMin = distSqr;
                    bestPoint = point;
                }
            }

            return bestPoint;
        }


        public static void RemoveItem(Item item)
        {
            _instance._items.Remove(item);
        }
    }
}