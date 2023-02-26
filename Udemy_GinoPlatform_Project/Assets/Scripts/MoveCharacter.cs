using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveCharacter : MonoBehaviour
{
    Rigidbody2D character;
    public float vel;
    public float jumpVel;
    bool jump;
    int stars;
    int lifes;
    private bool alive;
    [SerializeField] private float JumpForce;
    public Text lifesTxt;
    public Text starsTxt;
    public Button start;
    public Button restart;
    public Text gameoverTxt;
    public Text winTxt;
    public Animator heroAnim;
    public GameObject door;
    public AudioSource gameAudio;
    public AudioSource gameAudioMusic;
    public AudioClip[] soundsSFX;


    void Start()
    {
        character = gameObject.GetComponent<Rigidbody2D>();
        stars = FindObjectOfType<GameManager>().getStar();
        lifes = FindObjectOfType<GameManager>().getLife();

        lifesTxt.text = lifes.ToString();
        starsTxt.text = stars.ToString();

        restart.gameObject.SetActive(false);
        gameoverTxt.enabled = false;
        winTxt.enabled = false;

        if (!FindObjectOfType<GameManager>().getGameRun())
        {
            Time.timeScale = 0;
            alive = false;

            start.gameObject.SetActive(true);

            heroAnim.SetBool("Jump", false);
            heroAnim.SetBool("Walk", false);
            heroAnim.SetBool("Run", false);
            heroAnim.SetBool("JumpFinal", false);
            gameAudioMusic.Stop();
        }
        else
        {
            Starting();
        }
    }


    void Update()
    {
        if (alive)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                character.transform.Translate(vel * Time.deltaTime, 0, 0);
                if (this.gameObject.transform.localScale.x < 0)
                {
                    this.gameObject.transform.localScale = new Vector3((this.gameObject.transform.localScale.x * -1), this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
                }

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    vel = 20;
                    heroAnim.SetBool("Walk", true);
                    heroAnim.SetBool("Run", false);
                }
                else
                {
                    vel = 30;
                    heroAnim.SetBool("Walk", false);
                    heroAnim.SetBool("Run", true);
                }
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                character.transform.Translate(-vel * Time.deltaTime, 0, 0);
                if (this.gameObject.transform.localScale.x > 0)
                {
                    this.gameObject.transform.localScale = new Vector3((this.gameObject.transform.localScale.x * -1), this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
                }

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    vel = 20;
                    heroAnim.SetBool("Walk", true);
                    heroAnim.SetBool("Run", false);
                }
                else
                {
                    vel = 30;
                    heroAnim.SetBool("Walk", false);
                    heroAnim.SetBool("Run", true);
                }
            }

            if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                heroAnim.SetBool("Walk", false);
                heroAnim.SetBool("Run", false);
            }

            if (Input.GetKeyDown(KeyCode.Space) && jump)
            {
                character.AddForce(Vector2.up * jumpVel, ForceMode2D.Impulse);
                heroAnim.SetBool("Jump", true);
                heroAnim.SetBool("Walk", false);
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            jump = true;
            heroAnim.SetBool("Jump", false);
        }

        if (other.gameObject.CompareTag("venom"))
        {
            gameAudio.PlayOneShot(soundsSFX[3]);
            lifes = 0;
            gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 0, 0);
            lifesTxt.text = lifes.ToString();
            GameOver();
        }

        if (other.gameObject.CompareTag("spike") || other.gameObject.CompareTag("blade"))
        {
            gameAudio.PlayOneShot(soundsSFX[3]);
            lifes -= 1;
            gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 0, 0);
            lifesTxt.text = lifes.ToString();
            if (lifes <= 0)
            {
                GameOver();
            }
        }

        if (other.gameObject.CompareTag("rex"))
        {
            gameAudio.PlayOneShot(soundsSFX[3]);
            lifes -= 1;
            gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 0, 0);
            lifesTxt.text = lifes.ToString();
            if (lifes <= 0)
            {
                GameOver();
            }
        }

        if (other.gameObject.CompareTag("button"))
        {
            gameAudio.PlayOneShot(soundsSFX[0]);
            door.transform.eulerAngles = Vector3.Lerp(door.transform.eulerAngles, new Vector3(0, 90, 0), 0.7f);
            door.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }

        if (other.gameObject.CompareTag("flag"))
        {
            FindObjectOfType<GameManager>().setStartLife(stars, lifes);
            SceneManager.LoadScene(1);
        }

        if (other.gameObject.CompareTag("flagFinal"))
        {
            Win();
        }

    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            jump = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("star"))
        {
            stars += 1;
            gameAudio.PlayOneShot(soundsSFX[1]);
            starsTxt.text = stars.ToString();
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("heart"))
        {
            lifes += 1;
            gameAudio.PlayOneShot(soundsSFX[2]);
            lifesTxt.text = lifes.ToString();
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("rex"))
        {
            character.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            other.gameObject.GetComponent<RexController>().Die();
            gameAudio.PlayOneShot(soundsSFX[6]);
        }

    }

    public void Starting()
    {
        heroAnim.SetBool("JumpFinal", false);
        start.gameObject.SetActive(false);
        alive = true;
        Time.timeScale = 1;
        gameAudioMusic.Play();
        FindObjectOfType<GameManager>().switchGameRun(true);
    }

    public void Restart()
    {
        FindObjectOfType<GameManager>().setStartLife(0, 3);
        FindObjectOfType<GameManager>().switchGameRun(false);
        heroAnim.SetBool("JumpFinal", false);
        restart.gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }

    private void GameOver()
    {
        heroAnim.SetBool("JumpFinal", false);
        alive = false;
        gameAudioMusic.Stop();
        gameAudio.PlayOneShot(soundsSFX[4]);
        gameoverTxt.enabled = true;
        restart.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    private void Win()
    {
        alive = false;
        gameAudioMusic.Stop();
        gameAudio.PlayOneShot(soundsSFX[5]);
        winTxt.enabled = true;
        heroAnim.SetBool("JumpFinal", true);
        restart.gameObject.SetActive(true);
    }
}
