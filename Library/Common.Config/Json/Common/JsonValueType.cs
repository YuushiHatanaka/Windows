using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Config
{
    /// <summary>
    /// Value種別
    /// </summary>
    public enum JsonValueType
    {
        Unknown = 0,                // 不明
        ObjectStart,                // object開始
        Object,                     // object
        ObjectEnd,                  // object終了
        ArrayStart,                 // array開始
        Array,                      // array
        ArrayEnd,                   // array終了
        Element,                    // element
        Value,                      // value
        String,                     // string
        Number,                     // number
        Bool,                       // true/false
        Null,                       // null
    };
}
