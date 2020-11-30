using System.Text.RegularExpressions;

namespace UniMoonAdventure
{
    public class StringChecker
    {
        public static string StripHTMLTags(string text)
        {
            return Regex.Replace(text, "<[^>]*?>", "");
        }
        /// <summary>
        /// 文字列が英数/ひらがな/カタカナ/漢字のみで構成されているか？(タグや記号が含まれていないか？)
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool isNormalString(string text)
        {
            return (isKatakana(text) && isEisu(text) && isHiragana(text) && isKanji(text));
        }

        /// <summary>
        /// 文字列がカタカナのみで構成されているか?
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool isKatakana(string text)
        {
            //通常の全角カタカナの他に、カタカナフリガナ拡張、
            //　濁点と半濁点、半角カタカナもカタカナとする
            //https://dobon.net/vb/dotnet/string/ishiragana.html
            return (Regex.IsMatch(text, @"^[\p{IsKatakana}\u31F0-\u31FF\u3099-\u309C\uFF65-\uFF9F]+$"));
        }

        /// <summary>
        /// 文字列が英数文字（記号は含まない)のみで構成されているか?
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool isEisu(string text)
        {
            return (Regex.IsMatch(text, @"^[0-9a-zA-Z]+$"));
        }

        /// <summary>
        /// 文字列がひらがなのみで構成されているか？
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool isHiragana(string text)
        {
            return (Regex.IsMatch(text, @"^\p{IsHiragana}+$"));
        }

        /// <summary>
        /// 文字列が漢字だけで構成されているか？
        /// <para>CJK統合漢字、CJK互換漢字、CJK統合漢字拡張Aの範囲か？</para>
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool isKanji(string text)
        {
            //CJK統合漢字、CJK互換漢字、CJK統合漢字拡張Aの範囲にあるか調べる
            //return ('\u4E00' <= c && c <= '\u9FCF')
            //    || ('\uF900' <= c && c <= '\uFAFF')
            //    || ('\u3400' <= c && c <= '\u4DBF');

            return (Regex.IsMatch(text, @"^[\u4E00-\u9FCF\uF900-\uFAFF\u3400-\u4DBF]+$"));
        }

    }
}