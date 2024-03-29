﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Common;
using TrainTimeTable.Database.Table.Core;
using TrainTimeTable.Property;

namespace TrainTimeTable.Database.Table
{
    /// <summary>
    /// TrainTypeTableクラス
    /// </summary>
    public class TrainTypeTable : TableListCore<TrainTypeProperties, TrainTypeProperty>
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connection"></param>
        public TrainTypeTable(SQLiteConnection connection)
            : base("TrainType", connection)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeTable::TrainTypeTable(SQLiteConnection)");
            Logger.DebugFormat("connection:[{0}]", connection);

            // ロギング
            Logger.Debug("<<<<= TrainTypeTable::TrainTypeTable(SQLiteConnection)");
        }
        #endregion

        #region 作成
        /// <summary>
        /// 作成
        /// </summary>
        public void Create()
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeTable::Create()");

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("CREATE TABLE IF NOT EXISTS {0} (", m_TableName));
            query.Append("Name TEXT NOT NULL,");                                        // 種別名
            query.Append("Abbreviation TEXT,");                                         // 略称
            query.Append("StringsColor TEXT NOT NULL DEFAULT '#000000',");              // 文字色(ColorTranslator.FromHtml)
            query.Append("TimetableFontName TEXT NOT NULL DEFAULT '時刻表ビュー 1',");  // 時刻表Font名
            query.Append("DiagramLineColor TEXT NOT NULL DEFAULT '#000000',");          // ダイヤグラム線色(ColorTranslator.FromHtml)
            query.Append("DiagramLineStyle INTEGER NOT NULL DEFAULT 1,");               // ダイヤグラム線スタイル
            query.Append("DiagramLineBold TEXT NOT NULL DEFAULT 'False',");             // ダイヤグラム線スタイル(太線)
            query.Append("StopStationClearlyIndicated INTEGER NOT NULL DEFAULT 2,");    // 停車駅明示
            query.Append("created TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("updated TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("deleted TIMESTAMP,");
            query.Append("PRIMARY KEY(Name));");

            // 作成
            Create(query.ToString());

            // ロギング
            Logger.Debug("<<<<= TrainTypeTable::Create()");
        }
        #endregion

        #region SELECTデータ登録
        /// <summary>
        /// SELECTデータ登録
        /// </summary>
        /// <param name="sqliteDataReader"></param>
        /// <param name="result"></param>
        protected override void SelectDataRegston(SQLiteDataReader sqliteDataReader, ref TrainTypeProperties result)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeTable::SelectDataRegston(SQLiteDataReader, TrainTypeProperties)");
            Logger.DebugFormat("sqliteDataReader:[{0}]", sqliteDataReader);
            Logger.DebugFormat("result          :[{0}]", result);

            // TrainTypePropertyオブジェクト生成
            TrainTypeProperty property = new TrainTypeProperty();

            // 設定
            property.Name = sqliteDataReader["Name"].ToString();
            property.Abbreviation = sqliteDataReader["Abbreviation"].ToString();
            property.StringsColor = ColorTranslator.FromHtml(sqliteDataReader["StringsColor"].ToString());
            property.TimetableFontName = sqliteDataReader["TimetableFontName"].ToString();
            property.DiagramLineColor = ColorTranslator.FromHtml(sqliteDataReader["DiagramLineColor"].ToString());
            property.DiagramLineStyle = (DashStyle)int.Parse(sqliteDataReader["DiagramLineStyle"].ToString());
            property.DiagramLineBold = bool.Parse(sqliteDataReader["DiagramLineBold"].ToString());
            property.StopStationClearlyIndicated = (StopMarkDrawType)int.Parse(sqliteDataReader["StopStationClearlyIndicated"].ToString());

            // 登録
            result.Add(property);

            // ロギング
            Logger.Debug("<<<<= TrainTypeTable::SelectDataRegston(SQLiteDataReader, TrainTypeProperties)");
        }
        #endregion

        #region 削除キー取得
        /// <summary>
        /// 削除キー取得
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        protected override TrainTypeProperties GetRemoveKeys(TrainTypeProperties srcProperties, TrainTypeProperties dstProperties)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeTable::GetRemoveKeys(TrainTypeProperties, TrainTypeProperties)");
            Logger.DebugFormat("srcProperties:[{0}]", srcProperties);
            Logger.DebugFormat("dstProperties:[{0}]", dstProperties);

            // 結果オブジェクト生成
            TrainTypeProperties result = new TrainTypeProperties();

            // 削除要素作成
            foreach (var src in srcProperties)
            {
                // 削除されたか判定する
                bool removeId = true;
                foreach (var dst in dstProperties)
                {
                    // キーを比較
                    if (src.Name == dst.Name)
                    {
                        removeId = false;
                        break;
                    }
                }

                // 削除対象判定
                if (removeId)
                {
                    // 登録
                    result.Add(src);
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= TrainTypeTable::GetRemoveKeys(TrainTypeProperties, TrainTypeProperties)");

            // 返却
            return result;
        }
        #endregion

        #region 存在判定
        /// <summary>
        /// 存在判定
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected override bool Exist(TrainTypeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeTable::Exist(TrainTypeProperties)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE ", m_TableName));
            query.Append("Name = '" + property.Name + "';");

            // 存在判定
            bool result = Exist(query.ToString());

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= TrainTypeTable::Exist(TrainTypeProperties)");

            // 返却
            return result;
        }
        #endregion

        #region 挿入
        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="property"></param>
        protected override void Insert(TrainTypeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeTable::Insert(TrainTypeProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("INSERT INTO {0} ", m_TableName));
            query.Append("(");
            query.Append("Name,");                          // 種別名
            query.Append("Abbreviation,");                  // 略称
            query.Append("StringsColor,");                  // 文字色(ColorTranslator.FromHtml)
            query.Append("TimetableFontName,");             // 時刻表Font名
            query.Append("DiagramLineColor,");              // ダイヤグラム線色(ColorTranslator.FromHtml)
            query.Append("DiagramLineStyle,");              // ダイヤグラム線スタイル
            query.Append("DiagramLineBold,");               // ダイヤグラム線スタイル(太線)
            query.Append("StopStationClearlyIndicated");    // 停車駅明示
            query.Append(") VALUES ");
            query.Append("(");
            query.Append("'" + property.Name + "',");
            query.Append("'" + property.Abbreviation + "',");
            query.Append("'" + string.Format("#{0:X2}{1:X2}{2:X2}", property.StringsColor.R, property.StringsColor.G, property.StringsColor.B) + "',");
            query.Append("'" + property.TimetableFontName + "',");
            query.Append("'" + string.Format("#{0:X2}{1:X2}{2:X2}", property.DiagramLineColor.R, property.DiagramLineColor.G, property.DiagramLineColor.B) + "',");
            query.Append((int)property.DiagramLineStyle + ",");
            query.Append("'" + property.DiagramLineBold.ToString() + "',");
            query.Append((int)property.StopStationClearlyIndicated);
            query.Append(");");

            // 挿入
            Insert(query.ToString());

            // ロギング
            Logger.Debug("<<<<= TrainTypeTable::Insert(TrainTypeProperty)");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        protected override void Update(TrainTypeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeTable::Update(TrainTypeProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("UPDATE {0} SET ", m_TableName));
            query.Append("Abbreviation = '" + property.Abbreviation + "',");
            query.Append("StringsColor = '" + string.Format("#{0:X2}{1:X2}{2:X2}", property.StringsColor.R, property.StringsColor.G, property.StringsColor.B) + "',");
            query.Append("TimetableFontName = '" + property.TimetableFontName + "',");
            query.Append("DiagramLineColor = '" + string.Format("#{0:X2}{1:X2}{2:X2}", property.DiagramLineColor.R, property.DiagramLineColor.G, property.DiagramLineColor.B) + "',");
            query.Append("DiagramLineStyle = " + (int)property.DiagramLineStyle + ",");
            query.Append("DiagramLineBold = '" + property.DiagramLineBold.ToString() + "',");
            query.Append("StopStationClearlyIndicated = " + (int)property.StopStationClearlyIndicated + ",");
            query.Append("updated = '" + GetCurrentDateTime() + "' ");
            query.Append("WHERE Name = '" + property.Name + "';");

            // 更新
            Update(query.ToString());

            // ロギング
            Logger.Debug("<<<<= TrainTypeTable::Update(TrainTypeProperty)");
        }
        #endregion

        #region 削除
        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="removeKeys"></param>
        protected override void Remove(TrainTypeProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeTable::Rebuilding(TrainTypeProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 件数判定
            if (properties.Count > 0)
            {
                // SQLクエリ
                StringBuilder query = new StringBuilder();

                // 削除対象プロパティ分繰り返す
                foreach (var property in properties)
                {
                    query.Append(string.Format("DELETE FROM {0} ", m_TableName));
                    query.Append("WHERE Name = '" + property.Name + "';");
                }

                // 削除実行
                Remove(query.ToString());
            }

            // ロギング
            Logger.Debug("<<<<= TrainTypeTable::Rebuilding(TrainTypeProperties)");
        }
        #endregion
    }
}
