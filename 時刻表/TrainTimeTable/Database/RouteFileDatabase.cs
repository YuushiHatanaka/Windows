using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Common;
using TrainTimeTable.File;
using TrainTimeTable.Property;
using TrainTimeTable.Database.Table;
using static System.Collections.Specialized.BitVector32;
using System.IO;

namespace TrainTimeTable.Database
{
    /// <summary>
    /// RouteFileDatabaseクラス
    /// </summary>
    public class RouteFileDatabase : IDisposable
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Disposed
        /// <summary>
        /// Track whether Dispose has been called.
        /// </summary>
        private bool m_Disposed = false;
        #endregion

        #region データベースファイル名
        /// <summary>
        /// データベースファイル名
        /// </summary>
        public string FileName { get; set; }
        #endregion

        #region SQLiteConnectionオブジェクト
        /// <summary>
        /// SQLiteConnectionオブジェクト
        /// </summary>
        private SQLiteConnection m_SqliteConnection = null;
        #endregion

        #region テーブルオブジェクト
        /// <summary>
        /// FileInfomationTableオブジェクト
        /// </summary>
        private FileInfomationTable m_FileInfomationTable = null;

        /// <summary>
        /// RouteTableオブジェクト
        /// </summary>
        private RouteTable m_RouteTable = null;

        /// <summary>
        /// FontTablesオブジェクト
        /// </summary>
        private FontTable m_FontTable = null;

        /// <summary>
        /// ColorTablesオブジェクト
        /// </summary>
        private ColorTable m_ColorTable = null;

        /// <summary>
        /// DiagramScreenTableオブジェクト
        /// </summary>
        private DiagramScreenTable m_DiagramScreenTable = null;

        /// <summary>
        /// StationTableオブジェクト
        /// </summary>
        private StationTable m_StationTable = null;

        /// <summary>
        /// StationSequenceTableオブジェクト
        /// </summary>
        private StationSequenceTable m_StationSequenceTable = null;

        /// <summary>
        /// NextStationTableオブジェクト
        /// </summary>
        private NextStationTable m_NextStationTable = null;

        /// <summary>
        /// TrainTypeTableオブジェクト
        /// </summary>
        private TrainTypeTable m_TrainTypeTable = null;

        /// <summary>
        /// TrainTypeSequenceTableオブジェクト
        /// </summary>
        private TrainTypeSequenceTable m_TrainTypeSequenceTable = null;

        /// <summary>
        /// CommentTableオブジェクト
        /// </summary>
        private CommentTable m_CommentTable = null;

        /// <summary>
        /// DiagramTableオブジェクト
        /// </summary>
        private DiagramTable m_DiagramTable = null;

        /// <summary>
        /// DiagramSequenceTableオブジェクト
        /// </summary>
        private DiagramSequenceTable m_DiagramSequenceTable = null;

        /// <summary>
        /// TrainTableオブジェクト
        /// </summary>
        private TrainTable m_TrainTable = null;

        /// <summary>
        /// TrainSequenceTableオブジェクト
        /// </summary>
        private TrainSequenceTable m_TrainSequenceTable = null;

        /// <summary>
        /// TrainMarkTableオブジェクト
        /// </summary>
        private TrainMarkTable m_TrainMarkTable = null;

        /// <summary>
        /// TrainMarkSequenceTableオブジェクト
        /// </summary>
        private TrainMarkSequenceTable m_TrainMarkSequenceTable = null;

