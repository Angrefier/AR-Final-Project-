using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum PlayerSide
{
    Wizard,
    Crusader
}

public class GameManager : MonoBehaviour
{
    [Header("Characters")]
    public Wizard wizard;
    public Crusader crusader;

    [Header("UI Texts")]
    public TextMeshProUGUI wizardDiceText;
    public TextMeshProUGUI crusaderDiceText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI turnOrderText;
    public TextMeshProUGUI currentSelectorText;
    public TextMeshProUGUI statusText;

    [Header("UI Buttons")]
    public Button startGameButton;
    public Button resolveRoundButton;

    [Header("Timing")]
    public float damageApplyDelay = 0.6f;
    public float timeBetweenActions = 1.6f;

    [Header("Options")]
    public bool autoResolveWhenBothSelected = false;

    private int roundNumber = 0;

    private PlayerSide actionFirstPlayer;
    private PlayerSide currentSelectingPlayer;

    private bool gameStarted = false;
    private bool wizardHasSelected = false;
    private bool crusaderHasSelected = false;
    private bool isResolvingRound = false;
    private bool gameOver = false;

    private void Start()
    {
        SetButtonInteractable(resolveRoundButton, false);
        SetButtonInteractable(startGameButton, true);

        SetButtonVisible(startGameButton, true);

        if (wizardDiceText != null)
        {
            wizardDiceText.text = "-";
        }

        if (crusaderDiceText != null)
        {
            crusaderDiceText.text = "-";
            crusaderDiceText.fontSize *= 2;
        }

        if (wizardDiceText != null)
        {
            wizardDiceText.fontSize *= 2;
        }

        if (roundText != null)
        {
            roundText.text = "Round: -";
        }

        if (turnOrderText != null)
        {
            turnOrderText.text = "Turn Order: -";
            turnOrderText.fontSize *= 2;
        }

        if (currentSelectorText != null)
        {
            currentSelectorText.text = "Current Selector: -";
            currentSelectorText.fontSize *= 2;
        }

        if (statusText != null)
        {
            statusText.fontSize *= 2;
        }

        UpdateStatus("Press Start Game to begin.");
    }

    public void StartGame()
    {
        if (gameStarted || gameOver || isResolvingRound)
        {
            return;
        }

        RollInitialDice();

        roundNumber = 1;
        gameStarted = true;

        PrepareNewRound();

        SetButtonVisible(startGameButton, false);

        UpdateStatus(GetPlayerName(actionFirstPlayer) + " starts the game.");
    }

    private void RollInitialDice()
    {
        int wizardRoll;
        int crusaderRoll;
        int rerollCount = 0;

        do
        {
            wizardRoll = Random.Range(1, 7);
            crusaderRoll = Random.Range(1, 7);
            rerollCount++;

        } while (wizardRoll == crusaderRoll);

        if (wizardDiceText != null)
        {
            wizardDiceText.text = wizardRoll.ToString();
        }

        if (crusaderDiceText != null)
        {
            crusaderDiceText.text = crusaderRoll.ToString();
        }

        if (wizardRoll > crusaderRoll)
        {
            actionFirstPlayer = PlayerSide.Wizard;
        }
        else
        {
            actionFirstPlayer = PlayerSide.Crusader;
        }

        if (rerollCount > 1)
        {
            Debug.Log("Dice were equal. Dice rolled again automatically.");
        }
    }

    private void PrepareNewRound()
    {
        wizardHasSelected = false;
        crusaderHasSelected = false;
        isResolvingRound = false;

        currentSelectingPlayer = actionFirstPlayer;

        SetButtonInteractable(resolveRoundButton, false);

        if (roundText != null)
        {
            roundText.text = "Round: " + roundNumber;
        }

        UpdateTurnOrderText();
        UpdateCurrentSelectorText();

        UpdateStatus(GetPlayerName(currentSelectingPlayer) + " selects a skill first.");
    }

    public void SelectSkillForCurrentPlayer(SkillType skill)
    {
        if (!gameStarted)
        {
            UpdateStatus("Start the game first.");
            return;
        }

        if (gameOver || isResolvingRound)
        {
            return;
        }

        Character currentCharacter = GetCharacter(currentSelectingPlayer);

        if (currentCharacter == null || currentCharacter.IsDead())
        {
            return;
        }

        currentCharacter.SelectSkill(skill);

        if (currentSelectingPlayer == PlayerSide.Wizard)
        {
            wizardHasSelected = true;
        }
        else
        {
            crusaderHasSelected = true;
        }

        UpdateStatus(GetPlayerName(currentSelectingPlayer) + " selected " + skill + ".");

        if (BothPlayersSelected())
        {
            SetButtonInteractable(resolveRoundButton, true);
            UpdateCurrentSelectorText();
            UpdateStatus("Both players selected skills. Resolve the round.");

            if (autoResolveWhenBothSelected)
            {
                ResolveRound();
            }

            return;
        }

        currentSelectingPlayer = GetOtherPlayer(currentSelectingPlayer);

        UpdateCurrentSelectorText();

        UpdateStatus(GetPlayerName(currentSelectingPlayer) + " selects a skill now.");
    }

