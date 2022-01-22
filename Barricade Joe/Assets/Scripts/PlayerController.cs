using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Joystick m_joystickController;

    public float moveSpeed = 3;
    public GameObject bullet;
    public Transform spawnPoint;
    public float delay = 1;
    public GameObject Light;
    [Header("Audio")]
    [SerializeField] Audio ShotSound;
    [SerializeField] Audio GameLost;

    [Space(10)]
    [Header("UI")]
     GameObject YouLosePanel;
    float x = 0;
    float z = 0;
    Vector3 lookDirection = Vector3.zero;
    Animator anim;
    float _time;
    bool _canShoot = false;
    bool FirstTime = true;
    public static bool isEnded = false;
   
    private void Start()
    {

        
        m_joystickController = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();
        anim = GetComponent<Animator>();
        _time = delay;
        m_joystickController._OnPointerUp += ShootEnd;
        m_joystickController._OnPointerDown += () => _canShoot = true;
        m_joystickController._OnPointerDown += StartGame;
        //YouLosePanel = GameObject.FindGameObjectWithTag("LosePanel");
    }
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
       // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void StartGame()
    {
        if (FirstTime)
        {
            Time.timeScale = 1;
            FirstTime = false;
            GameManager.instance.StartGame();
            m_joystickController._OnPointerDown -= StartGame;
        }

    }

    void Update()
    {
        x = m_joystickController.Horizontal;
        z = m_joystickController.Vertical;
        lookDirection = new Vector3(x, 0, z);
        _time -= Time.deltaTime;
        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }

        if (_canShoot)
            Shoot();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            isEnded = true;
            GameOver();
           
        }
    }

    public void GameOver()
    {
       
        Timer.Register(.5f, () => AudioManager.instance.PlayAudio(GameLost));
        GameManager.instance.GameEnd();
        gameObject.SetActive(false);
    }
    private void Shoot()
    {
        anim.SetBool("Shooting", true);
        if (_time <= 0)
        {
            AudioManager.instance.PlayAudio(ShotSound.clip, ShotSound.volume);
            ObjectPooler.instance.SpawnFromPool("Bullet", spawnPoint.position, transform.rotation);
            //Instantiate(bullet, spawnPoint.position, transform.rotation);
            Light.SetActive(true);
            _time = delay;
        }
    }


    void ShootEnd()
    {
        _canShoot = false;
        anim.SetBool("Shooting", false);
    }
}
