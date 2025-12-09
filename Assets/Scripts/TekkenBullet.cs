using UnityEngine;

public class TekkenBullet : Bullet
{
    //deals double dmg that other bullets do, needs smth else tho
    public override int damage { get; set; } = 2;
    public int GoldCost = 12;
}
