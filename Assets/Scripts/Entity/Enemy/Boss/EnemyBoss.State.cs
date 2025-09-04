using System.Collections;
using System.Collections.Generic;
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
            Debug.Log("������ ���� ��");
        }

        public override void HandleUpdate()
        {
            EnemyMovement();
        }

        public override void PhysicsUpdate()
        {
        }

        public override void Start()
        {
            if (Component.player == null)
                Component._fsm.ChangeTo(EState.Wait);

            StartAnimation(AnimParam.Attack);
        }

        public override void Update()
        {
            if (Component.player == null) return;


            if (GetDistance() <= Component.monsterSO.attackRange) //�����Ÿ� ���� ����.
            {
                Component._fsm.ChangeTo(EState.Attack);
            }
        }

        public float GetDistance()
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

        public override void StartAnimation(int animationHash)
        {
            base.StartAnimation(animationHash);
        }

        public override void StopAnimation(int animationHash)
        {
            base.StopAnimation(animationHash);
        }
    }
    #endregion

    #region ������
    public class WaitState : BaseState<EnemyBoss>
    {
        public WaitState(EnemyBoss component) : base(component) { }

        public override void End()
        {
        }

        public override void HandleUpdate()
        {
        }

        public override void PhysicsUpdate()
        {
        }

        public override void Start()
        {
        }

        public override void Update()
        {
        }
    }
    #endregion

    #region ���ݻ���
    public class AttackState : BaseState<EnemyBoss>
    {
        public AttackState(EnemyBoss component) : base(component) { }

        public override void End()
        {
        }

        public override void HandleUpdate()
        {
        }

        public override void PhysicsUpdate()
        {
        }

        public override void Start()
        {
            Debug.Log("���� ���� ������");
        }

        public override void Update()
        {
        }
    }
    #endregion

    #region ��ų1
    public class SkillState : BaseState<EnemyBoss>
    {
        public SkillState(EnemyBoss component) : base(component) { }

        public override void End()
        {
        }

        public override void HandleUpdate()
        {
        }

        public override void PhysicsUpdate()
        {
        }

        public override void Start()
        {
        }

        public override void Update()
        {
        }
    }
    #endregion
}
