using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eagleboost.componentApp
{
  using System.Diagnostics;
  using System.Linq.Expressions;
  using System.Reflection;
  using System.Reflection.Emit;
  using eagleboost.component.Extensions;
  using eagleboost.component.Interceptions;
  using eagleboost.sharedcomponents.Contracts;
  using eagleboost.sharedcomponents.ViewModels;
  using Unity;
  using Unity.Interception.ContainerIntegration;
  using Unity.Interception.Interceptors.TypeInterceptors.VirtualMethodInterception;

  class Program
  {
    static void Main(string[] args)
    {
      var container = new UnityContainer();
      container.AddNewExtension<Interception>();

      var source = new CompositeSource();
      var param = Expression.Parameter(typeof(CompositeSource));
      var lambda = Expression.Lambda<Func<CompositeSource, int>>(Expression.Convert(
        Expression.Property(param, "Age"), typeof(int)), param).Compile();

      var method = typeof(CompositeSource).GetProperty("Age",
          BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        .GetGetMethod(true);
      var func = (Func<CompositeSource, int>)
        Delegate.CreateDelegate(typeof(Func<CompositeSource, int>), method);

      var getMethod = typeof(CompositeSource).GetProperty("Age").GetMethod;

      var propertyInfo = typeof(CompositeSource).GetProperty("Age");
        var getMethodDynamicCall = new DynamicMethod(
          string.Concat(getMethod.Name, "_DynamicGetter_", Guid.NewGuid().ToString("N").ToUpper()),
          propertyInfo.PropertyType,
          new[] { propertyInfo.DeclaringType },
          propertyInfo.DeclaringType,
          true
        );
        var il = getMethodDynamicCall.GetILGenerator();

        if (!getMethod.IsStatic)
        {
          il.Emit(OpCodes.Ldarg_0);
        }
        il.EmitCall(OpCodes.Call, getMethod, null);
        il.Emit(OpCodes.Ret);

        var func2 = (Func<CompositeSource, int>)getMethodDynamicCall.CreateDelegate(typeof(Func<,>).MakeGenericType(propertyInfo.DeclaringType, typeof(int)));
      var stopWatch = new Stopwatch();

      stopWatch.Start();
      for (int i = 0; i < 1000000; i++)
      {
        if (source.Age == 0)
        {

        }
      }
      stopWatch.Stop();
      Console.WriteLine(stopWatch.ElapsedTicks);

      stopWatch.Start();
      for (int i = 0; i < 1000000; i++)
      {
        if ((int)func(source) == 0)
        {

        }
      }
      stopWatch.Stop();
      Console.WriteLine(stopWatch.ElapsedTicks);

      stopWatch.Start();
      for (int i = 0; i < 1000000; i++)
      {
        if ((int)func2(source) == 0)
        {

        }
      }
      stopWatch.Stop();
      Console.WriteLine(stopWatch.ElapsedTicks);

      stopWatch.Start();
      for (int i = 0; i < 1000000; i++)
      {
        if ((int)lambda(source) == 0)
        {

        }
      }
      stopWatch.Stop();
      Console.WriteLine(stopWatch.ElapsedTicks);

      stopWatch = new Stopwatch();
      stopWatch.Start();
      for (int i = 0; i < 1000000; i++)
      {
        if ((int)getMethod.Invoke(source, null) == 0)
        {

        }
      }
      stopWatch.Stop();
      Console.WriteLine(stopWatch.ElapsedTicks);
      Console.ReadKey();

      //var result = getter(source);


      //// todo: Register an interface instead of a type.
      //container.RegisterAutoCompositeType<IPersonInfo, AutoComposite>();
      //var obj = container.Resolve<AutoComposite>();
      //obj.CompositeSource = new CompositeSource();
      //if (obj.FirstName == "Age")
      //{
      //}
    }
  }
}
