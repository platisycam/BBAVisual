using UnityEngine;  
using System.Collections;  
using UnityEngine.EventSystems;  
using UnityEngine.UI;  


public class DragBase : MonoBehaviour,IPointerDownHandler,IDragHandler{  
	
	private Vector2 Local_Pointer_Position;  
	private Vector3 Panel_Local_Position;  
	private RectTransform targetObject;   
	private RectTransform parentRectTransform;  //父窗口矩阵  
	private RectTransform targetRectTransform; //移动目标矩阵  
	
	void Awake()  
	{
        targetObject = this.targetRectTransform;

        if (targetObject == null) {  
			targetObject=transform as RectTransform;  
		}  
		

		
	}

	void Start(){
		parentRectTransform = targetObject.parent as RectTransform;  
		targetRectTransform=targetObject as RectTransform; 
		//Debug.Log("parentRectTransform: " + parentRectTransform + ",  targetRectTransform: " + targetRectTransform);
	}
	
	public void OnPointerDown(PointerEventData data)  
	{  
		Panel_Local_Position = targetRectTransform.localPosition;  
		RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, data.position, data.pressEventCamera, out Local_Pointer_Position);  
		targetObject.gameObject.transform.SetAsLastSibling();//保证当前操作的对象能够优先渲染，即不会被其它对象遮挡住  
		
		
	}  
	
	

	
	public void OnDrag(PointerEventData data)  
	{  
		
		Vector2 localPointerPosition;  
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, data.position, data.pressEventCamera, out localPointerPosition))  
		{  
			Vector3 offsetToOriginal = localPointerPosition - Local_Pointer_Position;  
			
			targetObject.localPosition = Panel_Local_Position + offsetToOriginal;  
		}  
		
	}  
//	
	// begin dragging
	public void OnBeginDrag(PointerEventData eventData)
	{
		SetDraggedPosition(eventData);
	}

	// during dragging
//	public void OnDrag(PointerEventData eventData)
//	{
//		SetDraggedPosition(eventData);
//	}
//
	// end dragging
	public void OnEndDrag(PointerEventData eventData)
	{
		SetDraggedPosition(eventData);
	}

	/// <summary>
	/// set position of the dragged game object
	/// </summary>
	/// <param name="eventData"></param>
	private void SetDraggedPosition(PointerEventData eventData)
	{
		var rt = gameObject.GetComponent<RectTransform>();

		// transform the screen point to world point int rectangle
		Vector3 globalMousePos;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out globalMousePos))
		{
			rt.position = globalMousePos;
		}
	}
	
	
}  