    public void ResolveRound()
    {
        if (!gameStarted)
        {
            UpdateStatus("Start the game first.");
            return;
        }

        if (gameOver || isResolvingRound)
        {
            return;
        }

        if (!BothPlayersSelected())
        {
            UpdateStatus("Both players must select a skill first.");
            return;
        }

        StartCoroutine(ResolveRoundCoroutine());
    }

    private IEnumerator ResolveRoundCoroutine()
    {
        isResolvingRound = true;

        SetButtonInteractable(resolveRoundButton, false);

        PlayerSide secondPlayer = GetOtherPlayer(actionFirstPlayer);

        UpdateStatus("Round " + roundNumber + " starts. " + GetPlayerName(actionFirstPlayer) + " acts first.");

        yield return ResolveAction(actionFirstPlayer);

        if (gameOver)
        {
            yield break;
        }

        yield return ResolveAction(secondPlayer);

        if (gameOver)
        {
            yield break;
        }

        wizard.EndRound();
        crusader.EndRound();

        roundNumber++;

        actionFirstPlayer = GetOtherPlayer(actionFirstPlayer);

        PrepareNewRound();
    }

    private IEnumerator ResolveAction(PlayerSide actorSide)
    {
        Character actor = GetCharacter(actorSide);
        Character target = GetOpponent(actorSide);

        if (actor == null || target == null)
        {
            yield break;
        }

        if (actor.IsDead())
        {
            yield break;
        }

        SkillType usedSkill = actor.GetSelectedSkill();

        UpdateStatus(GetPlayerName(actorSide) + " uses " + usedSkill + ".");

        actor.UseSelectedSkill();

        if (usedSkill == SkillType.Attack)
        {
            yield return new WaitForSeconds(damageApplyDelay);

            if (!target.IsDead())
            {
                int damage = GetAttackDamage(actor);

                target.GetHit(damage);

                UpdateStatus(GetPlayerName(actorSide) + " attacked for " + damage + " damage.");
            }

            if (target.IsDead())
            {
                EndGame(actorSide);
                yield break;
            }
        }

        yield return new WaitForSeconds(timeBetweenActions);
    }

    private int GetAttackDamage(Character character)
    {
        if (character is Wizard wizardCharacter)
        {
            return wizardCharacter.GetAttackDamage();
        }

        if (character is Crusader crusaderCharacter)
        {
            return crusaderCharacter.GetAttackDamage();
        }

        return 20;
    }

    private bool BothPlayersSelected()
    {
        return wizardHasSelected && crusaderHasSelected;
    }

    public bool IsSelectingSkills()
    {
        return gameStarted && !gameOver && !isResolvingRound && !BothPlayersSelected();
    }

    public PlayerSide GetCurrentSelectingPlayer()
    {
        return currentSelectingPlayer;
    }

    private Character GetCharacter(PlayerSide side)
    {
        if (side == PlayerSide.Wizard)
        {
            return wizard;
        }

        return crusader;
    }

    private Character GetOpponent(PlayerSide side)
    {
        if (side == PlayerSide.Wizard)
        {
            return crusader;
        }

        return wizard;
    }

    private PlayerSide GetOtherPlayer(PlayerSide side)
    {
        if (side == PlayerSide.Wizard)
        {
            return PlayerSide.Crusader;
        }

        return PlayerSide.Wizard;
    }

    private string GetPlayerName(PlayerSide side)
    {
        if (side == PlayerSide.Wizard)
        {
            return "Malakor";
        }

        return "Titus";
    }

    private void EndGame(PlayerSide winner)
    {
        gameOver = true;
        isResolvingRound = false;

        SetButtonInteractable(resolveRoundButton, false);
        SetButtonVisible(startGameButton, false);

        UpdateStatus("Game Over! Winner: " + GetPlayerName(winner));
    }

    private void UpdateTurnOrderText()
    {
        if (turnOrderText == null)
        {
            return;
        }

        PlayerSide secondPlayer = GetOtherPlayer(actionFirstPlayer);

        turnOrderText.text = "Turn Order: " + GetPlayerName(actionFirstPlayer) + " → " + GetPlayerName(secondPlayer);
    }

    private void UpdateCurrentSelectorText()
    {
        if (currentSelectorText == null)
        {
            return;
        }

        if (BothPlayersSelected())
        {
            currentSelectorText.text = "Current Selector: Both selected";
        }
        else
        {
            currentSelectorText.text = "Current Selector: " + GetPlayerName(currentSelectingPlayer);
        }
    }

    private void UpdateStatus(string message)
    {
        Debug.Log(message);

        if (statusText != null)
        {
            statusText.text = message;
        }
    }

    private void SetButtonInteractable(Button button, bool value)
    {
        if (button != null)
        {
            button.interactable = value;
        }
    }

    private void SetButtonVisible(Button button, bool value)
    {
        if (button != null)
        {
            button.gameObject.SetActive(value);
        }
    }

    // Temporary test methods for UI buttons before AR card detection is connected.
    public void SelectAttackCard()
    {
        SelectSkillForCurrentPlayer(SkillType.Attack);
    }

    public void SelectShieldCard()
    {
        SelectSkillForCurrentPlayer(SkillType.Shield);
    }

    public void SelectHealCard()
    {
        SelectSkillForCurrentPlayer(SkillType.Heal);
    }
}