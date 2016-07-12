namespace Game.Logics
{
	using Descriptors;

	public abstract class Logic : BaseController.IUpdatable
	{
		public event System.Action<Logic> OnDestroy;

		public Descriptor Descriptor
		{
			get;
			private set;
		}

		public GameController GameController
		{
			get;
			private set;
		}

		public Logic(GameController gameController, Descriptor descriptor)
		{
			GameController = gameController;
			Descriptor = descriptor;
		}

		public virtual void Start()
		{
			GameController.AddUpdatable(this);
		}

		public virtual void Destroy()
		{
			GameController.RemoveUpdatable(this);
			OnDestroy.SafeInvoke(this);
			OnDestroy = null;
		}

		protected virtual void Update(float dt)
		{
		}

		void BaseController.IUpdatable.Update(float dt)
		{
			Update(dt);
		}
	}
}
