using UnityEngine;

public class Rocket : MonoBehaviour
{
	private Rigidbody _rigidbody;
	private AudioSource _audioSource;
	
	// Use this for initialization
	void Start ()
	{
		_audioSource = GetComponent<AudioSource>();
		_rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		ProcessInput();
	}

	private void ProcessInput()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			if (!_audioSource.isPlaying)
				_audioSource.Play();
			
			_rigidbody.AddRelativeForce(Vector3.up);
		}
		else if (_audioSource.isPlaying)
		{
			_audioSource.Stop();
		}

		if (Input.GetKey(KeyCode.A))
		{
//			print(Time.deltaTime);
			transform.Rotate(Vector3.forward);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			transform.Rotate(-Vector3.forward);
		}
	}
}
