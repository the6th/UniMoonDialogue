# UniMoonDialogue

* Unityで動作するノベル風シナリオ進行ダイアログのフレームワーク
* C# と Lua でシナリオを記述できます。
    ![](docs/images/UniMoonDialogue.gif)

* 開発中のプロジェクトなので、大幅な変更が入る可能性があります

## How to Use
* Luaスクリプトのパースは[MoonSharp](https://github.com/moonsharp-devs/moonsharp)を利用しています。

1. Download [moonsharp_release_2.0.0.0.zip](https://github.com/moonsharp-devs/moonsharp/releases/tag/v2.0.0.0)

2. Import MoonSharp_2.0.0.0.unitypackage in the zip to your project.

## Example

* Csharpによる記述例
```cs
private int index = 0;

Scenario scenario = new Scenario(
    dialogs: new Dictionary<int, Dialogue>
    {
        {1,  new Dialogue("こんにちは。私はC#で書かれています。")},
        {2,  new Dialogue("次のメッセージ")},
        {3,  new Dialogue("質問です！",
            choices: new List<Choice>
            {
                new Choice("はい",4),
                new Choice("いいえ",5),
            }
        )},
        {4,  new Dialogue("「はい」を選びましたね",6)},
        {5,  new Dialogue("「いいえ」を選びましたね",6)},
        {6,  new Dialogue("おしまい")}
    }
);

public void StartScenario()
{
    ScenarioEngine.Instance.OnMessageStart += OnMessageStart;
    ScenarioEngine.Instance.OnMessageEnd += OnMessageEnd;
    ScenarioEngine.Instance.OnUserInput += OnUserInput;

    var data = new EventData(gameObject);
    if (ScenarioEngine.Instance.StartScenario(data))
    {
        index = scenario.dialogs.Keys.Min();
        ShowDialogue(data, index);
    }
}
```
## License
* MIT License
* Copyright (c) 2020 Tomoki Hayashi
