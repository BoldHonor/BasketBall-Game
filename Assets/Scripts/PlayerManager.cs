using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;


public class PlayerManager : MonoBehaviour
{
    private static int TopScore = 0;
    public static PlayerManager Instance;
    public int score = 0;
    [SerializeField] private Text ScoreCard;
    [SerializeField] private Text TimeCard;
    [SerializeField] private Text TopScoreCard;
    [SerializeField] GameObject basketBallPrefab;

    BasketBall currentBasketBall;



    private Vector3 maxVelocity = new Vector3(3f, 16f, 3.4f);
    private Vector3 minVelocity = new Vector3(0f, 8f, 2.6f);

    private Vector2 _fingerDown;
    private Vector2 _fingerUp;

    private float swipeTime = 1;
    private Vector2 _screenDim;

    private float TimeElapsed = 0;
    private float TimePerLevel = 30;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }



    void Start()
    {
        Instance.ScoreCard.text = Instance.score.ToString();
        _screenDim = new Vector2(Screen.width, Screen.height);
        spawnBall();
    }

    public void UpdateScore()
    {
        Instance.ScoreCard.text = Instance.score.ToString();
    }

    private void Update()
    {
        TimeElapsed += Time.deltaTime;
        if (TimeElapsed > TimePerLevel)
        {
            StartCoroutine(ShowScore());
        }
        TimeCard.text = "Time :" + (Mathf.Ceil(TimePerLevel - TimeElapsed)).ToString();

        if (Input.GetMouseButtonDown(0))
        {
            _fingerDown = (Input.mousePosition) / _screenDim;
            _fingerUp = Input.mousePosition / _screenDim;
            swipeTime = Time.timeSinceLevelLoad;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _fingerUp = Input.mousePosition / _screenDim;
            Debug.Log("Start " + _fingerDown);
            Debug.Log("End " + _fingerUp);
            var throwVal = _fingerUp - _fingerDown;

            throwBall(throwVal.x, throwVal.y, 1 / (1 + 2 * (Time.timeSinceLevelLoad - swipeTime)));

        }

    }
    public void spawnBall()
    {
        var go = Instantiate(basketBallPrefab);
        go.transform.position = Vector3.zero;
        go.transform.rotation = Quaternion.Euler(Vector3.zero);
        currentBasketBall = go.GetComponent<BasketBall>();
    }

    void throwBall(float x, float y, float power)
    {
        if (currentBasketBall.launched) return;
        currentBasketBall.velocity.x = (maxVelocity.x - minVelocity.x) * x + minVelocity.x;
        currentBasketBall.velocity.y = (maxVelocity.y - minVelocity.y) * y + minVelocity.y;
        currentBasketBall.velocity.z = (maxVelocity.z - minVelocity.z) * power + minVelocity.z;
        currentBasketBall.launched = true;
    }

    IEnumerator ShowScore()
    {
        TopScoreCard.gameObject.SetActive(true);
        TopScore = score > TopScore ? score : TopScore;
        TopScoreCard.text = "TOP SCORE : " + TopScore.ToString();
        yield return new WaitForSeconds(3);
        TopScoreCard.gameObject.SetActive(false);
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }
}
