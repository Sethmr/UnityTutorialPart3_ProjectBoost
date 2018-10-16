using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 25f;
    int currentScene;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        currentScene = 0;
        state = State.Alive;
    }
	
	// Update is called once per frame
	void Update () {
        ProcessInput();
	}

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly": case "Respawn":
                print("Friendly");
                break;
            case "Finish":
                if (state == State.Alive)
                {
                    state = State.Transcending;
                    Invoke("LoadNextScene", 1f);
                }         
                break;
            default:
                if (state == State.Alive)
                {
                    state = State.Dying;
                    Invoke("KillYou", 1f);
                }
                break;
        }
    }

    private void LoadNextScene()
    {
        currentScene++;
        SceneManager.LoadScene(currentScene);
        state = State.Alive;
    }

    private void KillYou()
    {
        SceneManager.LoadScene(currentScene);
        state = State.Alive;
    }

    private void ProcessInput()
    {
        bool isWPressed = Input.GetKey(KeyCode.W);

        if (isWPressed)
        {
            Accelerate();
        }
        else
        {
            Deaccelerate();
        }
        Rotate();
    }

    private void Deaccelerate()
    {
        audioSource.Stop();
    }

    private void Accelerate()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    private void Rotate()
    {
        bool isAPressed = Input.GetKey(KeyCode.A);
        bool isDPressed = Input.GetKey(KeyCode.D);

        if (isAPressed && isDPressed)
            return;

        float rotationSpeed = rcsThrust * Time.deltaTime;

        rigidBody.freezeRotation = true;

        if (isAPressed)
            transform.Rotate(Vector3.forward * rotationSpeed);

        if (isDPressed)
            transform.Rotate(Vector3.back * rotationSpeed);

        rigidBody.freezeRotation = false;
    }
}
