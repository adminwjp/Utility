
//#if !(NET40 || NET45 || NET451 || NET452 )
//using Autofac.Annotation;
//#endif
//#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
//using Microsoft.EntityFrameworkCore;//net core
//using Microsoft.EntityFrameworkCore.Design;
//#else
//using System.Data.Entity;
//#endif
//using NHibernate.Linq;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Data.Common;
//using System.Diagnostics.CodeAnalysis;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;
//using Utility.Database;
//using Utility.Domain.Entities;
//using Utility.Domain.Repositories;
//using Utility.Domain.Uow;
//using Utility.Ef.Repositories;
//using Utility.Ef.Uow;
//using Utility.Enums;

//namespace Utility.Temaplate
//{
//    /// <summary>
//    /// 基类
//    /// </summary>
//    public abstract class BaseEntity:IEntity<string>
//    {
//        /// <summary>
//        /// 主键  guid nhibernte sqlite 乱码 ef core mysql 乱码 最好 用 string
//        /// </summary>
//        [System.ComponentModel.DataAnnotations.Key]
//        [System.ComponentModel.DataAnnotations.MaxLength(36)]
//        // public Guid Id { get; set; }//mysql 乱码
//       [Required(ErrorMessage ="id is not null")]
      
//        public string Id { get; set; }
//        /// <summary>
//        /// 语言
//        /// </summary>
//        protected Language Language { get; set; } = Language.Chinese;
//        /// <summary>
//        /// 自动 生成 id
//        /// </summary>
//        public BaseEntity()
//        {
//            Id = Guid.NewGuid().ToString();
//        }
//    }


//    /// <summary>
//    /// 生成 的模板 代码
//    /// </summary>
//    public class TemplateCodeEntity: BaseEntity
//    {
//        /// <summary>
//        /// 代码
//        /// </summary>
//        [System.ComponentModel.DataAnnotations.MaxLength(int.MaxValue)]
//        public string Code { get; set; }
//        /// <summary>
//        /// 描述
//        /// </summary>
//        public string Desc { get; set; }
//        /// <summary>
//        /// 标识
//        /// </summary>
//        public Flag Flag { get; set; }
//    }
//    /// <summary>
//    /// 标识
//    /// </summary>
//    public enum Flag
//    {
//        /// <summary>
//        /// elementui
//        /// </summary>
//        ElementUI
//    }

//    /// <summary>
//    /// 数据库 实体
//    /// </summary>
//    public class DbEntity : BaseEntity,IValidatableObject
//    {
//        /// <summary>
//        /// 数据库 实体
//        /// </summary>
//        public DbEntity()
//        {

//        }

//        /// <summary>
//        ///数据库 名称 
//        /// </summary>
//        public string Database { get; set; }
//        /// <summary>
//        /// 项目 名称
//        /// </summary>
//        public string ProgramName { get; set; }
//        /// <summary>
//        /// 表
//        /// </summary>
//        public ICollection<TableEntity> Tables { get; set; }
//        /// <summary>
//        /// 验证 参数
//        /// </summary>
//        /// <param name="validationContext"></param>
//        /// <returns></returns>
//        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
//        {
//            if (!string.IsNullOrEmpty(Id))
//            {
//                yield return new ValidationResult("id is not null ");
//            }

//            if (!string.IsNullOrEmpty(Database))
//            {
//                yield return new ValidationResult("database is not null ");
//            }

//        }
//    }
//    /// <summary>
//    /// 表实体
//    /// </summary>
//    public class TableEntity : BaseEntity
//    {
//        /// <summary>
//        /// 表名
//        /// </summary>
//        [Required(ErrorMessage = "table is not null")]
//        public string Table { get; set; }

//        /// <summary>
//        /// 类名
//        /// </summary>
//        public string ClassName { get; set; }

//        /// <summary>
//        /// 注释
//        /// </summary>
//        public string Comment { get; set; }

