using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputButtonEvent : Button
{
	public new bool IsPressed { get; private set; }
	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown(eventData);
		IsPressed = true;
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		IsPressed = false;
	}
}
