using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyBoss : MonoBehaviour, IDamagable
{
    public Player player;

    public MonsterData monsterSO;
    public GameObject hitBox;
    public float CurrentHealth { get; private set; }

    [Header("Skill 1 Setting")] //나중에 So파일로 설정값을 빼는게 좋을지도?
    public float skill1Range = 10f;
    public float skill1CoolTime = 10f;
    public float skill1Speed = 10f;
    public float skill1Duration = 10f;
    public float skill1StopDistance = 10f;
    public float skill1DamageRadius = 5f; // 돌진 도착 시 피해를 줄 범위
    public float skill1Damage = 20f;
    public GameObject skill1Effect;
    private float _skill1Timer = 0f;

    private Rigidbody _rigid;
    private Animator _anim;

    protected FiniteStateMachine _fsm;
    protected Dictionary<EState, IState> _states;
    protected List<Collider> _hitTargets = new();

    private void Awake()
    {
        player = PlayerManager.Instance.Player;
        CurrentHealth = monsterSO.maxHealth;
        _rigid = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();

        _states = new Dictionary<EState, IState>()
        {
            { EState.Move, new MoveState(this) },
            { EState.Wait, new WaitState(this) },
            { EState.Attack, new AttackState(this) },
            { EState.Skill1, new Skill1State(this) },
            { EState.Death, new DeathState(this) }
        };

        _fsm = new FiniteStateMachine(_states);
    }

    private void Start()
    {
        _fsm.ChangeTo(EState.Move);
    }


    private void Update()
    {
        _fsm.HandleUpdate();
        _fsm.Update();
    
        //스킬 쿨타임 계산
        if(skill1CoolTime > 0)
        {
            _skill1Timer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        _fsm.PhysicsUpdate();
    }

    public void OnAnimationTrigger()
    {
        _fsm.HandleAnimation();
    }

    public void OnEnableHitBox()
    {
        _hitTargets.Clear();
        if(hitBox != null)
        {
            hitBox.SetActive(true);
        }
    }

    public void OnDisableHitBox()
    {
        if (hitBox != null)
        {
            hitBox.SetActive(false);
        }
    }

    public void OnHitBox(Collider other)
    {
        if (_hitTargets.Contains(other)) return;

        if(other.TryGetComponent<IDamagable>(out IDamagable target))
        {
            target.ValueChanged(-monsterSO.attackPower);
            _hitTargets.Add(other);
        }
    }

    public float ValueChanged(float value) //데미지 체크
    {
        CurrentHealth += value;

        if (CurrentHealth <= 0)
        {
            _fsm.ChangeTo(EState.Death);
        }

        return CurrentHealth;
    }
}