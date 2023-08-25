using UnityEngine;

public class SafeAreaHelper : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_rectTransform;
    private Rect m_lastSafeArea;

    private void Awake()
    {
        Refresh();
    }

    private void FixedUpdate()
    {
        Refresh();
    }

    private void Refresh()
    {
        Rect safeArea = Screen.safeArea;

        if (safeArea != m_lastSafeArea)
        {
            ApplySafeArea(safeArea);
        }
    }
    
    private void ApplySafeArea(Rect rect)
    {
        m_lastSafeArea = rect;

        // Convert safe area rectangle from absolute pixels to normalized anchor coordinates
        Vector2 anchorMin = rect.position;
        Vector2 anchorMax = rect.position + rect.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        m_rectTransform.anchorMin = anchorMin;
        m_rectTransform.anchorMax = anchorMax;
    }
}
