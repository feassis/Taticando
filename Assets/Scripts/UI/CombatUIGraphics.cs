using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Tools;
using MVC.Controler.Combat;

public class CombatUIGraphics : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI teamText;
    [SerializeField] private Button changeTurnButton;

    private void Start()
    {
        var combatManager = ServiceLocator.GetService<CombatManager>();
        changeTurnButton.onClick.AddListener(combatManager.ChangeTurn);
        UpdateTeamText();
        combatManager.OnTurnChange += UpdateTeamText;
    }
    private void UpdateTeamText()
    {
        var combatManager = ServiceLocator.GetService<CombatManager>();
        teamText.text = combatManager.GetCurrentTeamTurn().MyTeam.ToString();
    }

    private void OnDestroy()
    {
        var combatManager = ServiceLocator.GetService<CombatManager>();
        combatManager.OnTurnChange -= UpdateTeamText;
    }
}
