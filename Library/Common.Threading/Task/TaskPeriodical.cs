using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
using log4net;
using log4net.Config;
using Common.Logging;
using Common.Threading.Task;

namespace CommonLibrary
{
    /// <summary>
    /// 定期Taskクラス
    /// </summary>
    public class TaskPeriodical : IDisposable
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
        protected bool m_Disposed = false;

        /// <summary>
        /// インターバルタイマー
        /// </summary>
        protected TimeSpan m_IntervalTimer = new TimeSpan();

        /// <summary>
        /// Task終了待ちタイマー
        /// </summary>
        protected TimeSpan m_TaskWaitTimer = new TimeSpan(0, 0, 0, 10, 0);

        /// <summary>
        /// CancellationTokenSourceオブジェクト
        /// </summary>
        protected CancellationTokenSource m_CancellationTokenSource = null;

        /// <summary>
        /// CancellationTokenオブジェクト
        /// </summary>
        protected CancellationToken m_CancellationToken = default(CancellationToken);

        /// <summary>
        /// CancellationTokenSourceオブジェクト
        /// </summary>
        protected CancellationTokenSource m_PauseTokenSource = null;

        /// <summary>
        /// 監視タスク
        /// </summary>
        protected Task m_MonitoringTask = null;

        /// <summary>
        /// 一時停止状態
        /// </summary>
        public bool IsPause
        {
            get
            {
                if (m_PauseTokenSource != null)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Task状態
        /// </summary>
        public TaskStatus Status
        {
            get
            {
                if (m_MonitoringTask != null)
                {
                    return m_MonitoringTask.Status;
                }
                return TaskStatus.Faulted;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TaskPeriodical()
        {
            // ロギング
            Logger.Debug("=>>>> TaskPeriodical::TaskPeriodical()");

            // ロギング
            Logger.Debug("<<<<= TaskPeriodical::TaskPeriodical()");
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~TaskPeriodical()
        {
            // ロギング
            Logger.Debug("=>>>> TaskPeriodical::~TaskPeriodical()");

            // リソース破棄
            Dispose(false);

            // ロギング
            Logger.Debug("<<<<= TaskPeriodical::~TaskPeriodical()");
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // ロギング
            Logger.Debug("=>>>> TaskPeriodical::Dispose()");

            // リソース破棄
            Dispose(true);

            // ガベージコレクション
            GC.SuppressFinalize(this);

            // ロギング
            Logger.Debug("<<<<= TaskPeriodical::Dispose()");
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            // ロギング
            Logger.Debug("=>>>> TaskPeriodical::Dispose(bool)");
            Logger.DebugFormat("disposing:{0}", disposing);

            // Disposeフラグ判定
            if (!m_Disposed)
            {
                // Disposeフラグ指示判定
                if (disposing)
                {
                    // Dispose managed resources here.
                    m_CancellationTokenSource?.Dispose();
                    m_CancellationTokenSource = null;
                    m_MonitoringTask?.Dispose();
                    m_MonitoringTask = null;
                }

                // Free unmanaged resources here.

                // Note disposing has been done.
                m_Disposed = true;
            }

            // ロギング
            Logger.Debug("<<<<= TaskPeriodical::Dispose(bool)");
        }

        /// <summary>
        /// 開始
        /// </summary>
        public virtual void Start()
        {
            // ロギング
            Logger.Debug("=>>>> TaskPeriodical::Start()");

            // Task判定
            if (m_MonitoringTask != null)
            {
                // 実行済み
                return;
            }

            // CancellationTokenSourceオブジェクト生成
            m_CancellationTokenSource = new CancellationTokenSource();
            m_CancellationToken = m_CancellationTokenSource.Token;

            // Task実行
            m_MonitoringTask = Task.Run(() =>
            {
                // 実行
                Run(false);
            });

            // ロギング
            Logger.Debug("<<<<= TaskPeriodical::Start()");
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            // ロギング
            Logger.Debug("=>>>> TaskPeriodical::Stop()");

            // Task判定
            if (m_MonitoringTask != null)
            {
                // 終了
                End();

                // 終了待ち
                Wait();

                // 破棄
                Dispose();
            }

            // ロギング
            Logger.Debug("<<<<= TaskPeriodical::Stop()");
        }

        /// <summary>
        /// 再起動
        /// </summary>
        public virtual void Restart()
        {
            // ロギング
            Logger.Debug("=>>>> TaskPeriodical::Restart()");

            // 終了
            End();

            // 終了待ち
            Wait();

            // 破棄
            Dispose();

            // 開始
            Start();

            // ロギング
            Logger.Debug("<<<<= TaskPeriodical::Restart()");
        }

        /// <summary>
        /// 一時停止
        /// </summary>
        public virtual void Pause()
        {
            // ロギング
            Logger.Debug("=>>>> TaskPeriodical::Pause()");

            // 一時停止用CancellationTokenSourceオブジェクトを生成
            if (m_PauseTokenSource == null)
            {
                m_PauseTokenSource = new CancellationTokenSource();
            }

            // ロギング
            Logger.Debug("<<<<= TaskPeriodical::Pause()");
        }

        /// <summary>
        /// 再開
        /// </summary>
        public virtual void Resume()
        {
            // ロギング
            Logger.Debug("=>>>> TaskPeriodical::Resume()");

            // 一時停止をキャンセル
            if (m_PauseTokenSource != null)
            {
                m_PauseTokenSource.Cancel();
                m_PauseTokenSource.Dispose();
                m_PauseTokenSource = null;
            }

            // ロギング
            Logger.Debug("<<<<= TaskPeriodical::Resume()");
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="execution"></param>
        protected virtual async void Run(bool execution)
        {
            // ロギング
            Logger.Debug("=>>>> TaskPeriodical::Run(bool)");
            Logger.DebugFormat("execution:{0}", execution);

            // 前処理
            Preprocessing();

            try
            {
                // 強制実行の場合は、ループ前に実行メソッドを呼出す
                if (execution)
                {
                    // 実行(失敗しても周期起動処理は継続させる(例外時を除く))
                    await Execute();
                }

                // 無限ループ
                while (true)
                {
                    // キャンセルされてたら OperationCanceledException を投げる
                    m_CancellationToken.ThrowIfCancellationRequested();

                    // 一時停止要求がある場合、ここで一時停止
                    m_PauseTokenSource?.Token.WaitHandle.WaitOne(Timeout.InfiniteTimeSpan);

                    // 次周期処理待ち
                    Task.Delay(m_IntervalTimer, m_CancellationToken).Wait();

                    // 実行
                    if (!await Execute())
                    {
                        // 処理継続(例外時のみ処理終了)
                        continue;
                    }
                }
            }
            catch (AggregateException ex)
            {
                // 例外
                throw new TaskException("タスク実行で取消発生", ex);
            }
            catch (OperationCanceledException ex)
            {
                // 例外
                throw new TaskException("タスク実行でキャンセル発生", ex);
            }
            catch (Exception ex)
            {
                // 例外
                throw new TaskException("タスク実行で例外発生", ex);
            }
            finally
            {
                // 後処理
                PostProcessing();

                // ロギング
                Logger.Debug("<<<<= TaskPeriodical::Run(bool)");
            }
        }

        /// <summary>
        /// 実行(強制)
        /// </summary>
        /// <returns></returns>
        public void Execution()
        {
            // ロギング
            Logger.Debug("=>>>> TaskPeriodical::Execution()");

            // Task判定
            if (m_MonitoringTask != null)
            {
                // 実行済みなので、一度停止
                Stop();
            }

            // CancellationTokenSourceオブジェクト生成
            m_CancellationTokenSource = new CancellationTokenSource();
            m_CancellationToken = m_CancellationTokenSource.Token;

            // Task実行
            m_MonitoringTask = Task.Run(() =>
            {
                // 実行
                Run(true);
            });

            // ロギング
            Logger.Debug("<<<<= TaskPeriodical::Execution()");
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<bool> Execute()
        {
            // ロギング
            Logger.Debug("=>>>> TaskMonitoring::Execute()");

            // 警告回避
            await Task.Delay(10);

            // 例外
            throw new NotImplementedException();
        }

        /// <summary>
        /// 前処理
        /// </summary>
        protected virtual void Preprocessing()
        {
            // ロギング
            Logger.Debug("=>>>> TaskPeriodical::Preprocessing()");

            // ロギング
            Logger.Debug("<<<<= TaskPeriodical::Preprocessing()");
        }

        /// <summary>
        /// 後処理
        /// </summary>
        protected virtual void PostProcessing()
        {
            // ロギング
            Logger.Debug("=>>>> TaskPeriodical::PostProcessing()");

            // ロギング
            Logger.Debug("<<<<= TaskPeriodical::PostProcessing()");
        }

        /// <summary>
        /// 終了
        /// </summary>
        public virtual void End()
        {
            // ロギング
            Logger.Debug("=>>>> TaskPeriodical::End()");

            // Task状態判定
            if (m_MonitoringTask != null)
            {
                // 監視Taskキャンセル
                m_CancellationTokenSource.Cancel();
            }

            // ロギング
            Logger.Debug("<<<<= TaskPeriodical::End()");
        }

        /// <summary>
        /// 終了待ち
        /// </summary>
        public virtual void Wait()
        {
            // ロギング
            Logger.Debug("=>>>> TaskPeriodical::Wait()");

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

            // 初期化
            m_MonitoringTask = null;

            // ロギング
            Logger.Debug("<<<<= TaskPeriodical::Wait()");
        }
    }
}
