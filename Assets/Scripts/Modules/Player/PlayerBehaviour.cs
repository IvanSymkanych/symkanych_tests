using System;
using Core.Game;
using Core.Game.Pools;
using Cysharp.Threading.Tasks;
using Helpers;
using Modules.Tools;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Modules.Player
{
    [RequireComponent(typeof(PlayerInputHandler), typeof(PlayerCollisionHandler), typeof(PlayerInput))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerBehaviour : MonoBehaviour
    {
        public event Action OnDeath;

        public IProgressBarController HeathProgressBarController => _heathProgressBarController;
        public IProgressBarController EnergyProgressBarController => _energyProgressBarController;

        private const float Threshold = 0.01f;
        private const float SpeedOffset = 0.1f;
        private const float MaxMoveSpeed = 3.0f;
        private const float RotationSpeed = 1.0f;
        private const float SpeedChangeRate = 8.0f;
        private const float TopClamp = 70.0f;
        private const float BottomClamp = -70.0f;

        [SerializeField] private Transform cinemachineCameraTarget;

        private IGameFactory _gameFactory;
        private IGamePoolService _gamePoolService;
        private ArenaBehaviour _arenaBehaviour;

        private IProgressBarController _heathProgressBarController;
        private IProgressBarController _energyProgressBarController;

        private PlayerInputHandler _playerInputHandler;
        private PlayerCollisionHandler _playerCollisionHandler;
        private PlayerInput _playerInput;
        private CharacterController _characterController;

        private float _cinemachineTargetPitch;
        private float _currentMoveSpeed;
        private float _rotationVelocity;
        private float _verticalVelocity;

        private bool _onInitialize;

        [Inject]
        public void Construct(
            IGameFactory gameFactory,
            IGamePoolService gamePoolService,
            ArenaBehaviour arenaBehaviour)
        {
            _gameFactory = gameFactory;
            _gamePoolService = gamePoolService;
            _arenaBehaviour = arenaBehaviour;
        }

        public void Initialize(
            IProgressBarController heathProgressBarController,
            IProgressBarController energyProgressBarController)
        {
            _heathProgressBarController = heathProgressBarController;
            _energyProgressBarController = energyProgressBarController;

            _playerInputHandler = GetComponent<PlayerInputHandler>();
            _playerCollisionHandler = GetComponent<PlayerCollisionHandler>();
            _playerInput = GetComponent<PlayerInput>();
            _characterController = GetComponent<CharacterController>();

            SubscribeEvents();
            _playerInputHandler.IsEnable = true;
            _onInitialize = true;
        }

        private void Update()
        {
            if (!_onInitialize)
                return;

            if (_playerInputHandler.IsEnable)
                Move();
        }

        private void LateUpdate()
        {
            if (!_onInitialize)
                return;

            if (_playerInputHandler.IsEnable)
                RotateCamera();
        }

        private void SubscribeEvents()
        {
            _playerCollisionHandler.OnArenaExit += PlayerCollisionHandlerOnOnArenaExit;
            _playerCollisionHandler.OnTakeEnergyHit += TakeEnergyHit;
            _playerCollisionHandler.OnTakeLifeHit += TakeLifeHit;
            _playerInputHandler.OnFirePressed += Shot;
            _playerInputHandler.OnUltimatePressed += TryUseUltimate;
            _heathProgressBarController.OnValueMinimum += Death;
        }

        private void UnSubscribeEvents()
        {
            _playerCollisionHandler.OnArenaExit -= PlayerCollisionHandlerOnOnArenaExit;
            _playerCollisionHandler.OnTakeEnergyHit -= TakeEnergyHit;
            _playerCollisionHandler.OnTakeLifeHit -= TakeLifeHit;
            _playerInputHandler.OnFirePressed -= Shot;
            _playerInputHandler.OnUltimatePressed -= TryUseUltimate;
            _heathProgressBarController.OnValueMinimum -= Death;
        }

        private void RotateCamera()
        {
            if (!(_playerInputHandler.LookDirection.sqrMagnitude >= Threshold))
                return;

            var deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetPitch += _playerInputHandler.LookDirection.y * RotationSpeed * deltaTimeMultiplier;
            _rotationVelocity = _playerInputHandler.LookDirection.x * RotationSpeed * deltaTimeMultiplier;
            _cinemachineTargetPitch = GameHelper.GetClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
            cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
            transform.Rotate(Vector3.up * _rotationVelocity);
        }

        private void Move()
        {
            var targetSpeed = MaxMoveSpeed;

            if (_playerInputHandler.MoveDirection == Vector2.zero)
                targetSpeed = 0.0f;

            var currentHorizontalSpeed = new Vector3(_characterController.velocity.x, 0.0f, _characterController.velocity.z).magnitude;
            var inputMagnitude =  1f;

            if (currentHorizontalSpeed < targetSpeed - SpeedOffset ||
                currentHorizontalSpeed > targetSpeed + SpeedOffset)
            {
                _currentMoveSpeed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);
                _currentMoveSpeed = Mathf.Round(_currentMoveSpeed * 1000f) / 1000f;
            }
            else
                _currentMoveSpeed = targetSpeed;

            var inputDirection =
                new Vector3(_playerInputHandler.MoveDirection.x, 0.0f, _playerInputHandler.MoveDirection.y).normalized;

            if (_playerInputHandler.MoveDirection != Vector2.zero)
                inputDirection = transform.right * _playerInputHandler.MoveDirection.x +
                                 transform.forward * _playerInputHandler.MoveDirection.y;

            _characterController.Move(inputDirection.normalized * (_currentMoveSpeed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }


        private void PlayerCollisionHandlerOnOnArenaExit() => Teleport().Forget();

        private void Shot()
        {
            var projectile = _gamePoolService.PlayerProjectilePool.Pool.Get();
            var canRicochet = GameHelper.ShouldRicochetTrigger(_heathProgressBarController.GetPercentValue());
            projectile.Shot(cinemachineCameraTarget.position, cinemachineCameraTarget.forward, canRicochet);
        }

        private void TryUseUltimate()
        {
            if (!_energyProgressBarController.IsMaxValue)
                return;

            _energyProgressBarController.SetValueAnimated(0);
            _gameFactory.GameController.EnemyController.KillAllEnemyByUltimate();
        }

        private void TakeLifeHit(int damage) =>
            _heathProgressBarController.AddValueAnimated(-damage);

        private void TakeEnergyHit(int damage) =>
            _energyProgressBarController.AddValueAnimated(-damage);

        private void Death()
        {
            UnSubscribeEvents();
            _playerInputHandler.IsEnable = false;
            _playerCollisionHandler.enabled = false;
            OnDeath?.Invoke();
        }

        private async UniTaskVoid Teleport()
        {
            _playerInputHandler.IsEnable = false;
            _characterController.enabled = false;
            await UniTask.DelayFrame(1);
            transform.position = _arenaBehaviour.GetRandomPositionInArenaWithoutEnemy();
            await UniTask.DelayFrame(1);
            _playerInputHandler.IsEnable = true;
            _characterController.enabled = true;
        }

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }
    }
}