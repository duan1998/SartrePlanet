using Battle;
using UnityEngine;
public class Bullet : MonoBehaviour
{
    public int Damage;
    public int bulletColor;
    public Vector3 Direction;
    public float BulletSpeed;
    private void Awake()
    {
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector3.up * Time.fixedDeltaTime*BulletSpeed, Space.Self);
    }
    public void FadeAway()
    {
        Destroy(gameObject);
    }
    public void Absorb()
    {
        BasePlayer player = BasePlayer.Player;
        player.WhenEnergyChange(bulletColor, Damage);
        FadeAway();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Block"))
            Destroy(gameObject);
        if (collision.CompareTag("Player"))
        {
            BasePlayer player = BasePlayer.Player;
            if (bulletColor == player.colorStatus )
            {
                player.WhenEnergyChange(bulletColor,Damage);
            }
            else
            {
                player.Hurt(Damage);
            }
            FadeAway();
        }
    }
}
