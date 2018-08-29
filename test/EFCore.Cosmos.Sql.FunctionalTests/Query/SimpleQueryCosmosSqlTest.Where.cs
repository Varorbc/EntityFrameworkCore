﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.TestModels.Northwind;
using Microsoft.EntityFrameworkCore.TestUtilities.Xunit;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Cosmos.Sql.Query
{
    public partial class SimpleQueryCosmosSqlTest
    {
        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async virtual Task Where_add(bool isAsync)
        {
            await AssertQuery<Order>(
                isAsync,
                os => os.Where(o => o.OrderID + 10 == 10258),
                entryCount: 1);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND ((c[""OrderID""] + 10) = 10258))");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async virtual Task Where_subtract(bool isAsync)
        {
            await AssertQuery<Order>(
                isAsync,
                os => os.Where(o => o.OrderID - 10 == 10238),
                entryCount: 1);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND ((c[""OrderID""] - 10) = 10238))");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async virtual Task Where_multiply(bool isAsync)
        {
            await AssertQuery<Order>(
                isAsync,
                os => os.Where(o => o.OrderID * 1 == 10248),
                entryCount: 1);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND ((c[""OrderID""] * 1) = 10248))");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async virtual Task Where_divide(bool isAsync)
        {
            await AssertQuery<Order>(
                isAsync,
                os => os.Where(o => o.OrderID / 1 == 10248),
                entryCount: 1);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND ((c[""OrderID""] / 1) = 10248))");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async virtual Task Where_modulo(bool isAsync)
        {
            await AssertQuery<Order>(
                isAsync,
                os => os.Where(o => o.OrderID % 10248 == 0),
                entryCount: 1);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND ((c[""OrderID""] % 10248) = 0))");
        }

        public override async Task Where_bitwise_or(bool isAsync)
        {
            await base.Where_bitwise_or(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND ((c[""OrderID""] | 10248) = 10248))");
        }

        public override async Task Where_bitwise_and(bool isAsync)
        {
            await base.Where_bitwise_and(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND ((c[""OrderID""] & 11067) = 11067))");
        }

        public override async Task Where_bitwise_xor(bool isAsync)
        {
            await base.Where_bitwise_xor(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND ((c[""OrderID""] ^ 10248) = 0))");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async virtual Task Where_bitwise_leftshift(bool isAsync)
        {
            await AssertQuery<Order>(
                isAsync,
                os => os.Where(o => (o.OrderID << 1) == 20496),
                entryCount: 1);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND ((c[""OrderID""] << 1) = 20496))");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async virtual Task Where_bitwise_rightshift(bool isAsync)
        {
            await AssertQuery<Order>(
                isAsync,
                os => os.Where(o => (o.OrderID >> 1) == 5124),
                entryCount: 2);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND ((c[""OrderID""] >> 1) = 5124))");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async virtual Task Where_logical_and(bool isAsync)
        {
            await AssertQuery<Customer>(
                isAsync,
                cs => cs.Where(c => c.City == "Seattle" && c.ContactTitle == "Owner"),
                entryCount: 1);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND ((c[""City""] = ""Seattle"") AND (c[""ContactTitle""] = ""Owner"")))");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async virtual Task Where_logical_or(bool isAsync)
        {
            await AssertQuery<Customer>(
                isAsync,
                cs => cs.Where(c => c.CustomerID == "ALFKI" || c.CustomerID == "ANATR"),
                entryCount: 2);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND ((c[""CustomerID""] = ""ALFKI"") OR (c[""CustomerID""] = ""ANATR"")))");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async virtual Task Where_logical_not(bool isAsync)
        {
            await AssertQuery<Customer>(
                isAsync,
                cs => cs.Where(c => !(c.City != "Seattle")),
                entryCount: 1);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND NOT((c[""City""] != ""Seattle"")))");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async virtual Task Where_equality(bool isAsync)
        {
            await AssertQuery<Employee>(
                isAsync,
                es => es.Where(e => e.ReportsTo == 2),
                entryCount: 5);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Employee"") AND (c[""ReportsTo""] = 2))");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async virtual Task Where_inequality(bool isAsync)
        {
            await AssertQuery<Employee>(
                isAsync,
                es => es.Where(e => e.ReportsTo != 2),
                entryCount: 4);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Employee"") AND (c[""ReportsTo""] != 2))");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async virtual Task Where_greaterthan(bool isAsync)
        {
            await AssertQuery<Employee>(
                isAsync,
                es => es.Where(e => e.ReportsTo > 2),
                entryCount: 3);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Employee"") AND (c[""ReportsTo""] > 2))");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async virtual Task Where_greaterthanorequal(bool isAsync)
        {
            await AssertQuery<Employee>(
                isAsync,
                es => es.Where(e => e.ReportsTo >= 2),
                entryCount: 8);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Employee"") AND (c[""ReportsTo""] >= 2))");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async virtual Task Where_lessthan(bool isAsync)
        {
            await AssertQuery<Employee>(
                isAsync,
                es => es.Where(e => e.ReportsTo < 2),
                entryCount: 0);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Employee"") AND (c[""ReportsTo""] < 2))");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async virtual Task Where_lessthanorequal(bool isAsync)
        {
            await AssertQuery<Employee>(
                isAsync,
                es => es.Where(e => e.ReportsTo <= 2),
                entryCount: 5);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Employee"") AND (c[""ReportsTo""] <= 2))");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async virtual Task Where_string_concat(bool isAsync)
        {
            await AssertQuery<Customer>(
                isAsync,
                cs => cs.Where(c => c.CustomerID + "END" == "ALFKIEND"),
                entryCount: 1);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND ((c[""CustomerID""] || ""END"") = ""ALFKIEND""))");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async virtual Task Where_unary_minus(bool isAsync)
        {
            await AssertQuery<Order>(
                isAsync,
                os => os.Where(o => -o.OrderID == -10248),
                entryCount: 1);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND (-(c[""OrderID""]) = -10248))");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async virtual Task Where_bitwise_not(bool isAsync)
        {
            await AssertQuery<Order>(
                isAsync,
                os => os.Where(o => ~o.OrderID == -10249),
                entryCount: 1);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND (~(c[""OrderID""]) = -10249))");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async virtual Task Where_ternary(bool isAsync)
        {
            await AssertQuery<Customer>(
                isAsync,
#pragma warning disable IDE0029 // Use coalesce expression
                cs => cs.Where(c => (c.Region != null ? c.Region : "SP") == "BC"),
#pragma warning restore IDE0029 // Use coalesce expression
                entryCount: 2);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (((c[""Region""] != null) ? c[""Region""] : ""SP"") = ""BC""))");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async virtual Task Where_coalesce(bool isAsync)
        {
            await AssertQuery<Customer>(
                isAsync,
                cs => cs.Where(c => (c.Region ?? "SP") == "BC"),
                entryCount: 2);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND ((c[""Region""] ?? ""SP"") = ""BC""))");
        }

        public override async Task Where_simple(bool isAsync)
        {
            await base.Where_simple(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_as_queryable_expression(bool isAsync)
        {
            await base.Where_as_queryable_expression(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_simple_closure(bool isAsync)
        {
            await base.Where_simple_closure(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_indexer_closure(bool isAsync)
        {
            await base.Where_indexer_closure(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_dictionary_key_access_closure(bool isAsync)
        {
            await base.Where_dictionary_key_access_closure(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_tuple_item_closure(bool isAsync)
        {
            await base.Where_tuple_item_closure(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_named_tuple_item_closure(bool isAsync)
        {
            await base.Where_named_tuple_item_closure(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_simple_closure_constant(bool isAsync)
        {
            await base.Where_simple_closure_constant(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_simple_closure_via_query_cache(bool isAsync)
        {
            await base.Where_simple_closure_via_query_cache(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_method_call_nullable_type_closure_via_query_cache(bool isAsync)
        {
            await base.Where_method_call_nullable_type_closure_via_query_cache(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_method_call_nullable_type_reverse_closure_via_query_cache(bool isAsync)
        {
            await base.Where_method_call_nullable_type_reverse_closure_via_query_cache(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_method_call_closure_via_query_cache(bool isAsync)
        {
            await base.Where_method_call_closure_via_query_cache(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_field_access_closure_via_query_cache(bool isAsync)
        {
            await base.Where_field_access_closure_via_query_cache(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_property_access_closure_via_query_cache(bool isAsync)
        {
            await base.Where_property_access_closure_via_query_cache(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_static_field_access_closure_via_query_cache(bool isAsync)
        {
            await base.Where_static_field_access_closure_via_query_cache(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_static_property_access_closure_via_query_cache(bool isAsync)
        {
            await base.Where_static_property_access_closure_via_query_cache(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_nested_field_access_closure_via_query_cache(bool isAsync)
        {
            await base.Where_nested_field_access_closure_via_query_cache(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_nested_property_access_closure_via_query_cache(bool isAsync)
        {
            await base.Where_nested_property_access_closure_via_query_cache(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_new_instance_field_access_closure_via_query_cache(bool isAsync)
        {
            await base.Where_new_instance_field_access_closure_via_query_cache(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_simple_closure_via_query_cache_nullable_type(bool isAsync)
        {
            await base.Where_simple_closure_via_query_cache_nullable_type(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_simple_closure_via_query_cache_nullable_type_reverse(bool isAsync)
        {
            await base.Where_simple_closure_via_query_cache_nullable_type_reverse(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Where_subquery_closure_via_query_cache()
        {
            base.Where_subquery_closure_via_query_cache();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_simple_shadow(bool isAsync)
        {
            await base.Where_simple_shadow(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_simple_shadow_projection(bool isAsync)
        {
            await base.Where_simple_shadow_projection(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_shadow_subquery_FirstOrDefault(bool isAsync)
        {
            await base.Where_shadow_subquery_FirstOrDefault(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_client(bool isAsync)
        {
            await base.Where_client(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_subquery_correlated(bool isAsync)
        {
            await base.Where_subquery_correlated(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_subquery_correlated_client_eval(bool isAsync)
        {
            await base.Where_subquery_correlated_client_eval(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_client_and_server_top_level(bool isAsync)
        {
            await base.Where_client_and_server_top_level(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_client_or_server_top_level(bool isAsync)
        {
            await base.Where_client_or_server_top_level(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_client_and_server_non_top_level(bool isAsync)
        {
            await base.Where_client_and_server_non_top_level(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_client_deep_inside_predicate_and_server_top_level(bool isAsync)
        {
            await base.Where_client_deep_inside_predicate_and_server_top_level(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_equals_method_string(bool isAsync)
        {
            await base.Where_equals_method_string(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_equals_method_int(bool isAsync)
        {
            await base.Where_equals_method_int(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_equals_using_object_overload_on_mismatched_types(bool isAsync)
        {
            await base.Where_equals_using_object_overload_on_mismatched_types(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_equals_using_int_overload_on_mismatched_types(bool isAsync)
        {
            await base.Where_equals_using_int_overload_on_mismatched_types(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_equals_on_mismatched_types_nullable_int_long(bool isAsync)
        {
            await base.Where_equals_on_mismatched_types_nullable_int_long(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_equals_on_mismatched_types_nullable_long_nullable_int(bool isAsync)
        {
            await base.Where_equals_on_mismatched_types_nullable_long_nullable_int(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_equals_on_mismatched_types_int_nullable_int(bool isAsync)
        {
            await base.Where_equals_on_mismatched_types_int_nullable_int(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_equals_on_matched_nullable_int_types(bool isAsync)
        {
            await base.Where_equals_on_matched_nullable_int_types(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_equals_on_null_nullable_int_types(bool isAsync)
        {
            await base.Where_equals_on_null_nullable_int_types(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_comparison_nullable_type_not_null(bool isAsync)
        {
            await base.Where_comparison_nullable_type_not_null(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Employee"") AND (c[""Discriminator""] = ""Customer""))");
        }

        public override async Task Where_comparison_nullable_type_null(bool isAsync)
        {
            await base.Where_comparison_nullable_type_null(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_string_length(bool isAsync)
        {
            await base.Where_string_length(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_string_indexof(bool isAsync)
        {
            await base.Where_string_indexof(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_string_replace(bool isAsync)
        {
            await base.Where_string_replace(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_string_substring(bool isAsync)
        {
            await base.Where_string_substring(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_datetime_now(bool isAsync)
        {
            await base.Where_datetime_now(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_datetime_utcnow(bool isAsync)
        {
            await base.Where_datetime_utcnow(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_datetime_today(bool isAsync)
        {
            await base.Where_datetime_today(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_datetime_date_component(bool isAsync)
        {
            await base.Where_datetime_date_component(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_date_add_year_constant_component(bool isAsync)
        {
            await base.Where_date_add_year_constant_component(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_datetime_year_component(bool isAsync)
        {
            await base.Where_datetime_year_component(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_datetime_month_component(bool isAsync)
        {
            await base.Where_datetime_month_component(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_datetime_dayOfYear_component(bool isAsync)
        {
            await base.Where_datetime_dayOfYear_component(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_datetime_day_component(bool isAsync)
        {
            await base.Where_datetime_day_component(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_datetime_hour_component(bool isAsync)
        {
            await base.Where_datetime_hour_component(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_datetime_minute_component(bool isAsync)
        {
            await base.Where_datetime_minute_component(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_datetime_second_component(bool isAsync)
        {
            await base.Where_datetime_second_component(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_datetime_millisecond_component(bool isAsync)
        {
            await base.Where_datetime_millisecond_component(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_datetimeoffset_now_component(bool isAsync)
        {
            await base.Where_datetimeoffset_now_component(isAsync);
            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_datetimeoffset_utcnow_component(bool isAsync)
        {
            await base.Where_datetimeoffset_utcnow_component(isAsync);
            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_simple_reversed(bool isAsync)
        {
            await base.Where_simple_reversed(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_is_null(bool isAsync)
        {
            await base.Where_is_null(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_null_is_null(bool isAsync)
        {
            await base.Where_null_is_null(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_constant_is_null(bool isAsync)
        {
            await base.Where_constant_is_null(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_is_not_null(bool isAsync)
        {
            await base.Where_is_not_null(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_null_is_not_null(bool isAsync)
        {
            await base.Where_null_is_not_null(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_constant_is_not_null(bool isAsync)
        {
            await base.Where_constant_is_not_null(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_identity_comparison(bool isAsync)
        {
            await base.Where_identity_comparison(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_in_optimization_multiple(bool isAsync)
        {
            await base.Where_in_optimization_multiple(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_not_in_optimization1(bool isAsync)
        {
            await base.Where_not_in_optimization1(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_not_in_optimization2(bool isAsync)
        {
            await base.Where_not_in_optimization2(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_not_in_optimization3(bool isAsync)
        {
            await base.Where_not_in_optimization3(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_not_in_optimization4(bool isAsync)
        {
            await base.Where_not_in_optimization4(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_select_many_and(bool isAsync)
        {
            await base.Where_select_many_and(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_primitive(bool isAsync)
        {
            await base.Where_primitive(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_bool_member(bool isAsync)
        {
            await base.Where_bool_member(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_bool_member_false(bool isAsync)
        {
            await base.Where_bool_member_false(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_bool_client_side_negated(bool isAsync)
        {
            await base.Where_bool_client_side_negated(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_bool_member_negated_twice(bool isAsync)
        {
            await base.Where_bool_member_negated_twice(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_bool_member_shadow(bool isAsync)
        {
            await base.Where_bool_member_shadow(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_bool_member_false_shadow(bool isAsync)
        {
            await base.Where_bool_member_false_shadow(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_bool_member_equals_constant(bool isAsync)
        {
            await base.Where_bool_member_equals_constant(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_bool_member_in_complex_predicate(bool isAsync)
        {
            await base.Where_bool_member_in_complex_predicate(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_bool_member_compared_to_binary_expression(bool isAsync)
        {
            await base.Where_bool_member_compared_to_binary_expression(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_not_bool_member_compared_to_not_bool_member(bool isAsync)
        {
            await base.Where_not_bool_member_compared_to_not_bool_member(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_negated_boolean_expression_compared_to_another_negated_boolean_expression(bool isAsync)
        {
            await base.Where_negated_boolean_expression_compared_to_another_negated_boolean_expression(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_not_bool_member_compared_to_binary_expression(bool isAsync)
        {
            await base.Where_not_bool_member_compared_to_binary_expression(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_bool_parameter(bool isAsync)
        {
            await base.Where_bool_parameter(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_bool_parameter_compared_to_binary_expression(bool isAsync)
        {
            await base.Where_bool_parameter_compared_to_binary_expression(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_bool_member_and_parameter_compared_to_binary_expression_nested(bool isAsync)
        {
            await base.Where_bool_member_and_parameter_compared_to_binary_expression_nested(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_de_morgan_or_optimizated(bool isAsync)
        {
            await base.Where_de_morgan_or_optimizated(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_de_morgan_and_optimizated(bool isAsync)
        {
            await base.Where_de_morgan_and_optimizated(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_complex_negated_expression_optimized(bool isAsync)
        {
            await base.Where_complex_negated_expression_optimized(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_short_member_comparison(bool isAsync)
        {
            await base.Where_short_member_comparison(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Product"") AND (c[""Discriminator""] = ""Customer""))");
        }

        public override async Task Where_comparison_to_nullable_bool(bool isAsync)
        {
            await base.Where_comparison_to_nullable_bool(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_true(bool isAsync)
        {
            await base.Where_true(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_false(bool isAsync)
        {
            await base.Where_false(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_default(bool isAsync)
        {
            await base.Where_default(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_expression_invoke(bool isAsync)
        {
            await base.Where_expression_invoke(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_concat_string_int_comparison1(bool isAsync)
        {
            await base.Where_concat_string_int_comparison1(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_concat_string_int_comparison2(bool isAsync)
        {
            await base.Where_concat_string_int_comparison2(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_concat_string_int_comparison3(bool isAsync)
        {
            await base.Where_concat_string_int_comparison3(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_ternary_boolean_condition_true(bool isAsync)
        {
            await base.Where_ternary_boolean_condition_true(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_ternary_boolean_condition_false(bool isAsync)
        {
            await base.Where_ternary_boolean_condition_false(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_ternary_boolean_condition_with_another_condition(bool isAsync)
        {
            await base.Where_ternary_boolean_condition_with_another_condition(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_ternary_boolean_condition_with_false_as_result_true(bool isAsync)
        {
            await base.Where_ternary_boolean_condition_with_false_as_result_true(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_ternary_boolean_condition_with_false_as_result_false(bool isAsync)
        {
            await base.Where_ternary_boolean_condition_with_false_as_result_false(isAsync);

            AssertSql(
                @"@__flag_0='False'

SELECT c AS query
FROM root c
WHERE ((c[""Discriminator""] = ""Product"") AND (@__flag_0 ? (c[""UnitsInStock""] >= 20) : ""False""))");
        }

        public override async Task Where_compare_constructed_equal(bool isAsync)
        {
            await base.Where_compare_constructed_equal(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_compare_constructed_multi_value_equal(bool isAsync)
        {
            await base.Where_compare_constructed_multi_value_equal(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_compare_constructed_multi_value_not_equal(bool isAsync)
        {
            await base.Where_compare_constructed_multi_value_not_equal(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_compare_tuple_constructed_equal(bool isAsync)
        {
            await base.Where_compare_tuple_constructed_equal(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_compare_tuple_constructed_multi_value_equal(bool isAsync)
        {
            await base.Where_compare_tuple_constructed_multi_value_equal(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_compare_tuple_constructed_multi_value_not_equal(bool isAsync)
        {
            await base.Where_compare_tuple_constructed_multi_value_not_equal(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_compare_tuple_create_constructed_equal(bool isAsync)
        {
            await base.Where_compare_tuple_create_constructed_equal(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_compare_tuple_create_constructed_multi_value_equal(bool isAsync)
        {
            await base.Where_compare_tuple_create_constructed_multi_value_equal(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_compare_tuple_create_constructed_multi_value_not_equal(bool isAsync)
        {
            await base.Where_compare_tuple_create_constructed_multi_value_not_equal(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_compare_null(bool isAsync)
        {
            await base.Where_compare_null(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_Is_on_same_type(bool isAsync)
        {
            await base.Where_Is_on_same_type(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_chain(bool isAsync)
        {
            await base.Where_chain(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Where_navigation_contains()
        {
            base.Where_navigation_contains();

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_array_index(bool isAsync)
        {
            await base.Where_array_index(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_multiple_contains_in_subquery_with_or(bool isAsync)
        {
            await base.Where_multiple_contains_in_subquery_with_or(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_multiple_contains_in_subquery_with_and(bool isAsync)
        {
            await base.Where_multiple_contains_in_subquery_with_and(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_contains_on_navigation(bool isAsync)
        {
            await base.Where_contains_on_navigation(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_subquery_FirstOrDefault_is_null(bool isAsync)
        {
            await base.Where_subquery_FirstOrDefault_is_null(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Where_subquery_FirstOrDefault_compared_to_entity(bool isAsync)
        {
            await base.Where_subquery_FirstOrDefault_compared_to_entity(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Time_of_day_datetime(bool isAsync)
        {
            await base.Time_of_day_datetime(isAsync);

            AssertSql(
                @"SELECT c AS query
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }
    }
}
