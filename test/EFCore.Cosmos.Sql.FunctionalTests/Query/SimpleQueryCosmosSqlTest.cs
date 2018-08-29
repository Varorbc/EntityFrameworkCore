﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.TestModels.Northwind;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Microsoft.EntityFrameworkCore.TestUtilities.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.EntityFrameworkCore.Cosmos.Sql.Query
{
    public partial class SimpleQueryCosmosSqlTest : SimpleQueryTestBase<NorthwindQueryCosmosSqlFixture<NoopModelCustomizer>>
    {
        public SimpleQueryCosmosSqlTest(NorthwindQueryCosmosSqlFixture<NoopModelCustomizer> fixture,
            ITestOutputHelper testOutputHelper)
            : base(fixture)
        {
            ClearLog();
            //Fixture.TestSqlLoggerFactory.SetTestOutputHelper(testOutputHelper);
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async virtual Task Simple_IQueryable(bool isAsync)
        {
            await AssertQuery<Customer>(isAsync, cs => cs, entryCount: 91);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Shaper_command_caching_when_parameter_names_different()
        {
            base.Shaper_command_caching_when_parameter_names_different();

            AssertSql(
    @"SELECT c AS query
FROM root c
WHERE (((c[""Discriminator""] = ""Customer"") AND (c[""CustomerID""] = ""ALFKI"")) AND ""True"")",
    //
    @"SELECT c AS query
FROM root c
WHERE (((c[""Discriminator""] = ""Customer"") AND (c[""CustomerID""] = ""ALFKI"")) AND ""True"")");
        }

        public override void Lifting_when_subquery_nested_order_by_anonymous()
        {
            base.Lifting_when_subquery_nested_order_by_anonymous();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override void Lifting_when_subquery_nested_order_by_simple()
        {
            base.Lifting_when_subquery_nested_order_by_simple();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Local_array(bool isAsync)
        {
            await base.Local_array(isAsync);

            AssertSql(
                @"@__get_Item_0='ALFKI'

SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""CustomerID""] = @__get_Item_0))");
        }

        public override void Method_with_constant_queryable_arg()
        {
            base.Method_with_constant_queryable_arg();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Entity_equality_self(bool isAsync)
        {
            await base.Entity_equality_self(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""CustomerID""] = c[""CustomerID""]))");
        }

        public override async Task Entity_equality_local(bool isAsync)
        {
            await base.Entity_equality_local(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Join_with_entity_equality_local_on_both_sources(bool isAsync)
        {
            await base.Join_with_entity_equality_local_on_both_sources(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Entity_equality_local_inline(bool isAsync)
        {
            await base.Entity_equality_local_inline(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Entity_equality_null(bool isAsync)
        {
            await base.Entity_equality_null(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""CustomerID""] = null))");
        }

        public override async Task Entity_equality_not_null(bool isAsync)
        {
            await base.Entity_equality_not_null(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""CustomerID""] != null))");
        }

        public override async Task Queryable_reprojection(bool isAsync)
        {
            await base.Queryable_reprojection(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Default_if_empty_top_level(bool isAsync)
        {
            await base.Default_if_empty_top_level(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Employee"") AND (c[""EmployeeID""] = 4294967295))");
        }

        public override async Task Join_with_default_if_empty_on_both_sources(bool isAsync)
        {
            await base.Join_with_default_if_empty_on_both_sources(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Default_if_empty_top_level_followed_by_projecting_constant(bool isAsync)
        {
            await base.Default_if_empty_top_level_followed_by_projecting_constant(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Default_if_empty_top_level_positive(bool isAsync)
        {
            await base.Default_if_empty_top_level_positive(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Employee"") AND (c[""EmployeeID""] > 0))");
        }

        public override async Task Default_if_empty_top_level_arg(bool isAsync)
        {
            await base.Default_if_empty_top_level_arg(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Employee"") AND (c[""EmployeeID""] = 4294967295))");
        }


        public override async Task Default_if_empty_top_level_arg_followed_by_projecting_constant(bool isAsync)
        {
            await base.Default_if_empty_top_level_arg_followed_by_projecting_constant(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Default_if_empty_top_level_projection(bool isAsync)
        {
            await base.Default_if_empty_top_level_projection(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Employee"") AND (c[""EmployeeID""] = 4294967295))");
        }

        public override async Task Where_query_composition(bool isAsync)
        {
            await base.Where_query_composition(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_query_composition_is_null(bool isAsync)
        {
            await base.Where_query_composition_is_null(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_query_composition_is_not_null(bool isAsync)
        {
            await base.Where_query_composition_is_null(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_query_composition_entity_equality_one_element_SingleOrDefault(bool isAsync)
        {
            await base.Where_query_composition_entity_equality_one_element_SingleOrDefault(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_query_composition_entity_equality_one_element_FirstOrDefault(bool isAsync)
        {
            await base.Where_query_composition_entity_equality_one_element_FirstOrDefault(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_query_composition_entity_equality_no_elements_SingleOrDefault(bool isAsync)
        {
            await base.Where_query_composition_entity_equality_no_elements_SingleOrDefault(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_query_composition_entity_equality_no_elements_FirstOrDefault(bool isAsync)
        {
            await base.Where_query_composition_entity_equality_no_elements_FirstOrDefault(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_query_composition_entity_equality_multiple_elements_FirstOrDefault(bool isAsync)
        {
            await base.Where_query_composition_entity_equality_multiple_elements_FirstOrDefault(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_query_composition2(bool isAsync)
        {
            await base.Where_query_composition2(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_query_composition2_FirstOrDefault(bool isAsync)
        {
            await base.Where_query_composition2_FirstOrDefault(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_query_composition2_FirstOrDefault_with_anonymous(bool isAsync)
        {
            await base.Where_query_composition2_FirstOrDefault_with_anonymous(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Select_Subquery_Single()
        {
            base.Select_Subquery_Single();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Select_Where_Subquery_Deep_Single()
        {
            base.Select_Where_Subquery_Deep_Single();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Select_Where_Subquery_Deep_First()
        {
            base.Select_Where_Subquery_Deep_First();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Select_Where_Subquery_Equality()
        {
            base.Select_Where_Subquery_Equality();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_subquery_anon(bool isAsync)
        {
            await base.Where_subquery_anon(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_subquery_anon_nested(bool isAsync)
        {
            await base.Where_subquery_anon_nested(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_SelectMany(bool isAsync)
        {
            await base.OrderBy_SelectMany(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Let_any_subquery_anonymous(bool isAsync)
        {
            await base.Let_any_subquery_anonymous(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_arithmetic(bool isAsync)
        {
            await base.OrderBy_arithmetic(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_condition_comparison(bool isAsync)
        {
            await base.OrderBy_condition_comparison(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_ternary_conditions(bool isAsync)
        {
            await base.OrderBy_ternary_conditions(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void OrderBy_any()
        {
            base.OrderBy_any();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Skip(bool isAsync)
        {
            await base.Skip(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Skip_no_orderby(bool isAsync)
        {
            await base.Skip_no_orderby(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Skip_Take(bool isAsync)
        {
            await base.Skip_Take(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Join_Customers_Orders_Skip_Take(bool isAsync)
        {
            await base.Join_Customers_Orders_Skip_Take(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Join_Customers_Orders_Skip_Take_followed_by_constant_projection(bool isAsync)
        {
            await base.Join_Customers_Orders_Skip_Take_followed_by_constant_projection(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Join_Customers_Orders_Projection_With_String_Concat_Skip_Take(bool isAsync)
        {
            await base.Join_Customers_Orders_Projection_With_String_Concat_Skip_Take(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Join_Customers_Orders_Orders_Skip_Take_Same_Properties(bool isAsync)
        {
            await base.Join_Customers_Orders_Orders_Skip_Take_Same_Properties(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Take_Skip(bool isAsync)
        {
            await base.Take_Skip(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Take_Skip_Distinct(bool isAsync)
        {
            await base.Take_Skip_Distinct(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Take_Skip_Distinct_Caching(bool isAsync)
        {
            await base.Take_Skip_Distinct_Caching(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Take_Distinct_Count(bool isAsync)
        {
            await base.Take_Distinct_Count(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Take_Where_Distinct_Count(bool isAsync)
        {
            await base.Take_Where_Distinct_Count(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Null_conditional_simple(bool isAsync)
        {
            await base.Null_conditional_simple(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Null_conditional_deep(bool isAsync)
        {
            await base.Null_conditional_deep(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Queryable_simple(bool isAsync)
        {
            await base.Queryable_simple(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Queryable_simple_anonymous(bool isAsync)
        {
            await base.Queryable_simple_anonymous(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Queryable_nested_simple(bool isAsync)
        {
            await base.Queryable_nested_simple(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Queryable_simple_anonymous_projection_subquery(bool isAsync)
        {
            await base.Queryable_simple_anonymous_projection_subquery(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Queryable_simple_anonymous_subquery(bool isAsync)
        {
            await base.Queryable_simple_anonymous_subquery(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Take_simple(bool isAsync)
        {
            await base.Take_simple(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Take_simple_parameterized(bool isAsync)
        {
            await base.Take_simple_parameterized(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Take_simple_projection(bool isAsync)
        {
            await base.Take_simple_projection(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Take_subquery_projection(bool isAsync)
        {
            await base.Take_subquery_projection(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_Take_Count(bool isAsync)
        {
            await base.OrderBy_Take_Count(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Take_OrderBy_Count(bool isAsync)
        {
            await base.Take_OrderBy_Count(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Any_simple(bool isAsync)
        {
            await base.Any_simple(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Any_predicate(bool isAsync)
        {
            await base.Any_predicate(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Any_nested_negated(bool isAsync)
        {
            await base.Any_nested_negated(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Any_nested_negated2(bool isAsync)
        {
            await base.Any_nested_negated2(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Any_nested_negated3(bool isAsync)
        {
            await base.Any_nested_negated3(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Any_nested(bool isAsync)
        {
            await base.Any_nested(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Any_nested2(bool isAsync)
        {
            await base.Any_nested2(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Any_nested3(bool isAsync)
        {
            await base.Any_nested3(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Any_with_multiple_conditions_still_uses_exists()
        {
            base.Any_with_multiple_conditions_still_uses_exists();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task All_top_level(bool isAsync)
        {
            await base.All_top_level(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task All_top_level_column(bool isAsync)
        {
            await base.All_top_level_column(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task All_top_level_subquery(bool isAsync)
        {
            await base.All_top_level_subquery(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task All_top_level_subquery_ef_property(bool isAsync)
        {
            await base.All_top_level_subquery_ef_property(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task First_client_predicate(bool isAsync)
        {
            await base.First_client_predicate(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_select_many_or(bool isAsync)
        {
            await base.Where_select_many_or(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_select_many_or2(bool isAsync)
        {
            await base.Where_select_many_or2(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_select_many_or3(bool isAsync)
        {
            await base.Where_select_many_or3(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_select_many_or4(bool isAsync)
        {
            await base.Where_select_many_or4(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_select_many_or_with_parameter(bool isAsync)
        {
            await base.Where_select_many_or_with_parameter(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task SelectMany_mixed(bool isAsync)
        {
            await base.SelectMany_mixed(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task SelectMany_simple_subquery(bool isAsync)
        {
            await base.SelectMany_simple_subquery(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task SelectMany_simple1(bool isAsync)
        {
            await base.SelectMany_simple1(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task SelectMany_simple2(bool isAsync)
        {
            await base.SelectMany_simple2(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task SelectMany_entity_deep(bool isAsync)
        {
            await base.SelectMany_entity_deep(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task SelectMany_projection1(bool isAsync)
        {
            await base.SelectMany_projection1(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task SelectMany_projection2(bool isAsync)
        {
            await base.SelectMany_projection2(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task SelectMany_Count(bool isAsync)
        {
            await base.SelectMany_Count(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task SelectMany_LongCount(bool isAsync)
        {
            await base.SelectMany_LongCount(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task SelectMany_OrderBy_ThenBy_Any(bool isAsync)
        {
            await base.SelectMany_OrderBy_ThenBy_Any(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Join_Where_Count(bool isAsync)
        {
            await base.Join_Where_Count(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_Join_Any(bool isAsync)
        {
            await base.Where_Join_Any(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_Join_Exists(bool isAsync)
        {
            await base.Where_Join_Exists(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_Join_Exists_Inequality(bool isAsync)
        {
            await base.Where_Join_Exists_Inequality(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_Join_Exists_Constant(bool isAsync)
        {
            await base.Where_Join_Exists_Constant(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_Join_Not_Exists(bool isAsync)
        {
            await base.Where_Join_Not_Exists(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Join_OrderBy_Count(bool isAsync)
        {
            await base.Join_OrderBy_Count(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Multiple_joins_Where_Order_Any(bool isAsync)
        {
            await base.Multiple_joins_Where_Order_Any(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_join_select(bool isAsync)
        {
            await base.Where_join_select(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_orderby_join_select(bool isAsync)
        {
            await base.Where_orderby_join_select(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_join_orderby_join_select(bool isAsync)
        {
            await base.Where_join_orderby_join_select(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_select_many(bool isAsync)
        {
            await base.Where_select_many(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_orderby_select_many(bool isAsync)
        {
            await base.Where_orderby_select_many(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task SelectMany_cartesian_product_with_ordering(bool isAsync)
        {
            await base.SelectMany_cartesian_product_with_ordering(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task SelectMany_Joined_DefaultIfEmpty(bool isAsync)
        {
            await base.SelectMany_Joined_DefaultIfEmpty(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task SelectMany_Joined_DefaultIfEmpty2(bool isAsync)
        {
            await base.SelectMany_Joined_DefaultIfEmpty2(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task SelectMany_Joined_Take(bool isAsync)
        {
            await base.SelectMany_Joined_Take(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Take_with_single(bool isAsync)
        {
            await base.Take_with_single(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Take_with_single_select_many(bool isAsync)
        {
            await base.Take_with_single_select_many(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Distinct_Skip(bool isAsync)
        {
            await base.Distinct_Skip(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Distinct_Skip_Take(bool isAsync)
        {
            await base.Distinct_Skip_Take(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Skip_Distinct(bool isAsync)
        {
            await base.Skip_Distinct(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Skip_Take_Distinct(bool isAsync)
        {
            await base.Skip_Take_Distinct(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Skip_Take_Any(bool isAsync)
        {
            await base.Skip_Take_Any(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Skip_Take_All(bool isAsync)
        {
            await base.Skip_Take_All(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Take_All(bool isAsync)
        {
            await base.Take_All(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Skip_Take_Any_with_predicate(bool isAsync)
        {
            await base.Skip_Take_Any_with_predicate(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Take_Any_with_predicate(bool isAsync)
        {
            await base.Take_Any_with_predicate(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy(bool isAsync)
        {
            await base.OrderBy(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_true(bool isAsync)
        {
            await base.OrderBy_true(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_integer(bool isAsync)
        {
            await base.OrderBy_integer(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_parameter(bool isAsync)
        {
            await base.OrderBy_parameter(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_anon(bool isAsync)
        {
            await base.OrderBy_anon(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_anon2(bool isAsync)
        {
            await base.OrderBy_anon2(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_client_mixed(bool isAsync)
        {
            await base.OrderBy_client_mixed(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_multiple_queries(bool isAsync)
        {
            await base.OrderBy_multiple_queries(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Take_Distinct(bool isAsync)
        {
            await base.Take_Distinct(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Distinct_Take(bool isAsync)
        {
            await base.Distinct_Take(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Distinct_Take_Count(bool isAsync)
        {
            await base.Distinct_Take_Count(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task OrderBy_shadow(bool isAsync)
        {
            await base.OrderBy_shadow(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_multiple(bool isAsync)
        {
            await base.OrderBy_multiple(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_ThenBy_Any(bool isAsync)
        {
            await base.OrderBy_ThenBy_Any(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_correlated_subquery1(bool isAsync)
        {
            await base.OrderBy_correlated_subquery1(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_correlated_subquery2(bool isAsync)
        {
            await base.OrderBy_correlated_subquery2(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_subquery_recursive_trivial(bool isAsync)
        {
            await base.Where_subquery_recursive_trivial(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_query_composition4(bool isAsync)
        {
            await base.Where_query_composition4(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Select_DTO_distinct_translated_to_server()
        {
            base.Select_DTO_distinct_translated_to_server();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Select_DTO_constructor_distinct_translated_to_server()
        {
            base.Select_DTO_constructor_distinct_translated_to_server();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Select_DTO_with_member_init_distinct_translated_to_server()
        {
            base.Select_DTO_with_member_init_distinct_translated_to_server();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Select_nested_collection_count_using_DTO()
        {
            base.Select_nested_collection_count_using_DTO();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_DTO_with_member_init_distinct_in_subquery_translated_to_server(bool isAsync)
        {
            await base.Select_DTO_with_member_init_distinct_in_subquery_translated_to_server(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Select_DTO_with_member_init_distinct_in_subquery_used_in_projection_translated_to_server()
        {
            base.Select_DTO_with_member_init_distinct_in_subquery_used_in_projection_translated_to_server();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_correlated_subquery_projection(bool isAsync)
        {
            await base.Select_correlated_subquery_projection(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_correlated_subquery_filtered(bool isAsync)
        {
            await base.Select_correlated_subquery_filtered(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_correlated_subquery_ordered(bool isAsync)
        {
            await base.Select_correlated_subquery_ordered(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_subquery_on_bool(bool isAsync)
        {
            await base.Where_subquery_on_bool(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_subquery_on_collection(bool isAsync)
        {
            await base.Where_subquery_on_collection(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_many_cross_join_same_collection(bool isAsync)
        {
            await base.Select_many_cross_join_same_collection(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_null_coalesce_operator(bool isAsync)
        {
            await base.OrderBy_null_coalesce_operator(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_null_coalesce_operator(bool isAsync)
        {
            await base.Select_null_coalesce_operator(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_conditional_operator(bool isAsync)
        {
            await base.OrderBy_conditional_operator(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_conditional_operator_where_condition_null(bool isAsync)
        {
            await base.OrderBy_conditional_operator_where_condition_null(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_comparison_operator(bool isAsync)
        {
            await base.OrderBy_comparison_operator(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Projection_null_coalesce_operator(bool isAsync)
        {
            await base.Projection_null_coalesce_operator(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Filter_coalesce_operator(bool isAsync)
        {
            await base.Filter_coalesce_operator(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND ((c[""CompanyName""] ?? c[""ContactName""]) = ""The Big Cheese""))");
        }

        public override async Task Take_skip_null_coalesce_operator(bool isAsync)
        {
            await base.Take_skip_null_coalesce_operator(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_take_null_coalesce_operator(bool isAsync)
        {
            await base.Select_take_null_coalesce_operator(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_take_skip_null_coalesce_operator(bool isAsync)
        {
            await base.Select_take_skip_null_coalesce_operator(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_take_skip_null_coalesce_operator2(bool isAsync)
        {
            await base.Select_take_skip_null_coalesce_operator2(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_take_skip_null_coalesce_operator3(bool isAsync)
        {
            await base.Select_take_skip_null_coalesce_operator3(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Selected_column_can_coalesce()
        {
            base.Selected_column_can_coalesce();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task DateTime_parse_is_parameterized(bool isAsync)
        {
            await base.DateTime_parse_is_parameterized(isAsync);

            AssertSql(
                @"@__Parse_0='1998-01-01T12:00:00'

SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND (c[""OrderDate""] > @__Parse_0))");
        }

        public override void Random_next_is_not_funcletized_1()
        {
            base.Random_next_is_not_funcletized_1();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Random_next_is_not_funcletized_2()
        {
            base.Random_next_is_not_funcletized_2();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Random_next_is_not_funcletized_3()
        {
            base.Random_next_is_not_funcletized_3();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Random_next_is_not_funcletized_4()
        {
            base.Random_next_is_not_funcletized_4();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Random_next_is_not_funcletized_5()
        {
            base.Random_next_is_not_funcletized_5();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override void Random_next_is_not_funcletized_6()
        {
            base.Random_next_is_not_funcletized_6();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Environment_newline_is_funcletized(bool isAsync)
        {
            await base.Environment_newline_is_funcletized(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task String_concat_with_navigation1(bool isAsync)
        {
            await base.String_concat_with_navigation1(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task String_concat_with_navigation2(bool isAsync)
        {
            await base.String_concat_with_navigation2(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Select_bitwise_or()
        {
            base.Select_bitwise_or();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Select_bitwise_or_multiple()
        {
            base.Select_bitwise_or_multiple();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Select_bitwise_and()
        {
            base.Select_bitwise_and();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Select_bitwise_and_or()
        {
            base.Select_bitwise_and_or();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_bitwise_or_with_logical_or(bool isAsync)
        {
            await base.Where_bitwise_or_with_logical_or(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_bitwise_and_with_logical_and(bool isAsync)
        {
            await base.Where_bitwise_and_with_logical_and(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_bitwise_or_with_logical_and(bool isAsync)
        {
            await base.Where_bitwise_or_with_logical_and(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_bitwise_and_with_logical_or(bool isAsync)
        {
            await base.Where_bitwise_and_with_logical_or(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Select_bitwise_or_with_logical_or()
        {
            base.Select_bitwise_or_with_logical_or();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Select_bitwise_and_with_logical_and()
        {
            base.Select_bitwise_and_with_logical_and();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Handle_materialization_properly_when_more_than_two_query_sources_are_involved(bool isAsync)
        {
            await base.Handle_materialization_properly_when_more_than_two_query_sources_are_involved(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Parameter_extraction_short_circuits_1(bool isAsync)
        {
            await base.Parameter_extraction_short_circuits_1(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Parameter_extraction_short_circuits_2(bool isAsync)
        {
            await base.Parameter_extraction_short_circuits_2(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Parameter_extraction_short_circuits_3(bool isAsync)
        {
            await base.Parameter_extraction_short_circuits_3(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Subquery_member_pushdown_does_not_change_original_subquery_model(bool isAsync)
        {
            await base.Subquery_member_pushdown_does_not_change_original_subquery_model(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Query_expression_with_to_string_and_contains(bool isAsync)
        {
            await base.Query_expression_with_to_string_and_contains(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Select_expression_long_to_string(bool isAsync)
        {
            await base.Select_expression_long_to_string(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_expression_int_to_string(bool isAsync)
        {
            await base.Select_expression_int_to_string(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task ToString_with_formatter_is_evaluated_on_the_client(bool isAsync)
        {
            await base.ToString_with_formatter_is_evaluated_on_the_client(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_expression_other_to_string(bool isAsync)
        {
            await base.Select_expression_other_to_string(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_expression_date_add_year(bool isAsync)
        {
            await base.Select_expression_date_add_year(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_expression_datetime_add_month(bool isAsync)
        {
            await base.Select_expression_datetime_add_month(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_expression_datetime_add_hour(bool isAsync)
        {
            await base.Select_expression_datetime_add_hour(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_expression_datetime_add_minute(bool isAsync)
        {
            await base.Select_expression_datetime_add_minute(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_expression_datetime_add_second(bool isAsync)
        {
            await base.Select_expression_datetime_add_second(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_expression_date_add_milliseconds_above_the_range(bool isAsync)
        {
            await base.Select_expression_date_add_milliseconds_above_the_range(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_expression_date_add_milliseconds_below_the_range(bool isAsync)
        {
            await base.Select_expression_date_add_milliseconds_below_the_range(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_expression_date_add_milliseconds_large_number_divided(bool isAsync)
        {
            await base.Select_expression_date_add_milliseconds_large_number_divided(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_expression_references_are_updated_correctly_with_subquery(bool isAsync)
        {
            await base.Select_expression_references_are_updated_correctly_with_subquery(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void DefaultIfEmpty_without_group_join()
        {
            base.DefaultIfEmpty_without_group_join();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""City""] = ""London""))");
        }

        public override async Task DefaultIfEmpty_in_subquery(bool isAsync)
        {
            await base.DefaultIfEmpty_in_subquery(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task DefaultIfEmpty_in_subquery_nested(bool isAsync)
        {
            await base.DefaultIfEmpty_in_subquery_nested(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""City""] = ""Seattle""))");
        }

        public override async Task OrderBy_skip_take(bool isAsync)
        {
            await base.OrderBy_skip_take(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_skip_skip_take(bool isAsync)
        {
            await base.OrderBy_skip_skip_take(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_skip_take_take(bool isAsync)
        {
            await base.OrderBy_skip_take_take(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_skip_take_take_take_take(bool isAsync)
        {
            await base.OrderBy_skip_take_take_take_take(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_skip_take_skip_take_skip(bool isAsync)
        {
            await base.OrderBy_skip_take_skip_take_skip(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_skip_take_distinct(bool isAsync)
        {
            await base.OrderBy_skip_take_distinct(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_coalesce_take_distinct(bool isAsync)
        {
            await base.OrderBy_coalesce_take_distinct(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_coalesce_skip_take_distinct(bool isAsync)
        {
            await base.OrderBy_coalesce_skip_take_distinct(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_coalesce_skip_take_distinct_take(bool isAsync)
        {
            await base.OrderBy_coalesce_skip_take_distinct_take(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_skip_take_distinct_orderby_take(bool isAsync)
        {
            await base.OrderBy_skip_take_distinct_orderby_take(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task No_orderby_added_for_fully_translated_manually_constructed_LOJ(bool isAsync)
        {
            await base.No_orderby_added_for_fully_translated_manually_constructed_LOJ(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Employee"")");
        }

        public override async Task No_orderby_added_for_client_side_GroupJoin_dependent_to_principal_LOJ(bool isAsync)
        {
            await base.No_orderby_added_for_client_side_GroupJoin_dependent_to_principal_LOJ(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task No_orderby_added_for_client_side_GroupJoin_dependent_to_principal_LOJ_with_additional_join_condition1(bool isAsync)
        {
            await base.No_orderby_added_for_client_side_GroupJoin_dependent_to_principal_LOJ_with_additional_join_condition1(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task No_orderby_added_for_client_side_GroupJoin_dependent_to_principal_LOJ_with_additional_join_condition2(bool isAsync)
        {
            await base.No_orderby_added_for_client_side_GroupJoin_dependent_to_principal_LOJ_with_additional_join_condition2(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Orderby_added_for_client_side_GroupJoin_principal_to_dependent_LOJ(bool isAsync)
        {
            await base.Orderby_added_for_client_side_GroupJoin_principal_to_dependent_LOJ(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Contains_with_DateTime_Date(bool isAsync)
        {
            await base.Contains_with_DateTime_Date(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Contains_with_subquery_involving_join_binds_to_correct_table(bool isAsync)
        {
            await base.Contains_with_subquery_involving_join_binds_to_correct_table(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Complex_query_with_repeated_query_model_compiles_correctly(bool isAsync)
        {
            await base.Complex_query_with_repeated_query_model_compiles_correctly(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Complex_query_with_repeated_nested_query_model_compiles_correctly(bool isAsync)
        {
            await base.Complex_query_with_repeated_nested_query_model_compiles_correctly(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Anonymous_member_distinct_where(bool isAsync)
        {
            await base.Anonymous_member_distinct_where(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Anonymous_member_distinct_orderby(bool isAsync)
        {
            await base.Anonymous_member_distinct_orderby(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Anonymous_member_distinct_result(bool isAsync)
        {
            await base.Anonymous_member_distinct_result(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Anonymous_complex_distinct_where(bool isAsync)
        {
            await base.Anonymous_complex_distinct_where(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Anonymous_complex_distinct_orderby(bool isAsync)
        {
            await base.Anonymous_complex_distinct_orderby(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Anonymous_complex_distinct_result(bool isAsync)
        {
            await base.Anonymous_complex_distinct_result(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Anonymous_complex_orderby(bool isAsync)
        {
            await base.Anonymous_complex_orderby(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Anonymous_subquery_orderby(bool isAsync)
        {
            await base.Anonymous_subquery_orderby(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task DTO_member_distinct_where(bool isAsync)
        {
            await base.DTO_member_distinct_where(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task DTO_member_distinct_orderby(bool isAsync)
        {
            await base.DTO_member_distinct_orderby(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task DTO_member_distinct_result(bool isAsync)
        {
            await base.DTO_member_distinct_result(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task DTO_complex_distinct_where(bool isAsync)
        {
            await base.DTO_complex_distinct_where(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task DTO_complex_distinct_orderby(bool isAsync)
        {
            await base.DTO_complex_distinct_orderby(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task DTO_complex_distinct_result(bool isAsync)
        {
            await base.DTO_complex_distinct_result(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task DTO_complex_orderby(bool isAsync)
        {
            await base.DTO_complex_orderby(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task DTO_subquery_orderby(bool isAsync)
        {
            await base.DTO_subquery_orderby(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Include_with_orderby_skip_preserves_ordering(bool isAsync)
        {
            await base.Include_with_orderby_skip_preserves_ordering(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND ((c[""CustomerID""] != ""VAFFE"") AND (c[""CustomerID""] != ""ALFKI""))");
        }

        public override async Task Int16_parameter_can_be_used_for_int_column(bool isAsync)
        {
            await base.Int16_parameter_can_be_used_for_int_column(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND (c[""OrderID""] = 10300))");
        }

        public override async Task Subquery_is_null_translated_correctly(bool isAsync)
        {
            await base.Subquery_is_null_translated_correctly(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Subquery_is_not_null_translated_correctly(bool isAsync)
        {
            await base.Subquery_is_not_null_translated_correctly(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_take_average(bool isAsync)
        {
            await base.Select_take_average(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_take_count(bool isAsync)
        {
            await base.Select_take_count(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_orderBy_take_count(bool isAsync)
        {
            await base.Select_orderBy_take_count(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_take_long_count(bool isAsync)
        {
            await base.Select_take_long_count(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_orderBy_take_long_count(bool isAsync)
        {
            await base.Select_orderBy_take_long_count(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_take_max(bool isAsync)
        {
            await base.Select_take_max(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_take_min(bool isAsync)
        {
            await base.Select_take_min(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_take_sum(bool isAsync)
        {
            await base.Select_take_sum(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_skip_average(bool isAsync)
        {
            await base.Select_skip_average(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_skip_count(bool isAsync)
        {
            await base.Select_skip_count(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_orderBy_skip_count(bool isAsync)
        {
            await base.Select_orderBy_skip_count(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_skip_long_count(bool isAsync)
        {
            await base.Select_skip_long_count(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_orderBy_skip_long_count(bool isAsync)
        {
            await base.Select_orderBy_skip_long_count(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_skip_max(bool isAsync)
        {
            await base.Select_skip_max(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_skip_min(bool isAsync)
        {
            await base.Select_skip_min(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_skip_sum(bool isAsync)
        {
            await base.Select_skip_sum(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_distinct_average(bool isAsync)
        {
            await base.Select_distinct_average(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_distinct_count(bool isAsync)
        {
            await base.Select_distinct_count(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_distinct_long_count(bool isAsync)
        {
            await base.Select_distinct_long_count(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_distinct_max(bool isAsync)
        {
            await base.Select_distinct_max(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_distinct_min(bool isAsync)
        {
            await base.Select_distinct_min(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_distinct_sum(bool isAsync)
        {
            await base.Select_distinct_sum(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Comparing_to_fixed_string_parameter(bool isAsync)
        {
            await base.Comparing_to_fixed_string_parameter(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Comparing_entities_using_Equals(bool isAsync)
        {
            await base.Comparing_entities_using_Equals(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Comparing_different_entity_types_using_Equals(bool isAsync)
        {
            await base.Comparing_different_entity_types_using_Equals(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Comparing_entity_to_null_using_Equals(bool isAsync)
        {
            await base.Comparing_entity_to_null_using_Equals(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Comparing_navigations_using_Equals(bool isAsync)
        {
            await base.Comparing_navigations_using_Equals(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Comparing_navigations_using_static_Equals(bool isAsync)
        {
            await base.Comparing_navigations_using_static_Equals(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Comparing_non_matching_entities_using_Equals(bool isAsync)
        {
            await base.Comparing_non_matching_entities_using_Equals(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Comparing_non_matching_collection_navigations_using_Equals(bool isAsync)
        {
            await base.Comparing_non_matching_collection_navigations_using_Equals(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Comparing_collection_navigation_to_null(bool isAsync)
        {
            await base.Comparing_collection_navigation_to_null(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""CustomerID""] = null))");
        }

        public override async Task Comparing_collection_navigation_to_null_complex(bool isAsync)
        {
            await base.Comparing_collection_navigation_to_null_complex(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""OrderDetail"")");
        }

        public override async Task Compare_collection_navigation_with_itself(bool isAsync)
        {
            await base.Compare_collection_navigation_with_itself(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Compare_two_collection_navigations_with_different_query_sources(bool isAsync)
        {
            await base.Compare_two_collection_navigations_with_different_query_sources(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Compare_two_collection_navigations_using_equals(bool isAsync)
        {
            await base.Compare_two_collection_navigations_using_equals(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Compare_two_collection_navigations_with_different_property_chains(bool isAsync)
        {
            await base.Compare_two_collection_navigations_with_different_property_chains(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_ThenBy_same_column_different_direction(bool isAsync)
        {
            await base.OrderBy_ThenBy_same_column_different_direction(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_OrderBy_same_column_different_direction(bool isAsync)
        {
            await base.OrderBy_OrderBy_same_column_different_direction(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Complex_nested_query_doesnt_try_binding_to_grandparent_when_parent_returns_complex_result(bool isAsync)
        {
            await base.Complex_nested_query_doesnt_try_binding_to_grandparent_when_parent_returns_complex_result(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Complex_nested_query_properly_binds_to_grandparent_when_parent_returns_scalar_result(bool isAsync)
        {
            await base.Complex_nested_query_properly_binds_to_grandparent_when_parent_returns_scalar_result(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task OrderBy_Dto_projection_skip_take(bool isAsync)
        {
            await base.OrderBy_Dto_projection_skip_take(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Streaming_chained_sync_query()
        {
            base.Streaming_chained_sync_query();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Join_take_count_works(bool isAsync)
        {
            await base.Join_take_count_works(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND ((c[""OrderID""] > 690) AND (c[""OrderID""] < 710)))");
        }

        public override async Task OrderBy_empty_list_contains(bool isAsync)
        {
            await base.OrderBy_empty_list_contains(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task OrderBy_empty_list_does_not_contains(bool isAsync)
        {
            await base.OrderBy_empty_list_does_not_contains(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Manual_expression_tree_typed_null_equality()
        {
            base.Manual_expression_tree_typed_null_equality();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Let_subquery_with_multiple_occurences(bool isAsync)
        {
            await base.Let_subquery_with_multiple_occurences(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Let_entity_equality_to_null(bool isAsync)
        {
            await base.Let_entity_equality_to_null(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Let_entity_equality_to_other_entity(bool isAsync)
        {
            await base.Let_entity_equality_to_other_entity(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task SelectMany_after_client_method(bool isAsync)
        {
            await base.SelectMany_after_client_method(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Collection_navigation_equal_to_null_for_subquery(bool isAsync)
        {
            await base.Collection_navigation_equal_to_null_for_subquery(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Dependent_to_principal_navigation_equal_to_null_for_subquery(bool isAsync)
        {
            await base.Dependent_to_principal_navigation_equal_to_null_for_subquery(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Collection_navigation_equality_rewrite_for_subquery(bool isAsync)
        {
            await base.Collection_navigation_equality_rewrite_for_subquery(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        private void AssertSql(params string[] expected)
            => Fixture.TestSqlLoggerFactory.AssertBaseline(expected);

        private void AssertContainsSql(params string[] expected)
            => Fixture.TestSqlLoggerFactory.AssertBaseline(expected, assertOrder: false);

        protected override void ClearLog()
            => Fixture.TestSqlLoggerFactory.Clear();
    }
}
