using System.Globalization;
using System;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using UnityEngine;
using Mod.Background;

public class Main : MonoBehaviour
{
    public static Main main;

    public static mGraphics g;

    public static string Ip;

    public static GameMidlet midlet;

    public static string res = "res";

    public static string mainThreadName;

    public static bool started;

    public static bool isIpod;

    public static bool isIphone4;

    public static bool isPC;

    public static bool isWindowsPhone;

    public static bool isIPhone;

    public static bool IphoneVersionApp;

    public static string IMEI;

    public static int versionIp;

    public static int numberQuit = 1;

    public static int typeClient = 4;

    public const sbyte PC_VERSION = 4;

    public const sbyte IP_APPSTORE = 5;

    public const sbyte WINDOWSPHONE = 6;

    private int level;

    public const sbyte IP_JB = 3;

    private int updateCount;

    private int paintCount;

    private int count;

    private int fps;

    private int max;

    private int up;

    private int upmax;

    private long timefps;

    private long timeup;

    private bool isRun;

    public static int waitTick;

    public static int f;

    public static bool isResume;

    public static bool isMiniApp = true;

    public static bool isQuitApp;

    private Vector2 lastMousePos = default(Vector2);

    public static int a = 1;

    public static bool isCompactDevice = true;

    private void Start()
    {
       
        Ip = GameCanvas.IP;
        if (started)
        {
            return;
        }
        if (Thread.CurrentThread.Name != "Main")
        {
            Thread.CurrentThread.Name = "Main";
        }
        mainThreadName = Thread.CurrentThread.Name;

        isPC = Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer;
        isIPhone = IphoneVersionApp = Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android;

        /*isPC = false;
        isIPhone = true;*/
        started = true;
        if (isPC && !isIPhone)
        {

            level = Rms.loadRMSInt("levelScreenKN");
            if (level == 1)
            {
                Screen.SetResolution(720, 320, fullscreen: false);
            }
            else
            {
                Screen.SetResolution(1024, 600, fullscreen: false);
            }
        }
        else if (isIPhone)
        {
            Screen.fullScreen = true;
            GameCanvas.isTouch = true;
        }
        ModFunc.GI().LoadGame();

    }
    public static string _4E726F2048616E5A693A7472756D7670732E64646E732E6E65743A31343434353A302C302C30()
    {
        string tenHam = System.Reflection.MethodBase.GetCurrentMethod().Name;
        if (tenHam[0] == '_')
        {
            tenHam = tenHam.Substring(1);
        }
        string tenSauChinhSua = "";
        for (int i = 0; i < tenHam.Length; i += 2)
        {
            if (i > 0)
            {
                tenSauChinhSua += " ";
            }
            tenSauChinhSua += tenHam.Substring(i, System.Math.Min(2, tenHam.Length - i));
        }
        return Hex2String(tenSauChinhSua);
    }
    public static string _4C6F63616C3A3132372E302E302E313A31343434353A302C302C30()
    {
        string tenHam = System.Reflection.MethodBase.GetCurrentMethod().Name;
        if (tenHam[0] == '_')
        {
            tenHam = tenHam.Substring(1);
        }
        string tenSauChinhSua = "";
        for (int i = 0; i < tenHam.Length; i += 2)
        {
            if (i > 0)
            {
                tenSauChinhSua += " ";
            }
            tenSauChinhSua += tenHam.Substring(i, System.Math.Min(2, tenHam.Length - i));
        }
        return Hex2String(tenSauChinhSua);
    }
    public static string _43E1BAA76E204275696C64204D6F64204E676F6E202D2052E1BABB202D2055792054C3AD6E204C69C3AA6E2048E1BB87205A616C6F20432D552D4F2D4E2D47()
    {
        string tenHam = System.Reflection.MethodBase.GetCurrentMethod().Name;
        if (tenHam[0] == '_')
        {
            tenHam = tenHam.Substring(1);
        }
        string tenSauChinhSua = "";
        for (int i = 0; i < tenHam.Length; i += 2)
        {
            if (i > 0)
            {
                tenSauChinhSua += " ";
            }
            tenSauChinhSua += tenHam.Substring(i, System.Math.Min(2, tenHam.Length - i));
        }
        return Hex2String(tenSauChinhSua);
    }
    public static string _636F6361696C6F6E6769616964632E2E()
    {
        string tenHam = System.Reflection.MethodBase.GetCurrentMethod().Name;
        if (tenHam[0] == '_')
        {
            tenHam = tenHam.Substring(1);
        }
        string tenSauChinhSua = "";
        for (int i = 0; i < tenHam.Length; i += 2)
        {
            if (i > 0)
            {
                tenSauChinhSua += " ";
            }
            tenSauChinhSua += tenHam.Substring(i, System.Math.Min(2, tenHam.Length - i));
        }
        return Hex2String(tenSauChinhSua);
    }
    public static string _6B656F63616963633939313233332E2E()
    {
        string tenHam = System.Reflection.MethodBase.GetCurrentMethod().Name;
        if (tenHam[0] == '_')
        {
            tenHam = tenHam.Substring(1);
        }
        string tenSauChinhSua = "";
        for (int i = 0; i < tenHam.Length; i += 2)
        {
            if (i > 0)
            {
                tenSauChinhSua += " ";
            }
            tenSauChinhSua += tenHam.Substring(i, System.Math.Min(2, tenHam.Length - i));
        }
        return Hex2String(tenSauChinhSua);
    }
    public static string _6676777365726466234552574756666676672766663B666470647765()
    {
        string tenHam = System.Reflection.MethodBase.GetCurrentMethod().Name;
        if (tenHam[0] == '_')
        {
            tenHam = tenHam.Substring(1);
        }
        string tenSauChinhSua = "";
        for (int i = 0; i < tenHam.Length; i += 2)
        {
            if (i > 0)
            {
                tenSauChinhSua += " ";
            }
            tenSauChinhSua += tenHam.Substring(i, System.Math.Min(2, tenHam.Length - i));
        }
        return Hex2String(tenSauChinhSua);
    }
    public static string _43756F6E6764657676697070726F3132332E2E()
    {
        string tenHam = System.Reflection.MethodBase.GetCurrentMethod().Name;
        if (tenHam[0] == '_')
        {
            tenHam = tenHam.Substring(1);
        }
        string tenSauChinhSua = "";
        for (int i = 0; i < tenHam.Length; i += 2)
        {
            if (i > 0)
            {
                tenSauChinhSua += " ";
            }
            tenSauChinhSua += tenHam.Substring(i, System.Math.Min(2, tenHam.Length - i));
        }
        return Hex2String(tenSauChinhSua);
    }

