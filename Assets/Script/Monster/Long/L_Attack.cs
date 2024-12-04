using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_Attack : BaseState
{
    private Long l;
    private int fireCount = 0;       // 발사 횟수
    private float timeSinceLastFire = 0f; // 마지막 발사 후 경과 시간
    private float fireInterval = 0.1f; // 발사 간격
    private int maxFireCount = 3;    // 최대 발사 횟수

    public L_Attack(StateMachine stateMachine, Long monster) : base(stateMachine)
    {
        this.l = monster;
    }

    public override void Enter()
    {
        Debug.Log("공격 상태");

        fireCount = 0; // 발사 횟수 초기화
        timeSinceLastFire = 0f; // 타이머 초기화
    }

    public override void Execute()
    {
        // 매 프레임마다 타이머 증가
        timeSinceLastFire += Time.deltaTime;

        // 발사 간격이 지나면 발사
        if (timeSinceLastFire >= fireInterval && fireCount < maxFireCount)
        {
            FireBullet();
            timeSinceLastFire = 0f; // 타이머 초기화
            fireCount++; // 발사 횟수 증가
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
            // 발사체 생성
            GameObject bullet = GameObject.Instantiate(l.BulletPrefab, l.laserStart, Quaternion.identity);

            // 발사 방향 계산
            Vector3 direction = (l.laserEnd - l.laserStart).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Rigidbody2D를 이용해 발사체 이동
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * l.BulletSpeed; // 발사 속도 설정
            }
        }
    }
}