//        /// <summary>
//        /// 数据库
//        /// </summary>
//        public DbEntity Db { get; set; }
//       /// <summary>
//       /// 数据库 id
//       /// </summary>
//        public string DbId { get; set; }
//        /// <summary>
//        /// 列
//        /// </summary>
//        public ICollection<ColumnEntity> Columns { get; set; }
//    }

//    /// <summary>
//    /// 列实体
//    /// </summary>
//    public class ColumnEntity : BaseEntity
//    {
       
//        private string columnName;

//        /// <summary>
//        /// 注释
//        /// </summary>

//        public string Comment { get; set; }
//        /// <summary>
//        /// 列名
//        /// </summary>
//        public string ColumnName { get => !string.IsNullOrEmpty(columnName) ? columnName : PropertName ; set => columnName = value; }
//        /// <summary>
//        /// 属性名称
//        /// </summary>
//        public string PropertName { get; set; }
//        /// <summary>
//        /// 属性 类型
//        /// </summary>
//        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
//        public Type PropertyType { get; set; }
//        /// <summary>
//        /// mysql  列 数据类型
//        /// </summary>
//        public string MySqlValue { get; set; }
//        /// <summary>
//        /// sqlserver  列 数据类型
//        /// </summary>
//        public string SqlServerValue { get; set; }
//        /// <summary>
//        /// oracle  列 数据类型
//        /// </summary>
//        public string OracleValue { get; set; }
//        /// <summary>
//        /// sqlite  列 数据类型
//        /// </summary>
//        public string SqliteValue { get; set; }
//        /// <summary>
//        /// postgre  列 数据类型
//        /// </summary>
//        public string PostgreValue { get; set; }
//        /// <summary>
//        /// 注释
//        /// </summary>
//        public string Remark { get; set; }
//        /// <summary>
//        ///   列 默认值
//        /// </summary>
//        public string DefaultValue { get; set; }
//        /// <summary>
//        /// 约束
//        /// </summary>
//        public string CheckValue { get; set; }
//        /// <summary>
//        ///外键 表 id
//        /// </summary>
//        public string TableId { get; set; }
//        /// <summary>
//        /// 外键 列 id
//        /// </summary>
//        public string ReferenceId { get; set; }
//        /// <summary>
//        ///外键 表名
//        /// </summary>
//        public string TableName { get; set; }
//        /// <summary>
//        /// 外键名
//        /// </summary>
//        public string ConstraintName { get; set; }
//        /// <summary>
//        /// 表实体
//        /// </summary>
//        public TableEntity Table { get; set; }
//        /// <summary>
//        /// 长度
//        /// </summary>
//        public long Length { get; set; }
       
//    }

//    /// <summary>
//    /// 命令 更新 数据库 老是 出现问题
//    /// </summary>
//    public class TemplateDbContenxt:DbContext
//    {
//        //public TemplateDbContenxt()
//        //{
//        //    //Database.Migrate();
//        //    //Database.EnsureDeleted();
//        //    //if (Database.EnsureCreated())
//        //    //{

//        //    //    DbEntities.Add(TemplateHelper.DbEntity);
//        //    //    SaveChanges();
//        //    //}
//        //}

//#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
//         public TemplateDbContenxt(DbContextOptions<TemplateDbContenxt> dbContextOptions):base(dbContextOptions)
//        {

//        }
//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//           // optionsBuilder.UseMySql("Database=TemplateTest2020928;Data Source=localhost;User Id=root;Password=wjp930514.;Old Guids=True;charset=utf8;");
//            base.OnConfiguring(optionsBuilder);
//        }
//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            //he entity type 'CustomAttributeData' requires a primary key to be defined. If you intended to use a keyless entity type call 'HasNoKey()'.
//            //modelBuilder.Entity<DbEntity>().HasKey(it => it.Id);
//            //modelBuilder.Entity<TableEntity>().HasKey(it => it.Id);
//            //modelBuilder.Entity<ColumnEntity>().HasKey(it => it.Id);
//            base.OnModelCreating(modelBuilder);
//        }
//#endif
//        public DbSet<DbEntity> DbEntities { get; set; }
//        public DbSet<TableEntity> TableEntities { get; set; }
//        public DbSet<ColumnEntity> ColumnEntities { get; set; }

