using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
	[SerializeField] private float _rcsThrust = 100f;
	[SerializeField] private float _thrust = 10f;
	[SerializeField] private float _levelLoadDelay = 2f;
	
	[SerializeField] private AudioClip _mainEngine;
	[SerializeField] private AudioClip _death;
	[SerializeField] private AudioClip _success;
	
	[SerializeField] private ParticleSystem _mainEngineParticles;
	[SerializeField] private ParticleSystem _deathParticles;
	[SerializeField] private ParticleSystem _successParticles;
	
	private Rigidbody _rigidbody;
	private AudioSource _audioSource;
    private bool _godMode;

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
        _godMode = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_state == State.Alive)
        {
            RespondToRotateInput();
            RespondToThrustInput();
        }

        RespondToDebugKeys();
    }

    private void RespondToDebugKeys()
    {
        if (Debug.isDebugBuild)
        {
            if (Input.GetKey(KeyCode.L))
            {
                Invoke("LoadNextLevel", 0);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                _godMode = !_godMode;
            }
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
				    if (_godMode)
				        break;
				    StartDeathSequence();
					break;
		}
	}
	
	private void StartSuccessSequence()
	{
		_state = State.Transcending;
		_audioSource.Stop();
		_audioSource.PlayOneShot(_success);
		_successParticles.Play();
		Invoke("LoadNextLevel", _levelLoadDelay);
	}

	private void StartDeathSequence()
	{
		_state = State.Dying;
		_audioSource.Stop();
		_audioSource.PlayOneShot(_death);
		_deathParticles.Play();
		Invoke("LoadFirstLevel", _levelLoadDelay);
	}

	private void LoadFirstLevel()
	{
		SceneManager.LoadScene(0);
	}

	private void LoadNextLevel()
	{
	    int currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
	    int countOfAllScenes = SceneManager.sceneCountInBuildSettings;

	    int nextLevel = currentSceneBuildIndex + 1 < countOfAllScenes ? currentSceneBuildIndex + 1 : 0;

        SceneManager.LoadScene(nextLevel);
	}

	private void RespondToRotateInput()
	{
		_rigidbody.angularVelocity = Vector3.zero; // remove rotation due to physics

		float rotationThsFrame = _rcsThrust * Time.deltaTime;

	    if (Input.GetKey(KeyCode.A))
	        transform.Rotate(Vector3.forward * rotationThsFrame);
	    else if (Input.GetKey(KeyCode.D))
	        transform.Rotate(-Vector3.forward * rotationThsFrame);
	}

	private void RespondToThrustInput()
	{
	    if (Input.GetKey(KeyCode.Space))
	        ApplyThrust();
	    else if (_audioSource.isPlaying)
	        StopApplyingThrust();
	}

    private void StopApplyingThrust()
    {
        _audioSource.Stop();
        _mainEngineParticles.Stop();
    }

    private void ApplyThrust()
	{
		if (!_audioSource.isPlaying)
			_audioSource.PlayOneShot(_mainEngine);
		
		_mainEngineParticles.Play();

		_rigidbody.AddRelativeForce(Vector3.up * _thrust * Time.deltaTime );
	}
}
