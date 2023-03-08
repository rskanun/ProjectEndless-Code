using UnityEngine;

namespace Assets.Script.Control
{
    public abstract class InterfaceControl : MonoBehaviour
    {
        private const float MOVE_DELAY = 0.0975f; // 다음 연속해서 움직이기까지 걸리는 시간
        private const float DELAY_TIME = 0.35f; // 연속해서 움직이기까지 걸리는 시간

        private bool isPushX = false;
        private bool isPushY = false;
        private float accumTime = 0; // 키를 누르고 있는 시간 측정

        // 선택 가능한 아이콘의 최대 좌표(x, y)
        // ex) 2 x 2 -> (1, 1)
        private Vector2 iconPoint = Vector2.zero;

        // 현재 선택한 것에 대한 좌표
        private Vector2 selectPoint = Vector2.zero;

        // 해당 인터페이스의 윈도우
        public GameObject interfaceWindow;

        protected OptionSetting option;

        /************************************************************
        * [Child Method]
        * 
        * 자식 클래스에서 쓰일 메소드
        ************************************************************/

        public void setIconPoint(int x, int y)
        {
            iconPoint.x = x - 1;
            iconPoint.y = y - 1;
        }

        public void valueReset()
        {
            selectPoint = Vector2.zero;
            accumTime = 0;
        }

        public void setSelectPoint(int x, int y)
        {
            selectPoint.x = x;
            selectPoint.y = y;
        }

        /************************************************************
        * [Key Press]
        * 
        * 특정 키를 눌렀을 때에 대한 이벤트
        ************************************************************/

        protected void interfaceKeyPress()
        {
            //cursorMoveKeyPress();
            //selectKeyPress();
            cancelKeyPress();
        }
        private void cursorMoveKeyPress()
        {
            float v = Input.GetAxisRaw("Vertical");
            float h = Input.GetAxisRaw("Horizontal");

            // 일정시간동안 누른 키에 대해 한 번만 인식
            if (pushX()) setCursor(0, h);
            if (pushY()) setCursor(v, 0);

            if (v != 0 || h != 0)
            {
                moveCursor(v, h);
            }

            else if (accumTime != 0)
            {
                // 키를 모두 땠다면 초기화
                accumTime = 0;
            }
        }

        private void selectKeyPress()
        {
            if(Input.GetKeyDown(option.Interact))
                iconSelect((int)selectPoint.x, (int)selectPoint.y);
        }

        private void cancelKeyPress()
        {
            if (Input.GetKeyDown(option.Cancel) && interfaceWindow.activeSelf == true)
            {
                cancel();
            }
        }

        protected abstract void iconSelect(int x, int y);
        public virtual void cancel()
        {
            interfaceWindow.SetActive(false);
        }
        /************************************************************
        * [커서 이동 제어]
        * 
        * 커서 이동을 제어
        ************************************************************/

        private bool pushX()
        {
            if (!isPushX && Input.GetAxisRaw("Horizontal") != 0)
            {
                isPushX = true;
                return true;
            }
            else if (isPushX && Input.GetAxisRaw("Horizontal") == 0)
            {
                isPushX = false;
                return false;
            }

            return false;
        }

        private bool pushY()
        {
            if (!isPushY && Input.GetAxisRaw("Vertical") != 0)
            {
                isPushY = true;
                return true;
            }
            else if (isPushY && Input.GetAxisRaw("Vertical") == 0)
            {
                isPushY = false;
                return false;
            }

            return false;
        }

        private void moveCursor(float v, float h)
        {
            // 누르고 있는 시간을 측정
            accumTime += Time.deltaTime;

            // 일정시간 누르고 있으면 해당 방향으로 연속해서 이동
            if (accumTime >= DELAY_TIME + MOVE_DELAY)
            {
                setCursor(v, h);
                accumTime = DELAY_TIME;
            }
        }

        private void setCursor(float v, float h)
        {
            if (v > 0) selectPoint.y += 1;
            else if (v < 0) selectPoint.y -= 1;
            else if (h > 0) selectPoint.x += 1;
            else if (h < 0) selectPoint.x -= 1;

            // 현재 위치값이 아이콘 범위를 벗어나지 않게 보정
            vecCorrect();
        }

        private void vecCorrect()
        {
            // x값 보정
            if (selectPoint.x > iconPoint.x) selectPoint.x = 0;
            else if (selectPoint.x < 0) selectPoint.x = iconPoint.x;

            // y값 보정
            if (selectPoint.y > iconPoint.y) selectPoint.y = 0;
            else if (selectPoint.y < 0) selectPoint.y = iconPoint.y;
        }
    }
}