using MoonSharp.Interpreter;
using UnityEngine;

public class HelloLua : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var result  = MoonSharpFactorial();
        Debug.Log(result.ToString());
    }

    double MoonSharpFactorial()
    {
        string script = @"    
		-- defines a factorial function
		function fact (n)
			if (n == 0) then
				return 1
			else
				return n*fact(n - 1)
			end
		end

		return fact(5)";

        DynValue res = Script.RunString(script);
        return res.Number;
    }

}
