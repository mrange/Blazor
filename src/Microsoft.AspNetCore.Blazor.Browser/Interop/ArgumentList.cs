using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.AspNetCore.Blazor.Browser.Interop
{
    internal class ArgumentList
    {
        private const BindingFlags DeserializeFlags = BindingFlags.Static | BindingFlags.NonPublic;

        public static ArgumentList Instance { get; } = new ArgumentList();
        private static IDictionary<Type, Func<string, ArgumentList>> _deserializers = new Dictionary<Type, Func<string, ArgumentList>>();

        public static Type GetArgumentClass(Type[] arguments)
        {
            switch (arguments.Length)
            {
                case 0:
                    return typeof(ArgumentList);
                case 1:
                    return typeof(ArgumentList<>).MakeGenericType(arguments);
                case 2:
                    return typeof(ArgumentList<,>).MakeGenericType(arguments);
                case 3:
                    return typeof(ArgumentList<,,>).MakeGenericType(arguments);
                case 4:
                    return typeof(ArgumentList<,,,>).MakeGenericType(arguments);
                case 5:
                    return typeof(ArgumentList<,,,,>).MakeGenericType(arguments);
                case 6:
                    return typeof(ArgumentList<,,,,,>).MakeGenericType(arguments);
                case 7:
                    return typeof(ArgumentList<,,,,,,>).MakeGenericType(arguments);
                default:
                    return GetArgumentsClassCore(arguments, 0);
            }

            Type GetArgumentsClassCore(Type[] args, int position)
            {
                var rest = args.Length - position;
                switch (rest)
                {
                    case 0:
                        // We handle this case in the preamble. If there are more than 7 arguments, we pack the
                        // remaining arguments in nested argument list types, with at least one argument.
                        throw new InvalidOperationException("We shouldn't get here!");
                    case 1:
                        return typeof(ArgumentList<>).MakeGenericType(args.Skip(position).Take(rest).ToArray());
                    case 2:
                        return typeof(ArgumentList<,>).MakeGenericType(args.Skip(position).Take(rest).ToArray());
                    case 3:
                        return typeof(ArgumentList<,,>).MakeGenericType(args.Skip(position).Take(rest).ToArray());
                    case 4:
                        return typeof(ArgumentList<,,,>).MakeGenericType(args.Skip(position).Take(rest).ToArray());
                    case 5:
                        return typeof(ArgumentList<,,,,>).MakeGenericType(args.Skip(position).Take(rest).ToArray());
                    case 6:
                        return typeof(ArgumentList<,,,,,>).MakeGenericType(args.Skip(position).Take(rest).ToArray());
                    case 7:
                        return typeof(ArgumentList<,,,,,,>).MakeGenericType(args.Skip(position).Take(rest).ToArray());
                    case 8:
                        // When there are more than 7 arguments, we transparently package more arguments in a nested arguments type.
                        // {
                        //   argument1: ...,
                        //   argument2: ...,
                        //   argument3: ...,
                        //   argument4: ...,
                        //   argument5: ...,
                        //   argument6: ...,
                        //   argument7: ...,
                        //   argument8: {
                        //     argument1: ..., // Actually argument 8
                        //   }
                        // }

                        var typeArguments = args
                            .Skip(position)
                            .Take(7)
                            .Concat(new[] { GetArgumentsClassCore(args, position + 7) }).ToArray();
                        return typeof(ArgumentList<,,,,,,,>).MakeGenericType(typeArguments);
                    default:
                        throw new InvalidOperationException($"Unsupported number of arguments '{arguments.Length}'");
                }
            }
        }

        public static Func<string, ArgumentList> GetDeserializer(Type deserializedType)
        {
            if (_deserializers.TryGetValue(deserializedType, out var deserializer))
            {
                return deserializer;
            }

            switch (deserializedType.GetGenericArguments().Length)
            {
                case 0:
                    deserializer = JsonDeserialize;
                    break;
                case 1:
                    deserializer = (Func<string, ArgumentList>)deserializedType.GetMethod("JsonDeserialize1", DeserializeFlags)
                        .CreateDelegate(typeof(Func<string, ArgumentList>));
                    break;
                case 2:
                    deserializer = (Func<string, ArgumentList>)deserializedType.GetMethod("JsonDeserialize2", DeserializeFlags)
                        .CreateDelegate(typeof(Func<string, ArgumentList>));
                    break;
                case 3:
                    deserializer = (Func<string, ArgumentList>)deserializedType.GetMethod("JsonDeserialize3", DeserializeFlags)
                        .CreateDelegate(typeof(Func<string, ArgumentList>));
                    break;
                case 4:
                    deserializer = (Func<string, ArgumentList>)deserializedType.GetMethod("JsonDeserialize4", DeserializeFlags)
                        .CreateDelegate(typeof(Func<string, ArgumentList>));
                    break;
                case 5:
                    deserializer = (Func<string, ArgumentList>)deserializedType.GetMethod("JsonDeserialize5", DeserializeFlags)
                        .CreateDelegate(typeof(Func<string, ArgumentList>));
                    break;
                case 6:
                    deserializer = (Func<string, ArgumentList>)deserializedType.GetMethod("JsonDeserialize6", DeserializeFlags)
                        .CreateDelegate(typeof(Func<string, ArgumentList>));
                    break;
                case 7:
                    deserializer = (Func<string, ArgumentList>)deserializedType.GetMethod("JsonDeserialize7", DeserializeFlags)
                        .CreateDelegate(typeof(Func<string, ArgumentList>));
                    break;
                case 8:
                    deserializer = (Func<string, ArgumentList>)deserializedType.GetMethod("JsonDeserialize8", DeserializeFlags)
                        .CreateDelegate(typeof(Func<string, ArgumentList>));
                    break;
                default:
                    throw new InvalidOperationException("Shouldn't have gotten here!");
            }

            _deserializers[deserializedType] = deserializer;

            return deserializer;
        }

        public static ArgumentList JsonDeserialize(string item) => Instance;

        public virtual object[] ToParameterList() => Array.Empty<object>();
    }

    internal class ArgumentList<T1> : ArgumentList
    {
        public T1 Argument1 { get; set; }

        internal static ArgumentList<T1> JsonDeserialize1(string item) =>
            JsonUtil.Deserialize<ArgumentList<T1>>(item);

        public override object[] ToParameterList() => new object[] { Argument1 };
    }

    internal class ArgumentList<T1, T2> : ArgumentList
    {
        public T1 Argument1 { get; set; }
        public T2 Argument2 { get; set; }

        internal static ArgumentList<T1, T2> JsonDeserialize2(string item) =>
            JsonUtil.Deserialize<ArgumentList<T1, T2>>(item);

        public override object[] ToParameterList() => new object[] { Argument1, Argument2 };
    }

    internal class ArgumentList<T1, T2, T3> : ArgumentList
    {
        public T1 Argument1 { get; set; }
        public T2 Argument2 { get; set; }
        public T3 Argument3 { get; set; }

        internal static ArgumentList<T1, T2, T3> JsonDeserialize3(string item) =>
            JsonUtil.Deserialize<ArgumentList<T1, T2, T3>>(item);

        public override object[] ToParameterList() => new object[] { Argument1, Argument2, Argument3 };
    }

    internal class ArgumentList<T1, T2, T3, T4> : ArgumentList
    {
        public T1 Argument1 { get; set; }
        public T2 Argument2 { get; set; }
        public T3 Argument3 { get; set; }
        public T4 Argument4 { get; set; }

        internal static ArgumentList<T1, T2, T3, T4> JsonDeserialize4(string item) =>
            JsonUtil.Deserialize<ArgumentList<T1, T2, T3, T4>>(item);

        public override object[] ToParameterList() => new object[] { Argument1, Argument2, Argument3, Argument4 };
    }

    internal class ArgumentList<T1, T2, T3, T4, T5> : ArgumentList
    {
        public T1 Argument1 { get; set; }
        public T2 Argument2 { get; set; }
        public T3 Argument3 { get; set; }
        public T4 Argument4 { get; set; }
        public T5 Argument5 { get; set; }

        internal static ArgumentList<T1, T2, T3, T4, T5> JsonDeserialize5(string item) =>
            JsonUtil.Deserialize<ArgumentList<T1, T2, T3, T4, T5>>(item);

        public override object[] ToParameterList() => new object[] { Argument1, Argument2, Argument3, Argument4, Argument5 };
    }

    internal class ArgumentList<T1, T2, T3, T4, T5, T6> : ArgumentList
    {
        public T1 Argument1 { get; set; }
        public T2 Argument2 { get; set; }
        public T3 Argument3 { get; set; }
        public T4 Argument4 { get; set; }
        public T5 Argument5 { get; set; }
        public T6 Argument6 { get; set; }

        internal static ArgumentList<T1, T2, T3, T4, T5, T6> JsonDeserialize6(string item) =>
            JsonUtil.Deserialize<ArgumentList<T1, T2, T3, T4, T5, T6>>(item);

        public override object[] ToParameterList() => new object[] { Argument1, Argument2, Argument3, Argument4, Argument5, Argument6 };
    }

    internal class ArgumentList<T1, T2, T3, T4, T5, T6, T7> : ArgumentList
    {
        public T1 Argument1 { get; set; }
        public T2 Argument2 { get; set; }
        public T3 Argument3 { get; set; }
        public T4 Argument4 { get; set; }
        public T5 Argument5 { get; set; }
        public T6 Argument6 { get; set; }
        public T7 Argument7 { get; set; }

        internal static ArgumentList<T1, T2, T3, T4, T5, T6, T7> JsonDeserialize7(string item) =>
            JsonUtil.Deserialize<ArgumentList<T1, T2, T3, T4, T5, T6, T7>>(item);

        public override object[] ToParameterList() => new object[] { Argument1, Argument2, Argument3, Argument4, Argument5, Argument6, Argument7 };
    }

    internal class ArgumentList<T1, T2, T3, T4, T5, T6, T7, T8> : ArgumentList
    {
        public T1 Argument1 { get; set; }
        public T2 Argument2 { get; set; }
        public T3 Argument3 { get; set; }
        public T4 Argument4 { get; set; }
        public T5 Argument5 { get; set; }
        public T6 Argument6 { get; set; }
        public T7 Argument7 { get; set; }
        public T8 Argument8 { get; set; }

        internal static ArgumentList<T1, T2, T3, T4, T5, T6, T7, T8> JsonDeserialize8(string item) =>
            JsonUtil.Deserialize<ArgumentList<T1, T2, T3, T4, T5, T6, T7, T8>>(item);

        public override object[] ToParameterList()
        {
            if (Argument8 == null)
            {
                throw new InvalidOperationException("Argument8 can't be null!");
            }

            if (!(Argument8 is ArgumentList rest))
            {
                throw new InvalidOperationException("Argument 8 must be an ArgumentList");
            }
            if (rest.GetType().GetGenericArguments().Length < 1)
            {
                throw new InvalidOperationException("Argument 8 must contain an inner parameter!");
            }

            return new object[] { Argument1, Argument2, Argument3, Argument4, Argument5, Argument6, Argument7 }.Concat(rest.ToParameterList()).ToArray();
        }
    }
}