//    }

//#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
//    //public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TemplateDbContenxt>
//    //{
//    //    public TemplateDbContenxt CreateDbContext(string[] args)
//    //    {
//    //        return new TemplateDbContenxt();
//    //    }
//    //}
//#endif

//    public class TemplateHelper {
//        //At least one object must implement IComparable. Collections.Array
//        public static DbEntity DbEntity = new DbEntity() { Database = "Template", Tables = new List<TableEntity>() };
//        static TemplateHelper()
//        {
//            InitDb();
//            InitTable();
//            InitColumn();
//        }
//        private static void InitColumn()
//        {
//            TableEntity tableEntity = new TableEntity() { Table = "ColumnEntity", Columns = new List<ColumnEntity>() };
//            DbEntity.Tables.Add(tableEntity);
//            ColumnEntity columnEntity = new ColumnEntity() { PropertName = "Id", ColumnName = "Id", Remark = "列Id", PropertyType = typeof(Guid) };
//            tableEntity.Columns.Add(columnEntity);
//            columnEntity = new ColumnEntity() { PropertName = "ColumnName", ColumnName = "ColumnName", Remark = "列名", PropertyType = typeof(string) };
//            tableEntity.Columns.Add(columnEntity);
//            columnEntity = new ColumnEntity() { PropertName = "PropertName", ColumnName = "PropertName", Remark = "属性名", PropertyType = typeof(string) };
//            tableEntity.Columns.Add(columnEntity);
//            columnEntity = new ColumnEntity() { PropertName = "PropertyType", ColumnName = "PropertyType", Remark = "属性类型", PropertyType = typeof(string) };
//            tableEntity.Columns.Add(columnEntity);
//            columnEntity = new ColumnEntity() { PropertName = "MySqlValue", ColumnName = "MySqlValue", Remark = "mysql列数据类型", PropertyType = typeof(string) };
//            tableEntity.Columns.Add(columnEntity);
//            columnEntity = new ColumnEntity() { PropertName = "SqlServerValue", ColumnName = "SqlServerValue", Remark = "sqlserver列数据类型", PropertyType = typeof(string) };
//            tableEntity.Columns.Add(columnEntity);
//            columnEntity = new ColumnEntity() { PropertName = "OracleValue", ColumnName = "OracleValue", Remark = "oracle列数据类型", PropertyType = typeof(string) };
//            tableEntity.Columns.Add(columnEntity);
//            columnEntity = new ColumnEntity() { PropertName = "SqliteValue", ColumnName = "SqliteValue", Remark = "sqlite列数据类型", PropertyType = typeof(string) };
//            tableEntity.Columns.Add(columnEntity);
//            columnEntity = new ColumnEntity() { PropertName = "PostgreValue", ColumnName = "PostgreValue", Remark = "postgre列数据类型", PropertyType = typeof(string) };
//            tableEntity.Columns.Add(columnEntity);
//            columnEntity = new ColumnEntity() { PropertName = "Remark", ColumnName = "Remark", Remark = "备注", PropertyType = typeof(string) };
//            tableEntity.Columns.Add(columnEntity);
//            columnEntity = new ColumnEntity() { PropertName = "DefaultValue", ColumnName = "DefaultValue", Remark = "列默认值", PropertyType = typeof(string) };
//            tableEntity.Columns.Add(columnEntity);
//            columnEntity = new ColumnEntity() { PropertName = "CheckValue", ColumnName = "CheckValue", Remark = "列Check约束", PropertyType = typeof(string) };
//            tableEntity.Columns.Add(columnEntity);
//            columnEntity = new ColumnEntity() { PropertName = "CheckValue", ColumnName = "CheckValue", Remark = "列Check约束", PropertyType = typeof(string) };
//            tableEntity.Columns.Add(columnEntity);
//            columnEntity = new ColumnEntity() { PropertName = "TableId", ColumnName = "TableId", Remark = "实体模型id", PropertyType = typeof(Guid) };
//            tableEntity.Columns.Add(columnEntity);
//            columnEntity = new ColumnEntity() { PropertName = "ReferenceId", ColumnName = "ReferenceId", Remark = "外键表主键id列名", PropertyType = typeof(Guid) };
//            tableEntity.Columns.Add(columnEntity);
//            columnEntity = new ColumnEntity() { PropertName = "TableName", ColumnName = "TableName", Remark = "外键表名", PropertyType = typeof(Guid) };
//            tableEntity.Columns.Add(columnEntity);
//            columnEntity = new ColumnEntity() { PropertName = "ConstraintName", ColumnName = "ConstraintName", Remark = "外键名", PropertyType = typeof(Guid) };
//            tableEntity.Columns.Add(columnEntity);
//        }

