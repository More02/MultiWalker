using System;
using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

namespace Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class Abilities : NetworkBehaviour
    {
        private Rigidbody _rigidbody;
        [SerializeField] private float _distance = 5f;
        private Transform _cameraTransform;

        private const float Force = 3;
        private Vector3 _startPosition;
        
        public bool IsDashing { get; private set; }
        
        public static Abilities Instance { get; private set; }

        private void Start()
        {
            Instance = this;
        }

        public override void OnStartLocalPlayer()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _cameraTransform = Camera.main!.transform;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void Update()
        {
            if (!isLocalPlayer) return;
            if (!Input.GetMouseButtonDown(0)) return;
            if (IsDashing) return;
            IsDashing = true;
            _startPosition = transform.position;
            StartCoroutine(Dash());
        }

        private IEnumerator Dash()
        {
            if (!IsDashing) yield break;
            while (IsDashing)
            {
                _rigidbody.AddForce(_cameraTransform.forward * Force, ForceMode.Impulse);
                var currentDistance = transform.position - _startPosition;
                if (currentDistance.magnitude >= _distance)
                {
                    IsDashing = false;
                    _rigidbody.velocity = Vector3.zero;
                    yield break;
                }

                yield return null;
            }
        }
        
    }
}