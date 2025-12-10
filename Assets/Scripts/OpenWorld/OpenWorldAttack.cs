using UnityEngine;

public class OpenWorldAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    private Animator anim;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] arrows;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.J) && cooldownTimer > attackCooldown)
            Attack();

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        //pool fireball
        int arrowIndex = FindArrow();
        arrows[arrowIndex].transform.position = firePoint.position;
        arrows[arrowIndex].GetComponent<OpenWorldProjectile>().SetDirection(Mathf.Sign(transform.localScale.x));
        
    }

    private int FindArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
                return i;
        }

        return 0;
    }
}
