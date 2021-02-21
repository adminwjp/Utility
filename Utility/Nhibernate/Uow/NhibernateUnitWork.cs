#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET40 ||NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NET481 || NET482 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NETCOREAPP3_2 || NETSTANDARD2_0 || NETSTANDARD2_1
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using Utility.Application.Services.Dtos;
using Utility.Domain.Entities;
using Utility.Domain.Extensions;
using Utility.Domain.Uow;
using Utility.Threads;

namespace Utility.Nhibernate.Uow
{
    /// <summary>
    /// ��� ���߳�ÿ��ִ�� Save��ôȷ���ύ (��ô׷��ÿ�ε�����)
    /// </summary>
    public class NhibernateUnitWork : IUnitWork
    {
        /// <summary>
        /// 
        /// </summary>
        public  NHibernate.ISession Session;

        /// <summary>
        /// NhibernateTemplate
        /// </summary>
        protected NhibernateTemplate Template;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public NhibernateUnitWork(NHibernate.ISession session)
        {
            this.Session = session;
            this.Connection = session.Connection;
            this.Template = NhibernateTemplate.Empty;
            this.Thorw = false;
        }

        //public NhibernateUnitWork(AppSessionFactory sessionFactory) : base(sessionFactory)
        //{
        //    this.Connection = sessionFactory.OpenSession().Connection;
        //}
        /// <summary>δʵ�����׳��쳣���ǲ����κβ��� </summary>
        public bool Thorw { get; }

        /// <summary>���ݿ����Ӷ��� </summary>
        public IDbConnection Connection { get; private set; }