//        private static void InitDb()
//        {
//            TableEntity tableEntity = new TableEntity() { Table = "DbEntity", Columns = new List<ColumnEntity>() };
//            DbEntity.Tables.Add(tableEntity);
//            ColumnEntity columnEntity = new ColumnEntity() { PropertName = "Id", ColumnName = "Id", Remark = "数据库Id", PropertyType = typeof(Guid) };
//            tableEntity.Columns.Add(columnEntity);
//            columnEntity = new ColumnEntity() { PropertName = "Database", ColumnName = "Database", Remark = "数据库名称", PropertyType = typeof(string) };
//            tableEntity.Columns.Add(columnEntity);
//            columnEntity = new ColumnEntity() { PropertName = "ProgramName", ColumnName = "ProgramName", Remark = "项目名称", PropertyType = typeof(string) };
//            tableEntity.Columns.Add(columnEntity);
//        }

//        private static void InitTable()
//        {
//            TableEntity tableEntity = new TableEntity() { Table = "TableEntity", Columns = new List<ColumnEntity>() };
//            DbEntity.Tables.Add(tableEntity);
//            ColumnEntity columnEntity = new ColumnEntity() { PropertName = "Id", ColumnName = "Id", Remark = "表Id", PropertyType = typeof(Guid) };
//            tableEntity.Columns.Add(columnEntity);
//            columnEntity = new ColumnEntity() { PropertName = "Table", ColumnName = "Table", Remark = "表名", PropertyType = typeof(string) };
//            tableEntity.Columns.Add(columnEntity);
//            columnEntity = new ColumnEntity() { PropertName = "ClassName", ColumnName = "ClassName", Remark = "实体类名", PropertyType = typeof(string) };
//            tableEntity.Columns.Add(columnEntity);
//            columnEntity = new ColumnEntity() { PropertName = "DbId", ColumnName = "DbId", Remark = "项目id", PropertyType = typeof(Guid) };
//            tableEntity.Columns.Add(columnEntity);
//        }
//    }
//#if !(NET40 || NET45 || NET451 || NET452)
//    [Component(typeof(TemplateService), AutofacScope = AutofacScope.InstancePerLifetimeScope)]
//#endif

//    public class TemplateService:EfUnitWork
//    {
//        protected TemplateDbContenxt TemplateDbContenxt;
//        public TemplateService(TemplateDbContenxt contenxt):base(contenxt)
//        {
//            TemplateDbContenxt = contenxt;
//        }
//        public virtual List<StringEntity> GetDbs()
//        {
//            return base.Find<DbEntity>().Select(it => new StringEntity() { Key = it.Id, Value = it.Database }).ToList();
//        }
//        public virtual List<StringEntity> GetTables()
//        {
//            return base.Find <TableEntity>().Select(it => new StringEntity() { Key = it.Id, Value = it.Table }).ToList();
//        }


