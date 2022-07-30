namespace Drawer.Web.DataBinding
{
    /// <summary>
    /// 데이터바인드를 제외할 속성을 지정한다.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NoBindAttribute : Attribute
    {
    }
}
