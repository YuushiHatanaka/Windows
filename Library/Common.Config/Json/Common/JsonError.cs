using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Config
{
    /// <summary>
    /// エラーコード
    /// </summary>
    public enum JsonError
    {
        NormalEnd = 0,                // 正常終了
        NotFoundStartTag,             // 開始タグなしエラー
        NotFindEndTag,                // 終了タグなしエラー
        NotFoundKey,                  // Keyなしエラー
        NotFoundValue,                // Valueなしエラー
        InvalidFormat,                // フォーマットエラー
        InvalidSyntax,                // 構文エラー
        OutOfMemory,
        StackOverflow,
        CannotOpenFile,
        InvalidArgument,
        InvalidUtf8,
        PrematureEndOfInput,
        EndOfInputExpected,
        WrongType,
        NullCharacter,
        NullValue,
        NullByteInKey,
        DuplicateKey,
        NumericOverflow,
        ItemNotFound,
        IndexOutOfRange,              // インデックス範囲エラー
        SystemError                   // システムエラー
    };
}
