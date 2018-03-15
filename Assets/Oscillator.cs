using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

	[SerializeField] private Vector3 _movementVector = new Vector3(-18f, 0f, 0f);
	[SerializeField] private float _period = 6f;
	
	[Range(0, 1)] private float _movementFactor; // 0 is not moved, 1 is fully moved

	private Vector3 _startingPos;
	
	// Use this for initialization
	void Start ()
	{
		_startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{

		float cycles;

		if (_period <= Mathf.Epsilon)
			return;

		cycles = Time.time / _period; // Grows continually from 0	
		const float tau = Mathf.PI * 2;
		float rawSinWave = Mathf.Sin(cycles * tau);

		_movementFactor = rawSinWave / 2f + 0.5f;
		
		Vector3 offset = _movementVector * _movementFactor;
		transform.position = _startingPos + offset;
	}
}
