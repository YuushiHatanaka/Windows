using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Config
{
    /// <summary>
    /// 解析状態
    /// </summary>
    public enum JsonStatus : byte
    {
        NotStart = 0,                // 解析未実施
        StartObject,                 // Object開始
        NextObject,                  // 次Object開始
        Object,                      // Object処理
        EndObject,                   // Object終了
        StartArray,                  // Array開始
        NextArray,                   // 次Array処理
        EndArray,                    // Array終了
        EndKey,                      // Key終了
        StartValue,                  // Value開始
        Value,                       // Value処理
        EndValue,                    // Value終了
        EndArrayValue,               // Value終了(Array)
        NextWait,                    // 次遷移待ち
        End                          // 解析終了
    };
}
