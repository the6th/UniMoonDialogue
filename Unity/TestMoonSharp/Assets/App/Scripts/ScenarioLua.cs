using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.Events;

public class ScenarioLua : MonoBehaviour
{
    [MoonSharp.Interpreter.MoonSharpUserData]
    class EventData
    {
        // 画面にセリフを表示
        public void serif(string mes)
        {
            Debug.Log(mes);
            ScenarioLua.scenarioMode = ScenarioMode.TapToNext;

        }

        // 選択肢を表示
        public void select(string title, params string[] choices)
        {
            Debug.Log($"title: {title}");

            var c = "";
            foreach (var choice in choices)
            {
                c += choice + "/";
            }
            Debug.Log(c);
            ScenarioLua.scenarioMode = ScenarioMode.Choise;

        }
    }


    public enum ScenarioMode { TapToNext, Choise, None };
    public enum ScenarioChoice
    {
        SKIP = 0,
        SELECT_1,
        SELECT_2,
        SELECT_3,
        SELECT_4,
        SELECT_5,
        None
    };

    public UnityAction<ScenarioChoice> OnScenarioSelected;
    [SerializeField]
    public static ScenarioMode scenarioMode
    {
        set
        {
            m_scenarioMode = value;
            Debug.Log(m_scenarioMode.ToString());
        }
        get { return m_scenarioMode; }
    }
    private static ScenarioMode m_scenarioMode = ScenarioMode.None;



    private ScenarioChoice scenarioChoice = ScenarioChoice.None;
    private DynValue coroutine;
    void Start()
    {
        StartScenario();
    }

    private void Update()
    {
        CheckKey();
    }
    private void CheckKey()
    {
        if (scenarioMode == ScenarioMode.None) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            ScenarioSelect(ScenarioChoice.SELECT_1);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ScenarioSelect(ScenarioChoice.SELECT_2);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            ScenarioSelect(ScenarioChoice.SELECT_3);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            ScenarioSelect(ScenarioChoice.SELECT_4);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            ScenarioSelect(ScenarioChoice.SELECT_5);
        else if (Input.GetMouseButtonDown(0))
            ScenarioSelect(ScenarioChoice.SKIP);

    }
    public void ScenarioSelect(ScenarioChoice choice)
    {
        //Debug.Log("ScenarioSelect" + choice.ToString());
        if (coroutine.Coroutine.State != CoroutineState.Dead && scenarioMode != ScenarioMode.None)
        {
            scenarioChoice = choice;
            Debug.Log($"ScenarioSelect2:{choice.ToString()}");

            coroutine.Coroutine.Resume((int)scenarioChoice);
            //scenarioMode = ScenarioMode.None;
        }
    }

    void StartScenario()
    {
        // EventDataをスクリプト内で使えるようにする
        UserData.RegisterAssembly(typeof(EventData).Assembly);

        // スクリプトインスタンスを生成
        Script script = new Script();

        // EventDataクラスをスクリプト内部で使えるようにする
        script.Globals["event"] = new EventData();

        var scenario = LoadScenario(
            );
        // コードをコンパイル
        DynValue function = script.DoString(scenario);

        // コルーチンを生成
        coroutine = script.CreateCoroutine(function);

        coroutine.Coroutine.Resume();

    }

    string LoadScenario()
    {
        string code =
        @"
            return function()
 
            event.serif( 'やあ。ウチは[word:15000]から来た[name:10103]やで' )
            coroutine.yield()
 
            event.serif( '自分はどっからきたん？' )
            coroutine.yield()
 
            event.select( 'どこから？', 'ここが地元', '別の島', '[word:15000]', 'わからない' )
            local selected = coroutine.yield()
            if selected == 0 then
 
                event.serif( 'へえ。じゃあウチよりこの辺には詳しそうやね' )
                coroutine.yield()
 
            elseif selected == 1 then
 
                event.serif( 'この島へは何しにきたんやろなあ' )
                coroutine.yield()
 
            elseif selected == 2 then
 
                event.serif( '同郷やね。でもそんな風には見えへんなあ？' )
                coroutine.yield()
 
            elseif selected == 3 then
 
                event.serif( 'どういうこっちゃ' )
                coroutine.yield()
 
            end
            event.serif( 'おしまい' )
 
            end
        ";
        return code;

    }

}
