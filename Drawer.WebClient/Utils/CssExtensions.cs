namespace Drawer.WebClient.Utils
{
    public static class CssUtil
    {
        const string DisplayNone = "d-none";

        /// <summary>
        /// 조건을 만족하는 경우에만 요소를 보이도록 한다.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static string Visibility(bool condition)
        {
            return AddVisibility(string.Empty, condition);
        }

        /// <summary>
        /// 조건을 만족하는 경우에만 요소를 보이도록 한다.
        /// 조건에 따라 display: none을 적용한다.
        /// </summary>
        /// <param name="initialClass"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static string AddVisibility(string initialClass, bool condition)
        {
            if (condition)
                return initialClass;
            else
                return AddClass(initialClass, DisplayNone);
        }

        /// <summary>
        /// 클래스를 추가한다.
        /// </summary>
        /// <param name="initialClass"></param>
        /// <param name="additionalClass"></param>
        /// <returns></returns>
        public static string AddClass(string initialClass, string additionalClass)
        {
            if (string.IsNullOrWhiteSpace(initialClass))
                return additionalClass;
            else
                return initialClass + " " + additionalClass;
        }

    }
}
