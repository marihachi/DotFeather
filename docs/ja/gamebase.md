# GameBase の継承

DotFeather では、ゲームのメインループを GameBase 抽象クラスの派生クラスを定義し、その中に書きます。

```cs
public class Game : GameBase
{
	public Game(int width, int height, string title = null, int refreshRate = 60)
		: base(width, height, title, refreshRate) { }
}
```

**以降、こうして作った派生クラスを、ゲームクラスと呼びます。**

## エントリーポイント

他の C# プログラムと同様、 `Main()` メソッドからプログラムは開始します。 `Main()` メソッドに次のような文を記述し、作成したゲームクラスを起動させます。

```cs
static void Main()
{
	using (var g = new Game(320, 240))
	{
		g.Run();
	}
}
```

## イベントフック

メインループや、アセットの読み込みなどは、 GameBase クラスが提供する仮想メソッドをオーバーライドし、その上で実行します。次に、 GameBase クラスが提供する仮想メソッドの表を示します。


|メソッド|説明|
|---|---|
|OnLoad()|ゲーム開始時に呼び出されます。リソースやセーブデータの読み込みを書きます。|
|OnUpdate()|フレーム更新時に呼び出されます。ゲームのメインループ処理を書きます。|
|OnResize()|ゲームウィンドウのサイズが変更された場合に呼び出されます。|
|OnUnload()|ゲーム終了時に呼び出されます。オートセーブや独自に確保したリソースの破棄などを行ってください。|
|OnDragDrop()|ファイルがドラッグドロップされたときに呼び出されます。|

## プロパティ

プロパティの設定により、ゲームウィンドウのカスタマイズなどを行えます。

|プロパティ|説明|
|---|---|
|Title|ウィンドウのタイトル|
|X|ウィンドウ X座標|
|Y|ウィンドウ Y座標|
|Width|ウィンドウの幅|
|Height|ウィンドウの高さ|
|Visible|ウィンドウが表示されているか|
|BackgroundColor|ゲーム描画領域の背景色|
|Dpi|現在のディスプレイのDPI|
|RefreshRate|現在のディスプレイのリフレッシュレート|
|Root|トップレベルのコンテナー。この中に描画オブジェクトを入れる。|

## その他機能

### Randomize() メソッド

乱数を初期化します。シード値を指定できますが、省略すると現在時刻を乱数シード値として使用します。

### Random プロパティ

.NET Standard BCL の `Random` クラスのインスタンスが格納されます。

### Exit() メソッド

ゲームを終了します。終了コードを指定することもできます。

他にも多くの機能があります。詳しくは[API ドキュメント](https://dotfeather.netlify.com/api/dotfeather.gamebase)をご確認ください。

次: [Hello](hello.md)
