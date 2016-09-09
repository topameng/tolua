//using UnityEngine;
//
//public class PositionUtil
//{
//    public static Vector3 WorldPos2UIPos(Vector3 pos)
//    {
//        var cam = CameraControl.curCamera;
//        if (nill == cam)
//            return Vector3.zero;
//        Vector3 screenPos = cam.WorldToScreenPoint(pos);
//        var uiPos = ScreenPos2UIPos(screenPos);
//        return uiPos;
//    }
//
//    public static Vector3 WorldPos2ScreenPos(Vector3 pos)
//    {
//        var cam = CameraControl.curCamera;
//        if (nill == cam)
//            return Vector3.zero;
//        Vector3 screenPos = cam.WorldToScreenPoint(pos);
//        return screenPos;
//    }
//
//
//    public static Vector2 ScreenPos2UIPos(Vector2 pos)
//    {
//        float screenWidth = Screen.width;
//        float screenHeight = Screen.height;
//        pos.x = (pos.x / screenWidth) * Define.UIWIDTH;
//        pos.y = (pos.y / screenHeight) * Define.UIHEIGHT;
//        return pos;
//    }
//
//    public static bool CheckInScreen(Vector3 worldPos, float offsetX, float offsetY)
//    {
//        Vector2 uiPos = WorldPos2UIPos(worldPos);
//        if(uiPos.x < -offsetX || uiPos.y < -offsetY
//            || uiPos.x < Define.UIWIDTH + offsetX
//            || uiPos.y < Define.UIHEIGHT + offsetY)
//            return false;
//        return true;
//
//    }
//}