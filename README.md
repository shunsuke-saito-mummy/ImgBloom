# ImgBloomについて
ImgBloomはuGUIのImageに対して簡単にBloomのような表現を実現できるものです。
Blur画像の生成・保存機能と、Imageに対する加算合成シェーダの実装、それを用いて加算合成用の画像を表示するImageBloomコンポーネントが含まれます。

# 使い方
## 1. ブラー画像の生成
Blur画像を生成するため、任意のProject Windowで任意のTextureを選択した状態で`Tools/ImgBloom/Generabe Blurred Image`を実行します。
保存先を聞かれるので、好きな位置に保存します。

## 2. 元画像とブラー画像の配置
下記の構成になるようにGameObjectを配置します。
```
Image
|-Blur Image
```

## 3. ブラー画像を加算合成
`Blur Image`オブジェクトに`ImageBloom`コンポーネントをアタッチし、先ほど生成したブラー画像を割り当てます。
あとはカラーを調整することでBloom表現が実現できます。