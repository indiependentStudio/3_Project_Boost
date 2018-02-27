using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
	[SerializeField] private float _rcsThrust = 100f;
	[SerializeField] private float _thrust = 10f;
	[SerializeField] private AudioClip _mainEngine;
	[SerializeField] private AudioClip _death;
	[SerializeField] private AudioClip _success;
	
	private Rigidbody _rigidbody;
	private AudioSource _audioSource;

	enum State
	{
		Alive,
		Dying,
		Transcending
	}

	private State _state = State.Alive;
	
	// Use this for initialization
	void Start ()
	{
		_audioSource = GetComponent<AudioSource>();
		_rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// TODO now the engine sound still plays on death
		if (_state == State.Alive)
		{
			RespondToRotateInput();
			RespondToThrustInput();
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (_state != State.Alive)
			return;

		switch (collision.gameObject.tag)
		{
				case "Friendly":
					break;
				case "Finish":
					StartSuccessSequence();
					break;
				default:
					StartDeathSequence();
					break;
		}
	}
	
	private void StartSuccessSequence()
	{
		_state = State.Transcending;
		_audioSource.Stop();
		_audioSource.PlayOneShot(_success);
		Invoke("LoadNextLevel", 1f); // TODO parameterise time
	}

	private void StartDeathSequence()
	{
		_state = State.Dying;
		_audioSource.Stop();
		_audioSource.PlayOneShot(_death);
		Invoke("LoadFirstLevel", 1f); // TODO parameterise time
	}

	private void LoadFirstLevel()
	{
		SceneManager.LoadScene(0);
	}

	private void LoadNextLevel()
	{
		SceneManager.LoadScene(1); // TODO allow for more than 2 levels
	}

	private void RespondToRotateInput()
	{
		_rigidbody.freezeRotation = true; // switch off physics rotation

		float rotationThsFrame = _rcsThrust * Time.deltaTime;
				
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

	private void RespondToThrustInput()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			ApplyThrust();
		}
		else if (_audioSource.isPlaying)
		{
			_audioSource.Stop();
		}
	}

	private void ApplyThrust()
	{
		if (!_audioSource.isPlaying)
			_audioSource.PlayOneShot(_mainEngine);

		_rigidbody.AddRelativeForce(Vector3.up * _thrust);
	}
}
