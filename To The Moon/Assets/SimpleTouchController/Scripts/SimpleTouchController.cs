using UnityEngine;

public class SimpleTouchController : MonoBehaviour {

	// PUBLIC
	public delegate void TouchDelegate(Vector2 value);
	public event TouchDelegate TouchEvent;

	public delegate void TouchStateDelegate(bool touchPresent);
	public event TouchStateDelegate TouchStateEvent;

	// PRIVATE
	[SerializeField]
	private RectTransform joystickArea;
    private Vector2 movementVector;

    public Vector2 GetTouchPosition
	{
		get { return movementVector;}
	}

    public bool TouchPresent { get; set; } = false;

    public void BeginDrag()
	{
		TouchPresent = true;
        Debug.Log("Begin Drag: " + TouchPresent);
        TouchStateEvent?.Invoke(TouchPresent);
    }

	public void EndDrag()
	{
		TouchPresent = false;
        Debug.Log("End Drag: " + TouchPresent);
        movementVector = joystickArea.anchoredPosition = Vector2.zero;

        TouchStateEvent?.Invoke(TouchPresent);

    }

	public void OnValueChanged(Vector2 value)
	{
		if(TouchPresent)
		{
			// convert the value between 1 0 to -1 +1
			movementVector.x = ((1 - value.x) - 0.5f) * 2f;
			movementVector.y = ((1 - value.y) - 0.5f) * 2f;

            TouchEvent?.Invoke(movementVector);
        }

	}

}
