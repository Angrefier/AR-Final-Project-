using UnityEngine;
using Vuforia;

public class AbilityCardTarget : MonoBehaviour
{
    [Header("Game Manager")]
    public GameManager gameManager;

    [Header("Card Skill")]
    public SkillType cardSkill;

    [Header("Card Owner")]
    public PlayerSide cardOwner;

    private ObserverBehaviour observerBehaviour;
    private bool hasSentSelection = false;
    private bool isTracked = false;

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
        if (status.Status == Status.TRACKED)
        {
            isTracked = true;
        }
        else
        {
            isTracked = false;
            hasSentSelection = false;
            Debug.Log(name + " lost.");
        }
    }

    private void Update()
    {
        if (isTracked && !hasSentSelection && gameManager != null)
        {
            if (gameManager.IsSelectingSkills() && gameManager.GetCurrentSelectingPlayer() == cardOwner)
            {
                hasSentSelection = true;
                gameManager.SelectSkillForCurrentPlayer(cardSkill);
                Debug.Log(name + " detected and processed. Selected skill: " + cardSkill);
            }
        }
    }
}