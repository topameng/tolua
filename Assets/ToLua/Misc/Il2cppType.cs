using System;
using UnityEngine;
using LuaInterface;

public class Il2cppType
{
	public Type TypeOfFloat = typeof(float);
	public Type TypeOfInt = typeof(int);
	public Type TypeOfUInt = typeof(uint);
	public Type TypeOfDouble = typeof(double);
	public Type TypeOfBool = typeof(bool);
	public Type TypeOfLong = typeof(long);
	public Type TypeOfULong = typeof(ulong);
	public Type TypeOfSByte = typeof(sbyte);
	public Type TypeOfByte = typeof(byte);
	public Type TypeOfShort = typeof(short);
	public Type TypeOfUShort = typeof(ushort);
	public Type TypeOfChar = typeof(char);
    public Type TypeOfDecimal = typeof(decimal);
    public Type TypeOfIntPtr = typeof(IntPtr);
    public Type TypeOfUIntPtr = typeof(UIntPtr);    

    public Type TypeOfVector3 = typeof(Vector3);
    public Type TypeOfQuaternion = typeof(Quaternion);
    public Type TypeOfVector2 = typeof(Vector2);
    public Type TypeOfVector4 = typeof(Vector4);
    public Type TypeOfColor = typeof(Color);
    public Type TypeOfBounds = typeof(Bounds);
    public Type TypeOfRay = typeof(Ray);
    public Type TypeOfTouch = typeof(Touch);
    public Type TypeOfLayerMask = typeof(LayerMask);
    public Type TypeOfRaycastHit = typeof(RaycastHit);

    public Type TypeOfLuaTable = typeof(LuaTable);
    public Type TypeOfLuaThread = typeof(LuaThread);
    public Type TypeOfLuaFunction = typeof(LuaFunction);
    public Type TypeOfLuaBaseRef = typeof(LuaBaseRef);

    public Type MonoType = typeof(Type).GetType();
    public Type TypeOfObject = typeof(object);
    public Type TypeOfType = typeof(Type);
    public Type TypeOfString = typeof(string);
    public Type UObjectType = typeof(UnityEngine.Object);
    public Type TypeOfStruct = typeof(ValueType);
    public Type TypeOfLuaByteBuffer = typeof(LuaByteBuffer);
    public Type TypeOfEventObject = typeof(EventObject);
    public Type TypeOfNullObject = typeof(NullObject);

    public Type TypeOfArray = typeof(Array);
    public Type TypeOfByteArray = typeof(byte[]);
    public Type TypeOfBoolArray = typeof(bool[]);
    public Type TypeOfCharArray = typeof(char[]);
    public Type TypeOfStringArray = typeof(string[]);
    public Type TypeOfObjectArray = typeof(object[]);

    public Type TypeofGenericNullObject = typeof(Nullable<>);

    public Type TypeOfVoid = typeof(void);
}
