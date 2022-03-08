using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    private Player player;
    private Enemy enemy;
    
    
    public void RandomAttackAnimation()
    {
        if (player == null && enemy == null)
        {
             if (player == null) player = gameObject.GetComponentInParent<Player>();
             if (enemy == null) enemy = gameObject.GetComponentInParent<Enemy>();
        }
       
        if(player != null) player.RandomAnimation();
        if(enemy != null) enemy.RandomAnimation();
    }

    public void Attack()
    {
        if (player == null && enemy == null)
        {
            if (player == null) player = gameObject.GetComponentInParent<Player>();
            if (enemy == null) enemy = gameObject.GetComponentInParent<Enemy>();
        }
        if(player != null) player.Attack();
        if(enemy != null) enemy.Attack();
    }
}
