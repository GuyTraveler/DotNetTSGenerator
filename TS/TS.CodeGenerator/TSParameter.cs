using System;
using System.Reflection;

namespace TS.CodeGenerator
{
    public class TSParameter : IGenerateTS
    {
        private const string formatParameter = @"{0}:{1}/*{2}*/";
        private const string formatParameterNullable = @"{0}?:{1}/*{2}*/";

        private readonly ParameterInfo _parameterInfo;
        private readonly Func<Type, string> _mapType;

        public string ParameterName { get; private set; }
        public string ParameterType { get; private set; }
        public bool IsOptional { get; set; }

        public TSParameter(ParameterInfo parameterInfo, Func<Type, string> mapType)
        {
            _parameterInfo = parameterInfo;
            _mapType = mapType;
            ParameterName = parameterInfo.Name;
            IsOptional = _parameterInfo.IsOptional || IsNullable(_parameterInfo.ParameterType);
        }

        static bool IsNullable(Type type)
        {
            //if (!type.IsValueType) return true; // ref-type
            if (Nullable.GetUnderlyingType(type) != null) return true; // Nullable<T>
            return false; // value-type
        }

        public void Initialize()
        {
            ParameterType = _mapType(_parameterInfo.ParameterType);
        }

        public string ToTSString()
        {
            var res = string.Format(IsOptional ? formatParameterNullable : formatParameter, ParameterName, ParameterType, _parameterInfo.ParameterType.Name);
            return res;
        }
    }
}