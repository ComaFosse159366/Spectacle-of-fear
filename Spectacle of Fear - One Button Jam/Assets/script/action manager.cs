using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class actionmanager : MonoBehaviour
{
    public sound_and_music sound_manager;
    public inputcontroller inputController;
    public int distance = 6;
    public int mouvementspeed = 10;
    public GameObject player;
    public Camera playerCamera;
    public bool isinteracting = false;

    public bool debugMove = false;
    public string[] ignoreGraphicNames = new string[0]; // names of UI GameObjects to ignore (e.g. "floor")
    public string[] ignoreGraphicTags = new string[0];  // tags of UI GameObjects to ignore
    private Coroutine moveRoutine;

    public void move()
    {
        if (player == null) return;
        if (playerCamera == null) playerCamera = Camera.main;

        Vector3 origin = player.transform.position;
        Vector3 targetWorld = origin + player.transform.forward * distance;
        Vector3 midWorld = origin + player.transform.forward * (distance * 0.5f);

        Vector3 targetSP = playerCamera.WorldToScreenPoint(targetWorld);
        Vector3 midSP = playerCamera.WorldToScreenPoint(midWorld);

        // If either point is behind the camera, don't move
        if (targetSP.z <= 0f || midSP.z <= 0f)
        {
            if (debugMove) Debug.Log("Not moving: target or midpoint is behind the camera");
            return;
        }

        bool targetBlocked = IsScreenBlocked(new Vector2(targetSP.x, targetSP.y));
        bool midBlocked = IsScreenBlocked(new Vector2(midSP.x, midSP.y));

        if (debugMove) Debug.Log($"Move check - targetSP:{targetSP} midSP:{midSP} targetBlocked:{targetBlocked} midBlocked:{midBlocked}");

        // Require both midpoint and target to be blocked to consider the path blocked.
        // This reduces false positives when a distant canvas face partially covers the target.
        if (targetBlocked && midBlocked)
        {
            if (debugMove) Debug.Log("Not moving: UI blocks the path (both target and midpoint)");
            return;
        }

        // Path clear — slide to target instead of teleporting.
        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
        }

        moveRoutine = StartCoroutine(SlideTo(targetWorld));
    }

    private IEnumerator SlideTo(Vector3 targetWorld)
    {
        float speed = mouvementspeed;

        sound_manager.StartCoroutine(sound_manager.Walking_Sound());

        while ((player.transform.position - targetWorld).sqrMagnitude > 0.0001f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetWorld, speed * Time.deltaTime);
            yield return null;
        }

        player.transform.position = targetWorld;
        moveRoutine = null;
    }
    public void forcemove()
    {
        if (player == null) return;
        Vector3 origin = player.transform.position;
        Vector3 targetWorld = origin + player.transform.forward * distance;
        moveRoutine = StartCoroutine(SlideTo(targetWorld));
        player.transform.position = targetWorld;
    }

    // Check whether any UI Graphic blocks the given screen position
    private bool IsScreenBlocked(Vector2 screenPosition)
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (var r in results)
        {
            var go = r.gameObject;
            var g = go.GetComponent<Graphic>();
            if (g == null) continue;

            // Ignore configured names/tags first
            if (ignoreGraphicNames != null && System.Array.IndexOf(ignoreGraphicNames, go.name) >= 0)
            {
                //if (debugMove) Debug.Log("Ignoring graphic by name: " + go.name);
                continue;
            }
            if (ignoreGraphicTags != null && !string.IsNullOrEmpty(go.tag) && System.Array.IndexOf(ignoreGraphicTags, go.tag) >= 0)
            {
                //if (debugMove) Debug.Log("Ignoring graphic by tag: " + go.name + " tag=" + go.tag);
                continue;
            }

            // Respect raycastTarget property
            if (!g.raycastTarget)
            {
                if (debugMove) Debug.Log("Skipping graphic with raycastTarget=false: " + go.name);
                continue;
            }

            // If any parent CanvasGroup blocks raycasts = false, skip
            var groups = go.GetComponentsInParent<CanvasGroup>(true);
            bool blockedByGroup = false;
            foreach (var cg in groups)
            {
                if (!cg.blocksRaycasts)
                {
                    blockedByGroup = true;
                    break;
                }
            }
            if (blockedByGroup)
            {
                if (debugMove) Debug.Log("Skipping graphic due to CanvasGroup.blocksRaycasts=false: " + go.name);
                continue;
            }

            // Skip invisible/transparent graphics
            float effectiveAlpha = g.color.a;
            try { effectiveAlpha *= g.canvasRenderer.GetAlpha(); } catch { }
            if (effectiveAlpha <= 0.01f)
            {
                if (debugMove) Debug.Log("Skipping transparent graphic: " + go.name + " alpha=" + effectiveAlpha);
                continue;
            }

            // (back-facing graphics are treated the same — do not skip)

            // This graphic should count as blocking
            if (debugMove) Debug.Log("Screen blocked by: " + go.name + " (alpha=" + effectiveAlpha + ")");
            return true;
        }

        // Fallback: some world-space or camera-space canvases may not appear in EventSystem.RaycastAll for certain setups.
        // Iterate all GraphicRaycasters and ask each for raycast results using the same pointer data.
        var raycasters = FindObjectsByType<GraphicRaycaster>();
        foreach (var raycaster in raycasters)
        {
            if (raycaster == null) continue;
            List<RaycastResult> rc2 = new List<RaycastResult>();
            try
            {
                raycaster.Raycast(eventData, rc2);
            }
            catch { continue; }

            foreach (var r2 in rc2)
            {
                var go2 = r2.gameObject;
                var g2 = go2.GetComponent<Graphic>();
                if (g2 == null) continue;

                if (ignoreGraphicNames != null && System.Array.IndexOf(ignoreGraphicNames, go2.name) >= 0) { if (debugMove) Debug.Log("Ignoring graphic by name (fallback): " + go2.name); continue; }
                if (ignoreGraphicTags != null && !string.IsNullOrEmpty(go2.tag) && System.Array.IndexOf(ignoreGraphicTags, go2.tag) >= 0) { if (debugMove) Debug.Log("Ignoring graphic by tag (fallback): " + go2.name + " tag=" + go2.tag); continue; }
                if (!g2.raycastTarget) continue;

                var groups2 = go2.GetComponentsInParent<CanvasGroup>(true);
                bool blockedByGroup2 = false;
                foreach (var cg in groups2) if (!cg.blocksRaycasts) { blockedByGroup2 = true; break; }
                if (blockedByGroup2) continue;

                float effectiveAlpha2 = g2.color.a;
                try { effectiveAlpha2 *= g2.canvasRenderer.GetAlpha(); } catch { }
                if (effectiveAlpha2 <= 0.01f) continue;

                // (do not skip back-facing graphics in fallback)

                if (debugMove) Debug.Log("Fallback blocked by: " + go2.name + " on canvas " + raycaster.gameObject.name + " (alpha=" + effectiveAlpha2 + ")");
                return true;
            }
        }

        return false;
    }

    private bool IsGraphicFacingCamera(Graphic g)
    {
        if (g == null) return false;
        var canvas = g.canvas;
        if (canvas == null) return true;
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay) return true;
        var rt = g.rectTransform;
        Vector3 normal = rt.transform.forward;
        Vector3 toCamera = (playerCamera != null ? (playerCamera.transform.position - rt.transform.position).normalized : Vector3.forward);
        return Vector3.Dot(normal, toCamera) > 0f;
    }

    public void rotate()
    {
        if (player == null) return;
        player.transform.rotation = Quaternion.Euler(0, 90, 0) * player.transform.rotation;
    }

    public void interact()
    {
        // Implement interaction logic here
        isinteracting = true;
        Debug.Log("Interacting with the environment.");
    }
    public void stopinteract()
    {
        isinteracting = false;
        Debug.Log("Stopped interacting with the environment.");
    }
}
