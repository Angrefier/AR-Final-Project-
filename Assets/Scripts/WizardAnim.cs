using UnityEngine;

public class WizardAnim : MonoBehaviour
{


    public class WizardAnimations : MonoBehaviour
    {
        [Header("Wizard Animator")]
        public Animator wizardAnim;

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
        public ParticleSystem healGroundEffect;

        public void BuzAt()
        {
            wizardAnim.SetTrigger("makeIceAttack");
            if (iceAttackEffectLeft != null) iceAttackEffectLeft.Play();
            if (iceAttackEffectRight != null) iceAttackEffectRight.Play();
        }

        public void CanBas()
        {
            wizardAnim.SetTrigger("makeHeal");
            if (healGroundEffect != null) healGroundEffect.Play();
        }

        public void BeklemeAnimasyonu()
        {
            // Örnek: Vuforia kartý çýkýnca idle'a dönmek için
            wizardAnim.SetTrigger("makeIdle");
        }
    }
}
