# キーボード入力

キーボードからの入力を取得するには、 `Input` 静的クラスの `Keyboard` プロパティを使用します。

Keyboard プロパティの子要素には、あらゆるキーの名前がプロパティとしてあります。

実際の使用例を述べます。

```cs
// A を押したか判定する
if (Input.Keyboard.A.IsPressed)
{
    Console.WriteLine("Aが押されている");
}
```

次: [独自レンダリング](../plugin/render.md)