        /// <summary>
        /// �ֶ���ֵ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="flag">1 add 2 update 3 delete</param>
        protected virtual bool UpdateValue<T>(T entity, int flag = 1) where T : class
        {
            return entity.UpdateValue(flag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public virtual ResultDto<T> FindResultDtoByPage<T>(Expression<Func<T, bool>> exp = null,int pageindex = 1, int pagesize = 10) where T : class
        {
            var tuple = FindResultByPage(exp,pageindex,pagesize);
            return new ResultDto<T>(tuple, pageindex, pagesize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public virtual Tuple<List<T>,int> FindResultByPage<T>(Expression<Func<T, bool>> exp = null, int pageindex = 1, int pagesize = 10) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                return Template.FindTupleByPage(Session,exp,pageindex,pagesize);
            }
        }
        /// <summary>����������ѯʵ�������ݼ�������Ϣ</summary>
        /// <param name="where">����</param>
        ///<return>����ʵ�������ݼ�������Ϣ</return>

        public virtual int Count<T>(ICriteria where=null) where T : class
        {
            return Template.Count<T>(Session, where);
        }

        /// <summary>����������ѯʵ�������ݼ���Ϣ</summary>
        /// <param name="where">����</param>
        ///<return>����ʵ�������ݼ���Ϣ</return>
        public virtual List<T> FindList<T>(ICriteria where = null) where T : class
        {
            return Template.FindList<T>(Session, where);
        }

        /// <summary>������������ҳ��ѯʵ�������ݼ���Ϣ</summary>
        /// <param name="where">����</param>
        /// <param name="page">ҳ��</param>
        /// <param name="size">ÿҳ��¼</param>
        ///<return>����ʵ�������ݼ���Ϣ</return>

        public virtual List<T> FindListByPage<T>(ICriteria where = null,int page=1, int size=10) where T : class
        {
            return Template.FindListByPage<T>(Session, where, page, size);
        }

        /// <summary>������������ҳ��ѯʵ�������ݼ���Ϣ��ʵ�������ݼ�������Ϣ</summary>
        /// <param name="where">����</param>
        /// <param name="page">ҳ��</param>
        /// <param name="size">ÿҳ��¼</param>
        ///<return>����ʵ�������ݼ���Ϣ��ʵ�������ݼ�������Ϣ</return>
        public virtual ResultDto<T> FindResultDtoByPage<T>(ICriteria where = null, int page = 1, int size = 10) where T : class
        {
            var res = FindResultByPage<T>(where,page, size);
            return new ResultDto<T>(res.Item1,res.Item2,page,size);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public virtual Tuple<List<T>,int> FindResultByPage<T>(ICriteria where = null,int page=1, int size=10) where T : class
        {
            return Template.FindTupleByPage<T>(Session, where, page, size);
        }

#region async



        /// <summary>����������ѯʵ�������ݼ���Ϣ</summary>
        /// <param name="where">����</param>
        /// <param name="cancellationToken"></param>
        ///<return>����ʵ�������ݼ���Ϣ</return>

        public virtual  Task<List<T>> FindListAsync<T>(ICriteria where = null,
            CancellationToken cancellationToken = default) where T : class
        {
            return Template.FindListAsync<T>(Session,where, cancellationToken);
        }

        /// <summary>������������ҳ��ѯʵ�������ݼ���Ϣ</summary>
        /// <param name="where">����</param>
        /// <param name="page">ҳ��</param>
        /// <param name="size">ÿҳ��¼</param>
        /// <param name="cancellationToken"></param>
        ///<return>����ʵ�������ݼ���Ϣ</return>
        public virtual  Task<List<T>> FindListByPageAsync<T>(ICriteria where = null, int page=1, int size=10,
             CancellationToken cancellationToken = default) where T : class
        {
            return Template.FindListByPageAsync<T>(Session, where, page, size, cancellationToken);
        }
        /// <summary>������������ҳ��ѯʵ�������ݼ���Ϣ��ʵ�������ݼ�������Ϣ</summary>
        /// <param name="where">����</param>
        /// <param name="page">ҳ��</param>
        /// <param name="size">ÿҳ��¼</param>
        /// <param name="cancellationToken"></param>
        ///<return>����ʵ�������ݼ���Ϣ��ʵ�������ݼ�������Ϣ</return>
        public virtual  Task<Tuple<List<T>,int>> FindTupleByPageAsync<T>(ICriteria where = null, int page=1,
            int size=10, CancellationToken cancellationToken = default) where T : class
        {
            return Template.FindTupleByPageAsync<T>(Session, where,page,size,cancellationToken);
        }
#endregion async


        /// <summary>���ҵ������Ҳ��������������� ef,nhibernate ֧�� linq dapper ��֧��linq </summary>
        /// <param name="where">����</param>
        /// <returns></returns>
        public virtual T FindSingle<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
			return Template.FindSingle(Session,where);
        }

        /// <summary> �Ƿ���� Ĭ��ʵ�� ef,nhibernate ֧�� linq dapper ��֧��linq  </summary>
        /// <param name="where">����</param>
        /// <returns></returns>
        public bool IsExist<T>(Expression<Func<T, bool>> where) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                return Template.Count<T>(Session, where) >= 1;
            }
        }

        /// <summary> ��ѯ���� Ĭ��ʵ�� ef,nhibernate ֧�� linq dapper ��֧��linq dapperĬ�ϲ�ѯ���н���������ڴ� ������ѯ</summary>
        /// <param name="where">����</param>
        /// <returns></returns>
        public virtual IQueryable<T> Find<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
			return Template.Find(Session, where);
        }