//        public virtual DbEntity LoadColumns(string connectionString,SqlType dialect,string programName)
//        {
//            DbConnection connection = AbstractDatabaseProvider.CreateDatabaseTypeFactory(dialect).CreateConnection(connectionString);
//            if (base.IsExist<DbEntity>(it=>it.Database== connection.Database))
//            {
//#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
//                return base.Find<DbEntity>(null).Include(it => it.Tables).ThenInclude(it => it.Columns).FirstOrDefault(it => it.Database == connection.Database);
//#else
//                var result = from db in TemplateDbContenxt.DbEntities

//                          join table in TemplateDbContenxt.TableEntities
//                           on db.Id equals table.DbId into tables
//                          from temp in tables.DefaultIfEmpty() select new { Db = db, Table = temp } into res

//                          join column in TemplateDbContenxt.ColumnEntities
//                           on res.Table.Id equals column.TableId into columns
//                          from temp in columns.DefaultIfEmpty() select new { Db = res.Db, Table = res.Table, Column = temp }
//                               into col select col;
//                      /*  from db in TemplateDbContenxt.DbEntities where db.Database== connection.Database && db.Id==col.Db.Id
//                       *  
//                        select new { col,db};*/
//                return null;
//#endif
//            }
     
//            var tableEntries=Database.DbHelper.FindTableByDatabase(connection, connection.Database, dialect);
//            DbEntity dbEntity = new DbEntity() { Tables= new List<TableEntity>(),ProgramName=programName,Database=connection.Database,Id=Guid.NewGuid().ToString("N")};
//            foreach (ClassEntity tableEntry in tableEntries)
//            {
//                TableEntity tableEntity = new TableEntity() { Table=tableEntry.Table, Id = Guid.NewGuid().ToString("N"),Comment=tableEntry.Comment ,Columns=new List<ColumnEntity>()};
//                dbEntity.Tables.Add(tableEntity);
//                foreach (PropertyEntity columnEntry in tableEntry.PropertyEntities)
//                {
//                    if (!columnEntry.Valid())
//                    {
//                        continue;
//                    }
//                    ColumnEntity columnEntity = new ColumnEntity() { 
//                        Id = Guid.NewGuid().ToString("N"),
//                        ColumnName=columnEntry.Column,
//                        Comment= columnEntry.Comment,
//                        MySqlValue=columnEntry.DataType,
//                        DefaultValue=columnEntry.Default,
//                        CheckValue=columnEntry.Check,
//                    };
//                    tableEntity.Columns.Add(columnEntity);
//                    if(columnEntry.Flag== ColumnFlag.ForeignKey)
//                    {
//                        columnEntity.TableName = columnEntry.ForeignKeyColumnEntity.ReferenceTable;
//                        columnEntity.TableId = columnEntry.ForeignKeyColumnEntity.ReferenceId;
//                        columnEntity.ConstraintName = columnEntry.ForeignKeyColumnEntity.Constraint;
//                    }
//                }
//            }
//            base.Insert(dbEntity);
//            return dbEntity;
//        }
//        public virtual List<ColumnEntity> GetColumns()
//        {
//             return base.Filter<ColumnEntity>(null).Include(it => it.Table).Include(it => it.Table.Db).ToList();
//        }
//        //c# 8.0
//        //protected override IQueryable<T> Filter<T>(Expression<Func<T, bool>> where=null) where T:class
//        //{
//        //    if (typeof(T)==typeof(ColumnEntity))
//        //    //if (T is ColumnEntity)//语法不支持
//        //    //if (where is Expression<Func<ColumnEntity, bool>> newWhere)//null 不会执行
//        //    {
//        //        var newWhere = where as Expression<Func<ColumnEntity, bool>>;
//        //        //要么根据该条件查询所有
//        //        return (IQueryable<T>)base.Filter(newWhere).Include(it => it.Table).Include(it => it.Table.Db);
//        //    }
//        //    return base.Filter(where);
//        //}

