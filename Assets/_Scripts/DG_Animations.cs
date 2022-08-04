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
        //Camera.main.DOShakePosition(0.5f, 1, 10, 90);
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
    public void DeathAnimation()
    {
        Camera.main.DOShakePosition(0.5f, 1, 10, 90);

        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        gameObject.GetComponent<SpriteRenderer>().DOFade(0.3f, 2);
    }
}
