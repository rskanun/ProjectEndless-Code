using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using System.Linq;

public class ControlContext
{
    private static ControlContext _instance;
    public static ControlContext Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ControlContext();

            return _instance;
        }
    }

    // 현재 등록된 컨트롤러 목록
    private Dictionary<Type, IController> controllers = new Dictionary<Type, IController>();
    private HashSet<IController> activeControllers = new HashSet<IController>();

    private MainInput _keyInput;
    public MainInput KeyInput
    {
        get
        {
            if (_keyInput == null)
                _keyInput = new MainInput();

            return _keyInput;
        }
    }

    private bool _isKeyBlocked;
    public bool IsKeyBlocked
    {
        private set { _isKeyBlocked = value; }
        get { return _isKeyBlocked; }
    }

    public ControlContext()
    {
        KeyInput.Player.Enable();
        KeyInput.UI.Enable();
        KeyInput.Battle.Enable();
    }

    public void RegisterController(IController controller)
    {
        // 컨트롤러 등록
        controllers.Add(controller.GetType(), controller);
    }

    public void RemoveController(IController controller)
    {
        controller.ControlDisconnect();

        // 컨트롤러 삭제
        controllers.Remove(controller.GetType());
    }

    public void EnableController(IController controller)
    {
        // 컨트롤러 등록이 되어있지 않은 경우
        if (!controllers.ContainsValue(controller))
        {
            // 해당 컨트롤러 등록
            RegisterController(controller);
        }

        // 컨트롤러 연결
        EnableController(controller.GetType());
    }

    public void EnableController(Type type)
    {
        // 등록된 컨트롤러가 아니면 무시
        if (!controllers.ContainsKey(type)) return;

        // 모종의 이유로 컨트롤러가 파괴된 경우
        if (controllers[type] == null || controllers[type] as UnityEngine.Object == null)
        {
            // 해당 컨트롤러 삭제
            controllers.Remove(type);
            return;
        }

        // 컨트롤러 활성화
        activeControllers.Add(controllers[type]);
        controllers[type].ControlConnect();
    }

    public void DisableController(IController controller)
    {
        // 컨트롤러 등록이 되어있지 않은 경우
        if (!controllers.ContainsValue(controller))
        {
            // 해당 컨트롤러 등록
            RegisterController(controller);
        }

        // 컨트롤러 해제
        DisableController(controller.GetType());
    }

    public void DisableController(Type type)
    {
        // 등록된 컨트롤러가 아니면 무시
        if (!controllers.ContainsKey(type)) return;

        // 모종의 이유로 컨트롤러가 파괴된 경우
        if (controllers[type] == null || controllers[type] as UnityEngine.Object == null)
        {
            // 해당 컨트롤러 삭제
            controllers.Remove(type);
            return;
        }

        // 컨트롤러 비활성화
        activeControllers.Remove(controllers[type]);
        controllers[type].ControlDisconnect();
    }

    public void SetController(IController enableController)
    {
        // 현재 활성화된 모든 컨트롤러 비활성화
        foreach (IController controller in activeControllers.ToList())
        {
            DisableController(controller);
        }

        // 해당 컨트롤러만 활성화
        EnableController(enableController);
    }

    public void SetController(Type type)
    {
        // 등록된 컨트롤러가 아니면 무시
        if (!controllers.ContainsKey(type)) return;

        // 해당 컨트롤러만 활성화를 한 후 나머진 전부 비활성화
        SetController(controllers[type]);
    }

    public void KeyLock()
    {
        IsKeyBlocked = true;

        // 모든 키 맵 비활성화
        KeyInput.Player.Disable();
        KeyInput.UI.Disable();
        KeyInput.Battle.Disable();
    }

    public void KeyUnlock()
    {
        IsKeyBlocked = false;

        // 모든 키 맵 다시 활성화
        KeyInput.Player.Enable();
        KeyInput.UI.Enable();
        KeyInput.Battle.Enable();
    }
}