//        public virtual List<DbEntity> GetDbEntities()
//        {
//            List<DbEntity> dbEntities = new List<DbEntity>();
//            //方案 1  未完成
//            //List<ColumnEntity> columnEntities = GetColumns();
//            //foreach (var item in columnEntities)
//            //{
//            //    if (item.Table != null && item.Table.Db != null)
//            //    {  
//            //        //前提 如果不是同一个引用 这样可行 ,不然清空了 后面不好整合 
//            //        var db = new DbEntity() {Id= item.Table.Db.Id,Database= item.Table.Db.Database,ProgramName= item.Table.Db.ProgramName };//item.Table.Db
//            //        var table = item.Table;
//            //        db.Tables = db.Tables?? new List<TableEntity>();
//            //        db.Tables.Add(table);
//            //        table.Db = null;
//            //        table.Columns = table.Columns ?? new List<ColumnEntity>();
//            //        table.Columns.Add(item);
//            //        item.Table = null;
//            //        dbEntities.Add(db);
//            //    }
//            //}
//            //方案2 左连接查询(结果跟方案1 结果不同(但都关联外键在,实际上结果相同))
            
//            TemplateDbContenxt templateDbContenxt = context as TemplateDbContenxt;
//            //new object()字段多了不想多写  返回 SelectMany 不能返回集合 不然出错几率很大

//            var data = templateDbContenxt.DbEntities.GroupJoin(
//                templateDbContenxt.TableEntities.GroupJoin(templateDbContenxt.ColumnEntities, it => it.Id, it => it.TableId, (Table, Column) => new { Table, Column })
//                  .SelectMany(TableOrColumn => TableOrColumn.Column, (TableOrColumn, Column) => new ColumnEntity
//                  {
//                      ColumnName = Column.ColumnName,
//                      PropertName = Column.PropertName,
//                      MySqlValue = Column.MySqlValue,
//                      SqlServerValue = Column.SqlServerValue,
//                      OracleValue = Column.OracleValue,
//                      SqliteValue = Column.SqliteValue,
//                      PostgreValue = Column.PostgreValue,
//                      Remark = Column.Remark,
//                      DefaultValue = Column.DefaultValue,
//                      CheckValue = Column.CheckValue,
//                      TableId = Column.TableId,
//                      ReferenceId = Column.ReferenceId,
//                      TableName = Column.TableName,
//                      ConstraintName = Column.ConstraintName,
//                      Table = TableOrColumn.Table,
//                  }),
//                    //templateDbContenxt.TableEntities.Include(it => it.Columns),//这样可行 (怎么只查询一条数据)
//                    // templateDbContenxt.TableEntities,//这样可行

//                    it => it.Id, it => it.Table.DbId,
//                    (Db, Columns) => new { Db, Columns }
//                )
//                .SelectMany(it => it.Columns.DefaultIfEmpty(), (DbOrColumns, Column) => new { DbOrColumns.Db, Column.Table, Column })
//                .ToList();

