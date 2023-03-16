using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CircleLayoutManager : MonoBehaviour
{
   private CircleLayoutGroup CircleLayoutGroup =>
      _circleLayoutGroup ? _circleLayoutGroup : (_circleLayoutGroup = GetComponent<CircleLayoutGroup>());
   private CircleLayoutGroup _circleLayoutGroup;

   private List<Transform> _children = new List<Transform>();

   private void Awake()
   {
       _children=  CircleLayoutGroup.transform.GetComponentsInChildren<Transform>().ToList();   
   }

   private void FadeOpen()
   {
       var list = _children.ToList();
       CircleLayoutGroup.UpdateMinAngle();
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
       list.RemoveAt(0);
       list.ForEach(x => x.transform.localScale = Vector3.zero);
       list.ForEach(x=>x.transform.GetComponent<Image>().DOFade(0f,0f));

       for (var i = 0; i < list.Count; i++)
       {
           list[i].transform.DOScale(1f, .5f).SetEase(Ease.OutBack);
           list[i].transform.GetComponent<Image>().DOFade(1f, .25f);
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
           FadeOpen();
       }
       
       if (Input.GetKeyDown(KeyCode.S))
       {
           FadeClose();
       }
       
       if (Input.GetKeyDown(KeyCode.D))
       {
           ScaleOpen();
       }
       
       if (Input.GetKeyDown(KeyCode.F))
       {
           ScaleClose();
       }
   }
}
