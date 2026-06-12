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

    private readonly GameObjectResourceManager gameObjectResourceManager;
    private readonly InputAction clickAction;
    private readonly InputAction pointAction;
    private readonly List<RaycastResult> raycastResults = new();
    private readonly CombinedFilterID draggingEntityFilter = new(0, FilterContextID.GetNewContextID());

    private Canvas canvas;
    private RectTransform canvasRt;
    private GraphicRaycaster grc;

    public InputSystem(GameObjectResourceManager gameObjectResourceManager)
    {
        this.gameObjectResourceManager = gameObjectResourceManager;

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
            var (goRefs, count) = entitiesDB.QueryEntities<GameObjectReference>(CanvasGroup.Group);
            if (count > 0)
            {
                var goRef = goRefs[0];
                var go = gameObjectResourceManager[goRef.Id];
                canvas = go.GetComponent<Canvas>();
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
                        SlimeGroup.Includes(erh.EGID.groupID))
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
            }
        }

        if (pointerDown)
        {
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRt, pointerLocation, canvas.worldCamera, out var worldPosition))
            {
                GetDraggingFilter().RunOnFilteredComponents(entitiesDB,
                    (ref Position p) =>
                    {
                        p.Value = worldPosition;
                    });
            }
        }
    }

    private EntityFilterCollection GetDraggingFilter()
    {
        return entitiesDB.GetFilters().GetOrCreatePersistentFilter<Position>(draggingEntityFilter);
    }
}