using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Wizard Effects")]
    // Shield Idle
    public ParticleSystem shieldIdleEffectLeft;
    public ParticleSystem shieldIdleEffectRight;

    // Ice Attack
    public ParticleSystem iceAttackEffectLeft;
    public ParticleSystem iceAttackEffectRight;

    // Ice Idle
    public ParticleSystem iceIdleEffectLeft;
    public ParticleSystem iceIdleEffectRight;

    // Heal Idle
    public ParticleSystem healIdleEffectLeft;
    public ParticleSystem healIdleEffectRight;

    
    // Shield effect
    public ParticleSystem shieldEffect;

    // Heal Ground (Yerdeki çember)
    public ParticleSystem healGround;

   [Header("Crusader Effects")]
    public ParticleSystem crusShieldEffect;
    public ParticleSystem crusFireEffect; 
    public ParticleSystem healGroundCrus;


    void Start()


    {
        
    }

    void Update()
    {
        
    }
}
