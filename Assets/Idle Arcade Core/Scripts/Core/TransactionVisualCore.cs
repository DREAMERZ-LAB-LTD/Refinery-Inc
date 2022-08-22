using System;
using System.Collections.Generic;
using UnityEngine;

namespace IdleArcade.Core
{

    public abstract class TransactionVisualCore : MonoBehaviour
    {
        public delegate void ChangedVisual(List<Entity> totalEntitys);
        public ChangedVisual OnChangedVisual;

        [Tooltip("Collection of visual objects to represent how many amounts already stored")]
        protected List<Entity> visualAmounts = new List<Entity>();

        protected IVisualPattern visualPattern = null;
        protected IVisualEffect visualEffect = null;

        protected virtual void Awake()
        {
            var storePoints = GetComponents<TransactionContainer>();
            if (storePoints.Length > 0)
                foreach (var point in storePoints)
                    point.OnChangedValue += OnChanging;   
            
            visualPattern = GetComponentInChildren<IVisualPattern>();
            visualEffect = GetComponentInChildren<IVisualEffect>();
        }
        private void OnDestroy()
        {
            var storePoints = GetComponents<TransactionContainer>();
            if (storePoints.Length > 0)
                foreach (var point in storePoints)
                    point.OnChangedValue -= OnChanging;                
        }

        /// <summary>
        /// called each of the changes container transaction amount
        /// </summary>
        /// <param name="delta">change rate</param>
        /// <param name="currnet">New value of the container</param>
        /// <param name="max">max range to store on it</param>
        /// <param name="containerID">container ID</param>
        /// <param name="A">Sorce Container</param>
        /// <param name="B">Destination Container</param>
        private void OnChanging(int delta, int currnet, int max, string containerID, TransactionContainer A, TransactionContainer B)
        {
            if (delta > 0)
                OnAdding(delta, A, B);
            
            if (delta < 0)
                OnRemoving(delta, A, B);
        }

        /// <summary>
        /// Called when adding transaction amount of the container
        /// </summary>
        /// <param name="delta">change rate</param>
        /// <param name="A">Source Container</param>
        protected abstract void OnAdding(int delta, TransactionContainer A, TransactionContainer B);

        /// <summary>
        ///  Called when Removing transaction amount of the container
        /// </summary>
        /// <param name="delta">change rate</param>
        /// <param name="A">Source Containe</param>
        protected abstract void OnRemoving(int delta, TransactionContainer A, TransactionContainer B);

        /// <summary>
        /// return next pullable Entity from the collection Usning FiFO
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns></returns>
        public Entity PullNextElement_UsingFIFO(string id)
        {
            if (visualAmounts.Count == 0)
                return null;

            Entity result = null;
            for (int i = visualAmounts.Count - 1; i >= 0; i--)
            {
                result = visualAmounts[i];
                if (result.GetID == id)
                    return result;
            }
            return null;
        }

        /// <summary>
        /// return next pullable Entity from the collection Usning LIFO
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns></returns>
        public Entity PullNextElement_UsingLIFO(string id)
        {
            if (visualAmounts.Count == 0)
                return null;

            Entity result = null;
            for (int i = 0; i < visualAmounts.Count; i++)
            {
                result = visualAmounts[i];
                if (result.GetID == id)
                    return result;
            }
            return null;
        }

        /// <summary>
        /// return Entity and remove it from the collection Usning FIFO
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns></returns>
        public Entity Pull_UsingFIFO(string id)
        {
            if (visualAmounts.Count == 0)
                return null;

            Entity result = null;
            for (int i = visualAmounts.Count - 1; i >= 0; i--)
            {
                result = visualAmounts[i];
                if (result.GetID == id)
                {
                    visualAmounts.RemoveAt(i);
                    Refresh();
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// return Entity and remove it from the collection Usning LIFO
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns></returns>
        public Entity Pull_UsingLIFO(string id)
        {
            if (visualAmounts.Count == 0)
                return null;

            Entity result = null;
            for (int i = 0; i < visualAmounts.Count; i++)
            {
                result = visualAmounts[i];
                if (result.GetID == id)
                {
                    visualAmounts.RemoveAt(i);
                    Refresh();
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Push Entity to the collection
        /// </summary>
        /// <param name="entity">Entity item</param>
        /// <returns></returns>
        public bool Push(Entity entity)
        {
            if (entity == null)
                return false;

            visualAmounts.Add(entity);


            if (visualPattern != null)
            {
                var parentMono = visualPattern as MonoBehaviour;
                entity.transform.parent = parentMono.transform;
            }
            else
                entity.transform.parent = transform;

            if (visualEffect != null)
            {
                var from = entity.transform.localPosition;
                var to = GetLocalPointOf(visualAmounts.Count-1, entity.transform.localScale);
                visualEffect.OnAdding(entity, from, to, Refresh);
            }
            else
                Refresh();
            return true;
        }

        public virtual Vector3 GetLocalPointOf(int index, Vector3 scale)
        {
            try
            {
                return visualPattern.GetLocalPointOf(index, scale);
            }
            catch (Exception e)
            {
                var y = index * scale.y + 1;
                return new Vector3(0, y, 0);
            }
        }

        public void Refresh()
        {
            if (visualAmounts.Count == 0) return;

            if (visualEffect != null)
                visualEffect.OnChanged(visualAmounts);

            try
            {
                visualPattern.OnChanged(visualAmounts);
            }
            catch (Exception e)
            { 
                var scale = visualAmounts[0].transform.localScale;

                for (int i = visualAmounts.Count - 1; i >= 0; i--)
                {
                    var amount = visualAmounts[i];
                    var position = GetLocalPointOf(i, scale);
                    amount.transform.localPosition = position;
                    amount.transform.localRotation = Quaternion.identity;
                }
            }

            
            if (OnChangedVisual != null)
                OnChangedVisual.Invoke(visualAmounts);
        }
    }
}