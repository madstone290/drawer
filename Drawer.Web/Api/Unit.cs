namespace Drawer.Web.Api
{
    /// <summary>
    /// 제네릭 타입에서 void대신 사용
    /// </summary>
    public struct Unit
    {
        private static readonly Unit _value = new();
        public static Unit Value => _value;
    }
}
