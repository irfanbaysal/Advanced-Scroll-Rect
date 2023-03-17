using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum Type
{
    Fade,
    Scale,
    ScaleWithDistance
}

public class CircleLayoutManager : MonoBehaviour
{

    [SerializeField] private Type appearType, disappearType;
    
   private CircleLayoutGroup CircleLayoutGroup =>
      _circleLayoutGroup ? _circleLayoutGroup : (_circleLayoutGroup = GetComponent<CircleLayoutGroup>());
   private CircleLayoutGroup _circleLayoutGroup;

   private List<Transform> _children = new List<Transform>();

   private void Awake()
   {
       _children=  CircleLayoutGroup.transform.GetComponentsInChildren<Transform>().ToList();   
   }

   private void Appear()
   {
      PerformTypes(appearType,true);
   }

   private void Disappear()
   {
       PerformTypes(disappearType,false);
   }

   private void PerformTypes(Type type,bool isAppear)
   {
       switch (type)
       {
           case Type.Fade when isAppear:
               FadeOpen();
               break;
           case Type.Fade:
               FadeClose();
               break;
           case Type.Scale when isAppear:
               ScaleOpen();
               break;
           case Type.Scale:
               ScaleClose();
               break;
           case Type.ScaleWithDistance when isAppear:
               ScaleOpenWithDistance();
               break;
           case Type.ScaleWithDistance:
               ScaleCloseWithDistance();
               break;
       }
   }

   private void FadeOpen()
   {
       var list = _children.ToList();
       CircleLayoutGroup.UpdateMinAngle();
       CircleLayoutGroup.SetDistance(150);
       list.RemoveAt(0);
       list.ForEach(x => x.transform.localScale = Vector3.zero);
       list.ForEach(x=>x.transform.GetComponent<Image>().DOFade(0f,0f));
       for (int i = 0; i < list.Count; i++)
       {
           list[i].transform.DOScale(1f, .25f).SetDelay(i*.05f);
           list[i].transform.GetComponent<Image>().DOFade(1f, .25f).SetDelay(i * .05f);
       }
  
       DOVirtual.Float(CircleLayoutGroup.GetMaxAngle(), CircleLayoutGroup.GetMinAngle(), 1f, (value =>
       {
           CircleLayoutGroup.SetMinAngle(value);
           CircleLayoutGroup.UpdateLayoutGroup();
       })).SetEase(Ease.InOutSine);
   }

   private void FadeClose()
   {
       var l=  CircleLayoutGroup.transform.GetComponentsInChildren<Transform>().ToList();
       l.RemoveAt(0);
       l.Reverse();
       for (int i = 0; i < l.Count; i++)
       {
           l[i].transform.DOScale(0f, .55f).SetDelay(i*.05f);
           l[i].transform.GetComponent<Image>().DOFade(0f, .45f).SetDelay(i * .05f);
       }
  
       DOVirtual.Float(CircleLayoutGroup.GetMinAngle(), CircleLayoutGroup.GetMaxAngle(), 1f, (value =>
       {
           CircleLayoutGroup.SetMinAngle(value);
           CircleLayoutGroup.UpdateLayoutGroup();
       })).SetEase(Ease.InOutSine);
   }
   
   private void ScaleOpen()
   {
       var list = _children.ToList();
       CircleLayoutGroup.UpdateMinAngle();
       CircleLayoutGroup.UpdateLayoutGroup();
       CircleLayoutGroup.SetDistance(150);
       list.RemoveAt(0);
       list.ForEach(x => x.transform.localScale = Vector3.zero);
       list.ForEach(x=>x.transform.GetComponent<Image>().DOFade(0f,0f));

       for (var i = 0; i < list.Count; i++)
       {
           list[i].transform.DOScale(1f, .5f).SetEase(Ease.OutBack);
           list[i].transform.GetComponent<Image>().DOFade(1f, .25f);
       }
   }
   
   private void ScaleOpenWithDistance()
   {
       var list = _children.ToList();
       CircleLayoutGroup.UpdateMinAngle();
       CircleLayoutGroup.UpdateLayoutGroup();
       list.RemoveAt(0);
       list.ForEach(x => x.transform.localScale = Vector3.zero);
       list.ForEach(x=>x.transform.GetComponent<Image>().DOFade(1f,0f));

       DOVirtual.Float(0f, 150f, .5f, (value =>
       {
           CircleLayoutGroup.SetDistance(value);
           CircleLayoutGroup.UpdateLayoutGroup();

       })).SetEase(Ease.OutBack);

       foreach (var t in list)
       {
           t.transform.DOScale(1f, .25f).SetEase(Ease.OutBack);
       }
   }
   
   private void ScaleCloseWithDistance()
   {
       var list = _children.ToList();
       CircleLayoutGroup.UpdateLayoutGroup();
       list.RemoveAt(0);
       DOVirtual.Float(150f, 0f, .5f, (value =>
       {
           CircleLayoutGroup.SetDistance(value);
           CircleLayoutGroup.UpdateLayoutGroup();

       })).SetEase(Ease.InBack);

       foreach (var t in list)
       {
           Sequence sequence = DOTween.Sequence();
           sequence.Append(t.transform.DOScale(0f, .6f).SetEase(Ease.InBack));
           sequence.Join(t.transform.GetComponent<Image>().DOFade(0f, .5f));
       }
   }
   
   private void ScaleClose()
   {
       var list = _children.ToList();
       CircleLayoutGroup.UpdateMinAngle();
       list.RemoveAt(0);
       for (var i = 0; i < list.Count; i++)
       {
           list[i].transform.DOScale(0f, .5f).SetEase(Ease.InBack);
       }
   }
   
   

   private void Update()
   {
       if (Input.GetKeyDown(KeyCode.A))
       {
           Appear();
       }
       
       if (Input.GetKeyDown(KeyCode.S))
       {
          Disappear();
       }
   }
}
