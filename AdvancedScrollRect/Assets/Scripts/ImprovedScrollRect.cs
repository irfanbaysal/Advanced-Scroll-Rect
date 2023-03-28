using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImprovedScrollRect : ScrollRect
{
    protected virtual bool IsHandheld => SystemInfo.deviceType == DeviceType.Handheld;
    protected virtual bool IsDesktop => SystemInfo.deviceType == DeviceType.Desktop;

    private readonly Dictionary<int, Vector2> _activeTouches = new Dictionary<int, Vector2>();
    private const int MultiTouchCountTrigger = 1;
    private Vector2 _scrollingOffset = Vector2.zero;
    private bool _isScrolling;

    public void EnableHorizontal(bool isEnabled)
    {
        horizontal =isEnabled;
    }
    public void EnableVertical(bool isEnabled)
    {
        vertical = isEnabled;
    }
    public override void OnScroll(PointerEventData data)
    {
        base.OnScroll(data);
        _isScrolling = data.IsScrolling();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        UpdateDragCondition(false,Vector2.zero);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        switch (IsHandheld)
        {
            case true:
            {
                SetPointerID(eventData);
                if (Input.touchCount >= MultiTouchCountTrigger && !_isScrolling)
                {
                    base.OnBeginDrag(eventData);
                    UpdateDragCondition(true,eventData.position);
                }

                break;
            }
            default:
            {
                if (IsDesktop)
                {
                    base.OnBeginDrag(eventData);
                }

                break;
            }
        }
    }


    public override void OnDrag(PointerEventData eventData)
    {
        switch (IsHandheld)
        {
            case true when Input.touchCount < MultiTouchCountTrigger || !_activeTouches.ContainsKey(eventData.pointerId):
                return;
            case true:
                _scrollingOffset += eventData.position - _activeTouches[eventData.pointerId];
                SetPointerID(eventData);
                eventData.position = _scrollingOffset;
                base.OnDrag(eventData);
                break;
            default:
            {
                if (IsDesktop)
                {
                    base.OnDrag(eventData);
                }

                break;
            }
        }
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        switch (IsHandheld)
        {
            case true:
            {
                    
                if (_activeTouches.ContainsKey(eventData.pointerId))
                {
                    _activeTouches.Remove(eventData.pointerId);
                }

                if (Input.touchCount != MultiTouchCountTrigger)
                {
                    return;
                }

                base.OnEndDrag(eventData);
                UpdateDragCondition(false,Vector2.zero);
                break;
            }
            default:
            {
                if (IsDesktop)
                {
                    base.OnEndDrag(eventData);
                }

                break;
            }
        }
    }

    private void UpdateDragCondition(bool isBegan,Vector2 updatedOffset)
    {
        _isScrolling = isBegan;
        _scrollingOffset = updatedOffset;
    }

    private void SetPointerID(PointerEventData pointerEventData)
    {
        _activeTouches[pointerEventData.pointerId] = pointerEventData.position;
    }
}