        /// <summary> ��ѯ���� Ĭ��ʵ�� ef,nhibernate ֧�� linq dapper ��֧��linq orderby������Ч dapperĬ�ϲ�ѯ���н���������ڴ� ������ѯ </summary>
        /// <param name="where">����</param>
        /// <param name="page">ҳ��</param>
        /// <param name="size">��¼</param>
        /// <returns></returns>
        public virtual IQueryable<T> FindByPage<T>(Expression<Func<T, bool>> where = null,int page = 1, int size = 10) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
			return Template.FindByPage(Session, where, page, size);
        }

        /// <summary>��ѯ����  Ĭ��ʵ�� ef,nhibernate ֧�� linq dapper ��֧��linq  dapperĬ�ϲ�ѯ���н������  </summary>
        /// <param name="wehre">����</param>
        /// <returns></returns>
        public virtual int Count<T>(Expression<Func<T, bool>> wehre=null) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                return Template.Count<T>(Session, wehre);
            }
        }

        /// <summary> ��� </summary>
        /// <param name="entity">ʵ��</param>
        public virtual object Insert<T>(T entity) where T : class
        {
            // using (var session=AppSessionFactory.OpenSession())
            {
                UpdateValue(entity);
               return Template.Insert<T>(Session, entity);
            }
        }

        /// <summary>���� ��� </summary>
        /// <param name="entities">ʵ��</param>
        public virtual  void BatchInsert<T>(T[] entities) where T : class
        {
            //using (var session = AppSessionFactory.OpenSession())
            {
                foreach (var entity in entities)
                {
                    UpdateValue(entity);
                }
                Template.BatchInsert<T>(Session, entities);
            }
        }

        /// <summary> ����һ��ʵ����������� </summary>
        /// <param name="entity">ʵ��</param>
        public virtual void Update<T>(T entity) where T : class
        {
            //using (var session = AppSessionFactory.OpenSession())
            {
                Update(entity,2);
            }
        }

        /// <summary> ����һ��ʵ����������� </summary>
        /// <param name="entity">ʵ��</param>
        public virtual void Update<T>(T entity,int flag) where T : class
        {
            //using (var session = AppSessionFactory.OpenSession())
            {
                UpdateValue(entity, flag);
                Template.Update<T>(Session, entity);
            }
        }

        /// <summary> ɾ��</summary>
        /// <param name="entity">ʵ��</param>
        public virtual void Delete<T>(T entity) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                if(entity is IHasDeletionTime)
                {
                    Update(entity, 3);
                }
                else
                {
                    Template.Delete<T>(Session, entity);
                }
            }
        }

        /// <summary> ����ɾ�� Ĭ��ʵ��  nhibernate ֧�� EF dapper δʵ��</summary>
        public virtual void Delete<T>(object id) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                if (typeof(T).IsAssignableFrom(typeof(IHasDeletionTime)))
                {
                    if (string.IsNullOrEmpty(id?.ToString())) throw new ArgumentNullException("id");
                    ITransaction transaction = null;
                    try
                    {
                        using (transaction = Session.BeginTransaction())
                        {
                            var obj = Session.Get<T>(id);
                            if(obj is IHasDeletionTime deletionTime)
                            {
                                if (!deletionTime.IsDeleted)
                                {
                                    UpdateValue(obj,3);
                                    Session.Update(obj);
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (transaction != null)
                        {
                            transaction.Rollback();
                        }
                        throw e;
                    }
                }
                else
                {
                    Template.Delete<T>(Session, id);
                }
               
            }
        }

        /// <summary>
        /// ʵ�ְ���Ҫֻ���²��ָ��� Ĭ��ʵ�� ef,nhibernate ֧�� linq dapper ��֧��linq dapper δʵ��
        /// <para>�磺Update(u =>u.Id==1,u =>new User{Name="ok"});</para>
        /// </summary>
        /// <param name="where">��������</param>
        /// <param name="update">���º��ʵ��</param>
        public virtual void Update<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> update) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
            Template.Update<T>(Session, where, update);

        }

        /// <summary> ����ɾ�� Ĭ��ʵ�� ef,nhibernate ֧�� linq dapper ��֧��linq dapper δʵ��</summary>
        /// <param name="where">����</param>

        public virtual void Delete<T>(Expression<Func<T, bool>> where=null) where T : class
        {
            //using (var session = AppSessionFactory.OpenSession())
            {
                Template.Delete<T>(Session, where);
            }
        }


        /// <summary> ִ��sql </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual int ExecuteSql(string sql)
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                return Template.ExecuteQuery(Session, sql);
            }

        }
        /// <summary> ִ��sql </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual  Task<int> ExecuteSqlAsync(string sql)
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                return Template.ExecuteQueryAsync(Session, sql);
            }

        }
        /// <summary> �����ɹ� ���浽���� Ĭ��ʵ�� ef ֧��  dapper nhibernate ���κβ��� </summary>
        public virtual void Save()
        {
        
        }

