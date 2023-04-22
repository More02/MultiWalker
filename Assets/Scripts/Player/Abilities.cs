using System.Collections;
using Mirror;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Abilities : NetworkBehaviour
    {
        private Rigidbody _rigidbody;
        [SerializeField] private float _distance = 5f;
        private Transform _cameraTransform;

        private const float Force = 3;
        private Vector3 _startPosition;
        private bool _isMoving;

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
            if (_isMoving) return;
            _isMoving = true;
            _startPosition = transform.position;
            StartCoroutine(Dash());
        }

        private IEnumerator Dash()
        {
            if (!_isMoving) yield break;
            while (_isMoving)
            {
                _rigidbody.AddForce(_cameraTransform.forward * Force, ForceMode.Impulse);
                var currentDistance = transform.position - _startPosition;
                if (currentDistance.magnitude >= _distance)
                {
                    _isMoving = false;
                    _rigidbody.velocity = Vector3.zero;
                    yield break;
                }

                yield return null;
            }
        }
    }
}