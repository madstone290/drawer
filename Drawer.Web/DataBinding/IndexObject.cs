namespace Drawer.Web.DataBinding
{
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
