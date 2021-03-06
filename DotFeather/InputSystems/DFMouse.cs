using OpenTK.Input;


namespace DotFeather
{
	/// <summary>
	/// This class gets the mouse cursor position, mouse button status, etc. This class can not be inherited.
	/// </summary>
	public static class DFMouse
	{
		/// <summary>
		/// Get mouse cursor coordinates.
		/// </summary>
		/// <value>The position.</value>
		public static VectorInt Position { get; internal set; }

		/// <summary>
		/// Get or set whether left button pressed.
		/// </summary>
		public static bool IsLeft { get; private set; }

		/// <summary>
		/// Get or set whether right button pressed.
		/// </summary>
		public static bool IsRight { get; private set; }

		/// <summary>
		/// Get or set whether middle button pressed.
		/// </summary>
		public static bool IsMiddle { get; private set; }

		/// <summary>
		/// Get or set whether left button pressed down.
		/// </summary>
		public static bool IsLeftDown { get; private set; }

		/// <summary>
		/// Get or set whether right button pressed down.
		/// </summary>
		public static bool IsRightDown { get; private set; }

		/// <summary>
		/// Get or set whether middle button pressed down.
		/// </summary>
		public static bool IsMiddleDown { get; private set; }

		/// <summary>
		/// Get or set whether left button released up.
		/// </summary>
		public static bool IsLeftUp { get; private set; }

		/// <summary>
		/// Get or set whether right button released up.
		/// </summary>
		public static bool IsRightUp { get; private set; }

		/// <summary>
		/// Get or set whether middle button released up.
		/// </summary>
		public static bool IsMiddleUp { get; private set; }

		/// <summary>
		/// Get mouse wheel scroll amount.
		/// </summary>
		/// <value></value>
		public static Vector Scroll { get; private set; }

		internal static void Update()
		{
			// previous values
			bool pl = IsLeft, pr = IsRight, pm = IsMiddle;

			IsLeft = Mouse.GetState().LeftButton == ButtonState.Pressed;
			IsRight = Mouse.GetState().RightButton == ButtonState.Pressed;
			IsMiddle = Mouse.GetState().MiddleButton == ButtonState.Pressed;

			IsLeftDown = IsLeft && !pl;
			IsRightDown = IsRight && !pr;
			IsMiddleDown = IsMiddle && !pm;

			IsLeftUp = !IsLeft && pl;
			IsRightUp = !IsRight && pr;
			IsMiddleUp = !IsMiddle && pm;

			var scr = Mouse.GetCursorState().Scroll;
			var scroll = new Vector(scr.X, scr.Y);

			Scroll = scroll - prevScroll;
			prevScroll = scroll;
		}

		private static Vector prevScroll;
	}
}
