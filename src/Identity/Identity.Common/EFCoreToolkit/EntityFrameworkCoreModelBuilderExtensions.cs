using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Identity.Common.EFCoreToolkit
{
    public static class EntityFrameworkCoreModelBuilderExtensions
    {
        public static void SetDecimalPrecision(this ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
                                                              .SelectMany(t => t.GetProperties())
                                                              .Where(p => p.ClrType == typeof(decimal)
                                                                          || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18, 6)");
            }
        }

        public static void AddDateTimeOffsetConverter(this ModelBuilder builder)
        {
            // SQLite does not support DateTimeOffset
            foreach (var property in builder.Model.GetEntityTypes()
                                                  .SelectMany(t => t.GetProperties())
                                                  .Where(p => p.ClrType == typeof(DateTimeOffset)))
            {
                property.SetValueConverter(
                     new ValueConverter<DateTimeOffset, DateTime>(
                          convertToProviderExpression: dateTimeOffset => dateTimeOffset.UtcDateTime,
                          convertFromProviderExpression: dateTime => new DateTimeOffset(dateTime)
                    ));
            }

            foreach (var property in builder.Model.GetEntityTypes()
                                                  .SelectMany(t => t.GetProperties())
                                                  .Where(p => p.ClrType == typeof(DateTimeOffset?)))
            {
                property.SetValueConverter(
                     new ValueConverter<DateTimeOffset?, DateTime>(
                          convertToProviderExpression: dateTimeOffset => dateTimeOffset.Value.UtcDateTime,
                          convertFromProviderExpression: dateTime => new DateTimeOffset(dateTime)
                    ));
            }
        }

        public static void AddDateTimeUtcKindConverter(this ModelBuilder builder)
        {
            // If you store a DateTime object to the DB with a DateTimeKind of either `Utc` or `Local`,
            // when you read that record back from the DB you'll get a DateTime object whose kind is `Unspecified`.
            // Here is a fix for it!
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                        v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                        v => !v.HasValue ? v : (v.Value.Kind == DateTimeKind.Utc ? v : v.Value.ToUniversalTime()),
                        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            foreach (var property in builder.Model.GetEntityTypes()
                                                  .SelectMany(t => t.GetProperties()))
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetValueConverter(dateTimeConverter);
                }

                if (property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(nullableDateTimeConverter);
                }
            }
        }
        public static void AddQueryFilter<T>(this EntityTypeBuilder entityTypeBuilder, Expression<Func<T, bool>> expression)
        {
            var parameterType = Expression.Parameter(entityTypeBuilder.Metadata.ClrType);
            var expressionFilter = ReplacingExpressionVisitor.Replace(
                expression.Parameters.Single(), parameterType, expression.Body);

            var currentQueryFilter = entityTypeBuilder.Metadata.GetQueryFilter();
            if (currentQueryFilter != null)
            {
                var currentExpressionFilter = ReplacingExpressionVisitor.Replace(
                    currentQueryFilter.Parameters.Single(), parameterType, currentQueryFilter.Body);
                expressionFilter = Expression.AndAlso(currentExpressionFilter, expressionFilter);
            }

            var lambdaExpression = Expression.Lambda(expressionFilter, parameterType);
            entityTypeBuilder.HasQueryFilter(lambdaExpression);
        }
    }
}