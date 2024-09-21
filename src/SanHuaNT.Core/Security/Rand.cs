﻿using System.Reflection;
using System.Security.Cryptography;
using SanHuaNT.Collections;
using SanHuaNT.Reflection;

namespace SanHuaNT.Security;

/// <summary>随机数</summary>
public static class Rand
{
    /// <summary>返回一个小于所指定最大值的非负随机数</summary>
    /// <param name="max">返回的随机数的上界（随机数不能取该上界值）</param>
    /// <returns></returns>
    public static Int32 Next(Int32 max = Int32.MaxValue) => RandomNumberGenerator.GetInt32(max);

    /// <summary>返回一个指定范围内的随机数</summary>
    /// <remarks>
    /// 调用平均耗时37.76ns，其中GC耗时77.56%
    /// </remarks>
    /// <param name="min">返回的随机数的下界（随机数可取该下界值）</param>
    /// <param name="max">返回的随机数的上界（随机数不能取该上界值）</param>
    /// <returns></returns>
    public static Int32 Next(Int32 min, Int32 max) => RandomNumberGenerator.GetInt32(min, max);

    /// <summary>返回指定长度随机字节数组</summary>
    /// <remarks>
    /// 调用平均耗时5.46ns，其中GC耗时15%
    /// </remarks>
    /// <param name="count"></param>
    /// <returns></returns>
    public static Byte[] NextBytes(Int32 count)
    {
        return RandomNumberGenerator.GetBytes(count);
    }

    /// <summary>返回指定长度随机字符串</summary>
    /// <param name="length">长度</param>
    /// <param name="symbol">是否包含符号</param>
    /// <returns></returns>
    public static String NextString(Int32 length, Boolean symbol = false)
    {
        var sb = Pool.StringBuilder.Get();
        for (var i = 0; i < length; i++)
        {
            var ch = ' ';
            if (symbol)
                ch = (Char)Next(' ', 0x7F);
            else
            {
                var n = Next(0, 10 + 26 + 26);
                if (n < 10)
                    ch = (Char)('0' + n);
                else if (n < 10 + 26)
                    ch = (Char)('A' + n - 10);
                else
                    ch = (Char)('a' + n - 10 - 26);
            }
            sb.Append(ch);
        }

        return sb.Put(true);
    }

    /// <summary>随机填充指定对象的属性。可用于构造随机数据进行测试</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T Fill<T>(T value)
    {
        if (value == null) return value;

        foreach (var pi in value.GetType().GetProperties(true))
        {
            // 可空类型，有一定记录填充null
            var type = pi.PropertyType;
            if (type.IsNullable())
            {
                // 10%几率填充null
                if (Next(0, 10) == 0)
                {
                    pi.SetValue(value, null);
                    continue;
                }

                type = Nullable.GetUnderlyingType(type) ?? type;
            }

            // 给基础类型填充数据
            var code = type.GetTypeCode();
            switch (code)
            {
                case TypeCode.Empty:
                case TypeCode.Object:
                case TypeCode.DBNull:
                    break;
                case TypeCode.Boolean:
                    pi.SetValue(value, Next(2) > 0);
                    break;
                case TypeCode.Char:
                    pi.SetValue(value, (Char)Next(Char.MinValue, Char.MaxValue));
                    break;
                case TypeCode.SByte:
                    pi.SetValue(value, (SByte)Next(SByte.MinValue, SByte.MaxValue));
                    break;
                case TypeCode.Byte:
                    pi.SetValue(value, (Byte)Next(Byte.MinValue, Byte.MaxValue));
                    break;
                case TypeCode.Int16:
                    pi.SetValue(value, (Int16)Next(Int16.MinValue, Int16.MaxValue));
                    break;
                case TypeCode.UInt16:
                    pi.SetValue(value, (UInt16)Next(UInt16.MinValue, UInt16.MaxValue));
                    break;
                case TypeCode.Int32:
                    pi.SetValue(value, Next());
                    break;
                case TypeCode.UInt32:
                    pi.SetValue(value, (UInt32)Next());
                    break;
                case TypeCode.Int64:
                    pi.SetValue(value, (Int64)Next() * Next());
                    break;
                case TypeCode.UInt64:
                    pi.SetValue(value, (UInt64)Next() * (UInt64)Next());
                    break;
                case TypeCode.Single:
                    pi.SetValue(value, Next() / 100f);
                    break;
                case TypeCode.Double:
                    pi.SetValue(value, Next() / 10000d);
                    break;
                case TypeCode.Decimal:
                    pi.SetValue(value, (Decimal)Next() / 10000);
                    break;
                case TypeCode.DateTime:
                    pi.SetValue(value, new DateTime(2000, 1, 1).AddSeconds(Next(20 * 365 * 24 * 3600)));
                    break;
                case TypeCode.String:
                    pi.SetValue(value, NextString(8));
                    break;
                default:
                    break;
            }

            // 支持特殊类型
            if (code == TypeCode.Object)
            {
                if (type == typeof(Guid))
                    pi.SetValue(value, Guid.NewGuid());
                else if (type == typeof(DateTimeOffset))
                    pi.SetValue(value, new DateTimeOffset(new DateTime(2000, 1, 1).AddSeconds(Next(20 * 365 * 24 * 3600))));
                else if (type == typeof(TimeSpan))
                    pi.SetValue(value, new TimeSpan(Next(20 * 24 * 3600 * 1000)));
                else if (type == typeof(DateOnly))
                    pi.SetValue(value, new DateOnly(Next(1000, 2300), Next(1, 13), Next(1, 29)));
                else if (type == typeof(TimeOnly))
                    pi.SetValue(value, new TimeOnly(Next(0, 24), Next(0, 60), Next(0, 60), Next(0, 1000)));

            }
        }

        return value;
    }
}