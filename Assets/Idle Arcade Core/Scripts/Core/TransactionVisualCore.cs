using System.Collections.Generic;
using UnityEngine;

namespace IdleArcade.Core
{
    public class TransactionVisualCore : MonoBehaviour
    {
        public delegate void ChangedVisual(List<Entity> totalEntitys);
        public ChangedVisual OnChangedVisual;

        [Tooltip("Collection of visual objects to represent how many amounts already stored")]
        protected List<Entity> visualAmounts = new List<Entity>();

        protected virtual void Awake()
        {
            var storePoints = GetComponents<TransactionContainer>();

            if (storePoints.Length > 0)
                foreach (var point in storePoints)
                    point.OnChangedValue += OnChanging;                
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
                OnAdding(delta, A);
            
            if (delta < 0)
                OnRemoving(delta, A);
            
        }

        /// <summary>
        /// Called when adding transaction amount of the container
        /// </summary>
        /// <param name="delta">change rate</param>
        /// <param name="A">Source Container</param>
        protected virtual void OnAdding(int delta, TransactionContainer A) { }

        /// <summary>
        ///  Called when Removing transaction amount of the container
        /// </summary>
        /// <param name="delta">change rate</param>
        /// <param name="A">Source Containe</param>
        protected virtual void OnRemoving(int delta, TransactionContainer A) { }

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
        /// return next pullable Entity from the collection Usning FiFO
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns></returns>
        public Entity NextPullElement_UsingFIFO(string id)
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
        public Entity NextPullElement_UsingLIFO(string id)
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
        /// Push Entity to the collection
        /// </summary>
        /// <param name="visualEntity">Entity item</param>
        /// <returns></returns>
        public bool Push(Entity visualEntity)
        {
            if (visualEntity == null)
                return false;

            visualAmounts.Add(visualEntity);
            Refresh();
            return true;
        }



        public void Refresh()
        {
            if (visualAmounts.Count == 0) return;

            if (OnChangedVisual == null)
            { 
                var rotation = transform.rotation;
                var position = transform.position;
                var height = visualAmounts[0].transform.localScale.y;
                position.y += visualAmounts.Count * height + 1;

                for (int i = visualAmounts.Count - 1; i >= 0; i--)
                {
                    var amount = visualAmounts[i];
                    position.y -= height;
                    amount.transform.SetPositionAndRotation(position, rotation);
                }
            }
            else
                OnChangedVisual.Invoke(visualAmounts);
        }
    }
}