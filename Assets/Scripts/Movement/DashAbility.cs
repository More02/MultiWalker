using System.Collections;
using System.Threading.Tasks;
using Mirror;
using Player;
using UnityEngine;

namespace Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class DashAbility : NetworkBehaviour
    {
        [SerializeField] private float _distance = 5f;
        [SerializeField] private int _disabledTime = 3;
        private const float Force = 3f;
        private Rigidbody _rigidbody;
        private readonly Color _dashedColor = new((float)0.6226415, (float)0.2196867, (float)0.2196867);
        private readonly Color _baseColor = Color.white;
        private static readonly int _color = Shader.PropertyToID("_Color");
        private Component[] _skinnedMeshRenderers;
        private int _countOfSuccessDash;
        private bool _isAvailableForDash = true;
        private Vector3 _startPosition;
        private Transform _collisionRoot;
        private bool _isDashed;

        private IEnumerator _dashCoroutine;

        public bool IsDashing { get; private set; }

        private void Start()
        {
            _skinnedMeshRenderers = GetComponentsInChildren(typeof(SkinnedMeshRenderer), true);
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void Update()
        {
            if (!isLocalPlayer) return;
            if (!Input.GetMouseButtonDown(0)) return;
            if (IsDashing) return;
            IsDashing = true;
            _startPosition = transform.position;
            _dashCoroutine = Dash();
            StartCoroutine(_dashCoroutine);
        }

        private async void OnCollisionEnter(Collision collision)
        {
            if (!isLocalPlayer) return;
            _rigidbody.velocity = Vector3.zero;

            if (!collision.gameObject.CompareTag("Player")) return;
            _collisionRoot = collision.transform.root;
            _collisionRoot.GetComponent<Rigidbody>().velocity = Vector3.zero;

            if (!IsDashing) return;
            var collisionDashAbility = _collisionRoot.GetComponent<DashAbility>();

            if (!collisionDashAbility._isAvailableForDash) return;

            CmdChangeIsDashed(collisionDashAbility, !collisionDashAbility._isDashed);

            collisionDashAbility._isAvailableForDash = false;
            _countOfSuccessDash++;
            InfoCanvas.Instance.SetScore(_countOfSuccessDash, NetworkConnection.LocalConnectionId);
            StopCoroutine(_dashCoroutine);
            IsDashing = false;
            await Task.Delay(_disabledTime * 1000);
            ShowWin();
            CmdChangeIsDashed(collisionDashAbility, !collisionDashAbility._isDashed);
            collisionDashAbility._isAvailableForDash = true;
        }

        [Command(requiresAuthority = false)]
        private void CmdChangeIsDashed(DashAbility collisionDashAbility, bool isDashed)
        {
            RpcChangeIsDashed(collisionDashAbility, isDashed);
            ChangeIsDashed(collisionDashAbility, isDashed);
        }

        [ClientRpc]
        private void RpcChangeIsDashed(DashAbility collisionDashAbility, bool isDashed)
        {
            ChangeIsDashed(collisionDashAbility, isDashed);
        }

        private void ChangeIsDashed(DashAbility collisionDashAbility, bool isDashed)
        {
            collisionDashAbility._isDashed = isDashed;
            collisionDashAbility.ChangeColor(collisionDashAbility._isDashed ? _dashedColor : _baseColor);
        }

        private void ChangeColor(Color color)
        {
            foreach (var component in _skinnedMeshRenderers)
            {
                var skinnedMeshRenderer = (SkinnedMeshRenderer)component;
                skinnedMeshRenderer.materials[0].SetColor(_color, color);
            }
        }

        private IEnumerator Dash()
        {
            if (!IsDashing) yield break;
            while (IsDashing)
            {
                _rigidbody.AddForce(transform.GetChild(0).forward * Force, ForceMode.Impulse);
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

        private void ShowWin()
        {
            if (_countOfSuccessDash == 3)
            {
                Debug.Log("Win");
            }
        }
    }
}