using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class UIEventListener : UnityEngine.EventSystems.EventTrigger
{
	public delegate void VoidDelegate (GameObject go);
	public VoidDelegate onClick;
	public VoidDelegate onDown;
	public VoidDelegate onEnter;
	public VoidDelegate onExit;
	public VoidDelegate onUp;
	public VoidDelegate onSelect;
	public VoidDelegate onUpdateSelect;
	public VoidDelegate onBeginDrag;
	public VoidDelegate onEndDrag;

	static public UIEventListener Get (GameObject go)
	{
		UIEventListener listener = go.GetComponent<UIEventListener>();
		if (listener == null) listener = go.AddComponent<UIEventListener>();
		return listener;
	}
	public override void OnPointerClick(PointerEventData eventData)
	{
		if(onClick != null) 	onClick(gameObject);
	}
	public override void OnPointerDown (PointerEventData eventData){
		if(onDown != null) onDown(gameObject);
	}
	public override void OnPointerEnter (PointerEventData eventData){
		if(onEnter != null) onEnter(gameObject);
	}
	public override void OnPointerExit (PointerEventData eventData){
		if(onExit != null) onExit(gameObject);
	}
	public override void OnPointerUp (PointerEventData eventData){
		if(onUp != null) onUp(gameObject);
	}
	public override void OnSelect (BaseEventData eventData){
		if(onSelect != null) onSelect(gameObject);
	}
	public override void OnUpdateSelected (BaseEventData eventData){
		if(onUpdateSelect != null) onUpdateSelect(gameObject);
	}

	public override void OnBeginDrag(PointerEventData eventData)
	{
		if (onBeginDrag != null) onBeginDrag(gameObject);
	}

	public override void OnEndDrag(PointerEventData eventData)
	{
		if(onEndDrag != null)
			onEndDrag(gameObject);
	}
}