using UnityEngine.SceneManagement;
using UnityEngine;

public class Movement : MonoBehaviour {

    public Rigidbody Rigidbody;

    public AudioSource AudioSource;

    public float ThrustForce;

    string CurrentScene;

    public float TurnForce;

    public enum State { Alive, Dying, Transcending}

    [SerializeField] private AudioClip RocketThrust;

    State CurrentState;

     void OnCollisionEnter(Collision collision)
    {
        if (CurrentState != State.Alive)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;

            case "Finish":
                CurrentState = State.Transcending;
                AudioSource.Stop();
                Invoke("LoadNextScene", 1f);
                break;

            default:
                CurrentState = State.Dying;
                AudioSource.Stop();
                Invoke("Die", 1f);
                break;
        }
    }

     void Die()
    { 
        SceneManager.LoadScene(CurrentScene);
    }

    private void LoadNextScene()
    {
        CurrentState = State.Transcending;
        if (CurrentScene == "Level 1")
        {
            SceneManager.LoadScene(1);
        }

        else
        {
            SceneManager.LoadScene(0);
        }
    }

    private void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        CurrentScene = scene.name;
        Rigidbody = GetComponent<Rigidbody>();
        AudioSource = GetComponent<AudioSource>();
        CurrentState = State.Alive;
    }

    void Update()
    {
        if (CurrentState == State.Alive)
        {
            Thrust();
            Rotate();
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            SceneManager.LoadScene(0);
        }

        else if (Input.GetKey(KeyCode.Alpha2))
        {
            SceneManager.LoadScene(1);
        }
    }

    void Thrust()
    {
        if (Input.GetKey(KeyCode.Space) || (Input.GetKey(KeyCode.W)))
        {
            Rigidbody.AddRelativeForce(Vector3.up * (ThrustForce * Time.deltaTime));
            if (!AudioSource.isPlaying)
            {           
                AudioSource.Play();
            }
        }

        else if (!(Input.GetKey(KeyCode.Space)) || (Input.GetKey(KeyCode.W)))
        {
            AudioSource.Stop();
        }
    }

    void Rotate()
    {
        Rigidbody.freezeRotation = true;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * (TurnForce * Time.deltaTime));
        }

        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * (TurnForce * Time.deltaTime));
        }

        Rigidbody.freezeRotation = false;
    }
}
