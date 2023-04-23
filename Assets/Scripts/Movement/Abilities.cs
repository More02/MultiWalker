using System.Collections;
using Mirror;
using UnityEngine;

namespace Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class Abilities : NetworkBehaviour
    {
        private Rigidbody _rigidbody;
        [SerializeField] private float _distance = 5f;
        [SerializeField] private Transform _mesh;

        private const float Force = 3;
        private Vector3 _startPosition;
        //private Transform _cameraTransform;

        public bool IsDashing { get; private set; }

        public static Abilities Instance { get; private set; }

        private void Start()
        {
            Instance = this;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        }

        public override void OnStartLocalPlayer()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            //_cameraTransform = Camera.main!.transform;
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
                _rigidbody.AddForce(_mesh.forward * Force, ForceMode.Impulse);
                //_rigidbody.AddForce(_cameraTransform.forward * Force, ForceMode.Impulse);
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