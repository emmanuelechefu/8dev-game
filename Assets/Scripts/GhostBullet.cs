using UnityEngine;

public class GhostBullets : Bullet
{
	public EnemyChase EnemyChase;
	public int GoldCost = 9;
	// makes enemy run away
	public void Reverse(Collider2D Other)
	{	
		if (Other.CompareTag("Enemy"))
		{
			EnemyChase.set_moveSpeed(EnemyChase.moveSpeed * -1);
		}
	}
}
