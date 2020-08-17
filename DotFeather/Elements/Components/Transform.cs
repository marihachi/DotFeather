namespace DotFeather
{
	public class Transform : Component
	{
		public Vector Location { get; set; }
		public Vector Scale { get; set; }

		public Vector GlobalLocation { get; private set; }
		public Vector GlobalScale { get; private set; }

		public override void OnPreRender()
		{
			// Compute global location and scale
			GlobalLocation = Location;
			GlobalScale = Scale;

			var p = Element?.Parent?.Transform;
			if (p == null) return;
			GlobalLocation += p.GlobalLocation;
			GlobalScale *= p.GlobalScale;
		}
	}
}
