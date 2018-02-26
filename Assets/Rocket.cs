using UnityEngine;

public class Rocket : MonoBehaviour
{
	[SerializeField] private float rcsThrust = 100f;
	[SerializeField] private float thrust = 10f;
	
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
		Rotate();
		Thrust();
	}

	void OnCollisionEnter(Collision collision)
	{
		switch (collision.gameObject.tag)
		{
				case "Friendly":
					print("OK");
					break;
				default:
					print("Dead");
					break;
		}
	}

	private void Rotate()
	{
		_rigidbody.freezeRotation = true; // switch off physics rotation

		float rotationThsFrame = rcsThrust * Time.deltaTime;
		
		if (Input.GetKey(KeyCode.A))
		{
			transform.Rotate(Vector3.forward * rotationThsFrame);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			transform.Rotate(-Vector3.forward * rotationThsFrame);
		}

		_rigidbody.freezeRotation = false; // relinquish manual control of rotation
	}

	private void Thrust()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			if (!_audioSource.isPlaying)
				_audioSource.Play();

			_rigidbody.AddRelativeForce(Vector3.up * thrust);
		}
		else if (_audioSource.isPlaying)
		{
			_audioSource.Stop();
		}
	}
}
