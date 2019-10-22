using System;
using System.Drawing.Imaging;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections;
using System.IO;

namespace DotFeather
{
	/// <summary>
	/// DotFeather のメインループおよび、各種メソッドを揃えている、ゲームエントリーポイントの基底クラスです。
	/// </summary>
	public abstract class GameBase : IDisposable
	{
		/// <summary>
		/// ウィンドウの X 座標を取得または設定します。
		/// </summary>
		public int X
		{
			get => window.X;
			set => window.X = value;
		}

		/// <summary>
		/// ウィンドウの Y 座標を取得または設定します。
		/// </summary>
		public int Y
		{
			get => window.Y;
			set => window.Y = value;
		}

		/// <summary>
		/// ウィンドウが表示されているかどうかを示す値を取得または設定します。
		/// </summary>
		public bool Visible
		{
			get => window.Visible;
			set => window.Visible = value;
		}

		/// <summary>
		/// このウィンドウの幅を取得または設定します。
		/// </summary>
		/// <value>The width.</value>
		public int Width
		{
			get => window.ClientSize.Width;
			set => window.ClientSize = new Size(value, window.Size.Height);
		}

		/// <summary>
		/// このウィンドウの高さを取得または設定します。
		/// </summary>
		/// <value>The height.</value>
		public int Height
		{
			get => window.ClientSize.Height;
			set => window.ClientSize = new Size(window.Size.Width, value);
		}

		/// <summary>
		/// このウィンドウが現在フォーカスされているかどうかを取得します。
		/// </summary>
		/// <value>The height.</value>
		public bool IsFocused => window.Focused;

		/// <summary>
		/// ウィンドウの背景色を取得または設定します。
		/// </summary>
		public Color BackgroundColor { get; set; }

		/// <summary>
		/// このウィンドウのリフレッシュレートを取得または設定します。
		/// </summary>
		/// <value>The refresh rate.</value>
		public int RefreshRate { get; }

		/// <summary>
		/// このウィンドウのタイトルを取得または設定します。
		/// </summary>
		/// <value>ウィンドウタイトル。</value>
		public string Title
		{
			get => window.Title;
			set => window.Title = value;
		}

		/// <summary>
		/// このウィンドウのトップレベル <see cref="Container"/> を取得または設定します。
		/// </summary>
		public Container Root { get; } = new Container();

		/// <summary>
		/// 現在のディスプレイの DPI を取得します。
		/// </summary>
		public float Dpi { get; private set; }

		/// <summary>
		/// 起動後からのトータルフレーム数を取得します。
		/// </summary>
		/// <value></value>
		public long TotalFrame { get; private set; }

		/// <summary>
		/// ゲームがキャプチャモードであるかどうかを取得します。
		/// </summary>
		public bool IsCaptureMode { get; private set; }

		/// <summary>
		/// 現在のウィンドウ状態を取得または設定します。
		/// </summary>
		public WindowMode WindowMode
		{
			get
			{
				return  window.WindowBorder == WindowBorder.Resizable ? WindowMode.Resizable :
						window.WindowBorder == WindowBorder.Fixed ? WindowMode.Fixed :
						window.WindowBorder == WindowBorder.Hidden ? WindowMode.NoFrame :
						throw new InvalidOperationException("unexpected window state");
			}
			set
			{
				switch (value)
				{
					case WindowMode.Fixed:
						window.WindowBorder = WindowBorder.Fixed;
						break;
					case WindowMode.NoFrame:
						window.WindowBorder = WindowBorder.Hidden;
						break;
					case WindowMode.Resizable:
						window.WindowBorder = WindowBorder.Resizable;
						break;
				}
			}
		}

		/// <summary>
		/// ゲームがフルスクリーンであるかどうかを取得または設定します。
		/// </summary>
		public bool IsFullScreen
		{
			get => window.WindowState == WindowState.Fullscreen;
			set => window.WindowState = value ? WindowState.Fullscreen : WindowState.Normal;
		}

		/// <summary>
		/// 指定したパラメーターで、 <see cref="GameBase"/> クラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="width">幅.</param>
		/// <param name="height">高さ.</param>
		/// <param name="title">タイトル.</param>
		/// <param name="refreshRate">リフレッシュレート.</param>
		/// <param name="isCaptureMode"><see langword="true"/> にするとキャプチャーモードになります。キャプチャーモードにした場合、カレントディレクトリにcapturedフォルダが生成され、自動的に全フレームの連番画像が生成されます。非常に動作が遅くなりますが、常にリフレッシュレートとFPSが一致している状態として振る舞います。映像作品の制作に用いてください。</param>
		protected GameBase(int width, int height, string title = "", int refreshRate = 60, bool isCaptureMode = false)
		{
			RefreshRate = refreshRate;
			IsCaptureMode = isCaptureMode;

			window = new GameWindow(width, height, GraphicsMode.Default, title ?? "DotFeather Window", GameWindowFlags.FixedWindow)
			{
				VSync = VSyncMode.Adaptive,
				TargetRenderFrequency = refreshRate,
				TargetUpdateFrequency = refreshRate,
			};

			if (IsCaptureMode && !Directory.Exists("./shot"))
			{
				Directory.CreateDirectory("shot");
			}

			window.Load += (s, e) =>
			{
				GL.ClearColor(Color.Black);
				GL.LineWidth(1);
				GL.Disable(EnableCap.DepthTest);

				OnLoad(s, e);
			};

			window.Resize += (s, e) =>
			{
				GL.Viewport(window.ClientRectangle);
				OnResize(s, e);
			};

			window.RenderFrame += OnRenderFrame;
			window.Unload += OnUnload;
			window.KeyDown += (s, e) => OnKeyDown(s, new DFKeyEventArgs(e));
			window.KeyUp += (s, e) => OnKeyUp(s, new DFKeyEventArgs(e));

			window.MouseMove += (object sender, OpenTK.Input.MouseMoveEventArgs e) =>
				DFMouse.Position = new VectorInt((int)(e.Position.X / Dpi), (int)(e.Position.Y / Dpi));
		}

