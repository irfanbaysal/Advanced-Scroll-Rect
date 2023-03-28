# Advanced-Scroll-Rect
- A solution for mobile input that supports multi touching.
- It fixes scroll rect teleportation glitch while multi touching conditions. 
- It is derived from Unity's ScrollRect component.
- It uses default scroll rect implementations for Desktop.
```c#
ImprovedScrollRect : ScrollRect
```
<img width="510" alt="Screenshot 2023-03-28 at 14 46 26" src="https://user-images.githubusercontent.com/43708297/228226128-261f475c-b294-4fb1-9b60-1d76bf0418d0.png">

- OnBeginDrag checks its data handling according to device type.

```c#
 protected virtual bool IsHandheld => SystemInfo.deviceType == DeviceType.Handheld;
 protected virtual bool IsDesktop => SystemInfo.deviceType == DeviceType.Desktop;

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
 ```
