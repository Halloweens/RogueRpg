using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
	public static TimeManager Instance { get { if (!instance) instance = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>(); return instance; } }
	private static TimeManager instance;

	public double TimeSinceFirstStart { get { return timeSinceFirstStart; } set { timeSinceFirstStart = value; } }
	private double timeSinceFirstStart;

	public double TimeSinceStart { get { return timeSinceStart; } }
	private double timeSinceStart = 0f;

	public event Action changePause;

	public bool Pause { get { return pause; } set { pause = value; OnPauseChange(); ChangeTimeScale(); } }
	private bool pause = false;

	private bool started = false;
	
	void Start ()
	{
		started = true;
	}
	
	void Update ()
	{
		if (!pause && started)
		{
			timeSinceStart += Time.deltaTime;
			timeSinceFirstStart += Time.deltaTime;
		}
	}

	private void OnPauseChange()
	{
		if (changePause != null)
			changePause();
	}

	private void ChangeTimeScale()
	{
		Time.timeScale = pause ? 0f : 1f;
	}
}
