using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class NumberDisplay : MonoBehaviour
{
    [FormerlySerializedAs("SpriteSet")] public Sprite[] spriteSet;
    [FormerlySerializedAs("SpriteRenderers")] public SpriteRenderer[] spriteRenderers;

    public int displayNumberTest = 0;

    private Color clearWhite = new Color(1, 1, 1, 0);

    public void UpdateNumber(int displayNumber, float destroyTime)
    {
        int n1 = displayNumber % 10;
        int n2 = (displayNumber % 100) / 10;
        int n3 = (displayNumber % 1000) / 100;
        int n4 = (displayNumber % 10000) / 1000;
        int n5 = (displayNumber % 100000) / 10000;
        int n6 = (displayNumber % 1000000) / 100000;

        spriteRenderers[0].sprite = spriteSet[n1];
        spriteRenderers[1].sprite = spriteSet[n2];
        spriteRenderers[2].sprite = spriteSet[n3];
        spriteRenderers[3].sprite = spriteSet[n4];
        spriteRenderers[4].sprite = spriteSet[n5];
        spriteRenderers[5].sprite = spriteSet[n6];

        string s = displayNumber.ToString();
        int l = s.Length;
        
        for (int i = 0; i < spriteRenderers.Length; i++)
        {

            if (i >= l)
            {
                if (i != 0)
                {
                    spriteRenderers[i].gameObject.SetActive(false);
                }
                
            }
            else
            {
                spriteRenderers[i].gameObject.SetActive(true);
            }
            
            spriteRenderers[i].transform.localPosition = Vector3.right * ( (l - 1) * 0.2f - i * 0.4f); 
        }
        
        if (destroyTime != 0)
        {
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(destroyTime);
            seq.Append(transform.DOMove(Vector3.up * 0.5f, 0.3f).SetRelative(true));
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                seq.Join(spriteRenderers[i].DOColor(clearWhite, 0.3f));
            }

            seq.AppendCallback(() => Destroy(gameObject));
            //Destroy(gameObject, destroyTime);
        }
    }

    // 简单版本
    public void UpdateNumber(int displayNumber)
    {
        UpdateNumber(displayNumber, 0);
    }

    ////测试专用
    //public void OnValidate()
    //{
    //    UpdateNumber(displayNumberTest, 0);
    //}

    //public void Update()
    //{
    //    if (Input.GetKey(KeyCode.Space))
    //    {
    //        UpdateNumber(displayNumberTest, 1);
    //    }
    //}
}
