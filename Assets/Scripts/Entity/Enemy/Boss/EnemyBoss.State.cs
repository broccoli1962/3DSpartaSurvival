using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public partial class EnemyBoss : MonoBehaviour
{
    #region �̵�����
    public class MoveState : BaseState<EnemyBoss>
    {
        public MoveState(EnemyBoss component) : base(component) { }

        public override void End()
        {
            StopAnimation(AnimParam.Move);
        }

        public override void HandleUpdate()
        {
        }

        public override void PhysicsUpdate()
        {
            EnemyMovement();
        }

        public override void Start()
        {
            Debug.Log("�̵� ����");
            if (Component.player == null)
                Component._fsm.ChangeTo(EState.Wait);

            StartAnimation(AnimParam.Move);
        }

        public override void Update()
        {
            if (Component.player == null) return;

            if (GetDistance() <= Component.skill1Range && Component._skill1Timer <= 0)
            {
                Component._fsm.ChangeTo(EState.Skill1);
                return;
            }

            // 2. ��ų 2 ��� ����
            //if (GetDistance() <= Component.skill2Range && Component._skill2Timer <= 0)
            //{
            //    Component._fsm.ChangeTo(EState.Skill2);
            //    return;
            //}

            if (GetDistance() < Component.monsterSO.attackRange) //�����Ÿ� ���� ����.
            {
                Component._fsm.ChangeTo(EState.Attack);
            }
        }

        public float GetDistance() //�ߺ��Ǵ� ��찡 ���Ƽ� �и� ����
        {
            if (Component.player == null) return 0;
            return Vector3.Distance(Component.player.transform.position, Component.transform.position);
        }

        public void EnemyMovement()
        {
            //����
            Vector3 targetPos = Component.player.transform.position;
            Vector3 dir = (targetPos - Component.transform.position).normalized;
            Component._rigid.MovePosition(Component.transform.position + dir * Component.monsterSO.moveSpeed * Time.deltaTime);

            //ȸ��
            Quaternion targetRot = Quaternion.LookRotation(dir);
            Component._rigid.MoveRotation(Quaternion.Slerp(Component.transform.rotation, targetRot, Component.monsterSO.moveSpeed * Time.deltaTime));
        }
    }
    #endregion

    #region ������
    public class WaitState : BaseState<EnemyBoss>
    {
        float timer = 0f;
        float waitTimer = 1f;
        public WaitState(EnemyBoss component) : base(component) { }

        public override void End()
        {
            StopAnimation(AnimParam.Idle);
        }

        public override void HandleUpdate()
        {
        }

        public override void PhysicsUpdate()
        {
           
        }

        public override void Start()
        {
            Debug.Log("��� ����");
            timer = 0f;
            StartAnimation(AnimParam.Idle);
        }

        public override void Update()
        {
            timer += Time.deltaTime;

            if(timer > waitTimer)
            {
                if (Component.player != null)
                {
                    Component._fsm.ChangeTo(EState.Move);
                }
            }
        }
    }
    #endregion

    #region ���ݻ���
    public class AttackState : BaseState<EnemyBoss>
    {
        public AttackState(EnemyBoss component) : base(component) { }

        public override void End()
        {
            StopAnimation(AnimParam.Attack);
        }

        public override void HandleUpdate()
        {
            
        }

        public override void PhysicsUpdate()
        {
            GetRot();
        }

        public override void Start()
        {
            Debug.Log("���� ����");
            StartAnimation(AnimParam.Attack);
        }

        public override void Update()
        {
            if (GetDistance() > Component.monsterSO.attackRange)
            {
                Component._fsm.ChangeTo(EState.Wait);
            }
        }

        public float GetDistance()
        {
            if (Component.player == null) return 0;
            return Vector3.Distance(Component.player.transform.position, Component.transform.position);
        }

        public void GetRot()
        {
            Vector3 targetPos = Component.player.transform.position;
            Vector3 dir = (targetPos - Component.transform.position).normalized;
            Quaternion targetRot = Quaternion.LookRotation(dir);
            Component._rigid.MoveRotation(Quaternion.Slerp(Component.transform.rotation, targetRot, Component.monsterSO.moveSpeed * Time.deltaTime));
        }
    }
    #endregion

    //���� ��ų
    #region ��ų1
    public class Skill1State : BaseState<EnemyBoss>
    {
        private float _timer;
        private bool _rush;
        Vector3 attackPos;

        public Skill1State(EnemyBoss component) : base(component) { }

        public override void End()
        {
            StopAnimation(AnimParam.Skill1);
        }

        public override void HandleUpdate()
        {
        }

        public override void PhysicsUpdate()
        {
            if (_rush == true)
            {
                float distance = Vector3.Distance(Component.transform.position, attackPos);

                if(distance <= Component.skill1StopDistance)
                {
                    DamageArea();
                    Component._fsm.ChangeTo(EState.Wait);
                    return;
                }

                EnemySkill1();
            }
            else
            {
                GetRot();
                attackPos = Component.player.transform.position;
            }
        }

        public override void Start()
        {
            _timer = 0;
            _rush = false;
            
            Component._skill1Timer = Component.skill1CoolTime;
            StartAnimation(AnimParam.Skill1);
        }

        public override void Update()
        {
            _timer += Time.deltaTime;

            if (_timer > Component.skill1Duration)
            {
                Component._fsm.ChangeTo(EState.Wait);
            }
        }

        //���� ���� ����
        public void EnemySkill1()
        {
            Vector3 dir = (attackPos - Component.transform.position).normalized;
            Component.transform.position += dir * Component.skill1Speed * Time.deltaTime;
        }

        //���� ���� �� ���� ����
        public void DamageArea() //�ߵ��ɶ� ��ƼŬ �ʿ��ҵ�
        {
            Collider[] hitTargets = Physics.OverlapSphere(Component.transform.position, Component.skill1DamageRadius);

            foreach (var hit in hitTargets) {
                if (hit.TryGetComponent<IDamagable>(out var target)) {
                    if (hit.gameObject == Component.gameObject) continue;
                    target.ValueChanged(-Component.skill1Damage);
                }
            }
        }

        public void GetRot()
        {
            Vector3 targetPos = Component.player.transform.position;
            Vector3 dir = (targetPos - Component.transform.position).normalized;
            Quaternion targetRot = Quaternion.LookRotation(dir);
            Component._rigid.MoveRotation(Quaternion.Slerp(Component.transform.rotation, targetRot, Component.monsterSO.moveSpeed * Time.deltaTime));
        }

        public override void OnAnimationEvent()
        {
            _rush = true;
        }
    }
    #endregion
}
