using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class Player : Entity
{
    [SerializeField] private ThirdPersonCamera _cam;

    private Rigidbody _rb;
    private PlayerMovement _movement;
    private IAttack _primaryAttack;
    private IAttack _secondaryAttack;
    private Animator _animator;
    private bool _isPrimaryAttacking = false;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody>();
        _movement = GetComponent<PlayerMovement>();
        _primaryAttack = GetComponent<PrimaryAttack>();
        _secondaryAttack = GetComponent<SecondaryAttack>();
        _animator = GetComponent<Animator>();
        _movement.cameraTransform = _cam.transform;
    }

    private void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    private void HandleMovement()
    {
        Vector2 input = Vector2.zero;
        if (Keyboard.current.wKey.isPressed) input.y += 1f;
        if (Keyboard.current.sKey.isPressed) input.y -= 1f;
        if (Keyboard.current.aKey.isPressed) input.x -= 1f;
        if (Keyboard.current.dKey.isPressed) input.x += 1f;
        _movement.Move(input);
    }

    private void HandleAttack()
    {
        Vector3 direction = _cam.GetForwardDirection();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _isPrimaryAttacking = true;
            _animator.speed = 1f;
            _animator.SetTrigger("PrimaryAttack");
            _animator.SetBool("IsPrimaryAttacking", true);
        }

        if (Mouse.current.leftButton.isPressed)
        {
            _primaryAttack.Execute(direction);

            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("PrimaryAttack") && stateInfo.normalizedTime >= 0.95f)
                _animator.speed = 0f;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _isPrimaryAttacking = false;
            _animator.SetBool("IsPrimaryAttacking", false);
            _animator.speed = 1f;
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            _secondaryAttack.Execute(direction);
            _animator.SetTrigger("SecondaryAttack");
        }
    }

    private void OnEnable()
    {
        EventManager.SubscribeToEvent(GameEvents.Event_HealthOrbCollected, OnHealthOrbCollected);
        EventManager.SubscribeToEvent(GameEvents.Event_ApplyUpgrade, OnApplyUpgrade);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvents.Event_HealthOrbCollected, OnHealthOrbCollected);
        EventManager.Unsubscribe(GameEvents.Event_ApplyUpgrade, OnApplyUpgrade);
    }

    private void OnApplyUpgrade(params object[] parameters)
    {
        if (parameters == null || parameters[0] == null) return;

        bool isPrimary = (bool)parameters[0];
        float amount = (float)parameters[1];

        if (isPrimary)
            IncreasePrimaryDamage(amount);
        else
            IncreaseSecondaryDamage(amount);
    }

    private void OnHealthOrbCollected(params object[] parameters)
    {
        if (parameters == null || parameters[0] == null) return;

        float healAmount = (float)parameters[0];
        _life = Mathf.Min(_life + healAmount, maxLife); 

        EventManager.ExecuteEvent(GameEvents.Event_PlayerDamaged, _life, maxLife);
    }

    public override void Die()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("Lose");
    }
public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        EventManager.ExecuteEvent(GameEvents.Event_PlayerDamaged, _life, maxLife);
    }

    public void IncreasePrimaryDamage(float amount)
    {
        _primaryAttack.Initialize(_primaryAttack.GetDamage() + amount);
    }

    public void IncreaseSecondaryDamage(float amount)
    {
        _secondaryAttack.Initialize(_secondaryAttack.GetDamage() + amount);
    }
}