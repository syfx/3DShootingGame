using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponShowControl : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler{

    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        private set { moveSpeed = Mathf.Clamp01(value); }
    }

    bool isDrag = false;                                                                            //是否在拖拽中
    private Vector2 Effect;                                                                        //鼠标位置与现实面板之间的偏移量
    RectTransform rectTransform;


    void Start()
    {
        rectTransform = transform.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (!isDrag)
        {
            if (transform.GetComponent<RectTransform>().anchoredPosition.x > 0)
            {
                rectTransform.anchoredPosition =
                    new Vector2(Mathf.Lerp(rectTransform.anchoredPosition.x, 0, 0.5f), rectTransform.anchoredPosition.y);
            } 
            if (transform.GetComponent<RectTransform>().anchoredPosition.x < -3040)
            {
                rectTransform.anchoredPosition =
                    new Vector2(Mathf.Lerp(rectTransform.anchoredPosition.x, -3040, 0.5f), rectTransform.anchoredPosition.y);
            }
                
            
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        Effect = rectTransform.anchoredPosition - eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = new Vector2(eventData.position.x + Effect.x, rectTransform.anchoredPosition.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
    }
}
