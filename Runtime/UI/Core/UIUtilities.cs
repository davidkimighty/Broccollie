using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Broccollie.UI
{
    public static class UIUtilities
    {
        #region Public Functions
        public static async Task ExecuteBehaviorAsync(this BaseUIElement ui, UIStates state, bool instantChange, bool playAudio)
        {
            try
            {
                ui.RenewCancelToken();
                List<Task> featureTasks = new();
                foreach (BaseUIFeature feature in ui.Features)
                {
                    if (!feature.IsEnabled) continue;
                    List<Task> tasks = feature.GetFeatures(state, instantChange, playAudio, ui.Cts.Token);
                    if (tasks != null)
                        featureTasks.AddRange(tasks);
                }
                await Task.WhenAll(featureTasks);
            }
            catch (OperationCanceledException) { }
        }

        public static void MoveToNextSelectable(this BaseUIElement navigable, AxisEventData eventData, List<BaseUIElement> activeList)
        {
            switch (eventData.moveDir)
            {
                case MoveDirection.Left:
                    ChangeSelectedObject(GetNextSelectable(navigable.transform.rotation * Vector3.left));
                    break;

                case MoveDirection.Right:
                    ChangeSelectedObject(GetNextSelectable(navigable.transform.rotation * Vector3.right));
                    break;

                case MoveDirection.Up:
                    ChangeSelectedObject(GetNextSelectable(navigable.transform.rotation * Vector3.up));
                    break;

                case MoveDirection.Down:
                    ChangeSelectedObject(GetNextSelectable(navigable.transform.rotation * Vector3.down));
                    break;
            }

            BaseUIElement GetNextSelectable(Vector3 dir)
            {
                dir = dir.normalized;
                Vector3 localDir = Quaternion.Inverse(navigable.transform.rotation) * dir;
                Vector3 pos = navigable.transform.TransformPoint(GetPointOnRectEdge(navigable.transform as RectTransform, localDir));
                float maxScore = Mathf.NegativeInfinity;
                BaseUIElement bestPick = null;

                foreach (BaseUIElement selectable in activeList)
                {
                    if (selectable == navigable || selectable == null) continue;

                    if (!selectable.IsRaycastInteractive) continue; // adding navigation mode?

                    RectTransform rect = selectable.transform as RectTransform;
                    Vector3 center = rect != null ? (Vector3)rect.rect.center : Vector3.zero;
                    Vector3 calcDir = selectable.transform.TransformPoint(center) - pos;

                    float dot = Vector3.Dot(dir, calcDir);
                    if (dot <= 0) continue;

                    float score = dot / calcDir.sqrMagnitude;
                    if (score > maxScore)
                    {
                        maxScore = score;
                        bestPick = selectable;
                    }
                }
                return bestPick;
            }

            Vector3 GetPointOnRectEdge(RectTransform rect, Vector2 dir)
            {
                if (rect == null)
                    return Vector3.zero;
                if (dir != Vector2.zero)
                    dir /= Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
                dir = rect.rect.center + Vector2.Scale(rect.rect.size, dir * 0.5f);
                return dir;
            }

            void ChangeSelectedObject(BaseUIElement selected)
            {
                if (selected != null && navigable.enabled && navigable.isActiveAndEnabled && navigable.gameObject.activeInHierarchy)
                    eventData.selectedObject = selected.gameObject;
            }
        }

        #endregion

    }
}
