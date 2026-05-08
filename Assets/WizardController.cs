using UnityEngine;

public class WizardController : MonoBehaviour
{
    public GameObject wizard;
    public Animator wizardAnimator;
    // Wizard Stats
    public int maxHealth = 3;
    private int currentHealth;
    
    // Wizard Particles
    public ParticleSystem iceAttackParticle;
    public ParticleSystem shieldDefenceParticle;
    public ParticleSystem getHitParticle;

    private bool isDead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        if (wizardAnimator == null)
        {
            wizardAnimator = wizard.GetComponent<Animator>();
        }
        
        wizardAnimator.SetBool("isDead", false);
        wizardAnimator.SetBool("isSkillActive", false);
        wizardAnimator.SetInteger("skillType", 0);

    }

    public void UseIceAttack()
    {
        if (isDead) return;

        wizardAnimator.SetBool("isSkillActive", true);
        wizardAnimator.SetInteger("skillType", 1);
        wizardAnimator.SetTrigger("attack");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
