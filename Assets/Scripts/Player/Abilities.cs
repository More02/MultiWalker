using System.Collections;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Abilities : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        [SerializeField] private float _distance = 5f;
        [SerializeField] private Transform _cameraTransform;

        private const float Force = 3;
        private Vector3 _startPosition;
        private bool _isMoving;

        private void Start()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            if (_isMoving) return; 
            _isMoving = true;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _startPosition = transform.position;
            StartCoroutine(Dash());
        }
        private IEnumerator Dash()
        {
            if (!_isMoving) yield break;
            while (true)
            {
                _rigidbody.AddForce(_cameraTransform.forward * Force, ForceMode.Impulse);
                var currentDistance = transform.position - _startPosition;
                if (currentDistance.magnitude >= _distance)
                {
                    _isMoving = false;
                    _rigidbody.freezeRotation = false;
                    _rigidbody.velocity = Vector3.zero;
                    yield break;
                }
                yield return null;
            }
        }
    }
}