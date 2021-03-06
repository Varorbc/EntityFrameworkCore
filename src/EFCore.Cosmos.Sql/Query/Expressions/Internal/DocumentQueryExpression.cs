﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Azure.Documents;
using Microsoft.EntityFrameworkCore.Cosmos.Sql.Storage.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json.Linq;

namespace Microsoft.EntityFrameworkCore.Cosmos.Sql.Query.Expressions.Internal
{
    public class DocumentQueryExpression : Expression
    {
        private readonly bool _async;
        private readonly string _collectionId;
        private readonly CosmosClient _cosmosClient;

        public DocumentQueryExpression(bool async, string collectionId, SelectExpression selectExpression, CosmosClient cosmosClient)
        {
            _async = async;
            _collectionId = collectionId;
            SelectExpression = selectExpression;
            _cosmosClient = cosmosClient;
        }

        public SelectExpression SelectExpression { get; }

        public override bool CanReduce => true;

        // TODO: Reduce based on sync/async
        public override Expression Reduce()
            => Call(
                typeof(DocumentQueryExpression).GetTypeInfo().GetDeclaredMethod(nameof(_Query)),
                Constant(_cosmosClient),
                EntityQueryModelVisitor.QueryContextParameter,
                Constant(_collectionId),
                Constant(SelectExpression));

        private static IEnumerable<JObject> _Query(
            CosmosClient cosmosClient,
            QueryContext queryContext,
            string collectionId,
            SelectExpression selectExpression)
            => new DocumentEnumerable(cosmosClient, queryContext, collectionId, selectExpression);

        private class DocumentEnumerable : IEnumerable<JObject>
        {
            private readonly CosmosClient _cosmosClient;
            private readonly QueryContext _queryContext;
            private readonly string _collectionId;
            private readonly SelectExpression _selectExpression;

            public DocumentEnumerable(
                CosmosClient cosmosClient, QueryContext queryContext, string collectionId, SelectExpression selectExpression)
            {
                _cosmosClient = cosmosClient;
                _queryContext = queryContext;
                _collectionId = collectionId;
                _selectExpression = selectExpression;
            }

            public IEnumerator<JObject> GetEnumerator() => new Enumerator(this);

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            private class Enumerator : IEnumerator<JObject>
            {
                private IEnumerator<Document> _underlyingEnumerator;
                private readonly CosmosClient _cosmosClient;
                private readonly QueryContext _queryContext;
                private readonly string _collectionId;
                private readonly SelectExpression _selectExpression;

                public Enumerator(DocumentEnumerable documentEnumerable)
                {
                    _cosmosClient = documentEnumerable._cosmosClient;
                    _queryContext = documentEnumerable._queryContext;
                    _collectionId = documentEnumerable._collectionId;
                    _selectExpression = documentEnumerable._selectExpression;
                }

                public JObject Current { get; private set; }

                object IEnumerator.Current => Current;

                public void Dispose()
                {
                    _underlyingEnumerator.Dispose();
                }

                public bool MoveNext()
                {
                    if (_underlyingEnumerator == null)
                    {
                        _underlyingEnumerator = _cosmosClient.ExecuteSqlQuery(
                            _collectionId,
                            _selectExpression.ToSqlQuery(_queryContext.ParameterValues));
                    }

                    var hasNext = _underlyingEnumerator.MoveNext();

                    Current = hasNext
                        ? _underlyingEnumerator.Current.GetPropertyValue<JObject>("query")
                        : default;

                    return hasNext;
                }

                public void Reset() => throw new NotImplementedException();
            }
        }

        protected override Expression VisitChildren(ExpressionVisitor visitor) => this;

        public override Type Type
            => (_async ? typeof(IAsyncEnumerable<>) : typeof(IEnumerable<>)).MakeGenericType(typeof(JObject));

        public override ExpressionType NodeType => ExpressionType.Extension;
    }
}