        /// <summary>
        /// StationTimeTableオブジェクト
        /// </summary>
        private StationTimeTable m_StationTimeTable = null;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileName"></param>
        public RouteFileDatabase(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileDatabase::RouteFileDatabase(string)");
            Logger.DebugFormat("fileName:[{0}]", fileName);

            // 設定
            FileName = fileName;

            // SQLiteConnectionStringBuilderオブジェクト作成
            SQLiteConnectionStringBuilder sqliteConnectionStringBuilder = new SQLiteConnectionStringBuilder { DataSource = FileName };

            // SQLiteConnectionオブジェクト作成
            m_SqliteConnection = new SQLiteConnection(sqliteConnectionStringBuilder.ToString());

            // テーブルオブジェクト生成
            m_FileInfomationTable = new FileInfomationTable(m_SqliteConnection);
            m_RouteTable = new RouteTable(m_SqliteConnection);
            m_FontTable = new FontTable(m_SqliteConnection);
            m_ColorTable = new ColorTable(m_SqliteConnection);
            m_DiagramScreenTable = new DiagramScreenTable(m_SqliteConnection);
            m_StationTable = new StationTable(m_SqliteConnection);
            m_StationSequenceTable = new StationSequenceTable(m_SqliteConnection);
            m_NextStationTable = new NextStationTable(m_SqliteConnection);
            m_TrainTypeTable = new TrainTypeTable(m_SqliteConnection);
            m_TrainTypeSequenceTable = new TrainTypeSequenceTable(m_SqliteConnection);
            m_CommentTable = new CommentTable(m_SqliteConnection);
            m_DiagramTable = new DiagramTable(m_SqliteConnection);
            m_DiagramSequenceTable = new DiagramSequenceTable(m_SqliteConnection);
            m_TrainTable = new TrainTable(m_SqliteConnection);
            m_TrainSequenceTable = new TrainSequenceTable(m_SqliteConnection);
            m_TrainMarkTable = new TrainMarkTable(m_SqliteConnection);
            m_TrainMarkSequenceTable = new TrainMarkSequenceTable(m_SqliteConnection);
            m_StationTimeTable = new StationTimeTable(m_SqliteConnection);

            // データベース接続
            m_SqliteConnection.Open();

            // ロギング
            Logger.Debug("<<<<= RouteFileDatabase::RouteFileDatabase(string)");
        }
        #endregion

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~RouteFileDatabase()
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileDatabase::~RouteFileDatabase()");

            // リソース破棄
            Dispose(false);

            // ロギング
            Logger.Debug("<<<<= RouteFileDatabase::~RouteFileDatabase()");
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileDatabase::Dispose()");

            // リソース破棄
            Dispose(true);

            // ガベージコレクション
            GC.SuppressFinalize(this);

            // ロギング
            Logger.Debug("<<<<= RouteFileDatabase::Dispose()");
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileDatabase::Dispose(bool)");
            Logger.DebugFormat("disposing:[{0}]", disposing);

            if (!m_Disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources here.
                    m_SqliteConnection?.Dispose();
                    m_SqliteConnection = null;
                }

                // TODO: Free unmanaged resources here.

