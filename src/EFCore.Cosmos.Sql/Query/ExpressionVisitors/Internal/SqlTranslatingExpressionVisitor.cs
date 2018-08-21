// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Cosmos.Sql.Query.Expressions.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Expressions.Internal;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using Remotion.Linq.Clauses.Expressions;

namespace Microsoft.EntityFrameworkCore.Cosmos.Sql.Query.ExpressionVisitors.Internal
{
    public class SqlTranslatingExpressionVisitor : ExpressionVisitorBase
    {
        private readonly SelectExpression _selectExpression;
        private readonly QueryCompilationContext _queryCompilationContext;

        public SqlTranslatingExpressionVisitor(SelectExpression selectExpression,
            QueryCompilationContext queryCompilationContext)
        {
            _selectExpression = selectExpression;
            _queryCompilationContext = queryCompilationContext;
        }

        public virtual bool Translated { get; private set; } = true;

        protected override Expression VisitMember(MemberExpression memberExpression)
        {
            var properties = MemberAccessBindingExpressionVisitor.GetPropertyPath(memberExpression,
                _queryCompilationContext, out var qsre);

            var newExpression = _selectExpression.BindPropertyPath(qsre, properties);
            if (newExpression != null)
            {
                return newExpression;
            }
            else
            {
                Translated = false;
                return memberExpression;
            }
        }

        protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
            var properties = MemberAccessBindingExpressionVisitor.GetPropertyPath(methodCallExpression,
                _queryCompilationContext, out var qsre);

            var newExpression = _selectExpression.BindPropertyPath(qsre, properties);
            if (newExpression != null)
            {
                return newExpression;
            }
            else
            {
                Translated = false;
                return methodCallExpression;
            }
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected override Expression VisitExtension(Expression node)
        {
            if (node is NullConditionalExpression nullConditionalExpression)
            {
                return Visit(nullConditionalExpression.AccessOperation);
            }

            if (node is NullSafeEqualExpression nullConditionalEqualExpression)
            {
                return Visit(nullConditionalEqualExpression.EqualExpression);
            }

            return base.VisitExtension(node);
        }

        protected override Expression VisitSubQuery(SubQueryExpression subQueryExpression)
        {
            Translated = false;
            return subQueryExpression;
        }
    }
}
