using System;
using System.Collections.Generic;

namespace ES.Lab.Infrastructure
{
    public static class Extensions
    {
         public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
         {
             foreach (var item in self)
             {
                 action(item);
             }
         }

        public static T Tap<T>(this T self, Action<T> action)
        {
            action(self);
            return self;
        }
    }
}