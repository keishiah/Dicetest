using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Infrastructure.Factories
{
    public class DicePooler
    {
        private readonly GameObject _prefab;
        private readonly Transform _parent;
        private List<GameObject> _pool;

        public DicePooler(GameObject prefab, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
        }

        public GameObject GetFreeElement()
        {
            if (HasFreeElement(out var element))
                return element;

            return CreateObject(true);
        }

        public List<GameObject> GetActiveElements()
        {
            var activeElements = new List<GameObject>();

            foreach (var unit in _pool)
            {
                if (unit.gameObject.activeInHierarchy)
                {
                    activeElements.Add(unit);
                }
            }

            return activeElements;
        }

        public void CreatePool(int count)
        {
            _pool = new List<GameObject>();

            for (int i = 0; i < count; i++)
            {
                CreateObject();
            }
        }

        public void DeactivateAllPoolUnits()
        {
            foreach (var unit in _pool)
            {
                unit.gameObject.SetActive(false);
            }
        }

        private GameObject CreateObject(bool isActiveByDefault = false)
        {
            var createdObject = Object.Instantiate(_prefab, _parent);
            createdObject.gameObject.SetActive(isActiveByDefault);
            _pool.Add(createdObject);
            return createdObject;
        }

        private bool HasFreeElement(out GameObject element)
        {
            foreach (var unit in _pool)
            {
                if (!unit.gameObject.activeInHierarchy)
                {
                    element = unit;
                    unit.gameObject.SetActive(true);
                    return true;
                }
            }

            element = null;
            return false;
        }
    }
}