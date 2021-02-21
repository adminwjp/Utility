//net40 - net48 netcoreapp2.0 - net5.0 netstandard2.0 - netstandard2.1
//#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
#if  NET40 ||NET45 || NET451 || NET452 || NET46 ||NET461 || NET462|| NET47 || NET471 || NET472|| NET48 ||  NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1


#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
#endif
using Utility.Model;

namespace Utility.Ef.DAL.Mapp
{


#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
    /// <summary>ef 基类 </summary>
    public abstract class BaseEfMapp<Model,Key>  : BaseEfMapp<Model> where Model: class,IModel<Key>
    {
        /// <summary>
        /// 
        /// </summary>
        public BaseEfMapp(string tableName):base(tableName)
        {
             //编号
            if (typeof(Key).IsValueType)
            {
                HasKey(it => it.Id);//.HasAnnotation("MySql: ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);
                //Property(it => it.Id).ValueGeneratedOnAdd();
            }
            else
            {  
                HasKey(it => it.Id);
               // Property(it => it.Id).HasMaxLength(36).IsRequired();
            }
        }

    }
     /// <summary>ef 基类 </summary>
    public abstract class BaseEfMapp<Model>  : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Model> where Model : class
    {
        /// <summary>
        /// 
        /// </summary>
        public BaseEfMapp(string tableName)
        {
            ToTable(tableName);
            this.Set();
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract void Set();
    }

#else

       /// <summary>ef 基类 </summary>
    public abstract class BaseEfMapp<Model,Key> : BaseEfMapp<Model> where Model : class,IModel<Key>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Model> builder)
        {
            builder.ToTable(this.TableName);

            //编号
            if (typeof(Key).IsValueType)
            {
                builder.HasKey(it => it.Id);//.HasAnnotation("MySql: ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);
            }
            else
            {
                builder.HasKey(it => it.Id);
                builder.Property(it => it.Id).HasMaxLength(36).IsRequired(false);//.ValueGeneratedOnAdd();
            }
        

            this.Set(builder);

        }

        /// <summary>
        /// 
        /// </summary>
        protected string TableName { get; set; }
    }

    /// <summary>ef 基类 </summary>
    public abstract class BaseEfMapp<Model> : Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<Model> where Model : class
    {

        /// <summary>
        /// 
        /// </summary>
        public virtual void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Model> builder)
        {
            this.Set(builder);

        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract void Set(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Model> builder);
    }

#endif
}
#endif