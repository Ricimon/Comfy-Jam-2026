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

    private readonly ResourceManagers resourceManagers;
    private readonly InputAction clickAction;
    private readonly InputAction rightClickAction;
    private readonly InputAction pointAction;
    private readonly List<RaycastResult> raycastResults = new();
    private readonly CombinedFilterID draggingEntityFilter = new(0, FilterContextID.GetNewContextID());

    private Canvas canvas;
    private RectTransform canvasRt;
    private GraphicRaycaster grc;

    public InputSystem(ResourceManagers resourceManagers)
    {
        this.resourceManagers = resourceManagers;

        var defaultActions = UnityEngine.InputSystem.InputSystem.actions;
        clickAction = defaultActions.FindAction("Click");
        rightClickAction = defaultActions.FindAction("RightClick");
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
                var go = resourceManagers.Get<GameObject>(goRef.Id);
                canvas = go.GetComponent<Canvas>();
                canvasRt = go.GetComponent<RectTransform>();
                grc = go.GetComponent<GraphicRaycaster>();
            }
        }

        var pointerLocation = pointAction.ReadValue<Vector2>();
        UpdateLeftClick(pointerLocation);
        UpdateRightClick(pointerLocation);
    }

    private EntityFilterCollection GetDraggingFilter()
    {
        return entitiesDB.GetFilters().GetOrCreatePersistentFilter<RectPosition>(draggingEntityFilter);
    }

    private void UpdateLeftClick(Vector2 pointerLocation)
    {
        var pointerDown = clickAction.ReadValue<float>() != 0;

        if (clickAction.WasPerformedThisFrame() && pointerDown)
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
                    erh.EGID.IsValid() &&
                    SlimeGroup.Includes(erh.EGID.groupID))
                {
                    // Debug.Log($"Hit {hit.gameObject}", hit.gameObject);

                    if (entitiesDB.TryGetEntity(erh.EGID, out Slime slime))
                    {
                        if (!slime.CanPickUp)
                        {
                            break;
                        }
                    }

                    if (entitiesDB.TryQueryEntitiesAndIndex<RectPosition>(erh.EGID, out var i, out var positions))
                    {
                        GetDraggingFilter().Add(erh.EGID, i);
                    }

                    if (entitiesDB.TryQueryEntitiesAndIndex<SlimeBrain>(erh.EGID, out var slimeIdx, out var brains))
                    {
                        brains[slimeIdx].MovementState = MovementState.Grabbed;
                    }

                    // entitiesDB.TryGetComponent(erh.EGID,
                    //     (ref GameObjectReference slimeGor) =>
                    //     {
                    //         var slime = resourceManagers.Get<GameObject>(slimeGor.Id);
                    //         var gameCanvas = entitiesDB.GetSingletonComponent<GameCanvas>(CanvasGroup.Group);
                    //         var grabbedSlimeParent = resourceManagers.Get<GameObject>(gameCanvas.GrabbedObjectGoId);
                    //         slime.transform.SetParent(grabbedSlimeParent.transform);
                    //         slime.transform.SetAsLastSibling();
                    //     });

                    break;
                }
            }
        }

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRt, pointerLocation, canvas.worldCamera, out var worldPosition))
        {
            var canvasPosition = worldPosition / canvasRt.localScale.x;
            GetDraggingFilter().RunOnFilteredComponents(entitiesDB,
                (ref RectPosition p, ref Slime slime) =>
                {
                    if (!slime.CanPickUp)
                    {
                        GetDraggingFilter().Clear();
                        return;
                    }
                    p.Value = canvasPosition;
                });
        }

        if (clickAction.WasPerformedThisFrame() && !pointerDown)
        {
            // Debug.Log($"Released at {pointerLocation}");
            var draggingFilter = GetDraggingFilter();

            draggingFilter.RunOnFilteredComponents(entitiesDB,
                (EGID egid, ref RectPosition p, ref Slime slime, ref SlimeBrain brain, ref Direction direction) =>
                {
                    var position = p.Value;
                    EGID newPenId = default;
                    bool isSortingPen = false;
                    bool isMatchingColor = false;
                    SlimeColor penColor = SlimeColor.None;

                    entitiesDB.QueryEntities<RectPosition, RectBoundary, GameObjectReference>(PenGroupTag.Groups)
                        .Each((EGID egid, ref RectPosition pp, ref RectBoundary pb, ref GameObjectReference gor) =>
                        {
                            if (RectUtils.CreateCenteredRect(pp.Value, new(pb.Width, pb.Height)).Contains(position))
                            {
                                var penGo = resourceManagers.Get<GameObject>(gor.Id);
                                if (penGo != null)
                                {
                                    if (entitiesDB.TryGetEntity<SortingPen>(egid, out var sortingPen))
                                    {
                                        Debug.Log($"Slime dropped in {penGo}. Sorting pen type is {sortingPen.Type}");
                                        isSortingPen = true;
                                        penColor = sortingPen.Type;
                                    }
                                    else
                                    {
                                        Debug.Log($"Slime dropped in {penGo}");
                                        isSortingPen = false;
                                    }

                                }
                                newPenId = egid;
                            }
                        });

                    // var gameCanvas = entitiesDB.GetSingletonComponent<GameCanvas>(CanvasGroup.Group);
                    // var slimeParent = resourceManagers.Get<GameObject>(gameCanvas.SlimesParentGoId);
                    // var gor = entitiesDB.QueryEntity<GameObjectReference>(egid);
                    // var go = resourceManagers.Get<GameObject>(gor.Id);
                    // go.transform.SetParent(slimeParent.transform);
                    // go.transform.SetAsLastSibling();

                    brain.MovementState = MovementState.Wander;
                    direction.Value = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                    slime.CanPickUp = !isSortingPen;
                    isMatchingColor = slime.SlimeColor == penColor;
                    if (isMatchingColor)
                    {
                        var (score, count) = entitiesDB.QueryEntities<Score>(GameStatTag.Group);
                        score[0].Value++;
                        brain.IsSpeedUp = false;
                    }
                    else if (penColor != SlimeColor.None && !isMatchingColor)
                    {
                        var (lives, count) = entitiesDB.QueryEntities<Lives>(GameStatTag.Group);
                        lives[0].Value--;
                        brain.IsSpeedUp = true;
                    }

                    if (newPenId != default)
                        brain.PenId = newPenId;
                });

            draggingFilter.Clear();
        }
    }

    private void UpdateRightClick(Vector2 pointerLocation)
    {
        var pointerDown = rightClickAction.ReadValue<float>() != 0;

        if (rightClickAction.WasPerformedThisFrame() && pointerDown)
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
                    erh.EGID.IsValid())
                {
                    // Disguise
                    if (DisguiseEntity.Group == erh.EGID.groupID)
                    {
                        bool breakLoop = true;
                        entitiesDB.TryGetComponent(erh.EGID,
                            (ref Disguise disguise) =>
                            {
                                if (!disguise.SlimeId.IsValid())
                                {
                                    breakLoop = false;
                                    return;
                                }

                                // Make sure the slime can be picked up
                                var slime = entitiesDB.GetComponent<Slime>(disguise.SlimeId);
                                if (!slime.CanPickUp)
                                {
                                    breakLoop = true;
                                    return;
                                }

                                Debug.Log("Removing disguise");
                                disguise.ShouldRemove = true;
                            });

                        if (breakLoop)
                        {
                            break;
                        }
                    }
                    // Slime
                    else if (SlimeGroup.Includes(erh.EGID.groupID))
                    {
                        entitiesDB.TryGetComponent(erh.EGID,
                            (ref Slime slime, ref SlimeBrain sb) =>
                            {
                                if (!slime.CanPickUp)
                                {
                                    return;
                                }

                                sb.MovementState = MovementState.Flying;
                                sb.FlightAnimationTime = 0.0f;
                                GetDraggingFilter().Clear();
                            });
                        break;
                    }
                }
            }
        }
    }
}