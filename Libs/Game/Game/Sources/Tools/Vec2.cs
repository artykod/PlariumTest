using System;

public struct Vec2
{
	public float x;
	public float y;

	public Vec2(float X, float Y)
	{
		x = X;
		y = Y;
	}

	public float Length
	{
		get
		{
			return (float)Math.Sqrt(x * x + y * y);
		}
	}

	public void Normalize()
	{
		var len = Length;
		x /= len;
		y /= len;
	}

	public Vec2 Normalized
	{
		get
		{
			var v = this;
			v.Normalize();
			return v;
		}
	}

	public void InvertX()
	{
		x = -x;
	}
	public void InvertY()
	{
		y = -y;
	}

	public static Vec2 operator +(Vec2 a, Vec2 b)
	{
		return new Vec2(a.x + b.x, a.y + b.y);
	}

	public static Vec2 operator -(Vec2 a, Vec2 b)
	{
		return new Vec2(a.x - b.x, a.y - b.y);
	}

	public static Vec2 operator *(Vec2 a, float scale)
	{
		return new Vec2(a.x * scale, a.y * scale);
	}

	public static Vec2 operator *(float scale, Vec2 a)
	{
		return a * scale;
	}

	public static Vec2 operator /(Vec2 a, float scale)
	{
		return a * (1f / scale);
	}

	public static float Dot(Vec2 a, Vec2 b)
	{
		return a.x * b.x + a.y * a.y;
	}

	public static float Cross(Vec2 a, Vec2 b)
	{
		return a.x * b.y - a.y * b.x;
	}

	public static float DistanceSqr(Vec2 a, Vec2 b)
	{
		var x = a.x - b.x;
		var y = a.y - b.y;
		return x * x + y * y;
	}

	public static float Distance(Vec2 a, Vec2 b)
	{
		return (a - b).Length;
	}

	public static Vec2 Lerp(Vec2 a, Vec2 b, float t)
	{
		return a + (b - a) * t;
	}

	public static Vec2 FromAngle(float degrees)
	{
		var radians = degrees * Math.PI / 180f;
		return new Vec2((float)Math.Cos(radians), (float)Math.Sin(radians));
	}
}