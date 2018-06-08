using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	public Action m_OnMoveStart;
	public Action<int, int> m_OnMove;
	public Action m_OnMoveEnd;
	public enum JoystickType { Dynamic, Static };
	[SerializeField]
	private Graphic m_BackGround;
	[SerializeField]
	private Graphic m_Thumb;
	[SerializeField]
	private JoystickType m_Type;
	[SerializeField]
	private float m_Radius = 50;

	private float m_IntervalTime = 0;
	private float m_DeltaTime = 0;
	private bool m_InMove = false;
	private int unit = 10000;

	private RectTransform m_RectTransform;
	private Vector2 m_CurrentPosition = Vector2.zero;
	private Vector2 m_StartPosition = Vector2.zero;
	private Vector2 normalDelata = Vector2.zero;
	private bool m_IsDynamic;

	void Awake()
	{
		m_RectTransform = transform as RectTransform;
		m_IsDynamic = m_Type == JoystickType.Dynamic;
		m_BackGround.gameObject.SetActive(!m_IsDynamic);
		m_Thumb.gameObject.SetActive(!m_IsDynamic);
		m_InMove = false;
	}

	public void Register(Action onMoveStart, Action<int, int> onMove, Action onMoveEnd, float time = 0)
	{
		m_OnMoveStart = onMoveStart;
		m_OnMove = onMove;
		m_OnMoveEnd = onMoveEnd;
		m_IntervalTime = time;
	}

	public void AddMoveStartListener(Action onMoveStart)
	{
		m_OnMoveStart = onMoveStart;
	}

	public void AddMoveListener(Action<int, int> onMove)
	{
		m_OnMove = onMove;
	}

	public void AddMoveEndListener(Action onMoveEnd)
	{
		m_OnMoveEnd = onMoveEnd;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		ResetLocalPoint(eventData);
		if (m_IsDynamic)
		{
			m_BackGround.gameObject.SetActive(true);
			m_Thumb.gameObject.SetActive(true);
			m_BackGround.rectTransform.anchoredPosition = m_CurrentPosition;
			m_Thumb.rectTransform.anchoredPosition = m_CurrentPosition;
			m_StartPosition = m_CurrentPosition;
		}
		if (m_OnMoveStart != null)
		{
			m_OnMoveStart();
		}
		m_InMove = true;
		m_DeltaTime = 0;
		normalDelata = Vector2.zero;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (m_IsDynamic)
		{
			m_BackGround.gameObject.SetActive(false);
			m_Thumb.gameObject.SetActive(false);
		}
		if (m_OnMoveEnd != null)
		{
			m_OnMoveEnd();
		}
		m_InMove = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		ResetLocalPoint(eventData);
		normalDelata = (m_CurrentPosition - m_StartPosition).normalized;
		if (Vector2.Distance(m_CurrentPosition, m_StartPosition) > m_Radius)
		{
			m_Thumb.rectTransform.anchoredPosition = normalDelata * m_Radius + m_StartPosition;
		}
		else
		{
			m_Thumb.rectTransform.anchoredPosition = m_CurrentPosition;
		}
	}

	void Update()
	{
		if (m_InMove)
		{
			if (m_DeltaTime <= 0)
			{
				if (m_OnMove != null)
				{
					m_OnMove((int)(normalDelata.x * unit), (int)(normalDelata.y * unit));
				}
				m_DeltaTime = m_IntervalTime;
			}
			m_DeltaTime -= Time.deltaTime;
		}
	}

	private void ResetLocalPoint(PointerEventData eventData)
	{
		RectTransformUtility.ScreenPointToLocalPointInRectangle(m_RectTransform, eventData.position, eventData.pressEventCamera, out m_CurrentPosition);
	}
}
