using System.Reflection;

namespace Drawer.Web.DataBinding
{
    public interface IBindingObject
    {
        IndexObject<string, string?> PropertyStringValues { get; }
    }

    public class BindingObject : IBindingObject
    {
        private IndexObject<string, string?>? _propertyStringValues;
        public IndexObject<string, string?> PropertyStringValues
        {
            get
            {
                if (_propertyStringValues == null)
                    _propertyStringValues = new IndexObject<string, string?>(GetStrValue, SetStrValue);
                return _propertyStringValues;
            }
        }

        private PropertyInfo[]? _propertyInfos;
        public PropertyInfo[] PropertyInfos
        {
            get
            {
                if (_propertyInfos == null)
                    _propertyInfos = this.GetType().GetProperties().ToArray();
                return _propertyInfos;
            }
        }

        private string? GetStrValue(string propName)
        {
            var property = PropertyInfos.FirstOrDefault(x => x.Name == propName);
            if (property == null)
                return null;

            return property.GetValue(this)?.ToString();

        }

        private void SetStrValue(string propName, string? value)
        {
            var property = PropertyInfos.FirstOrDefault(x => x.Name == propName);
            if (property == null)
                return;

            try
            {
                var convertedValue = Convert.ChangeType(value, property.PropertyType);
                property.SetValue(this, convertedValue);
            }
            catch
            {
            }

        }
    }
}
