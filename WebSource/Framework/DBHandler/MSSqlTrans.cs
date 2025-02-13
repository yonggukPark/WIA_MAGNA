using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace MES.FW.Common.DBHandler
{
    public class MSSqlTrans
    {
        SqlTransaction transaction = null;

        // 생성자
        public MSSqlTrans() { }

        // 트랜젝션 연결
        public SqlTransaction BeginTrans
        {
            set
            {
                transaction = value;
            }
        }

        // 트랜젝션 셋팅
        public SqlTransaction GetTrans
        {
            get
            {
                return transaction;
            }
        }

        // 트랜젝션 성공 후 완료
        public void CommitTrans()
        {
            transaction.Commit();
        }

        // 트랜젝션 실패 후 롤빽
        public void RollBackTrans()
        {
            transaction.Rollback();
        }

        // 객체 소명
        public void Dispose()
        {
            transaction.Dispose();
        }
    }
}
