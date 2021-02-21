#if !(NET10  || NET11  ||NET20 || NET30 || NET35)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Utility.Extensions
{
    /// <summary>
    /// ef 支持的 linq nhibernate linq 不支持 需要自己转换(支持一次的linq (多次组合的linq 不支持)) dapper linq 不支持 需要自己转换
    /// </summary>
    public static class LinqExpression
    {
        /// <summary>
        /// 组装linq 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="merge"></param>
        /// <returns></returns>
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
            // apply composition of lambda expression bodies to parameters from the first expression
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first == null ? second : first.Compose(second, Expression.And);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first==null?second: first.Compose(second, Expression.Or);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Key"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> IdEqual<T,Key>(this Key id)
        {
            return PropertyOrFieldBind<T, Key>(id, "Id");
        }

        /// <summary>
        /// 单条件 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Key"></typeparam>
        /// <param name="id"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Tuple<MemberExpression, UnaryExpression, ParameterExpression> GetPropertyOrFieldBind<T, Key>(this Key id, string propertyName)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T));
            MemberExpression memberExpression = Expression.PropertyOrField(parameterExpression, propertyName);
            object idValue = Convert.ChangeType(id, typeof(Key));
            UnaryExpression right = Expression.Convert(((Expression<Func<object>>)(() => idValue)).Body, memberExpression.Type);
            return new Tuple<MemberExpression, UnaryExpression, ParameterExpression>(memberExpression,right,parameterExpression);
        }

        /// <summary>
        /// 单条件 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Key"></typeparam>
        /// <param name="id"></param>
        /// <param name="propertyName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> PropertyOrFieldBind<T, Key>(this Key id, string propertyName,Func<Expression,Expression,Expression> where)
        {
            var tuple = GetPropertyOrFieldBind<T, Key>(id, propertyName);
            ParameterExpression parameterExpression = tuple.Item3;
            MemberExpression memberExpression = tuple.Item1;
            UnaryExpression right = tuple.Item2;
            return Expression.Lambda<Func<T, bool>>(where(memberExpression, right), new ParameterExpression[1]
             {
                    parameterExpression
             });
        }
            /// <summary>
            /// 单条件 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <typeparam name="Key"></typeparam>
            /// <param name="id"></param>
            /// <param name="propertyName"></param>
            /// <param name="expressionType"></param>
            /// <returns></returns>
        public static Expression<Func<T, bool>> PropertyOrFieldBind<T, Key>(this Key id,string propertyName,ExpressionType expressionType= ExpressionType.Equal)
            {
            Func<Expression, Expression, Expression> where=null;
            if (expressionType== ExpressionType.Equal)
            {
                where = (left, right) => Expression.Equal(left, right);
            }
            if (expressionType == ExpressionType.NotEqual)
            {
                where = (left, right) => Expression.NotEqual(left, right);
            }
            if (expressionType == ExpressionType.GreaterThan)
            {
                where = (left, right) => Expression.GreaterThan(left, right);
            }
            if (expressionType == ExpressionType.GreaterThanOrEqual)
            {
                where = (left, right) => Expression.GreaterThanOrEqual(left, right);
            }
            if (expressionType == ExpressionType.LessThan)
            {
                where = (left, right) => Expression.LessThan(left, right);
            }
            if (expressionType == ExpressionType.LessThanOrEqual)
            {
                where = (left, right) => Expression.LessThanOrEqual(left, right);
            }
            return where==null?null: PropertyOrFieldBind<T, Key>(id,propertyName,where);
        }
    }

   /// <summary>
   /// 
   /// </summary>
    public class ParameterRebinder : ExpressionVisitor
    {

        private readonly Dictionary<ParameterExpression, ParameterExpression> map;
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }
    }
}
#endif