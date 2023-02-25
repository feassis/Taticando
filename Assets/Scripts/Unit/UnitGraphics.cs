using MVC.Controller.Combat;
using MVC.Controller.Graph;
using MVC.Controller.Movement;
using MVC.Controller.Unit;
using MVC.Model.Combat;
using MVC.Model.Elements;
using MVC.View.Elements;
using MVC.View.Grid;
using MVC.View.UI;
using MVC.View.VFX;
using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace MVC.View.Unit
{
    [SelectionBase]
    public class UnitGraphics : MonoBehaviour
    {
        [SerializeField] private TeamEnum team;
        [SerializeField] private GridCoordinates coords;

        [SerializeField] private UnitAction action;//remove this afte5r unit indtsntistion is done
        [SerializeField] private HealthBarGraphics hpBar;
        [SerializeField] private ShieldHolderGraphics shieldHolder;
        [SerializeField] private UnitElementGraphics unitElemets;

        public int MovementPoints { get => ServiceLocator.GetService<CombatManager>().GetUnitMovementPoints(this); }

        [SerializeField]
        private float movementDuration = 1, rotationDuration = 0.3f;

        private GlowHighlight glowHighlight;
        private Queue<Vector3> pathPositions = new Queue<Vector3>();
        private bool isRotatingInPlace = false;

        public event Action<UnitGraphics> MovementFinished;

        private BFSResult actionRange = new BFSResult();

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
            combatManager.AddUnitToTeam(team, this);
            combatManager.SubscribeActionToUnitOnDamage(this, hpBar.UpdateHealthbar);
            combatManager.SubscribeActionToUnitOnHeal(this, hpBar.UpdateHealthbar);
            combatManager.SubscribeActionToUnitShieldChange(this, shieldHolder.DisplayShield);

            (int currentHp, int maxHP) hpStatus = combatManager.GetUnitHPStatus(this);
            hpBar.Setup(hpStatus.currentHp, hpStatus.maxHP);
        }

        public BFSResult GetCurrentActionRange()
        {
            return actionRange;
        }

        public void Deselect()
        {
            glowHighlight.ToggleGlow(false);
        }

        public void Select()
        {
            glowHighlight.ToggleGlow(true);
        }

        public void ResetActionRange()
        {
            var grid = ServiceLocator.GetService<IGrid>(); 

            if (actionRange == null)
            {
                return;
            }

            foreach (var position in actionRange.GetRangePositions())
            {
                var tile = grid.GetTileAt(position);

                if (tile != null)
                {
                    tile.ResetActionHightlight();
                }
            }

            actionRange = new BFSResult();
        }

        public void ShowActionRange(IGrid grid)
        {
            ServiceLocator.GetService<IGrid>().ResetNeighbourhoodTileDicts();
            ResetActionRange();

            var startPosition = CalculateActionOrigin(GetMyTilePosition(), GetRotationDirection());

            var graphSearch = ServiceLocator.GetService<GraphSearch>();

            var actionInfo = action.GetRangeInfo();

            actionRange = graphSearch.BFSRangeAllCosts1(grid, startPosition, actionInfo.ActionRangeAmount, actionInfo.NeighbourhoodType, true);

            foreach (var position in actionRange.GetRangePositions())
            {
                var tile = grid.GetTileAt(position);

                if (tile != null)
                {
                    tile.HighlightActionRange();
                }
            }
        }

        public void UseUnitAction()
        {
            action.Execute(this);

            var grid = ServiceLocator.GetService<IGrid>();
            ResetActionRange();
        }

        private Vector3Int CalculateActionOrigin(Vector3Int currentCoordinates, CardinalDirection direction)
        {
            var actionRange = action.GetRangeInfo();

            return direction switch
            {
                CardinalDirection.Up => currentCoordinates + new Vector3Int(0, 0, actionRange.ActionDistance),
                CardinalDirection.Down => currentCoordinates + new Vector3Int(0, 0, -actionRange.ActionDistance),
                CardinalDirection.Right => currentCoordinates + new Vector3Int(actionRange.ActionDistance, 0, 0),
                CardinalDirection.Left => currentCoordinates + new Vector3Int(-actionRange.ActionDistance, 0, 0),
                _ => throw new NotImplementedException(),
            };
        }

        public void MoveThroughPath(List<Vector3> currentPath, int currentCost)
        {
            var combatManager = ServiceLocator.GetService<CombatManager>();
            pathPositions = new Queue<Vector3>(currentPath);
            Vector3 firstTarget = pathPositions.Dequeue();
            combatManager.SpendMovementPointsOfAUnit(this, currentCost);
            StartCoroutine(RotationCoroutine(firstTarget, rotationDuration));
        }

        public Vector3Int GetMyTilePosition()
        {
            return coords.GetCoords() - new Vector3Int(0, 1, 0);
        }

        public void UpdateElementVisibility(ElementsEnum element)
        {
            unitElemets.UpdateElementsVisibility(element);
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

        private IEnumerator RotationCoroutine(RotationOrientarition direction, float rotationDuration, Action onRotarionFinished)
        {
            Quaternion startRotation = transform.rotation;

            Quaternion endRotation;
            if (direction == RotationOrientarition.Clockwise)
            {
                endRotation = startRotation * Quaternion.Euler(0, 90, 0);
            }
            else
            {
                endRotation = startRotation * Quaternion.Euler(0, -90, 0);
            }

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

            onRotarionFinished?.Invoke();
            isRotatingInPlace = false;
        }

        public void RotateInPlace(RotationOrientarition direction, Action onRotationFinished)
        {
            if (isRotatingInPlace)
            {
                return;
            }

            isRotatingInPlace = true;

            StartCoroutine(RotationCoroutine(direction, rotationDuration, onRotationFinished));
        }

        private CardinalDirection GetRotationDirection()
        {
            int rotationDegrees = Mathf.RoundToInt(transform.rotation.eulerAngles.y);

            int reaminigValue = rotationDegrees / 90 % 4;

            return reaminigValue switch
            {
                0 => CardinalDirection.Up,
                1 => CardinalDirection.Right,
                2 => CardinalDirection.Down,
                3 => CardinalDirection.Left,
                _ => throw new NotImplementedException()
            };
        }

        private enum CardinalDirection
        {
            Up = 0,
            Right = 1,
            Down = 2,
            Left = 3
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
}


