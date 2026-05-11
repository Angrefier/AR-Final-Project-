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
    //Stats
    public int maxHealth=100;
    public int currentHealth;

    //Animator
    public Animator animator;

    protected bool isDead = false;
    protected bool defenceActive = false;
    protected SkillType selectedSkill = SkillType.None;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        currentHealth = maxHealth;

        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }

        SetNormalIdle();
    }

    public void SelectSkill(SkillType skill)
    {

        selectedSkill = skill;
        animator.SetBool("isSkillActive", true);

    }

    public void UseSelectedSkill(){

        if (selectedSkill == SkillType.None)
        {
            Debug.Log(name + " has no selected skill.");
            return;
        }

        UseSkill(selectedSkill);
    }

    protected abstract void UseSkill(SkillType skill);

    public virtual void GetHit(int damage){

        if (defenceActive)
        {
            Defence();
            defenceActive = false;
            return;
        }

        currentHealth -= damage;
        Debug.Log(name + " takes " + damage + " damage. Current health: " + currentHealth);

        if (currentHealth < 0) currentHealth = 0;

        if (currentHealth <= 0)
        {
            Die();
        }else{
            animator.SetTrigger("getHit");
        }
    }

    protected virtual void Defence(){
        defenceActive = true;
        Debug.Log(name + " raises a shield!");
        animator.SetTrigger("defence");
    }
    protected virtual void Heal(int amount){
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        animator.SetTrigger("getHeal");
        Debug.Log(name + " heals for " + amount + ". Current health: " + currentHealth);
    }

    protected virtual void Attack(){
        if (isDead)
        {
            Debug.Log("Game Over");
            return;
        }

        Debug.Log(name + " performs an attack!");
        animator.SetTrigger("attack");
    }

    protected virtual void Die()
    {
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

    public bool IsDead()
    {
        return isDead;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
