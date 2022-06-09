using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �κ��� �÷��̾� ������ ����ϴ� ������Ʈ
/// ������ ������ �ش� ������Ʈ�� ��ȯ�ȴ�.
/// </summary>
public class Robot_State_Chase : Interface_Base<Robot_Base>
{
    private bool isTarget = false;
    private float currentDist = 0;      //���� �Ÿ�
    private float closetDist = 100f;    //����� �Ÿ�
    private float targetDist = 100f;   //Ÿ�� �Ÿ�
    private int closeDistIndex = 0;    //���� ����� �ε���
    private int targetIndex = -1;      //Ÿ���� �� �ε���

    public void OnEnter(Robot_Base robot)
    {
        robot.isChasing = true;
        robot.StartMove();
    }

    //���ݹ����� ������ ������ �غ�, �ƴϸ� ��� �߰��Ѵ�.
    public void OnUpdate(Robot_Base robot)
    {
        UpdateTarget(robot);
        if (robot.target == null)
        {
            return;
        }

        float distance = (robot.target.position - robot.transform.position).sqrMagnitude;

        if (distance <= robot.attackRange * robot.attackRange &&
                        robot.attacking == false)
        {
            robot.ChangeState(Robot_Base.Robot_State.READY);
        }
        else
        {
            robot.navMeshAgent.SetDestination(robot.target.transform.position);
        }
    }

    public void OnExit(Robot_Base robot)
    {
        robot.isChasing = false;
        robot.StopMove();
    }

    public void OnFixedUpdate(Robot_Base robot)
    {


    }

    void UpdateTarget(Robot_Base robot)
    {
        if (robot.target)
            return;

        Collider[] enemies = Physics.OverlapSphere(robot.transform.position, robot.SightRange, robot.layerMask);

        float shortestDistance = Mathf.Infinity;
        Collider nearestEnemy = null;

        foreach (Collider enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(robot.transform.position, enemy.transform.position);
            if (distanceToEnemy <= shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= robot.SightRange)
        {
            robot.target = nearestEnemy.transform;

        }
        else
        {
            robot.target = null;
        }
    }
}
