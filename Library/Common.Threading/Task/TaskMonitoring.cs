using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using log4net;
using System.Reflection;
using Common.Logging;
using Common.Threading.Task;

namespace CommonLibrary
{
    /// <summary>
    /// 監視Taskクラス
    /// </summary>
    public class TaskMonitoring : IDisposable
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        protected static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// Disposeフラグ
        /// </summary>
        private bool m_Disposed = false;

        /// <summary>
        /// Poollingタイマー
        /// </summary>
        protected TimeSpan m_PoollingTimer = new TimeSpan(0, 0, 0, 1, 0);

        /// <summary>
        /// Task終了待ちタイマー
        /// </summary>
        protected TimeSpan m_TaskWaitTimer = new TimeSpan(0, 0, 0, 10, 0);

        /// <summary>
        /// タスク状態
        /// </summary>
        public TaskStatus TaskStatus
        {
            get
            {
                if (m_MonitoringTask == null)
                {
                    return TaskStatus.Created;
                }

                return m_MonitoringTask.Status;
            }
        }

        /// <summary>
        /// CancellationTokenSourceオブジェクト
        /// </summary>
        protected CancellationTokenSource m_CancellationTokenSource = null;

        /// <summary>
        /// CancellationTokenオブジェクト
        /// </summary>
        protected CancellationToken m_CancellationToken = default(CancellationToken);

        /// <summary>
        /// 監視タスク
        /// </summary>
        protected Task m_MonitoringTask = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TaskMonitoring()
        {
            // ロギング
            Logger.Debug("=>>>> TaskMonitoring::TaskMonitoring()");

            // CancellationTokenSourceオブジェクト生成
            m_CancellationTokenSource = new CancellationTokenSource();
            m_CancellationToken = m_CancellationTokenSource.Token;

            // ロギング
            Logger.Debug("<<<<= TaskMonitoring::TaskMonitoring()");
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~TaskMonitoring()
        {
            // ロギング
            Logger.Debug("=>>>> TaskMonitoring::~TaskMonitoring()");

            // リソース破棄
            Dispose(false);

            // ロギング
            Logger.Debug("<<<<= TaskMonitoring::~TaskMonitoring()");
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // ロギング
            Logger.Debug("=>>>> TaskMonitoring::Dispose()");

            // リソース破棄
            Dispose(true);

            // ガベージコレクション
            GC.SuppressFinalize(this);

            // ロギング
            Logger.Debug("<<<<= TaskMonitoring::Dispose()");
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            // ロギング
            Logger.Debug("=>>>> TaskMonitoring::Dispose(bool)");
            Logger.DebugFormat("disposing:{0}", disposing);

            if (!m_Disposed)
            {
                if (disposing)
                {
                    // TODO: Dispose managed resources here.
                    m_CancellationTokenSource?.Dispose();
                    m_CancellationTokenSource = null;
                    m_MonitoringTask?.Dispose();
                    m_MonitoringTask = null;
                }

                // TODO: Free unmanaged resources here.

                // Note disposing has been done.
                m_Disposed = true;
            }

            // ロギング
            Logger.Debug("<<<<= TaskMonitoring::Dispose(bool)");
        }

        /// <summary>
        /// 開始
        /// </summary>
        public virtual void Start()
        {
            // ロギング
            Logger.Debug("=>>>> TaskMonitoring::Start()");

            // Task実行
            m_MonitoringTask = Task.Run(() =>
            {
                // 実行
                Run();
            });

            // ロギング
            Logger.Debug("<<<<= TaskMonitoring::Start()");
        }

        /// <summary>
        /// 実行
        /// </summary>
        protected virtual async void Run()
        {
            // ロギング
            Logger.Debug("=>>>> TaskMonitoring::Run()");

            // 警告回避
            await Task.Delay(10);

            // 例外
            throw new NotImplementedException();
        }

        /// <summary>
        /// 終了
        /// </summary>
        public virtual void End()
        {
            // ロギング
            Logger.Debug("=>>>> TaskMonitoring::End()");

            // Task状態判定
            if (m_MonitoringTask != null)
            {
                // 監視Taskキャンセル
                m_CancellationTokenSource.Cancel();
            }

            // ロギング
            Logger.Debug("<<<<= TaskMonitoring::End()");
        }

        /// <summary>
        /// 終了待ち
        /// </summary>
        public virtual void Wait()
        {
            // ロギング
            Logger.Debug("=>>>> TaskMonitoring::Wait()");

            // Task状態判定
            if (m_MonitoringTask != null)
            {
                try
                {
                    // 終了待ち
                    if (!m_MonitoringTask.Wait(m_TaskWaitTimer))
                    {
                        // ロギング
                        Logger.WarnFormat("タスク終了待ちを完了できませんでした:[{0}]", m_TaskWaitTimer.ToString());
                    }
                }
                catch (AggregateException ex)
                {
                    // 例外
                    throw new TaskException("タスク終了待ちでタスク取消発生", ex);
                }
                catch (OperationCanceledException ex)
                {
                    // 例外
                    throw new TaskException("タスク終了待ちでキャンセル発生", ex);
                }
                catch (Exception ex)
                {
                    // 例外
                    throw new TaskException("タスク終了待ちで例外発生", ex);
                }
            }

            // ロギング
            Logger.Debug("<<<<= TaskMonitoring::Wait()");
        }
    }
}
