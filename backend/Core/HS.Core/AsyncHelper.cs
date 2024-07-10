using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public static class AsyncHelper
    {
        public static T Sincronizar<T>(Task<T> task)
        {
            try
            {
                task.Wait();
                return task.Result;
            }
            catch(AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public static void Sincronizar(Task task)
        {
            try
            {
                task.Wait();
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public static T Sincronizar<T>(Func<Task<T>> fnc)
        {
            try
            {
                var tarea = fnc();
                tarea.Wait();
                return tarea.Result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public static void Sincronizar(Func<Task> fnc)
        {
            try
            {
                var tarea = fnc();
                tarea.Wait();
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
