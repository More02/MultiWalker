using System.Collections;
using System.Threading.Tasks;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Player;
using UnityEngine;

namespace Movement
{
    /// <summary>
    /// Класс, отвечающий за способность Рывок
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class DashAbility : NetworkBehaviour
    {
        [SerializeField] private float _distance = 5f;
        [SerializeField] private int _disabledTime = 3;
        private const float Force = 3f;
        private Rigidbody _rigidbody;
        private Vector3 _startPosition;
        private readonly Color _dashedColor = new((float)0.6226415, (float)0.2196867, (float)0.2196867);
        private readonly Color _baseColor = Color.white;
        private static readonly int _color = Shader.PropertyToID("_Color");
        private Component[] _skinnedMeshRenderers;
        private bool _isAvailableForDash = true;
        private bool _isPlayerDashedBy;
        private IEnumerator _dashCoroutine;

        [field: SyncVar] public int CountOfSuccessDash { get; set; }
        public bool IsDashing { get; private set; }
        [field: SyncVar] public bool IsWin { get; set; }
        public static DashAbility Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _skinnedMeshRenderers = GetComponentsInChildren(typeof(SkinnedMeshRenderer), true);
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void Update()
        {
            if (!Owner.IsLocalClient) return;
            if (!Input.GetMouseButtonDown(0)) return;
            if (IsDashing) return;
            IsDashing = true;
            _startPosition = transform.position;
            _dashCoroutine = Dash();
            StartCoroutine(_dashCoroutine);
        }

        private async void OnCollisionEnter(Collision collision)
        {
            if (!Owner.IsLocalClient) return;
            _rigidbody.velocity = Vector3.zero;

            if (!collision.gameObject.CompareTag("Player")) return;
            var collisionRoot = collision.transform.root;
            collisionRoot.GetComponent<Rigidbody>().velocity = Vector3.zero;

            if (!IsDashing) return;
            var collisionDashAbility = collisionRoot.GetComponent<DashAbility>();
            
            if ((!collisionDashAbility._isAvailableForDash) ||
                (IsWin)) return;

            CmdChangeIsDashed(collisionDashAbility, !collisionDashAbility._isPlayerDashedBy);
            collisionDashAbility._isAvailableForDash = false;
            CountOfSuccessDash++;
            gameObject.GetComponent<PlayerScore>().CmdChangeScore(CountOfSuccessDash, gameObject.name);
            StopCoroutine(_dashCoroutine);
            IsDashing = false;
            await Task.Delay(_disabledTime * 1000);
            CmdChangeIsDashed(collisionDashAbility, !collisionDashAbility._isPlayerDashedBy);
            collisionDashAbility._isAvailableForDash = true;
        }

        [ServerRpc(RequireOwnership = false)]
        private void CmdChangeIsDashed(DashAbility collisionDashAbility, bool isDashed)
        {
            RpcChangeIsDashed(collisionDashAbility, isDashed);
            ChangeIsDashed(collisionDashAbility, isDashed);
        }

        [ObserversRpc(ExcludeServer = true)]
        private void RpcChangeIsDashed(DashAbility collisionDashAbility, bool isDashed)
        {
            if (!IsServerInitialized) ChangeIsDashed(collisionDashAbility, isDashed);
        }

        private void ChangeIsDashed(DashAbility collisionDashAbility, bool isDashed)
        {
            collisionDashAbility._isPlayerDashedBy = isDashed;
            collisionDashAbility.ChangeColor(collisionDashAbility._isPlayerDashedBy ? _dashedColor : _baseColor);
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
            var time = 0f;
            while (IsDashing)
            {
                time += Time.deltaTime;
                _rigidbody.AddForce(transform.GetChild(0).forward * Force, ForceMode.Impulse);
                var currentDistance = transform.position - _startPosition;
                if ((currentDistance.magnitude >= _distance) || (time > 1f))
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