using System.Collections;
using UnityEngine;

public enum SkillType
{
    None = 0,
    Attack = 1,
    Shield = 2,
    Heal = 3
}

public abstract class Character : MonoBehaviour
{
    // Stats
    public int maxHealth = 100;
    public int currentHealth;

    // Animator
    public Animator animator;

    protected bool isDead = false;
    protected bool defenceActive = false;
    protected SkillType selectedSkill = SkillType.None;

    // Return Back to Idle State after using a skill
    public float returnToIdleDelay = 1.5f;

    protected virtual void Start()
    {
        currentHealth = maxHealth;

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (animator == null)
        {
            Debug.LogError(name + " has no Animator component.");
            return;
        }

        SetNormalIdle();

        animator.SetBool("isDead", false);
    }

    public virtual void SelectSkill(SkillType skill)
    {
        if (isDead)
        {
            Debug.Log(name + " is dead and cannot select a skill.");
            return;
        }

        selectedSkill = skill;

        if (selectedSkill == SkillType.None)
        {
            SetNormalIdle();
            return;
        }

        animator.SetBool("isSkillActive", true);

        Debug.Log(name + " selected skill: " + selectedSkill);
    }

    public virtual void UseSelectedSkill()
    {
        if (isDead)
        {
            Debug.Log(name + " is dead and cannot use a skill.");
            return;
        }

        if (selectedSkill == SkillType.None)
        {
            Debug.Log(name + " has no selected skill.");
            return;
        }

        UseSkill(selectedSkill);

        StartCoroutine(ReturnToNormalIdleAfterDelay(returnToIdleDelay));
    }

    protected abstract void UseSkill(SkillType skill);

    public virtual void GetHit(int damage)
    {
        if (isDead)
        {
            return;
        }

        if (defenceActive)
        {
            BlockDamage();
            return;
        }

        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        Debug.Log(name + " takes " + damage + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("getHit");
        }
    }

    protected virtual void Attack()
    {
        if (isDead)
        {
            return;
        }

        Debug.Log(name + " performs an attack.");

        animator.SetTrigger("attack");
    }

    protected virtual void Defence()
    {
        if (isDead)
        {
            return;
        }

        defenceActive = true;

        Debug.Log(name + " activates defence.");

        animator.SetTrigger("defence");
    }

    protected virtual void BlockDamage()
    {
        defenceActive = false;

        Debug.Log(name + " blocked the attack with defence.");
    }

    protected virtual void Heal(int amount)
    {
        if (isDead)
        {
            return;
        }

        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        animator.SetTrigger("getHeal");

        Debug.Log(name + " heals for " + amount + ". Current health: " + currentHealth);
    }

    protected virtual void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        animator.SetBool("isDead", true);

        Debug.Log(name + " has died.");
    }

    public virtual void SetNormalIdle()
    {
        selectedSkill = SkillType.None;

        if (animator != null)
        {
            animator.SetBool("isSkillActive", false);
        }
    }

    protected IEnumerator ReturnToNormalIdleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!isDead)
        {
            SetNormalIdle();
        }
    }

    public virtual void EndRound()
    {
        if (isDead)
        {
            return;
        }

        defenceActive = false;

        SetNormalIdle();

        Debug.Log(name + " round ended. Defence is now inactive.");
    }

    public bool IsDead()
    {
        return isDead;
    }

    public bool IsDefenceActive()
    {
        return defenceActive;
    }

    public SkillType GetSelectedSkill()
    {
        return selectedSkill;
    }
}