#region async
        /// <summary>���ҵ������Ҳ��������������� ef,nhibernate ֧�� linq dapper ��֧��linq </summary>
        /// <param name="where">����</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<T> FindSingleAsync<T>(Expression<Func<T, bool>> where = null, CancellationToken cancellationToken = default) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
			return Template.FindSingleAsync(Session,where,cancellationToken);
        }

        /// <summary> �Ƿ���� Ĭ��ʵ�� ef,nhibernate ֧�� linq dapper ��֧��linq  </summary>
        /// <param name="where">����</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual  Task<bool> IsExistAsync<T>(Expression<Func<T, bool>> where=null, CancellationToken cancellationToken = default) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                return new Task<bool>(()=>Template.CountAsync<T>(Session, where).Result >= 1);
            }
        }

        /// <summary> ��ѯ���� Ĭ��ʵ�� ef,nhibernate ֧�� linq dapper ��֧��linq dapperĬ�ϲ�ѯ���н���������ڴ� ������ѯ</summary>
        /// <param name="where">����</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<IQueryable<T>> FindAsync<T>(Expression<Func<T, bool>> where = null, CancellationToken cancellationToken = default
            ) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
			var res = Template.FindAsync(Session,where);
			return res;
        }

        /// <summary> ��ѯ���� Ĭ��ʵ�� ef,nhibernate ֧�� linq dapper ��֧��linq orderby������Ч dapperĬ�ϲ�ѯ���н���������ڴ� ������ѯ </summary>
        /// <param name="page">ҳ��</param>
        /// <param name="size">��¼</param>
        /// <param name="where">����</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<IQueryable<T>> FindByPageAsync<T>(Expression<Func<T, bool>> where = null, int page = 1, int size = 10, CancellationToken cancellationToken = default) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
			var res = Template.FindByPageAsync(Session,where, page, size);
			return res;
        }

        /// <summary>��ѯ����  Ĭ��ʵ�� ef,nhibernate ֧�� linq dapper ��֧��linq  dapperĬ�ϲ�ѯ���н������  </summary>
        /// <param name="where">����</param>
        /// <returns></returns>
        /// <param name="cancellationToken"></param>
        public virtual Task<int> CountAsync<T>(Expression<Func<T, bool>> where = null, CancellationToken cancellationToken = default) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                return Template.CountAsync<T>(Session, where, cancellationToken);
            }
        }
        /// <summary>��ѯ����  Ĭ��ʵ�� ef,nhibernate ֧�� linq dapper ��֧��linq  dapperĬ�ϲ�ѯ���н������  </summary>
        /// <param name="where">����</param>
        /// <returns></returns>
        /// <param name="cancellationToken"></param>
        public virtual Task<int> CountAsync<T>(ICriteria  where = null, CancellationToken cancellationToken = default) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                return Template.CountAsync<T>(Session, where, cancellationToken);
            }
        }
        /// <summary> ��� </summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="cancellationToken"></param>
        public virtual Task InsertAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            // using (var session=AppSessionFactory.OpenSession())
            {
                return Template.InsertAsync<T>(Session, entity, cancellationToken);
            }
        }

        /// <summary>���� ��� </summary>
        /// <param name="entities">ʵ��</param>
        /// <param name="cancellationToken"></param>
        public virtual Task BatchInsertAsync<T>(T[] entities, CancellationToken cancellationToken = default) where T : class
        {
            //using (var session = AppSessionFactory.OpenSession())
            {
                return Template.BatchInsertAsync<T>(Session, entities, cancellationToken);
            }
        }

        /// <summary> ����һ��ʵ����������� </summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="cancellationToken"></param>
        public virtual Task UpdateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            //using (var session = AppSessionFactory.OpenSession())
            {
                return UpdateAsync( entity,2,cancellationToken);
            }
        }
        /// <summary> ����һ��ʵ����������� </summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="flag"></param>
        /// <param name="cancellationToken"></param>
        public virtual Task UpdateAsync<T>(T entity,int flag=2, CancellationToken cancellationToken = default) where T : class
        {
            //using (var session = AppSessionFactory.OpenSession())
            {
                UpdateValue(entity, flag);
                return Template.UpdateAsync<T>(Session, entity,cancellationToken);
            }
        }
        /// <summary> ɾ��</summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="cancellationToken"></param>
        public virtual Task DeleteAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                if(entity is IHasDeletionTime)
                {
                    return UpdateAsync(entity,3,cancellationToken);
                }
                return Template.DeleteAsync<T>(Session, entity, cancellationToken);
            }
        }

        /// <summary> ����ɾ�� Ĭ��ʵ��  nhibernate ֧�� EF dapper δʵ��</summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        public virtual Task DeleteAsync<T>(object id, CancellationToken cancellationToken = default) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                if (typeof(T).IsAssignableFrom(typeof(IHasDeletionTime)))
                {
                    if (string.IsNullOrEmpty(id?.ToString())) throw new ArgumentNullException("id");
                    ITransaction transaction = null;
                    try
                    {
                        using (transaction = Session.BeginTransaction())
                        {
#if NET40 || NET45 || NET451 || NET452 || NET46
                            var obj = Session.Get<T>(id);
#else
                            var obj = Session.GetAsync<T>(id, cancellationToken).Result;
#endif
                            if (obj is IHasDeletionTime deletionTime)
                            {
                                if (!deletionTime.IsDeleted)
                                {
                                    UpdateValue(obj, 3);
#if NET40 || NET45 || NET451 || NET452 || NET46
                                    Session.Update(obj);
                                    transaction.Commit();
                                    return new Task(()=>{});
#else
                                    Session.UpdateAsync(obj).GetAwaiter().GetResult();
                                    return transaction.CommitAsync(cancellationToken);
#endif
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (transaction != null)
                        {
#if NET40 || NET45 || NET451 || NET452 || NET46
                                    transaction.Rollback();
                                    return new Task(()=>{});
#else
                            transaction.RollbackAsync(cancellationToken);
#endif
                        }
                    }
                    return TaskHelper.CompletedTask;
                }
                else
                {
                    return Template.DeleteAsync<T>(Session, id, cancellationToken);
                }
            }
        }

        /// <summary>
        /// ʵ�ְ���Ҫֻ���²��ָ��� Ĭ��ʵ�� ef,nhibernate ֧�� linq dapper ��֧��linq dapper δʵ��
        /// <para>�磺Update(u =>u.Id==1,u =>new User{Name="ok"});</para>
        /// </summary>
        /// <param name="where">��������</param>
        /// <param name="update">���º��ʵ��</param>
        /// <param name="cancellationToken"></param>
        public virtual Task UpdateAsync<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> update, CancellationToken cancellationToken = default) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
            return Template.UpdateAsync<T>(Session, where, update, cancellationToken);
        }

        /// <summary> ����ɾ�� Ĭ��ʵ�� ef,nhibernate ֧�� linq dapper ��֧��linq dapper δʵ��</summary>
        /// <param name="where">����</param>
        /// <param name="cancellationToken"></param>
        public virtual Task DeleteAsync<T>(Expression<Func<T, bool>> where=null, CancellationToken cancellationToken = default) where T : class
        {
            //using (var session = AppSessionFactory.OpenSession())
             return Template.DeleteAsync<T>(Session, where, cancellationToken);
        }


        /// <summary> ִ��sql </summary>
        /// <param name="sql"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<int> ExecuteSqlAsync(string sql, CancellationToken cancellationToken = default)
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                return Template.ExecuteQueryAsync(Session, sql, cancellationToken);
            }

        }


        /// <summary> �����ɹ� ���浽���� Ĭ��ʵ�� ef ֧��  dapper nhibernate ���κβ��� </summary>
        /// <param name="cancellationToken"></param>
        public virtual Task SaveAsync(CancellationToken cancellationToken = default)
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return  new Task(()=>{});
#else
            return Task.CompletedTask;
#endif
        }
#endregion async
        /// <summary>
        /// 
        /// </summary>
        void IDisposable.Dispose()
        {
            //SessionFactory.Dispose();
            Session.Dispose();
        }

      
    }
}
#endif