                // Note disposing has been done.
                m_Disposed = true;
            }

            // ロギング
            Logger.Debug("<<<<= RouteFileDatabase::Dispose(bool)");
        }
        #endregion

        #region 作成
        /// <summary>
        /// 作成
        /// </summary>
        public void Create()
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileDatabase::Create()");

            // 作成
            m_FileInfomationTable.Create();
            m_RouteTable.Create();
            m_FontTable.Create();
            m_ColorTable.Create();
            m_DiagramScreenTable.Create();
            m_StationTable.Create();
            m_StationSequenceTable.Create();
            m_NextStationTable.Create();
            m_TrainTypeTable.Create();
            m_TrainTypeSequenceTable.Create();
            m_CommentTable.Create();
            m_DiagramTable.Create();
            m_DiagramSequenceTable.Create();
            m_TrainTable.Create();
            m_TrainSequenceTable.Create();
            m_TrainMarkTable.Create();
            m_TrainMarkSequenceTable.Create();
            m_StationTimeTable.Create();

            // ロギング
            Logger.Debug("<<<<= RouteFileDatabase::Create()");
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        public void Save(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileDatabase::Save(RouteProperties)");
            Logger.DebugFormat("property:[{0}]", property);

            // 保存
            // トランザクション開始
            using (SQLiteTransaction sqliteTransaction = m_SqliteConnection.BeginTransaction())
            {
                m_FileInfomationTable.Save(property.FileInfo);
                m_RouteTable.Save(property.Route);
                m_FontTable.Save(property.Fonts);
                m_ColorTable.Save(property.Colors);
                m_DiagramScreenTable.Save(property.DiagramScreen);
                m_StationTable.Save(property.Stations);
                m_StationSequenceTable.Save(property.StationSequences);
                m_NextStationTable.Save(property.Stations);
                m_TrainTypeTable.Save(property.TrainTypes);
                m_TrainTypeSequenceTable.Save(property.TrainTypeSequences);
                m_CommentTable.Save(property.Comment);
                m_DiagramTable.Save(property.Diagrams);
                m_DiagramSequenceTable.Save(property.DiagramSequences);
                m_TrainTable.Save(property.Diagrams);
                m_TrainSequenceTable.Save(property.Diagrams);
                m_TrainMarkTable.Save(property.Diagrams);
                m_TrainMarkSequenceTable.Save(property.Diagrams);
                m_StationTimeTable.Save(property.Diagrams);

                // トランザクションコミット
                sqliteTransaction.Commit();
            }

            // ロギング
            Logger.Debug("<<<<= RouteFileDatabase::Save(RouteProperties)");
        }

        /// <summary>
        /// 再構築
        /// </summary>
        /// <param name="property"></param>
        public void Rebuilding(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileDatabase::Rebuilding(RouteProperties)");
            Logger.DebugFormat("property:[{0}]", property);

            // 保存
            // トランザクション開始
            using (SQLiteTransaction sqliteTransaction = m_SqliteConnection.BeginTransaction())
            {
                m_FileInfomationTable.Rebuilding(property.FileInfo);
                m_RouteTable.Rebuilding(property.Route);
                m_FontTable.Rebuilding(property.Fonts);
                m_ColorTable.Rebuilding(property.Colors);
                m_DiagramScreenTable.Rebuilding(property.DiagramScreen);
                m_StationTable.Rebuilding(property.Stations);
                m_StationSequenceTable.Rebuilding(property.StationSequences);
                m_NextStationTable.Rebuilding(property.Stations);
                m_TrainTypeTable.Rebuilding(property.TrainTypes);
                m_TrainTypeSequenceTable.Rebuilding(property.TrainTypeSequences);
                m_CommentTable.Rebuilding(property.Comment);
                m_DiagramTable.Rebuilding(property.Diagrams);
                m_DiagramSequenceTable.Rebuilding(property.DiagramSequences);
                m_TrainTable.Rebuilding(property.Diagrams);
                m_TrainSequenceTable.Rebuilding(property.Diagrams);
                m_TrainMarkTable.Rebuilding(property.Diagrams);
                m_TrainMarkSequenceTable.Rebuilding(property.Diagrams);
                m_StationTimeTable.Rebuilding(property.Diagrams);

                // トランザクションコミット
                sqliteTransaction.Commit();
            }

            // ロギング
            Logger.Debug("<<<<= RouteFileDatabase::Rebuilding(RouteFileProperty, RouteProperties)");
        }
        #endregion

        #region 読込
        /// <summary>
        /// 読込
        /// </summary>
        /// <returns></returns>
        public RouteFileProperty Load()
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileDatabase::Load()");

            // 結果オブジェクト生成
            RouteFileProperty result = new RouteFileProperty();

            // 読込
            result.FileInfo = m_FileInfomationTable.Load();
            result.Route = m_RouteTable.Load();
            result.Fonts = m_FontTable.Load();
            result.Colors = m_ColorTable.Load();
            result.DiagramScreen = m_DiagramScreenTable.Load();
            result.Stations = m_StationTable.Load();
            foreach (var station in result.Stations)
            {
                station.NextStations = m_NextStationTable.Load(station);
            }
            result.StationSequences = m_StationSequenceTable.Load();
            result.TrainTypes = m_TrainTypeTable.Load();
            result.TrainTypeSequences = m_TrainTypeSequenceTable.Load();
            result.Comment = m_CommentTable.Load();
            result.Diagrams = m_DiagramTable.Load();
            foreach (var diagram in result.Diagrams)
            {
                diagram.Trains = m_TrainTable.Load(diagram.Seq - 1);
                diagram.TrainSequence = m_TrainSequenceTable.Load(diagram.Seq - 1);

                foreach (var train in diagram.Trains)
                {
                    m_TrainMarkTable.Load(train);
                    m_TrainMarkSequenceTable.Load(train);
                    m_StationTimeTable.Load(train);
                }
            }
            result.DiagramSequences = m_DiagramSequenceTable.Load();

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= RouteFileDatabase::Load()");

            // 返却
            return result;
        }
        #endregion

        #region 削除
        /// <summary>
        /// 削除
        /// </summary>
        public void Remove()
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileDatabase::Remove()");

            // 削除
            m_FileInfomationTable.Remove();
            m_RouteTable.Remove();
            m_FontTable.Remove();
            m_ColorTable.Remove();
            m_DiagramScreenTable.Remove();
            m_StationTable.Remove();
            m_StationSequenceTable.Remove();
            m_NextStationTable.Remove();
            m_TrainTypeTable.Remove();
            m_TrainTypeSequenceTable.Remove();
            m_CommentTable.Remove();
            m_DiagramTable.Remove();
            m_DiagramSequenceTable.Remove();
            m_TrainTable.Remove();
            m_TrainSequenceTable.Remove();
            m_TrainMarkTable.Remove();
            m_TrainMarkSequenceTable.Remove();
            m_StationTimeTable.Remove();

            // ロギング
            Logger.Debug("<<<<= RouteFileDatabase::Remove()");
        }
        #endregion

        #region インポート
        /// <summary>
        /// インポート
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="fileName"></param>
        public void Import(ImportFileType fileType, string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileDatabase::Import(ImportFileType, string)");
            Logger.DebugFormat("fileType:[{0}({1})]", fileType, fileType.GetStringValue());
            Logger.DebugFormat("fileName:[{0}]", fileName);

            // インポートファイル種別で分岐
            switch (fileType)
            {
                case ImportFileType.WinDIA:
                    // インポート(WinDia)
                    WinDiaImport(fileName);
                    break;
                case ImportFileType.OuDia:
                    // インポート(OuDia)
                    OuDiaImport(fileName);
                    break;
                case ImportFileType.OuDia2:
                    // インポート(OuDia2)
                    OuDia2Import(fileName);
                    break;
                default:
                    // 例外
                    throw new ArgumentException();
            }

            // ロギング
            Logger.Debug("<<<<= RouteFileDatabase::Import(ImportFileType, string)");
        }

        #region インポート(WinDia)
        /// <summary>
        /// WinDiaImport
        /// </summary>
        /// <param name="fileName"></param>
        private void WinDiaImport(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileDatabase::WinDiaImport(string)");
            Logger.DebugFormat("fileName:[{0}]", fileName);

            // WinDiaFileオブジェクト生成
            WinDiaFile diaFile = new WinDiaFile(fileName);

            // 読込
            RouteFileProperty routeProperties = diaFile.Load();

            // ロギング
            Logger.Info("＜インポート情報:[WinDIA]＞\r\n" + routeProperties.ToString());

            // 保存
            Save(routeProperties);

            // ロギング
            Logger.Debug("<<<<= RouteFileDatabase::WinDiaImport(string)");
        }
        #endregion

        #region インポート(OuDia)
        /// <summary>
        /// OuDiaImport
        /// </summary>
        /// <param name="fileName"></param>
        private void OuDiaImport(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileDatabase::OuDiaImport(string)");
            Logger.DebugFormat("fileName:[{0}]", fileName);

            // OudFileオブジェクト生成
            OudiaFile diaFile = new OudiaFile(fileName);

            // 読込
            RouteFileProperty routeProperties = diaFile.Load();

            // ロギング
            Logger.Info("＜インポート情報:[OuDia]＞\r\n" + routeProperties.ToString());

            // 保存
            Save(routeProperties);

            // ロギング
            Logger.Debug("<<<<= RouteFileDatabase::OuDiaImport(string)");
        }
        #endregion

        #region インポート(OuDia2)
        /// <summary>
        /// OuDia2Import
        /// </summary>
        /// <param name="fileName"></param>
        private void OuDia2Import(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileDatabase::OuDia2Import(string)");
            Logger.DebugFormat("fileName:[{0}]", fileName);

            // OudFileオブジェクト生成
            Oudia2File diaFile = new Oudia2File(fileName);

            // 読込
            RouteFileProperty routeProperties = diaFile.Load();

            // ロギング
            Logger.Info("＜インポート情報:[OuDia2]＞\r\n" + routeProperties.ToString());

            // 保存
            Save(routeProperties);

            // ロギング
            Logger.Debug("<<<<= RouteFileDatabase::OuDia2Import(string)");
        }
        #endregion
        #endregion
    }
}
