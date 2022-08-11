using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace IdleArcade.Core
{
    public class TriggerDetector : MonoBehaviour
    {
        public interface ITriggerable
        {
            /// <summary>
            /// Called when Trigger object intersect begin with other valid taged object 
            /// </summary>
            /// <param name="other"></param>
            public void OnEnter(Collider other);


            /// <summary>
            /// Called when Trigger object intersect released with other valid taged object 
            /// </summary>
            /// <param name="other"></param>
            public void OnExit(Collider other);
        }
        public enum ResponseMode
        {
            Self,
            Child,
            parent
        }

        [SerializeField, Tooltip("Which interface will interact with this object\n" +
            "if you selected 'Self' than all of the self components that has already impelmented 'ITriggerable' Interface will execute it self. " +
            "if you selected 'Child' than all of the child components that has already impelmented 'ITriggerable' Interface will execute it self. " +
            "if you selected 'Parent' than all of the child components that has already impelmented 'ITriggerable' Interface will execute it self.")]
        private ResponseMode responseMode = ResponseMode.Self;

        [SerializeField, Tooltip("Masking the target object tags, which object is valid to trigger with this object")]
        private string[] tags;
        [Header("Callback Events")]
        [SerializeField] private UnityEvent OnEnter;
        [SerializeField] private UnityEvent OnExit;

        private void Awake()
        {
            //set attached collider trigger to true
            var collider = GetComponent<Collider>();
            if (collider)
                collider.isTrigger = true;
        }

        /// <summary>
        /// Called when Trigger object intersect begin with other valid taged object 
        /// </summary>
        /// <param name="other">Which object intersect with this object</param>
        private void Enter(Collider other)
        {
            switch (responseMode)
            {
                case ResponseMode.Self:
                    var traiggerables = GetComponents<ITriggerable>();
                    foreach (var triggerable in traiggerables)
                        triggerable.OnEnter(other);
                    break;

                case ResponseMode.Child:
                    traiggerables = GetComponentsInChildren<ITriggerable>();
                    var selfComponents = GetComponents<ITriggerable>();
                    if (selfComponents.Length > 0)
                    {
                        var triggerableChilds = traiggerables.ToList();
                        foreach (var selfComponent in selfComponents)
                            triggerableChilds.Remove(selfComponent);
                        traiggerables = triggerableChilds.ToArray();
                    }

                    foreach (var triggerable in traiggerables)
                        triggerable.OnEnter(other);
                    break;

                case ResponseMode.parent:
                    traiggerables = GetComponentsInParent<ITriggerable>();
                    selfComponents = GetComponents<ITriggerable>();
                    if (selfComponents.Length > 0)
                    {
                        var triggerableChilds = traiggerables.ToList();
                        foreach (var selfComponent in selfComponents)
                            triggerableChilds.Remove(selfComponent);
                        traiggerables = triggerableChilds.ToArray();
                    }

                    foreach (var triggerable in traiggerables)
                        triggerable.OnEnter(other);
                    break;
            }
            OnEnter.Invoke();
        }

        /// <summary>
        /// Called when Trigger object intersect released with other valid taged object 
        /// </summary>
        /// <param name="other">Which object intersect with this object</param>
        private void Exit(Collider other)
        {
            switch (responseMode)
            {
                case ResponseMode.Self:
                    var traiggerables = GetComponents<ITriggerable>();
                    foreach (var triggerable in traiggerables)
                        triggerable.OnExit(other);
                    break;

                case ResponseMode.Child:
                    traiggerables = GetComponentsInChildren<ITriggerable>();
                    var selfComponents = GetComponents<ITriggerable>();
                    if (selfComponents.Length > 0)
                    {
                        var triggerableChilds = traiggerables.ToList();
                        foreach (var selfComponent in selfComponents)
                            triggerableChilds.Remove(selfComponent);
                        traiggerables = triggerableChilds.ToArray();
                    }

                    foreach (var triggerable in traiggerables)
                        triggerable.OnExit(other);
                    break;

                case ResponseMode.parent:
                    traiggerables = GetComponentsInParent<ITriggerable>();
                    selfComponents = GetComponents<ITriggerable>();
                    if (selfComponents.Length > 0)
                    {
                        var triggerableChilds = traiggerables.ToList();
                        foreach (var selfComponent in selfComponents)
                            triggerableChilds.Remove(selfComponent);
                        traiggerables = triggerableChilds.ToArray();
                    }

                    foreach (var triggerable in traiggerables)
                        triggerable.OnExit(other);
                    break;
            }

            OnExit.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (tags.Length == 0)
                Enter(other);//execute if no tag assigned
            else
                foreach (var tag in tags)
                    if (tag == other.tag)//masking the target tag
                    {
                        Enter(other);
                        break;
                    }
        }

        private void OnTriggerExit(Collider other)
        {
            if (tags.Length == 0)
                Exit(other);//execute if no tag assigned
            else
                foreach (var tag in tags)
                    if (tag == other.tag)//masking the target tag
                    {
                        Exit(other);
                        break;
                    }
        }
    }
}