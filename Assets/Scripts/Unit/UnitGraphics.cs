using MVC.Controler.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

[SelectionBase]
public class UnitGraphics : MonoBehaviour
{
    [SerializeField] private TeamEnum team;

    public int MovementPoints { get => ServiceLocator.GetService<CombatManager>().GetUnitMovementPoints(gameObject); }

    [SerializeField]
    private float movementDuration = 1, rotationDuration = 0.3f;

    private GlowHighlight glowHighlight;
    private Queue<Vector3> pathPositions = new Queue<Vector3>();
    public Vector2Int CurrentUnitPosition;

    public event Action<UnitGraphics> MovementFinished;

    private void Awake()
    {
        if (glowHighlight == null)
        {
            glowHighlight = GetComponent<GlowHighlight>();
        }
    }

    private void Start()
    {
        var combatManager = ServiceLocator.GetService<CombatManager>();
        combatManager.AddUnitToTeam(team, gameObject);
    }

    public void Deselect()
    {
        glowHighlight.ToggleGlow(false);
    }

    public void Select()
    {
        glowHighlight.ToggleGlow(true);
    }

    public void MoveThroughPath(List<Vector3> currentPath, int currentCost)
    {
        var combatManager = ServiceLocator.GetService<CombatManager>();
        pathPositions = new Queue<Vector3>(currentPath);
        Vector3 firstTarget = pathPositions.Dequeue();
        combatManager.SpendMovementPointsOfAUnit(gameObject, currentCost);
        StartCoroutine(RotationCoroutine(firstTarget, rotationDuration));
    }

    private IEnumerator RotationCoroutine(Vector3 endPosition, float rotationDuration)
    {
        Quaternion startRotation = transform.rotation;
        endPosition.y = transform.position.y;
        Vector3 direction = endPosition - transform.position;
        Quaternion endRotation = Quaternion.LookRotation(direction, Vector3.up);

        if (Mathf.Approximately(Mathf.Abs(Quaternion.Dot(startRotation, endRotation)), 1.0f) == false)
        {
            float timeElapsed = 0;

            while (timeElapsed < rotationDuration)
            {
                timeElapsed += Time.deltaTime;
                float lerpStep = timeElapsed / rotationDuration;
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, lerpStep);
                yield return null;
            }  
        }

        StartCoroutine(MovementCoroutine(endPosition));
    }

    private IEnumerator MovementCoroutine(Vector3 endPosition)
    {
        Vector3 startPosition = transform.position;
        endPosition.y = startPosition.y;

        float timeElapsed = 0;

        while (timeElapsed < movementDuration)
        {
            timeElapsed += Time.deltaTime;
            float lerpStep = timeElapsed / movementDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, lerpStep);
            yield return null;
        }
        transform.position = endPosition;

        if (pathPositions.Count > 0)
        {
            Debug.Log("Selecting next position");
            StartCoroutine(RotationCoroutine(pathPositions.Dequeue(), rotationDuration));
        }
        else
        {
            Debug.Log("Movement Finished");
            MovementFinished?.Invoke(this);
        }
    }
}
