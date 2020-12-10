using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShwasherSys.ReflectionMagic
{
    public class ReflectionMapper<TIn,TOut>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="tIn"></param>
        /// <returns></returns>
        public static TOut Trans<TIn, TOut>(TIn tIn)
        {
            TOut tout = Activator.CreateInstance<TOut>();
            foreach (var itemOut in tout.GetType().GetProperties())
            {
                var propIn = tIn.GetType().GetProperty(itemOut.Name);
                itemOut.SetValue(tout,propIn?.GetValue(tIn));
            }
            foreach (var itemOut in tout.GetType().GetFields())
            {
                var fieldIn = tIn.GetType().GetField(itemOut.Name);
                itemOut.SetValue(tout, fieldIn?.GetValue(tIn));
            }

            return tout;
        }
    }
}
