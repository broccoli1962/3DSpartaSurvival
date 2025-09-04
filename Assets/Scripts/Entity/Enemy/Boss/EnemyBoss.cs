using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyBoss : MonoBehaviour
{
    public Transform player;

    public MonsterData monsterSO;

    [Header("Skill 1 Setting")] //나중에 So파일로 설정값을 빼는게 좋을지도?
    public float skill1Range = 10f;
    public float skill1CoolTime = 10f;
    private float _skill1Timer = 0f;

    [Header("Skill 2 Setting")]
    public float skill2Range = 10f;
    public float skill2CoolTime = 10f;
    private float _skill2Timer = 0f;


    private Rigidbody _rigid;
    private Animator _anim;

    protected FiniteStateMachine _fsm;
    protected Dictionary<EState, IState> _states;

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
    
        //스킬 쿨타임 계산
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

    protected void OnEnableAttack()
    {

    }

    protected void OnDisableAttack()
    {
        
    }
}