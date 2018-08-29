﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Cosmos.Sql.Query.Internal;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using Newtonsoft.Json.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing;

namespace Microsoft.EntityFrameworkCore.Cosmos.Sql.Query.ExpressionVisitors.Internal
{
    public class CosmosSqlMemberAccessBindingExpressionVisitor : RelinqExpressionVisitor
    {
        private static readonly MethodInfo _getItemMethodInfo
            = typeof(JObject).GetTypeInfo().GetRuntimeProperties()
                .Single(pi => pi.Name == "Item" && pi.GetIndexParameters()[0].ParameterType == typeof(string))
                .GetMethod;

        private readonly QuerySourceMapping _querySourceMapping;
        private readonly CosmosSqlQueryModelVisitor _queryModelVisitor;
        private readonly bool _inProjection;

        public CosmosSqlMemberAccessBindingExpressionVisitor(
            QuerySourceMapping querySourceMapping,
            EntityQueryModelVisitor queryModelVisitor,
            bool inProjection)
        {
            _querySourceMapping = querySourceMapping;
            _queryModelVisitor = (CosmosSqlQueryModelVisitor)queryModelVisitor;
            _inProjection = inProjection;
        }

        protected override Expression VisitMember(MemberExpression memberExpression)
        {
            var newExpression = Visit(memberExpression.Expression);

            if (newExpression != memberExpression.Expression)
            {
                if (newExpression.Type == typeof(JObject))
                {
                    var properties = MemberAccessBindingExpressionVisitor.GetPropertyPath(
                        memberExpression, _queryModelVisitor.QueryCompilationContext, out var qsre);

                    if (qsre != null)
                    {
                        foreach (var property in properties)
                        {
                            newExpression = CreateGetValueExpression(
                                newExpression, property);
                        }

                        return newExpression;
                    }
                    else
                    {
                        var modelVisitor = _queryModelVisitor;
                        modelVisitor.AllMembersBoundToJObject = false;

                        return memberExpression;
                    }
                }

                return Expression.MakeMemberAccess(newExpression, memberExpression.Member);
            }

            return memberExpression;
        }

        protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
            if (_queryModelVisitor.CurrentParameter?.Type == typeof(JObject))
            {
                if (methodCallExpression.Method.IsEFPropertyMethod())
                {
                    var source = methodCallExpression.Arguments[0];
                    var newSource = Visit(source);

                    if (source != newSource
                        && _queryModelVisitor.AllMembersBoundToJObject
                        && newSource.Type == typeof(JObject))
                    {
                        var properties = MemberAccessBindingExpressionVisitor.GetPropertyPath(
                            methodCallExpression, _queryModelVisitor.QueryCompilationContext, out var qsre);

                        if (qsre != null)
                        {
                            foreach (var property in properties)
                            {
                                newSource = CreateGetValueExpression(newSource, property);
                            }

                            return newSource;
                        }
                    }
                }

                _queryModelVisitor.AllMembersBoundToJObject = false;
                return methodCallExpression;
            }

            var newExpression = (MethodCallExpression)base.VisitMethodCall(methodCallExpression);

            return _queryModelVisitor.BindMethodCallToEntity(methodCallExpression, newExpression) ?? newExpression;
        }

        protected override Expression VisitQuerySourceReference(QuerySourceReferenceExpression querySourceReferenceExpression)
        {
            if (_querySourceMapping.ContainsMapping(querySourceReferenceExpression.ReferencedQuerySource))
            {
                var mappedExpression = _querySourceMapping.GetExpression(querySourceReferenceExpression.ReferencedQuerySource);
                if(!(mappedExpression is ParameterExpression mappedParameter)
                    || mappedParameter.Name != _queryModelVisitor.CurrentParameter?.Name)
                {
                    _queryModelVisitor.AllMembersBoundToJObject = false;
                }

                return mappedExpression;
            }
            else
            {
                return querySourceReferenceExpression;
            }
        }

        private static Expression CreateGetValueExpression(
            Expression jObjectExpression,
            IPropertyBase property)
        {
            return Expression.Convert(
                Expression.Call(
                    jObjectExpression,
                    _getItemMethodInfo,
                    Expression.Constant(property.Name)),
                property.ClrType);
        }
    }
}
