using UnityEngine;

public class MatchaBullet : Bullet
{
    public EnemyChase EnemyChase;
    public int GoldCost = 9;
    // half their speed
    public void Reverse(Collider2D Other)
    {
        if (Other.CompareTag("Enemy"))
        {
            EnemyChase.set_moveSpeed(EnemyChase.moveSpeed / 2);
        }
    }
}
