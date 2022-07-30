namespace Drawer.Web.DataBinding
{
    /// <summary>
    /// 인덱스를 통해 Value Get/Set을 제공한다.
    /// </summary>
    /// <typeparam name="TIndex"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class IndexObject<TIndex, TValue>
    {
        readonly Action<TIndex, TValue?> SetAction;
        readonly Func<TIndex, TValue?> GetFunc;

        public IndexObject(Func<TIndex, TValue?> getFunc, Action<TIndex, TValue?> setAction)
        {
            this.GetFunc = getFunc;
            this.SetAction = setAction;
        }
        public TValue? this[TIndex key]
        {
            get
            {
                return GetFunc(key);
            }
            set
            {
                SetAction(key, value);
            }
        }
    }
}
