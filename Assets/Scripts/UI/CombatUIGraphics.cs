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
    [SerializeField] private Button actionButton;
    [SerializeField] private Button turnLeftButton;
    [SerializeField] private Button turnRightButton;
    [SerializeField] private Button useActionButton;

    private void Start()
    {
        var combatManager = ServiceLocator.GetService<CombatManager>();
        changeTurnButton.onClick.AddListener(combatManager.ChangeTurn);
        combatManager.OnTurnChange += ResetActionButton;
        UpdateTeamText();
        combatManager.OnTurnChange += UpdateTeamText;

        SetupButtons();
    }

    private void SetupButtons()
    {
        actionButton.onClick.AddListener(OnActionButtonClicked);
        turnRightButton.onClick.AddListener(OnTurnRightButton);
        turnLeftButton.onClick.AddListener(OnTurnLeftButton);
        useActionButton.onClick.AddListener(OnUseActionButtonClicked);
    }

    private void UpdateTeamText()
    {
        var combatManager = ServiceLocator.GetService<CombatManager>();
        teamText.text = combatManager.GetCurrentTeamTurn().MyTeam.ToString();
    }

    private void OnTurnRightButton()
    {
        ServiceLocator.GetService<UnitManager>().RotateUnitInPlace(RotationOrientarition.Clockwise);
    }

    private void OnTurnLeftButton()
    {
        ServiceLocator.GetService<UnitManager>().RotateUnitInPlace(RotationOrientarition.AntiClockwise);
    }

    private void OnActionButtonClicked()
    {
        var unitManager = ServiceLocator.GetService<UnitManager>();
        unitManager.HideMovementRange();
        unitManager.ShowUnitActionRange();

        actionButton.gameObject.SetActive(false);
        turnLeftButton.gameObject.SetActive(true);
        turnRightButton.gameObject.SetActive(true);
        useActionButton.gameObject.SetActive(true);
    }

    private void OnUseActionButtonClicked()
    {
        ServiceLocator.GetService<CombatManager>().ExecuteActionOfSelectedUnit();
    }

    private void ResetActionButton()
    {
        actionButton.gameObject.SetActive(true);
        turnLeftButton.gameObject.SetActive(false);
        turnRightButton.gameObject.SetActive(false);
        useActionButton.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        var combatManager = ServiceLocator.GetService<CombatManager>();
        combatManager.OnTurnChange -= UpdateTeamText;
        combatManager.OnTurnChange -= ResetActionButton;
    }
}