		/// <summary>
		/// 乱数を指定したシード値で初期化します。
		/// </summary>
		/// <param name="seed">シード値。<c>null</c> であれば、 <see cref="System.Random"/> の標準のコンストラクターを呼びます。</param>
		public void Randomize(int? seed = null)
		{
			Random = seed is int s ? new Random(s) : new Random();
		}

		/// <summary>
		/// ゲームを実行します。
		/// </summary>
		/// <returns>返り値。</returns>
		public int Run()
		{
			window.Run(RefreshRate);
			return statusCode ?? 0;
		}

		/// <summary>
		/// ゲームを終了します。
		/// </summary>
		/// <param name="status">返り値。</param>
		public void Exit(int status = 0)
		{
			statusCode = status;
			window.Close();
		}

		/// <summary>
		/// Releases all resource used by the <see cref="T:DotFeather.GameBase"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="T:DotFeather.GameBase"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="T:DotFeather.GameBase"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="T:DotFeather.GameBase"/> so the garbage
		/// collector can reclaim the memory that the <see cref="T:DotFeather.GameBase"/> was occupying.</remarks>
		public void Dispose()
		{
			window.Dispose();
		}

		/// <summary>
		/// 現在の画面のスクリーンショットを撮影します。
		/// </summary>
		public Bitmap TakeScreenshot()
		{
			if (GraphicsContext.CurrentContext == null)
				throw new GraphicsContextMissingException();
			int w = Width;
			int h = Height;
			Bitmap bmp = new Bitmap(w, h);
			System.Drawing.Imaging.BitmapData data =
				bmp.LockBits(new Rectangle(0, 0, w, h), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			GL.ReadPixels(0, 0, w, h, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
			bmp.UnlockBits(data);

			bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
			return bmp;
		}

		/// <summary>
		/// コルーチンを開始します。
		/// </summary>
		public Coroutine StartCoroutine(IEnumerator coroutine) => CoroutineRunner.Start(coroutine);

		/// <summary>
		/// コルーチンを停止します。
		/// </summary>
		public void StopCoroutine(Coroutine coroutine) => CoroutineRunner.Stop(coroutine);

		/// <summary>
		/// ゲームのフレーム更新時に呼び出されます。このメソッドをオーバーライドして、ゲームのメインループを記述してください。
		/// </summary>
		protected virtual void OnUpdate(object sender, DFEventArgs e) { }

		/// <summary>
		/// ウィンドウが開かれたときに一度だけ呼び出されます。
		/// </summary>
		protected virtual void OnLoad(object sender, EventArgs e) { }

		/// <summary>
		/// ウィンドウが閉じられるときに一度だけ呼び出されます。
		/// </summary>
		protected virtual void OnUnload(object sender, EventArgs e) { }

		/// <summary>
		/// ウィンドウがリサイズされたときに呼び出されます。
		/// </summary>
		protected virtual void OnResize(object sender, EventArgs e) { }

		/// <summary>
		/// キーが押されたときに呼び出されます。
		/// </summary>
		protected virtual void OnKeyDown(object sender, DFKeyEventArgs e) { }

		/// <summary>
		/// キーが離されたときに呼び出されます。
		/// </summary>
		protected virtual void OnKeyUp(object sender, DFKeyEventArgs e) { }

		/// <summary>
		/// 乱数生成器を取得します。
		/// </summary>
		protected Random Random { get; private set; } = new Random();

		private void OnRenderFrame(object sender, FrameEventArgs e)
		{
			GL.ClearColor(BackgroundColor);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			var deltaTime = IsCaptureMode ? 1f / RefreshRate : (float)e.Time;
			Time.Now += deltaTime;
			Time.DeltaTime = deltaTime;

			CalculateFps();

			Root.Draw(this, Vector.Zero);

			Update(sender);

			Dpi = (float)window.ClientSize.Width / window.Size.Width;

			window.ProcessEvents();

			if (IsCaptureMode)
			{
				var path = $"./shot/{TotalFrame:00000000}.png";
				if (!File.Exists(path))
				{
					GL.Flush();
					var bmp = TakeScreenshot();
					bmp.Save(path, ImageFormat.Png);
					bmp.Dispose();
				}
				TotalFrame++;
			}
			window.SwapBuffers();
		}

		private void Update(object sender)
		{
			DFKeyboard.Update();
			DFMouse.Update();
			CoroutineRunner.Update();
			Root.OnUpdate(this);
			OnUpdate(sender, new DFEventArgs { DeltaTime = (float)Time.DeltaTime });
		}

		private void CalculateFps()
		{
			frameCount++;
			if (prevSecond != DateTime.Now.Second)
			{
				Time.Fps = frameCount;
				frameCount = 0;
				prevSecond = DateTime.Now.Second;
			}
		}

		private int? statusCode;
		private int frameCount;
		private int prevSecond;
		private readonly GameWindow window;
	}
}
