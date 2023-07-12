using api.Extensions;
using System;

namespace tests.Factories
{
    public static class ConstantFactory
    {
        private static int id = 0;
        public static int Id => ++id;
        public static string Text => Guid.NewGuid().ToString().OnlyLetters();
    }
}