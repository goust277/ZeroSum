using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_Attack : BaseState
{
    private Long l;
    private int fireCount = 0;       // �߻� Ƚ��
    private float timeSinceLastFire = 0f; // ������ �߻� �� ��� �ð�
    private float fireInterval = 0.1f; // �߻� ����
    private int maxFireCount = 3;    // �ִ� �߻� Ƚ��

    public L_Attack(StateMachine stateMachine, Long monster) : base(stateMachine)
    {
        this.l = monster;
    }

    public override void Enter()
    {
        Debug.Log("���� ����");

        fireCount = 0; // �߻� Ƚ�� �ʱ�ȭ
        timeSinceLastFire = 0f; // Ÿ�̸� �ʱ�ȭ
    }

    public override void Execute()
    {
        // �� �����Ӹ��� Ÿ�̸� ����
        timeSinceLastFire += Time.deltaTime;

        // �߻� ������ ������ �߻�
        if (timeSinceLastFire >= fireInterval && fireCount < maxFireCount)
        {
            FireBullet();
            timeSinceLastFire = 0f; // Ÿ�̸� �ʱ�ȭ
            fireCount++; // �߻� Ƚ�� ����
        }

        AnimatorStateInfo stateInfo = l.anim.GetCurrentAnimatorStateInfo(0);

        if (fireCount >= maxFireCount)
        {
            stateMachine.ChangeState(new L_Chase(stateMachine, l));
            return;
        }
    }
    public override void Exit()
    {
        l.anim.SetBool("isAttack", false);
        l.canAttack = true;
        l.canShot = false;
        l.attackCooldown = 3f;
    }

    private void FireBullet()
    {
        if (l.BulletPrefab != null)
        {
            // �߻�ü ����
            GameObject bullet = GameObject.Instantiate(l.BulletPrefab, l.laserStart, Quaternion.identity);

            // �߻� ���� ���
            Vector3 direction = (l.laserEnd - l.laserStart).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Rigidbody2D�� �̿��� �߻�ü �̵�
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * l.BulletSpeed; // �߻� �ӵ� ����
            }
        }
    }
}
