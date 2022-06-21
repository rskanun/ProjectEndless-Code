/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class charactor : MonoBehaviour
{
    objectManager objData;
    option keySet = new option();

    TextManager TextManager;

    bool noKeyDown = false;
    bool isTalking = false;
    bool isDash = false;
    bool npcInArea = false;

    string key_dash;
    string key_interact;
    string key_attack;

    public float speed;
    public float highSpeed;
    public float dash_distance;
    public float dash_speed;
    public float stop_distance;

    int axisH = 0;
    int axisV = 0;

    public GameObject subRigid;

    public Text textLine;
    public GameObject textDialogue;

    Vector2 moveVec;

    Rigidbody2D rigid;

    Animator animator;

    Transform mainEntity;
    Transform subEntity;

    Vector2 loc_dash = new Vector2();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

        textLine.gameObject.SetActive(false);
        textDialogue.gameObject.SetActive(false);

        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

        init();
    }
    
    private void init()
    {
        key_dash = keySet.getKey(Key.dash);
        key_interact = keySet.getKey(Key.interact);
        key_attack = keySet.getKey(Key.attack);

        mainEntity = this.gameObject.transform;
        subEntity = subRigid.transform;

        TextManager.init(textLine, textDialogue);

        moveVec = Vector2.zero;
    }

    void Update()
    {
        if(!noKeyDown)
            moveKeyPress();

        if (isTalking)
            textKeyPress();
    }

    // 키 입력
    private void moveKeyPress()
    {
        // 마우스 우클릭 대쉬
        if (Input.GetKeyDown(key_dash))
        {
            dash();
        }

        // 방향키
        if (Input.GetKey("up") || Input.GetKey("down") ||
            Input.GetKey("left") || Input.GetKey("right"))
        {
            // 화살표 방향키 입력방지
        }

        else if(!noKeyDown)
        {
            moveVec.x = Input.GetAxisRaw("Horizontal");
            moveVec.y = Input.GetAxisRaw("Vertical");

            axisH = Mathf.CeilToInt(moveVec.x);
            axisV = Mathf.CeilToInt(moveVec.y);
        }

        if(Input.GetKeyDown(key_interact) && npcInArea)
        {
            noKeyDown = true;
            interact();
        }


        // Animation
        animator.SetInteger("axisH", axisH);
        animator.SetInteger("axisV", axisV);
    }

    private void textKeyPress()
    {
        // 대화
        if (Input.GetKeyDown(key_attack))
        {
            interact();
        }
    }

    private void dash()
    {
        noKeyDown = true;
        isDash = true;

        subEntity.position = mainEntity.position;

        loc_dash = Vector2.zero;

        Vector2 loc_click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 loc_chr = mainEntity.position;

        float angle = Mathf.Atan2(loc_click.y - loc_chr.y, loc_click.x - loc_chr.x)
            * Mathf.Rad2Deg;

        loc_dash.x = Mathf.Cos(angle * Mathf.Deg2Rad) * dash_distance;
        loc_dash.y = Mathf.Sin(angle * Mathf.Deg2Rad) * dash_distance;

        int tmpX = (int)loc_dash.x;
        int tmpY = (int)loc_dash.y;

        axisH = (-10 <= tmpX && tmpX <= 10) ? 0 : tmpX;
        axisV = (-10 <= tmpY && tmpY <= 10) ? 0 : tmpY;

        loc_dash.x += loc_chr.x;
        loc_dash.y += loc_chr.y;
    }

    private void interact()
    {
        isTalking = TextManager.talk(objData);
    }

    private void FixedUpdate()
    {
        if (isDash || isTalking)
        {
            moveVec = Vector2.zero;
        }

        if (isDash)
        {
            rigid.MovePosition(Vector2.Lerp(mainEntity.position, loc_dash, dash_speed));
            subEntity.position = Vector2.Lerp(subEntity.position, loc_dash, dash_speed);

            if (Vector2.Distance(loc_dash, subEntity.position) <= stop_distance)
            {
                Debug.Log("ready");
                isDash = false;
                noKeyDown = false;
            }
        }


        rigid.velocity = moveVec.normalized * speed;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            objData = null;

            npcInArea = false;
            Debug.Log("exit");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            objData = collision.gameObject.GetComponent<objectManager>();

            npcInArea = true;
            Debug.Log("keydown " + key_interact);
        }
    }
}
*/