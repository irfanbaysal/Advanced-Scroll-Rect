using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public sealed class CircleLayoutGroup : UIBehaviour,ILayoutElement
{
    [Range( 0f, 360f)]
    [SerializeField] private float minAngle;

    [Range( 0f, 360f)]
    [SerializeField] private float maxAngle;

    [Range( 0f, 360f)]
    [SerializeField] private float startAngle;
    
    [SerializeField] private float distance;
    public float minWidth { get; }
    public float preferredWidth { get; }
    public float flexibleWidth { get; }
    public float minHeight { get; }
    public float preferredHeight { get; }
    public float flexibleHeight { get; }
    public int layoutPriority { get; }
    
    
    public CircleLayoutGroup(float minWidth, float preferredWidth, float flexibleWidth, float minHeight, float preferredHeight, float flexibleHeight, int layoutPriority)
    {
        this.minWidth = minWidth;
        this.preferredWidth = preferredWidth;
        this.flexibleWidth = flexibleWidth;
        this.minHeight = minHeight;
        this.preferredHeight = preferredHeight;
        this.flexibleHeight = flexibleHeight;
        this.layoutPriority = layoutPriority;
    }
    
    protected override void OnEnable()
    { 
        base.OnEnable();
        UpdateLayoutGroup();
    }
    
    protected override void OnRectTransformDimensionsChange()
    {
         UpdateLayoutGroup();
    }

    public void CalculateLayoutInputVertical()
    {
        UpdateLayoutGroup();
    }

    public void CalculateLayoutInputHorizontal()
    {
        UpdateLayoutGroup();
    }
    
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        UpdateLayoutGroup();
    }
#endif
    private void UpdateLayoutGroup()
    {
        if ( GetChildCount() == 0 )
            return;
        var angleOffset = ( ( maxAngle - minAngle ) ) / ( transform.childCount - 1 );
    
        var currentAngle = startAngle;
        for ( int i = 0; i < GetChildCount(); i++ )
        {
            var child = (RectTransform)transform.GetChild(i);
            if (child == null) continue;
            var vPos = new Vector3( Mathf.Cos(GetUpdatedAngle(currentAngle)), Mathf.Sin(GetUpdatedAngle(currentAngle)), 0);
            child.localPosition = vPos * distance;
            child.anchorMin = child.anchorMax = child.pivot = new Vector2(0.5f, 0.5f);
            currentAngle += angleOffset;
        }
    }

    private int GetChildCount()
    {
        return transform.childCount;
    }

    private float GetUpdatedAngle(float currentAngle) => currentAngle * Mathf.Deg2Rad;
}