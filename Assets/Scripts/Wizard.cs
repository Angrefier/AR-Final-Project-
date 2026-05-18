using UnityEngine;

public class Wizard : Character
{
    [Header("Wizard Skill Idle Particles")]
    public GameObject attackIdleRightParticle;
    public GameObject attackIdleLeftParticle;
    public GameObject defenceIdleRightParticle;
    public GameObject defenceIdleLeftParticle;
    public GameObject healIdleRightParticle;
    public GameObject healIdleLeftParticle;

    [Header("Wizard Action Particles")]
    public ParticleSystem attackRightParticle;
    public ParticleSystem attackLeftParticle;
    public ParticleSystem defenceParticle;
    public ParticleSystem healParticle;
    public ParticleSystem hitParticle;
    public ParticleSystem deathParticle;

    [Header("Wizard Audio")]
    public AudioSource audioSource;
    public AudioClip attackClip;
    public AudioClip defenceClip;
    public AudioClip healClip;
    public AudioClip hitClip;
    public AudioClip blockClip;
    public AudioClip deathClip;

    [Header("Wizard Skill Values")]
    public int attackDamage = 25;
    public int healAmount = 20;

    protected override void Start()
    {
        base.Start();

        if (audioSource == null)
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }

        CloseAllIdleParticles();
        CloseAllActionParticles();
    }

    public override void SelectSkill(SkillType skill)
    {
        base.SelectSkill(skill);

        CloseAllIdleParticles();

        if (skill == SkillType.Attack)
        {
            if (attackIdleRightParticle != null)
            {
                attackIdleRightParticle.SetActive(true);
            }
            if (attackIdleLeftParticle != null)
            {
                attackIdleLeftParticle.SetActive(true);
            }
        }
        else if (skill == SkillType.Shield)
        {
            if (defenceIdleRightParticle != null)
            {
                defenceIdleRightParticle.SetActive(true);
            }
            if (defenceIdleLeftParticle != null)
            {
                defenceIdleLeftParticle.SetActive(true);
            }
        }
        else if (skill == SkillType.Heal)
        {
            if (healIdleRightParticle != null)
            {
                healIdleRightParticle.SetActive(true);
            }
            if (healIdleLeftParticle != null)
            {
                healIdleLeftParticle.SetActive(true);
            }
        }
    }

    protected override void UseSkill(SkillType skill)
    {
        switch (skill)
        {
            case SkillType.Attack:
                UseWizardAttack();
                break;

            case SkillType.Shield:
                UseWizardDefence();
                break;

            case SkillType.Heal:
                UseWizardHeal();
                break;

            default:
                Debug.Log("Wizard has no valid skill selected.");
                break;
        }
    }

    private void UseWizardAttack()
    {
        Attack();

        PlayAttackParticles();

        PlaySound(attackClip);

        Debug.Log("Wizard used Attack.");
    }

    public void PlayAttackParticles()
    {
        if (attackRightParticle != null)
        {
            attackRightParticle.gameObject.SetActive(true);
            attackRightParticle.Play();
        }
        if (attackLeftParticle != null)
        {
            attackLeftParticle.gameObject.SetActive(true);
            attackLeftParticle.Play();
        }
    }

    private void UseWizardDefence()
    {
        Defence();

        if (defenceParticle != null)
        {
            defenceParticle.gameObject.SetActive(true);
            defenceParticle.Play();
        }

        PlaySound(defenceClip);

        Debug.Log("Wizard used Defence.");
    }

    private void UseWizardHeal()
    {
        Heal(healAmount);

        if (healParticle != null)
        {
            healParticle.gameObject.SetActive(true);
            healParticle.Play();
        }

        PlaySound(healClip);

        Debug.Log("Wizard used Heal.");
    }

    public override void GetHit(int damage)
    {
        bool hadDefence = defenceActive;
        int healthBeforeHit = currentHealth;

        base.GetHit(damage);

        if (hadDefence)
        {
            PlaySound(blockClip);
            return;
        }

        if (!isDead && currentHealth < healthBeforeHit)
        {
            if (hitParticle != null)
            {
                hitParticle.gameObject.SetActive(true);
                hitParticle.Play();
            }

            PlaySound(hitClip);
        }
    }

    protected override void Die()
    {
        base.Die();

        CloseAllIdleParticles();
        CloseAllActionParticles();

        if (deathParticle != null)
        {
            deathParticle.gameObject.SetActive(true);
            deathParticle.Play();
        }

        PlaySound(deathClip);
    }

    public override void SetNormalIdle()
    {
        base.SetNormalIdle();
        CloseAllIdleParticles();
        CloseAllActionParticles();
    }

    private void CloseAllActionParticles()
    {
        if (attackRightParticle != null) { attackRightParticle.Stop(); attackRightParticle.gameObject.SetActive(false); }
        if (attackLeftParticle != null) { attackLeftParticle.Stop(); attackLeftParticle.gameObject.SetActive(false); }
        if (defenceParticle != null) { defenceParticle.Stop(); defenceParticle.gameObject.SetActive(false); }
        if (healParticle != null) { healParticle.Stop(); healParticle.gameObject.SetActive(false); }
        if (hitParticle != null) { hitParticle.Stop(); hitParticle.gameObject.SetActive(false); }
        if (deathParticle != null) { deathParticle.Stop(); deathParticle.gameObject.SetActive(false); }
    }

    private void CloseAllIdleParticles()
    {
        if (attackIdleRightParticle != null)
        {
            attackIdleRightParticle.SetActive(false);
        }
        if (attackIdleLeftParticle != null)
        {
            attackIdleLeftParticle.SetActive(false);
        }

        if (defenceIdleRightParticle != null)
        {
            defenceIdleRightParticle.SetActive(false);
        }
        if (defenceIdleLeftParticle != null)
        {
            defenceIdleLeftParticle.SetActive(false);
        }

        if (healIdleRightParticle != null)
        {
            healIdleRightParticle.SetActive(false);
        }
        if (healIdleLeftParticle != null)
        {
            healIdleLeftParticle.SetActive(false);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public int GetAttackDamage()
    {
        return attackDamage;
    }
}