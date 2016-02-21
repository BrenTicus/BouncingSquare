using UnityEngine;
using UnityEngine.SceneManagement;

public class DudeScript : MonoBehaviour {
	public GameObject floor;
	public GameObject floors;
	public GameObject boom;
	public Sprite happy;
	public Sprite neutral;
	public Sprite sad;
	public UnityEngine.UI.Text score;
	public int maxJumps;
	public int maxCount;
	public float force;
	public float baseSpeed;
	public float boostMult;
	public float boostSpin;
	public float speedIncrease;

	private GameObject gui;
	private Animator anim;
	private Rigidbody2D rigid;
	private SpriteRenderer spriteRender;
	private int jumps;
	private int startSpawn = 0;
	private float speed;
	private bool release;
	private float lastSpot;
	private float count;
	private bool boost = false;
	private bool isQuitting;

	void Start()
	{
		speed = baseSpeed;
		anim = gameObject.GetComponent<Animator>();
		rigid = gameObject.GetComponent<Rigidbody2D>();
		spriteRender = gameObject.GetComponent<SpriteRenderer>();
		gui = GameObject.Find("GameOver");
		gui.SetActive(false);
		lastSpot = transform.position.x;
		count = maxCount;
		jumps = maxJumps;
		if (!PlayerPrefs.HasKey("highscore")) PlayerPrefs.SetInt("highscore", 0);
		if (!score) score = GameObject.Find("Score").GetComponent<UnityEngine.UI.Text>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		handleInput();
		determineSpeed();
		spawnFloors();
		checkDeath();
		checkOrientation();

		//Update score
		score.text = Mathf.RoundToInt(transform.position.x).ToString();

		//Make sure velocity is set
		rigid.velocity = new Vector2(speed, rigid.velocity.y);
	}

	void handleInput()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) { SceneManager.LoadScene(0); }
#if UNITY_WP8 || UNITY_ANDROID || UNITY_IOS
		if (Input.touchCount == 0) { }
		else if (Input.GetTouch(0).deltaPosition.y / Time.deltaTime > sensitivity && jumps > 0 && release)
		{
			jumps--;
			release = false;
			anim.SetTrigger("Jumping");
			rigid.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
		}
		else if (Input.GetTouch(0).deltaPosition.x / Time.deltaTime > sensitivity && release && !boost)
		{
			boost = true;
			release = false;
			rigid.angularVelocity = boostSpin;
		}
		else if (Input.GetTouch(0).deltaPosition.y / Time.deltaTime < -sensitivity && jumps > 0 && release)
		{
			jumps = 0;
			release = false;
			rigid.AddForce(new Vector2(0, -force * 3), ForceMode2D.Impulse);
		}
		else if (Input.GetTouch(0).deltaPosition.y == 0) release = true;
#else
		if (Input.GetAxis("Vertical") > 0 && jumps > 0 && release)
		{
			jumps--;
			release = false;
			anim.SetTrigger("Jumping");
			rigid.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
		}
		else if (Input.GetAxis("Vertical") < 0 && jumps > 0 && release)
		{
			jumps = 0;
			release = false;
			rigid.AddForce(new Vector2(0, -force * 3), ForceMode2D.Impulse);
		}
		else if (Input.GetAxis("Horizontal") > 0 && release && !boost)
		{
			boost = true;
			release = false;
			rigid.angularVelocity = boostSpin;
		}
		else if (Input.GetAxis("Vertical") == 0) release = true;
#endif
	}

	void setFloors(GameObject floors)
	{
		this.floors = floors;
	}

	void spawnFloors()
	{
		//Spawn new floor pieces as you go.
		if (startSpawn - transform.position.x < 0.1)
		{
			startSpawn++;
			GameObject temp;
			temp = (GameObject)Instantiate(floor, new Vector3(Mathf.Round(startSpawn + 12), Random.Range(-4, 1)), Quaternion.identity);
			//temp = (GameObject)Instantiate(floor, new Vector3(Mathf.Round(startSpawn + 12), -4), Quaternion.identity);
			temp.transform.parent = floors.transform;
			temp = (GameObject)Instantiate(floor, new Vector3(Mathf.Round(startSpawn + 12), 5), Quaternion.identity);
			temp.transform.parent = floors.transform;
		}
	}

	void determineSpeed()
	{
		speed = baseSpeed;
		if (transform.position.x > speedIncrease)
			speed += ((transform.position.x - speedIncrease) % speedIncrease) / speedIncrease;
		if (boost)
			speed *= boostMult;
	}

	void checkDeath()
	{
		//If you fall, die.
		if (transform.position.y < -4.5)
		{
			Destroy(gameObject);
		}

		//If you're stuck, die.
		if (transform.position.x - lastSpot < 0.1)
		{
			count--;
			if (count <= 0) Destroy(gameObject);
		}
		else
		{
			count = maxCount;
			lastSpot = transform.position.x;
		}
	}

	void checkOrientation()
	{
		if (transform.rotation.eulerAngles.z < 45.0f || transform.rotation.eulerAngles.z > 315.0f)
			spriteRender.sprite = happy;
		else if (transform.rotation.eulerAngles.z > 125.0f && transform.rotation.eulerAngles.z < 225.0f)
			spriteRender.sprite = sad;
		else
			spriteRender.sprite = neutral;
	}

	void doneJump()
	{
		anim.SetTrigger("doneJump");
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		jumps = maxJumps;
		boost = false;
	}

	void OnDestroy()
	{
		if (!isQuitting)
		{
			Instantiate(boom, transform.position, Quaternion.identity);

			int score = PlayerPrefs.GetInt("highscore");
			if(transform.position.x > score)
			{
				PlayerPrefs.SetInt("highscore", Mathf.RoundToInt(transform.position.x));
			}
			gui.SetActive(true);
		}
	}

	void OnApplicationQuit()
	{
		isQuitting = true;
	}
}