//            //多个左连接支持 这种写法 . 
//            //这种写法数据都关联好了
//            //var data = from db in templateDbContenxt.DbEntities
//            //           join table in templateDbContenxt.TableEntities
//            //           on db.Id equals table.DbId into tables
//            //           from t in tables.DefaultIfEmpty()
//            //           join Column in templateDbContenxt.ColumnEntities
//            //           on t.Id equals Column.TableId into columns
//            //           from c in columns.DefaultIfEmpty()
//            //            //数据都关联好了(可能是相互引用)
//            //            //select new
//            //            //{
//            //            //    Db = db,
//            //            //    Table= t,
//            //            //    Column =c
//            //            //};
//            //            //(去引用不然 api json输出时 又得遍历去引用)
//            //           select new
//            //           {
//            //               Db = new DbEntity() { Id = t.Id, ProgramName = db.ProgramName, Database = db.Database},//db,
//            //               Table = new TableEntity() { Id=t.Id,ClassName=t.ClassName,Table=t.Table,DbId=t.DbId},//t,
//            //               Column =//c
//            //                 new ColumnEntity
//            //                 {
//            //                     ColumnName = c.ColumnName,
//            //                     PropertName = c.PropertName,
//            //                     MySqlValue = c.MySqlValue,
//            //                     SqlServerValue = c.SqlServerValue,
//            //                     OracleValue = c.OracleValue,
//            //                     SqliteValue = c.SqliteValue,
//            //                     PostgreValue = c.PostgreValue,
//            //                     Remark = c.Remark,
//            //                     DefaultValue = c.DefaultValue,
//            //                     CheckValue = c.CheckValue,
//            //                     TableId = c.TableId,
//            //                     ReferenceId = c.ReferenceId,
//            //                     TableName = c.TableName,
//            //                     ConstraintName = c.ConstraintName,
//            //                 }
//            //           };


//            //整理数据
//            foreach (var item in data.ToList())
//            {
//                DbEntity dbEntity = item.Db;
//                int index = Collections.CollectionHelper.FindIndex(dbEntities.ToArray(), dbEntity, dbEntities.Count, DbEqualityCompare.Empty);
//                if (index> -1)
//                {
//                    dbEntity = dbEntities[index];
//                }
//                else
//                {
//                    dbEntities.Add(dbEntity);
//                }
//                dbEntity.Tables = dbEntity.Tables ?? new List<TableEntity>();
//                index = Collections.CollectionHelper.FindIndex(dbEntity.Tables.ToArray(), item.Table, dbEntity.Tables.Count, TableEqualityCompare.Empty);
//                TableEntity tableEntity=null;
//                if (index > -1)
//                {
//                    if(dbEntity.Tables is List<TableEntity>)
//                    {
//                        tableEntity = ((List<TableEntity>)dbEntity.Tables)[index];
//                    }
//                    //else  if (dbEntity.Tables is HashSet<TableEntity> tables)
//                    else if (dbEntity.Tables is ICollection<TableEntity> tables)
//                    {
//                        using (var iterator = tables.GetEnumerator())
//                        {
//                            int i = 0;
//                            while (iterator.MoveNext())
//                            {
//                                if (i == index)
//                                {
//                                    tableEntity = iterator.Current;
//                                    break;
//                                }
//                                i++;
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    tableEntity = item.Table;
//                    dbEntity.Tables.Add(item.Table);
//                }
//                tableEntity.Columns = tableEntity.Columns ?? new List<ColumnEntity>();
//                tableEntity.Columns.Add(item.Column);
//            }
//            return dbEntities;
//        }


//        //public virtual void GeneratorCode()
//        //{
//        //    var data= base.Filter<ColumnEntity>(null).ToList();
//        //}

//        private class DbEqualityCompare : IEqualityComparer<DbEntity>
//        {
//            public static readonly DbEqualityCompare Empty = new DbEqualityCompare();
//            public bool Equals(DbEntity x,DbEntity y)
//            {
//                return x != null && y != null && x.Id == y.Id;
//            }

//            public int GetHashCode(DbEntity obj)
//            {
//                return obj != null ? obj.Id.GetHashCode() : 0;
//            }
//        }

//        private class TableEqualityCompare : IEqualityComparer<TableEntity>
//        {
//            public static readonly TableEqualityCompare Empty = new TableEqualityCompare();
//            public bool Equals(TableEntity x,  TableEntity y)
//            {
//                return x != null && y != null && x.Id == y.Id;
//            }

//            public int GetHashCode( TableEntity obj)
//            {
//                return obj != null ? obj.Id.GetHashCode() : 0;
//            }
//        }
//    }

//}
