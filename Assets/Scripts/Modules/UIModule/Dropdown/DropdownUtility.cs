using TMPro;
using UnityEngine;

public class DropdownUtility : MonoBehaviour
{
    [SerializeField]
    private string _sortingLayerName = "Default";
    private void Awake()
    {
        SetDropdownLayer();
    }

    private void SetDropdownLayer()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.sortingLayerName = _sortingLayerName;
        }
    }
} 
