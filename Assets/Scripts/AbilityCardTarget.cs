using UnityEngine;
using Vuforia;

public class AbilityCardTarget : MonoBehaviour
{
    [Header("Game Manager")]
    public GameManager gameManager;

    [Header("Card Skill")]
    public SkillType cardSkill;

    private ObserverBehaviour observerBehaviour;
    private bool hasSentSelection = false;

    private void Awake()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();

        if (observerBehaviour != null)
        {
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }
        else
        {
            Debug.LogError(name + " has no ObserverBehaviour component.");
        }
    }

    private void OnDestroy()
    {
        if (observerBehaviour != null)
        {
            observerBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            OnCardFound();
        }
        else
        {
            OnCardLost();
        }
    }

    private void OnCardFound()
    {
        if (hasSentSelection)
        {
            return;
        }

        hasSentSelection = true;

        if (gameManager != null)
        {
            gameManager.SelectSkillForCurrentPlayer(cardSkill);
            Debug.Log(name + " detected. Selected skill: " + cardSkill);
        }
        else
        {
            Debug.LogError(name + " has no GameManager assigned.");
        }
    }

    private void OnCardLost()
    {
        hasSentSelection = false;

        Debug.Log(name + " lost.");
    }
}