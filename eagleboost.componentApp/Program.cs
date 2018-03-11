using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eagleboost.componentApp
{
  using System.Linq.Expressions;
  using eagleboost.component.Interceptions;
  using Unity;
  using Unity.Interception.ContainerIntegration;
  using Unity.Interception.Interceptors.TypeInterceptors.VirtualMethodInterception;

  class Program
  {
    static void Main(string[] args)
    {
      //OrderByDerivedProperty(new B(){M = "a"}, typeof(B), "M");
      var param = Expression.Parameter(typeof(object), "x");
      var compiled = Expression.Lambda<Func<object, bool>>(CreateExpression(param), param);
      var func = compiled.Compile();
      Console.WriteLine(func(new object()));
      Console.WriteLine(func(new A()));
    }

    private static Expression CreateExpression(ParameterExpression param)
    {
      var result = Expression.Variable(typeof(bool));
      var typeExpr = Expression.Variable(typeof(A));
      var castType = Expression.Assign(typeExpr, Expression.TypeAs(param, typeof(A)));
      return Expression.Block(new[] { result, typeExpr},
        castType,
        Expression.IfThenElse(Expression.NotEqual(typeExpr, Expression.Constant(null, typeof(A))), 
          Expression.Assign(result, Expression.Constant(true)), 
          Expression.Assign(result, Expression.Constant(false))),
        result);
    }

    public static object OrderByDerivedProperty<TSource>(TSource source, Type derivedType, string propertyOrFieldName)
    {
      if (!derivedType.IsClass)
      {
        throw new Exception("Derived type must be a class.");
      }
      ParameterExpression sourceParameter = Expression.Parameter(typeof(object), "source");
      ParameterExpression typeAsVariable = Expression.Variable(derivedType);
      ParameterExpression returnVariable = Expression.Variable(typeof(object));
      BlockExpression block = Expression.Block(
          new[] { typeAsVariable, returnVariable },
          Expression.Assign(
              typeAsVariable,
              Expression.TypeAs(
                  sourceParameter,
                  derivedType
              )
          ),
          Expression.Condition(
              Expression.NotEqual(
                  typeAsVariable,
                  Expression.Constant(
                      null,
                      derivedType
                  )
              ),
              Expression.Assign(
                  returnVariable,
                  Expression.Convert(
                      Expression.PropertyOrField(
                          typeAsVariable,
                          propertyOrFieldName
                      ),
                      typeof(object)
                  )
              ),
              Expression.Assign(
                  returnVariable,
                  Expression.Constant(
                      null,
                      typeof(object)
                  )
              )
          ),
          returnVariable
      );
      var lambda = Expression.Lambda<Func<object, object>>(block, new[] { sourceParameter });
      return lambda.Compile().Invoke(source);
    }
  }

  public class A
  {

  }

  public class B : A
  {
    public string M { get; set; }
  }
}
