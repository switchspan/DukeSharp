using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace Duke
{
    /// <summary>
    /// An abstract SQL-based link database implementation which can can
    /// maintain a set of links in an H2 or Oracle database over JDBC. It
    /// could be extended to work with more database implementations. What
    /// the abstract class cannot do is create a connection, which is left
    /// for subclasses to do.
    /// </summary>
    public abstract class RDBMSLinkDatabase : ILinkDatabase
    {
        public enum DatabaseType
        {
           Mssqlserver = 1,
           Oracle = 2,
           MySql = 3
        }

        #region Private member variables

        private DatabaseType _dbtype;

        private static Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Member Properties

        public String TblPrefix { get; set; } // prefix for table names ("foo."); never null
        #endregion

        #region Constructors
        public RDBMSLinkDatabase(string dbType)
        {
            _logger.Debug("Database Type = {0}", dbType);
            _dbtype = GetDatabaseType(dbType);
            TblPrefix = "";

        }
        #endregion

        #region Member methods
        public List<Link> GetChangesSince(long since)
        {
            throw new NotImplementedException();
        }

        public List<Link> GetChangesSince(long since, long before, int pagesize)
        {
            //String where = "";
            //if (since != 0 || before != 0)
            //    where = "where ";
            //if (since != 0)
            //    where += "timestamp > TIMESTAMP '";
            throw new NotImplementedException();
        } 

        public List<Link> GetAllLinks()
        {
            throw new NotImplementedException();
        }

        public List<Link> GetAllLinksFor(string id)
        {
            throw new NotImplementedException();
        }

        public void AssertLink(Link link)
        {
            throw new NotImplementedException();
        }

        public Link InferLink(string id1, string id2)
        {
            throw new NotImplementedException();
        }

        public void ValidateConnection()
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }



        private static DatabaseType GetDatabaseType(string dbtype)
        {
            var theType = dbtype.ToUpper().Trim();
            switch (theType)
            {
                case "MSSQLSERVER":
                    return DatabaseType.Mssqlserver;
                case "ORACLE":
                    return DatabaseType.Oracle;
                case "MYSQL":
                    return DatabaseType.MySql;
                default:
                    throw new DukeConfigException(String.Format("Unknown database type: {0}", dbtype));
            }
        }

       private string GetCreateTable(DatabaseType dbType)
       {
           var sb = new StringBuilder();
           sb.Append("create table LINKS (");
           sb.Append("  id1 varchar not null, ");
           sb.Append("  id2 varchar not null, ");
           sb.Append("  kind int not null, ");
           sb.Append("  status int not null, ");
           if (dbType == DatabaseType.Mssqlserver)
           {
            sb.Append("  timestamp datetime not null, ");
           }
           else
           {
               sb.Append("    timestamp timestamp not null, ");
           }
          
           sb.Append("  primary key (id1, id2)) ");

           return sb.ToString();
       }
        #endregion


    }
}
