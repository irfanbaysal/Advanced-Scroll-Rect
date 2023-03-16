using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(LayoutElement))]
public sealed class CircleLayoutGroup : UIBehaviour,ILayoutElement,ILayoutGroup
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

    private DrivenRectTransformTracker _mTracker;
    private RectTransform RectTransform => transform as RectTransform;
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

    #region UI-Behaviour
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
    public void SetLayoutHorizontal()
    {
        UpdateMinAngle();
    }
    public void SetLayoutVertical()
    {
        UpdateMinAngle();
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        UpdateLayoutGroup();
    }
#endif
    #endregion
    public void UpdateLayoutGroup()
    {
        var childCount = GetChildCount();
        if (childCount == 0)
            return;
        var angleOffset = (( maxAngle - minAngle)) / ( childCount - 1 );
        var currentAngle = startAngle;
        for ( var i = 0; i < childCount; i++ )
        {
            var child = (RectTransform)transform.GetChild(i);
            if (child == null) continue;
            var currentPosition = new Vector2(Mathf.Cos(GetUpdatedAngle(currentAngle)), Mathf.Sin(GetUpdatedAngle(currentAngle)));
            child.localPosition = currentPosition * distance;
            child.anchorMin = child.anchorMax = child.pivot = Vector2.one *.5f;
            currentAngle += angleOffset;
        }
    }
    private int GetChildCount()
    {
        return transform.childCount;
    }

    public void UpdateMinAngle()
    {
        var proportion = maxAngle / GetChildCount();
        minAngle = proportion;
    }
    public float GetMinAngle()
    {
        return minAngle;
    }
    public void SetMinAngle(float updatedValue)
    {
        minAngle = updatedValue;
    }
    
    public float GetMaxAngle()
    {
        return maxAngle;
    }
    
    private float GetUpdatedAngle(float currentAngle) => currentAngle * Mathf.Deg2Rad;

}