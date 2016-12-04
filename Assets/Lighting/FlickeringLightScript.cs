// credit: Dream Reaper Studios, from Unity Asset Store
// modified for project
using UnityEngine;

public class FlickeringLightScript : MonoBehaviour
{
	[Tooltip("This is how big the light is. Experiment with it.")]public float scale;
	[Tooltip("The moves (new targets for properties; intensity, range, position) your light will do per second.")]public float speed;

	public Light light;

	private float deltaSum = 0;
	private float intensityOrigin;
	private float intensityOffset;
	private float intensityTarget;
	private float intensityDelta;
	private float rangeOrigin;
	private float rangeOffset;
	private float rangeTarget;
	private float rangeDelta;

	private bool setNewTargets;

	void Start ()
	{
		intensityOrigin = light.intensity;
		rangeOrigin = light.range;

		setNewTargets = true;

		scale *= 0.1f;
		speed *= 0.02f;

		intensityOffset = light.intensity * scale;
		rangeOffset = light.range * scale;
	}

	void Update()
	{
		if (deltaSum >= 0.02f)
		{
			if (setNewTargets)
			{
				intensityTarget = intensityOrigin + Random.Range (-intensityOffset, intensityOffset);
				intensityDelta = (intensityTarget - light.intensity) * speed;

				rangeTarget = rangeOrigin + Random.Range(-rangeOffset, rangeOffset);
				rangeDelta = (rangeTarget - light.range) * speed;

				setNewTargets = false;
			}

			light.intensity += intensityDelta;
			light.range += rangeDelta;

			if (Mathf.Abs(light.intensity - intensityTarget) < 5f * scale)
				setNewTargets = true;

			deltaSum -= 0.02f;
		}
		deltaSum += Time.deltaTime;
	}
}