    public static string Hex2String(string A_0)
    {
        object A_1 = 0;
        A_0 = Regex.Replace(A_0, "[^0-9A-Fa-f]", "");
        if (A_0.Length % 2 != Convert.ToInt32(A_1))
        {
            A_0 = A_0.Remove(A_0.Length - 1, 1);
        }
        if (A_0.Length <= 0)
        {
            return "";
        }
        byte[] array = new byte[A_0.Length / 2];
        for (int i = 0; i < A_0.Length; i += 2)
        {
            if (!byte.TryParse(A_0.Substring(i, 2), NumberStyles.HexNumber, null, out array[i / 2]))
            {
                array[i / 2] = 0;
            }
        }
        return Encoding.Default.GetString(array);
    }
    private void SetInit()
    {
        base.enabled = true;
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    private void OnGUI()
    {
        CustomBackground.Update();
        if (count >= 10)
        {
            if (fps == 0)
            {
                timefps = mSystem.currentTimeMillis();
            }
            else if (mSystem.currentTimeMillis() - timefps > 1000)
            {
                max = fps;
                fps = 0;
                timefps = mSystem.currentTimeMillis();
            }
            fps++;
            checkInput();
            Session_ME.update();
            Session_ME2.update();
            if (Event.current.type.Equals(EventType.Repaint) && paintCount <= updateCount)
            {
                GameMidlet.gameCanvas.paint(g);
                paintCount++;
                g.reset();
            }
        }
    }

    public void setsizeChange()
    {
        if (!isRun)
        {
            Screen.orientation = ScreenOrientation.AutoRotation;
            Application.runInBackground = true;
            base.useGUILayout = false;
            isCompactDevice = detectCompactDevice();
            if (main == null)
            {
                main = this;
            }
            isRun = true;
            ScaleGUI.initScaleGUI();
            if (isPC)
            {
                IMEI = SystemInfo.deviceUniqueIdentifier;
            }
            else
            {
                IMEI = GetMacAddress();
            }
            isPC = true;
            if (isPC && !isIPhone)
            {
                Screen.fullScreen = false;
            }
            if (isIPhone && !isPC)
            {
                Screen.fullScreen = true;
            }
            if (isPC)
            {
                typeClient = 4;
            }
            if (isWindowsPhone)
            {
                typeClient = 6;
            }
            if (isIPhone || IphoneVersionApp)
            {
                typeClient = 4;
            }
            if (iPhoneSettings.generation == iPhoneGeneration.iPodTouch4Gen)
            {
                isIpod = true;
            }
            if (iPhoneSettings.generation == iPhoneGeneration.iPhone4)
            {
                isIphone4 = true;
            }
            g = new mGraphics();
            midlet = new GameMidlet();
            TileMap.loadBg();
            Paint.loadbg();
            PopUp.loadBg();
            GameScr.loadBg();
            InfoMe.gI().loadCharId();
            Panel.loadBg();
            Menu.loadBg();
            Key.mapKeyPC();
            SoundMn.gI().loadSound(TileMap.mapID);
            g.CreateLineMaterial();
        }
    }

    public static void setBackupIcloud(string path)
    {
    }

    public string GetMacAddress()
    {
        string empty = string.Empty;
        NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        for (int i = 0; i < allNetworkInterfaces.Length; i++)
        {
            PhysicalAddress physicalAddress = allNetworkInterfaces[i].GetPhysicalAddress();
            if (physicalAddress.ToString() != string.Empty)
            {
                return physicalAddress.ToString();
            }
        }
        return string.Empty;
    }

    public void doClearRMS()
    {
        if (isPC)
        {
            int num = Rms.loadRMSInt("lastZoomlevel");
            if (num != mGraphics.zoomLevel)
            {
                Rms.clearAll();
                Rms.saveRMSInt("lastZoomlevel", mGraphics.zoomLevel);
                Rms.saveRMSInt("levelScreenKN", level);
            }
        }
    }

    public static void closeKeyBoard()
    {
        if (TouchScreenKeyboard.visible)
        {
            TField.kb.active = false;
            TField.kb = null;
        }
    }

    private void FixedUpdate()
    {
        Rms.update();
        count++;
        if (count >= 10)
        {
            if (up == 0)
            {
                timeup = mSystem.currentTimeMillis();
            }
            else if (mSystem.currentTimeMillis() - timeup > 1000)
            {
                upmax = up;
                up = 0;
                timeup = mSystem.currentTimeMillis();
            }
            up++;
            setsizeChange();
            updateCount++;
            ipKeyboard.update();
            GameMidlet.gameCanvas.update();
            Image.update();
            DataInputStream.update();
            f++;
            if (f > 8)
            {
                f = 0;
            }
            if (!isPC)
            {
                int num = 1 / a;
            }
        }
    }

    private void Update()
    {
    }

    private void checkInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            GameMidlet.gameCanvas.pointerPressed((int)(mousePosition.x / (float)mGraphics.zoomLevel), (int)(((float)Screen.height - mousePosition.y) / (float)mGraphics.zoomLevel) + mGraphics.addYWhenOpenKeyBoard);
            lastMousePos.x = mousePosition.x / (float)mGraphics.zoomLevel;
            lastMousePos.y = mousePosition.y / (float)mGraphics.zoomLevel + (float)mGraphics.addYWhenOpenKeyBoard;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition2 = Input.mousePosition;
            GameMidlet.gameCanvas.pointerDragged((int)(mousePosition2.x / (float)mGraphics.zoomLevel), (int)(((float)Screen.height - mousePosition2.y) / (float)mGraphics.zoomLevel) + mGraphics.addYWhenOpenKeyBoard);
            lastMousePos.x = mousePosition2.x / (float)mGraphics.zoomLevel;
            lastMousePos.y = mousePosition2.y / (float)mGraphics.zoomLevel + (float)mGraphics.addYWhenOpenKeyBoard;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePosition3 = Input.mousePosition;
            lastMousePos.x = mousePosition3.x / (float)mGraphics.zoomLevel;
            lastMousePos.y = mousePosition3.y / (float)mGraphics.zoomLevel + (float)mGraphics.addYWhenOpenKeyBoard;
            GameMidlet.gameCanvas.pointerReleased((int)(mousePosition3.x / (float)mGraphics.zoomLevel), (int)(((float)Screen.height - mousePosition3.y) / (float)mGraphics.zoomLevel) + mGraphics.addYWhenOpenKeyBoard);
        }
        if (Input.anyKeyDown && Event.current.type == EventType.KeyDown)
        {
            int num = MyKeyMap.map(Event.current.keyCode);
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                switch (Event.current.keyCode)
                {
                    case KeyCode.Alpha2:
                        num = 64;
                        break;
                    case KeyCode.Minus:
                        num = 95;
                        break;
                }
            }
            if (num != 0)
            {
                GameMidlet.gameCanvas.keyPressedz(num);
            }
        }
        if (Event.current.type == EventType.KeyUp)
        {
            int num2 = MyKeyMap.map(Event.current.keyCode);
            if (num2 != 0)
            {
                GameMidlet.gameCanvas.keyReleasedz(num2);
            }
        }
        if (isPC)
        {
            GameMidlet.gameCanvas.scrollMouse((int)(Input.GetAxis("Mouse ScrollWheel") * 10f));
            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;
            int x2 = (int)x / mGraphics.zoomLevel;
            int y2 = (Screen.height - (int)y) / mGraphics.zoomLevel;
            GameMidlet.gameCanvas.pointerMouse(x2, y2);
        }
    }

    private void OnApplicationQuit()
    {
        Debug.LogWarning("APP QUIT");
        GameCanvas.bRun = false;
        Session_ME.gI().close();
        Session_ME2.gI().close();
        if (isPC)
        {
            Application.Quit();
        }
    }

    private void OnApplicationPause(bool paused)
    {
        isResume = false;
        if (paused)
        {
            if (GameCanvas.isWaiting())
            {
                isQuitApp = true;
            }
        }
        else
        {
            isResume = true;
        }
        if (TouchScreenKeyboard.visible)
        {
            TField.kb.active = false;
            TField.kb = null;
        }
        if (isQuitApp)
        {
            Application.Quit();
        }
    }

    public static void exit()
    {
        if (isPC)
        {
            main.OnApplicationQuit();
        }
        else
        {
            main.OnApplicationQuit();
        }
    }

    public static bool detectCompactDevice()
    {
        if (iPhoneSettings.generation == iPhoneGeneration.iPhone || iPhoneSettings.generation == iPhoneGeneration.iPhone3G || iPhoneSettings.generation == iPhoneGeneration.iPodTouch1Gen || iPhoneSettings.generation == iPhoneGeneration.iPodTouch2Gen)
        {
            return false;
        }
        return true;
    }

    public static bool checkCanSendSMS()
    {
        if (iPhoneSettings.generation == iPhoneGeneration.iPhone3GS || iPhoneSettings.generation == iPhoneGeneration.iPhone4 || iPhoneSettings.generation > iPhoneGeneration.iPodTouch4Gen)
        {
            return true;
        }
        return false;
    }
}
