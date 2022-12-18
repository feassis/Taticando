using MVC.Controler.Combat;
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

        [SerializeField] private int actionDistance; //remove this once character can be seted up out of scene
        [SerializeField] private NeighbourhoodType neighbourhoodType; //remove this once character can be seted up out of scene
        [SerializeField] private int actionRangeAmount; //remove this once character can be seted up out of scene
        [SerializeField] private UnitActionVisuals action;

        public int MovementPoints { get => ServiceLocator.GetService<CombatManager>().GetUnitMovementPoints(gameObject); }

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
            combatManager.AddUnitToTeam(team, gameObject);
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
            ResetActionRange();

            var startPosition = CalculateActionOrigin(coords.GetCoords() + new Vector3Int(0, -1, 0), GetRotationDirection());// -1 unit offset, remove magic number

            var graphSearch = ServiceLocator.GetService<GraphSearch>();

            actionRange = graphSearch.BFSRangeAllCosts1(grid, startPosition, actionRangeAmount, neighbourhoodType, true);

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
            return direction switch
            {
                CardinalDirection.Up => currentCoordinates + new Vector3Int(0, 0, actionDistance),
                CardinalDirection.Down => currentCoordinates + new Vector3Int(0, 0, -actionDistance),
                CardinalDirection.Right => currentCoordinates + new Vector3Int(actionDistance, 0, 0),
                CardinalDirection.Left => currentCoordinates + new Vector3Int(-actionDistance, 0, 0),
                _ => throw new NotImplementedException(),
            };
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


