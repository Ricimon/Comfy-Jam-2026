using System.Collections.Generic;
using ECS;
using Svelto.ECS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputSystem : ISystem, IQueryingEntitiesEngine
{
    public EntitiesDB entitiesDB { get; set; }

    private readonly InputAction clickAction;
    private readonly InputAction pointAction;
    private readonly List<RaycastResult> raycastResults = new();
    private readonly CombinedFilterID draggingEntityFilter = new(0, FilterContextID.GetNewContextID());

    private UnityEngine.Canvas canvas;
    private RectTransform canvasRt;
    private GraphicRaycaster grc;
    private bool dragging;
    private Vector2 lastPointerDragPosition;

    public InputSystem()
    {
        var defaultActions = UnityEngine.InputSystem.InputSystem.actions;
        clickAction = defaultActions.FindAction("Click");
        pointAction = defaultActions.FindAction("Point");
    }

    public void Ready()
    {
    }

    public void Update()
    {
        if (canvas == null || grc == null)
        {
            var (goRefs, count) = entitiesDB.QueryEntities<GameObjectReference>(Canvas.Group);
            if (count > 0)
            {
                var goRef = goRefs[0];
                var go = GameContext.GameObjectResourceManager[goRef.Id];
                canvas = go.GetComponent<UnityEngine.Canvas>();
                canvasRt = go.GetComponent<RectTransform>();
                grc = go.GetComponent<GraphicRaycaster>();
            }
        }

        var pointerDown = clickAction.ReadValue<float>() != 0;
        var pointerLocation = pointAction.ReadValue<Vector2>();

        if (clickAction.WasPerformedThisFrame())
        {
            if (pointerDown)
            {
                // Debug.Log($"Clicked at {pointerLocation}");

                var ped = new PointerEventData(null)
                {
                    position = pointerLocation
                };
                raycastResults.Clear();
                grc.Raycast(ped, raycastResults);

                foreach (var hit in raycastResults)
                {
                    if (hit.gameObject.TryGetComponent(out EntityReferenceHolder erh) &&
                        Slime.Includes(erh.EGID.groupID))
                    {
                        // Debug.Log($"Hit {hit.gameObject}", hit.gameObject);
                        if (entitiesDB.TryQueryEntitiesAndIndex<Position>(erh.EGID, out var i, out var positions))
                        {
                            GetDraggingFilter().Add(erh.EGID, i);
                        }
                        break;
                    }
                }
            }
            else
            {
                // Debug.Log($"Released at {pointerLocation}");

                GetDraggingFilter().Clear();

                dragging = false;
            }
        }

        if (pointerDown)
        {
            if (!dragging)
            {
                dragging = true;
            }
            else
            {
                foreach(var (fis, group) in GetDraggingFilter())
                {
                    var delta = CalculateWorldPositionDelta(pointerLocation, lastPointerDragPosition);
                    var (positions, _) = entitiesDB.QueryEntities<Position>(group);
                    for (var i = 0; i < fis.count; i++)
                    {
                        var fi = fis[i];
                        positions[fi].Value += delta;
                    }
                }
            }

            lastPointerDragPosition = pointerLocation;
        }
    }

    private EntityFilterCollection GetDraggingFilter()
    {
        return entitiesDB.GetFilters().GetOrCreatePersistentFilter<Position>(draggingEntityFilter);
    }

    private Vector3 CalculateWorldPositionDelta(Vector2 newScreenPosition, Vector2 oldScreenPosition)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRt, newScreenPosition, canvas.worldCamera, out var newWorldPoint) &&
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRt, oldScreenPosition, canvas.worldCamera, out var oldWorldPoint))
        {
            return newWorldPoint - oldWorldPoint;
        }
        return default;
    }
}