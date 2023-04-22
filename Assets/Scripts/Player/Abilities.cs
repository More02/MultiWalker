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
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Mouse0)) return;
            _isMoving = true;
            _startPosition = transform.position;
            Dash();

            // if (Input.GetMouseButtonUp(0))
            // {
            //     _isMoving = false;
            // }
        }

        private void Dash()
        {
            if (!_isMoving) return;
            _rigidbody.AddForce(_cameraTransform.forward * (_distance * Force), ForceMode.Impulse);
            var currentDistance = transform.position - _startPosition;
            if (!(currentDistance.magnitude >= _distance)) return;
            _isMoving = false;
            // _rigidbody.velocity = Vector3.zero;
            // _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.freezeRotation = false;
            
            
            // if (!_isMoving) return;
            // var targetPosition = transform.position;
            // targetPosition.z += _distance;
            // var direction = targetPosition - transform.position;
            // _rigidbody.AddForce(direction.normalized *  Force, ForceMode.Impulse);
            // var currentDistance = transform.position - _startPosition;
            // if (!(currentDistance.magnitude >= _distance)) return;
            // _isMoving = false;
            // _rigidbody.freezeRotation = false;
            
        }
    }
}