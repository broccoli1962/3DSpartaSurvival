using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyBoss : MonoBehaviour
{
    public Transform player;

    public MonsterData monsterSO;
    public GameObject hitBox;

    [Header("Skill 1 Setting")] //���߿� So���Ϸ� �������� ���°� ��������?
    public float skill1Range = 10f;
    public float skill1CoolTime = 10f;
    public float skill1Speed = 10f;
    public float skill1Duration = 10f;
    public float skill1StopDistance = 1f;
    public float skill1DamageRadius = 3f; // ���� ���� �� ���ظ� �� ����
    public float skill1Damage = 20f;
    private float _skill1Timer = 0f;

    [Header("Skill 2 Setting")]
    public float skill2Range = 10f;
    public float skill2CoolTime = 10f;
    private float _skill2Timer = 0f;


    private Rigidbody _rigid;
    private Animator _anim;

    protected FiniteStateMachine _fsm;
    protected Dictionary<EState, IState> _states;
    protected List<Collider> _hitTargets = new();

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();

        _states = new Dictionary<EState, IState>()
        {
            { EState.Move, new MoveState(this) },
            { EState.Wait, new WaitState(this) },
            { EState.Attack, new AttackState(this) },
            { EState.Skill1, new Skill1State(this) }
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
    
        //��ų ��Ÿ�� ���
        if(skill1CoolTime > 0)
        {
            _skill1Timer -= Time.deltaTime;
        }

        if (skill2CoolTime > 0) {
            _skill2Timer -= Time.deltaTime;
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
}