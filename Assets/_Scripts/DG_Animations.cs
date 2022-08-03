using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DG_Animations : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] bool scalingAnim, shoot;
    void Start()
    {
        if (scalingAnim) ScalingAnimation(2, 0.6f);
        if (shoot) Shoot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ScalingAnimation(float speed, float scale)
    {   

        gameObject.GetComponent<RectTransform>().DOScale(scale, speed).SetLoops(-1, LoopType.Yoyo);
    }
    public void Shoot()
    {
        //gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(255, 255, 255), 1f);
        gameObject.transform.DOScale(1, 0.5f);
    }
}
