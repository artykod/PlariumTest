public abstract class Singleton<TypeBase, TypeImpl> where TypeBase : class where TypeImpl : TypeBase, new()
{
	private static TypeBase instance;

	public static TypeBase Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new TypeImpl();
			}
			return instance;
		}
	}

	public virtual string SingletonName
	{
		get
		{
			return GetType().ToString();
		}
	}

	public void Create()
	{
	}
}
